using Backend.Domain.Models.Journals;
using Backend.Events.Converters;
using Backend.Events.DTO;

namespace Backend.Events.Diffing;

public static class QuestDiffer
{
    public static QuestDTO? Diff(Quest? previousQuest, Quest newQuest)
    {
        if (previousQuest == null)
        {
            return QuestConverter.ToDTO(newQuest);
        }

        var requisitesDelta = new List<RequisiteDTO>();
        foreach (var newRequisite in newQuest.Requisites)
        {
            var previousRequisite = previousQuest.Requisites.FirstOrDefault(r => r.Id == newRequisite.Id);
            var requisiteDelta = RequisiteDiffer.Diff(previousRequisite, newRequisite);
            if (requisiteDelta != null)
            {
                requisitesDelta.Add(requisiteDelta);
            }
        }

        var stepsDelta = new List<StepDTO>();
        foreach (var newStep in newQuest.Steps)
        {
            var previousStep = previousQuest.Steps.FirstOrDefault(s => s.Number == newStep.Number);
            var stepDelta = StepDiffer.Diff(previousStep, newStep);
            if (stepDelta != null)
            {
                stepsDelta.Add(stepDelta);
            }
        }

        if (requisitesDelta.Count > 0 || stepsDelta.Count > 0)
        {
            var questDto = new QuestDTO { Id = newQuest.Id };
            if (requisitesDelta.Count > 0)
            {
                questDto = questDto with { Requisites = requisitesDelta };
            }
            if (stepsDelta.Count > 0)
            {
                questDto = questDto with { Steps = stepsDelta };
            }
            return questDto;
        }

        return null;
    }
}
