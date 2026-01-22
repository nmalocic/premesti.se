using PremestiSe.Database.Contracts.Dtos;
using PremestiSe.Database.Entities;
using VeloxDB.ObjectInterface;
using VeloxDB.Protocol;

namespace PremestiSe.Database.Operations;

[DbAPI(Name = "KindergartenApi")]
public class KindergartenOperations
{
    [DbAPIOperation]
    public KindergartenDto[] GetAll(ObjectModel om)
    {
        return om.GetAllObjects<KindergartenEntity>()
            .Select(ToDto)
            .ToArray();
    }

    [DbAPIOperation]
    public KindergartenDto? GetById(ObjectModel om, long id)
    {
        var entity = om.GetObject<KindergartenEntity>(id);
        return entity is null ? null : ToDto(entity);
    }

    [DbAPIOperation]
    public KindergartenDto[] GetByMunicipality(ObjectModel om, string municipality)
    {
        return om.GetAllObjects<KindergartenEntity>()
            .Where(k => k.Municipality == municipality)
            .Select(ToDto)
            .ToArray();
    }

    [DbAPIOperation]
    public KindergartenDto[] GetByInstitution(ObjectModel om, string institution)
    {
        return om.GetAllObjects<KindergartenEntity>()
            .Where(k => k.Institution == institution)
            .Select(ToDto)
            .ToArray();
    }

    [DbAPIOperation]
    public long Create(ObjectModel om, KindergartenDto dto)
    {
        var entity = om.CreateObject<KindergartenEntity>();
        MapFromDto(entity, dto);
        return entity.Id;
    }

    [DbAPIOperation]
    public int Seed(ObjectModel om, KindergartenDto[] kindergartens)
    {
        foreach (var dto in kindergartens)
        {
            var entity = om.CreateObject<KindergartenEntity>();
            MapFromDto(entity, dto);
        }
        return kindergartens.Length;
    }

    [DbAPIOperation]
    public void DeleteAll(ObjectModel om)
    {
        foreach (var entity in om.GetAllObjects<KindergartenEntity>())
        {
            entity.Delete();
        }
    }

    private static KindergartenDto ToDto(KindergartenEntity entity) => new()
    {
        Id = entity.Id,
        Administration = entity.Administration,
        Municipality = entity.Municipality,
        Settlement = entity.Settlement,
        Institution = entity.Institution,
        Name = entity.Name,
        Street = entity.Street,
        Number = entity.Number,
        PostalCode = entity.PostalCode,
        LocationType = entity.LocationType
    };

    private static void MapFromDto(KindergartenEntity entity, KindergartenDto dto)
    {
        entity.Administration = dto.Administration;
        entity.Municipality = dto.Municipality;
        entity.Settlement = dto.Settlement;
        entity.Institution = dto.Institution;
        entity.Name = dto.Name;
        entity.Street = dto.Street;
        entity.Number = dto.Number;
        entity.PostalCode = dto.PostalCode;
        entity.LocationType = dto.LocationType;
    }
}
