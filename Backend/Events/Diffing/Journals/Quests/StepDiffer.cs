using Backend.Domain.Models.Journals.Quests;
using Backend.Events.Converters.Journals.Quests;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO.Journals.Quests;

namespace Backend.Events.Diffing.Journals.Quests;

public static class StepDiffer
{
    public static StepDTO? Diff(Step? previousStep, Step newStep)
    {
        if (newStep.HasNoChanges(previousStep))
        {
            return null;
        }

        if (previousStep == null)
        {
            return StepConverter.ToDTO(newStep);
        }

        bool valueChanged = previousStep.Value != newStep.Value;

        var requisitesDelta = new List<RequisiteDTO>();
        foreach (var newRequisite in newStep.Requisites)
        {
            var previousRequisite = previousStep.Requisites.FirstOrDefault(r => r.Id == newRequisite.Id);
            var requisiteDelta = RequisiteDiffer.Diff(previousRequisite, newRequisite);
            if (requisiteDelta != null)
            {
                requisitesDelta.Add(requisiteDelta);
            }
        }

        if (!valueChanged && requisitesDelta.Count == 0)
        {
            return null;
        }

        var dto = new StepDTO { Number = newStep.Number };
        if (valueChanged)
        {
            dto = dto with { Value = newStep.Value };
        }
        if (requisitesDelta.Count > 0)
        {
            dto = dto with { Requisites = requisitesDelta };
        }
        return dto;
    }
}
