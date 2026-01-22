using PremestiSe.Database.Contracts.Dtos;
using VeloxDB.Client;
using VeloxDB.Protocol;

namespace PremestiSe.Database.Contracts.Interfaces;

[DbAPI(Name = "KindergartenApi")]
public interface IKindergartenApi
{
    [DbAPIOperation]
    DatabaseTask<KindergartenDto[]> GetAllAsync();

    [DbAPIOperation]
    DatabaseTask<KindergartenDto?> GetByIdAsync(long id);

    [DbAPIOperation]
    DatabaseTask<KindergartenDto[]> GetByMunicipalityAsync(string municipality);

    [DbAPIOperation]
    DatabaseTask<KindergartenDto[]> GetByInstitutionAsync(string institution);

    [DbAPIOperation]
    DatabaseTask<long> CreateAsync(KindergartenDto dto);

    [DbAPIOperation]
    DatabaseTask<int> SeedAsync(KindergartenDto[] kindergartens);

    [DbAPIOperation]
    DatabaseTask DeleteAllAsync();
}
