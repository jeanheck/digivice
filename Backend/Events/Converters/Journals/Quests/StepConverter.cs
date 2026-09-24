using Backend.Domain.Models.Journals.Quests;
using Backend.Events.DTO.Journals.Quests;

namespace Backend.Events.Converters.Journals.Quests;

public static class StepConverter
{
    public static StepDTO ToDTO(Step step) => new()
    {
        Number = step.Number,
        IsDone = step.IsDone,
        Requisites = step.Requisites.Select(RequisiteConverter.ToDTO).ToList()
    };
}
