using ClosedXML.Excel;
using PremestiSe.Database.Contracts.Dtos;
using PremestiSe.Database.Contracts.Interfaces;
using VeloxDB.Client;

const string DefaultExcelPath = "docs/obdanista_csv_cyrillic.xlsx";
const string DefaultServerAddress = "localhost:7568";

var excelPath = args.Length > 0 ? args[0] : DefaultExcelPath;
var serverAddress = args.Length > 1 ? args[1] : DefaultServerAddress;

Console.WriteLine("=== PremestiSe Kindergarten Seeder ===");
Console.WriteLine();

// Read Excel file
Console.WriteLine($"Reading Excel file: {excelPath}");
var kindergartens = ReadKindergartensFromExcel(excelPath);
Console.WriteLine($"Found {kindergartens.Length} kindergartens");
Console.WriteLine();

// Connect to VeloxDB
Console.WriteLine($"Connecting to VeloxDB at {serverAddress}...");
var connectionParams = new ConnectionStringParams();
connectionParams.AddAddress(serverAddress);

try
{
    var api = ConnectionFactory.Get<IKindergartenApi>(connectionParams.GenerateConnectionString());

    // Clear existing data
    Console.WriteLine("Clearing existing data...");
    await api.DeleteAllAsync();

    // Seed data in batches
    const int batchSize = 500;
    var totalSeeded = 0;

    for (var i = 0; i < kindergartens.Length; i += batchSize)
    {
        var batch = kindergartens.Skip(i).Take(batchSize).ToArray();
        var seeded = await api.SeedAsync(batch);
        totalSeeded += seeded;
        Console.WriteLine($"Seeded batch {i / batchSize + 1}: {seeded} records (Total: {totalSeeded})");
    }

    Console.WriteLine();
    Console.WriteLine($"Successfully seeded {totalSeeded} kindergartens!");

    // Verify
    var all = await api.GetAllAsync();
    Console.WriteLine($"Verification: {all.Length} records in database");
}
catch (Exception ex)
{
    Console.WriteLine($"Error connecting to VeloxDB: {ex.Message}");
    Console.WriteLine();
    Console.WriteLine("Make sure the VeloxDB server is running:");
    Console.WriteLine("  dotnet run --project src/PremestiSe.Database");
    return 1;
}

return 0;

static KindergartenDto[] ReadKindergartensFromExcel(string filePath)
{
    using var workbook = new XLWorkbook(filePath);
    var worksheet = workbook.Worksheet(1);
    var rows = worksheet.RangeUsed()?.RowsUsed();

    if (rows is null)
        return [];

    var kindergartens = new List<KindergartenDto>();

    foreach (var row in rows.Skip(1)) // Skip header row
    {
        var locationType = row.Cell(9).GetString();

        kindergartens.Add(new KindergartenDto
        {
            Administration = row.Cell(1).GetString(),
            Municipality = row.Cell(2).GetString(),
            Settlement = row.Cell(3).GetString(),
            Institution = row.Cell(4).GetString(),
            Name = row.Cell(5).GetString(),
            Street = row.Cell(6).GetString(),
            Number = row.Cell(7).GetString(),
            PostalCode = row.Cell(8).GetString(),
            LocationType = locationType == "Матична локација" ? 0 : 1
        });
    }

    return kindergartens.ToArray();
}
