using Backend.Domain.Models.Journals.Quests;
using Backend.Events.DTO.Journal;

namespace Backend.Events.Converters;

public static class QuestStepConverter
{
    public static StepDTO ToDTO(Step step) => new()
    {
        Number = step.Number,
        Value = step.Value,
        Requisites = step.Requisites.Select(RequisiteConverter.ToDTO).ToList()
    };
}
