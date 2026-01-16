
## Project Structure
```
MyProject.Api/
├── Controllers/ or Endpoints/
├── Models/
│   ├── Requests/
│   ├── Responses/
│   └── Entities/
├── Services/
│   ├── Interfaces/
│   └── Implementations/
├── Data/
│   ├── ApplicationDbContext.cs
│   ├── Configurations/
│   └── Migrations/
├── Middleware/
├── Filters/
└── Program.cs
```

## Dependency Injection Patterns

### Service Lifetimes
- **Transient**: Created each time requested (stateless services)
- **Scoped**: Created once per request (DbContext, UnitOfWork)
- **Singleton**: Created once (caching, configuration)
```csharp
// Transient
builder.Services.AddTransient<IEmailService, EmailService>();

// Scoped (default for DbContext)
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddDbContext<ApplicationDbContext>();

// Singleton
builder.Services.AddSingleton<ICache, MemoryCache>();
```

## Configuration Management
```csharp
// appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "..."
  },
  "Jwt": {
    "Key": "...",
    "Issuer": "...",
    "Audience": "..."
  }
}

// Strongly-typed configuration
public class JwtSettings
{
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}

// Program.cs
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));

// Usage in service
public class TokenService
{
    private readonly JwtSettings _jwtSettings;
    
    public TokenService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }
}
```

## Middleware Pipeline Order
```csharp
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
```

## Error Handling
```csharp
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }
    
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An error occurred");
        
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An error occurred",
            Detail = exception.Message
        };
        
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        
        return true;
    }
}

// Program.cs
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
```

## Validation
```csharp
// Using FluentValidation
public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);
            
        RuleFor(x => x.Price)
            .GreaterThan(0)
            .LessThanOrEqualTo(1000000);
    }
}

// Register
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>();

// Use in endpoint
app.MapPost("/api/products", async (
    CreateProductRequest request,
    IValidator<CreateProductRequest> validator,
    IProductService productService) =>
{
    var validationResult = await validator.ValidateAsync(request);
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }
    
    var product = await productService.CreateAsync(request);
    return Results.Created($"/api/products/{product.Id}", product);
});
```

## Logging
```csharp
// Use structured logging
_logger.LogInformation(
    "Creating product {ProductName} with price {Price}",
    request.Name,
    request.Price);

// Don't log sensitive data
_logger.LogWarning(
    "Login failed for user {UserId}",
    userId); // Don't log passwords, tokens, etc.
```

## Async/Await Best Practices

1. Use async all the way
2. Don't use `.Result` or `.Wait()`
3. Use `ConfigureAwait(false)` in libraries (not needed in ASP.NET Core)
4. Cancel long-running operations with CancellationToken
```csharp
public async Task<Product> GetProductAsync(int id, CancellationToken cancellationToken = default)
{
    return await _context.Products
        .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
}
```