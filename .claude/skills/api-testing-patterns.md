	# API Testing Patterns for ASP.NET Core

## Integration Tests with WebApplicationFactory
```csharp
public class ProductsApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;
    
    public ProductsApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Replace real DbContext with in-memory database
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });
            });
        });
        
        _client = _factory.CreateClient();
    }
    
    [Fact]
    public async Task GetProducts_ReturnsSuccessStatusCode()
    {
        // Arrange
        await SeedTestData();
        
        // Act
        var response = await _client.GetAsync("/api/products");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var products = await response.Content.ReadFromJsonAsync<List<ProductResponse>>();
        Assert.NotNull(products);
        Assert.NotEmpty(products);
    }
    
    [Fact]
    public async Task CreateProduct_WithValidData_ReturnsCreated()
    {
        // Arrange
        var request = new CreateProductRequest("Test Product", 99.99m, 1);
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/products", request);
        
        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var product = await response.Content.ReadFromJsonAsync<ProductResponse>();
        Assert.NotNull(product);
        Assert.Equal("Test Product", product.Name);
    }
    
    private async Task SeedTestData()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        context.Categories.Add(new Category { Id = 1, Name = "Electronics" });
        context.Products.Add(new Product
        {
            Id = 1,
            Name = "Laptop",
            Price = 999.99m,
            CategoryId = 1
        });
        
        await context.SaveChangesAsync();
    }
}
```

## Unit Tests for Services
```csharp
public class ProductServiceTests
{
    private readonly Mock<ApplicationDbContext> _contextMock;
    private readonly ProductService _service;
    
    public ProductServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
            
        _contextMock = new Mock<ApplicationDbContext>(options);
        _service = new ProductService(_contextMock.Object);
    }
    
    [Fact]
    public async Task CreateAsync_WithValidProduct_AddsToDatabase()
    {
        // Arrange
        var request = new CreateProductRequest("Test", 10.00m, 1);
        
        // Act
        var result = await _service.CreateAsync(request);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test", result.Name);
        _contextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }
}
```

## Testing with Authentication
```csharp
public class AuthenticatedApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    
    public AuthenticatedApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
            });
        }).CreateClient();
    }
    
    [Fact]
    public async Task AdminEndpoint_WithAdminRole_ReturnsSuccess()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Test");
        
        // Act
        var response = await _client.GetAsync("/api/admin/users");
        
        // Assert
        response.EnsureSuccessStatusCode();
    }
}

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }
    
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
            new Claim(ClaimTypes.Role, "Admin")
        };
        
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");
        
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
```