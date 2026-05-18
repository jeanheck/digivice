using System.Collections.Generic;
using System.Linq;
using Backend.Domain.Models.Journals;
using Backend.Events.DTO;
using Backend.Events.Converters;

namespace Backend.Events.Diffing;

public static class QuestDiffer
{
    public static QuestDTO? Diff(Quest? previous, Quest current)
    {
        if (previous == null)
        {
            return QuestConverter.ToDTO(current);
        }

        var requisitesDelta = new List<RequisiteDTO>();
        var stepsDelta = new List<StepDTO>();

        // 1. Diff Quest Requisites
        foreach (var currReq in current.Requisites)
        {
            var prevReq = previous.Requisites.FirstOrDefault(r => r.Id == currReq.Id);
            var reqDelta = RequisiteDiffer.Diff(prevReq, currReq);
            if (reqDelta != null)
            {
                requisitesDelta.Add(reqDelta);
            }
        }

        // 2. Diff Quest Steps
        foreach (var currStep in current.Steps)
        {
            var prevStep = previous.Steps.FirstOrDefault(s => s.Number == currStep.Number);
            var stepDelta = StepDiffer.Diff(prevStep, currStep);
            if (stepDelta != null)
            {
                stepsDelta.Add(stepDelta);
            }
        }

        if (requisitesDelta.Count > 0 || stepsDelta.Count > 0)
        {
            var questDto = new QuestDTO { Id = current.Id };
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
