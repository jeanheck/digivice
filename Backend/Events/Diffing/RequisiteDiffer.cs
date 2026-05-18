using Backend.Domain.Models.Journals.Quests;
using Backend.Events.DTO;

namespace Backend.Events.Diffing;

public static class RequisiteDiffer
{
    public static RequisiteDTO? Diff(Requisite? previous, Requisite current)
    {
        if (previous == null || previous.Value != current.Value)
        {
            return new RequisiteDTO { Id = current.Id, Value = current.Value };
        }

        return null;
    }
}
