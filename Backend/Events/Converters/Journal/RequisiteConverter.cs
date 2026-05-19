using Backend.Domain.Models.Journals.Quests;
using Backend.Events.DTO.Journal;

namespace Backend.Events.Converters.Journal;

public static class RequisiteConverter
{
    public static RequisiteDTO ToDTO(Requisite requisite) => new()
    {
        Id = requisite.Id,
        Value = requisite.Value
    };
}
