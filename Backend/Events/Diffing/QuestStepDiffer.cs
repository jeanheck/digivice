using Backend.Domain.Models.Journals.Quests;
using Backend.Events.DTO;

namespace Backend.Events.Diffing;

public static class QuestStepDiffer
{
    public static StepDTO? Diff(Step? previous, Step current)
    {
        if (previous == null)
        {
            return new StepDTO
            {
                Number = current.Number,
                Value = current.Value,
                Requisites = current.Requisites
                    .Select(r => RequisiteDiffer.Diff(null, r)!)
                    .ToList()
            };
        }

        bool valueChanged = previous.Value != current.Value;
        var requisitesDelta = new List<RequisiteDTO>();

        foreach (var currReq in current.Requisites)
        {
            var prevReq = previous.Requisites.FirstOrDefault(r => r.Id == currReq.Id);
            var reqDelta = RequisiteDiffer.Diff(prevReq, currReq);
            if (reqDelta != null)
            {
                requisitesDelta.Add(reqDelta);
            }
        }

        if (valueChanged || requisitesDelta.Count > 0)
        {
            var stepDto = new StepDTO { Number = current.Number };
            if (valueChanged)
            {
                stepDto = stepDto with { Value = current.Value };
            }
            if (requisitesDelta.Count > 0)
            {
                stepDto = stepDto with { Requisites = requisitesDelta };
            }
            return stepDto;
        }

        return null;
    }
}
