using Backend.Domain.Models.Journals.Quests;
using Backend.Events.DTO.Journals.Quests;

namespace Backend.Events.Converters.Journals.Quests;

public static class RequisiteConverter
{
    public static RequisiteDTO ToDTO(Requisite requisite) => new()
    {
        Id = requisite.Id,
        Value = requisite.Value
    };
}
