using System.Linq;
using Backend.Domain.Models.Journals.Quests;
using Backend.Events.DTO;

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
