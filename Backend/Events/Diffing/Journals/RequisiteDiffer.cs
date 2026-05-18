using Backend.Domain.Models.Journals.Quests;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO.Journal;

namespace Backend.Events.Diffing.Journals;

public static class RequisiteDiffer
{
    public static RequisiteDTO? Diff(Requisite? previousRequisite, Requisite newRequisite)
    {
        if (newRequisite.HasNoChanges(previousRequisite))
        {
            return null;
        }

        if (previousRequisite == null || previousRequisite.Value != newRequisite.Value)
        {
            return new RequisiteDTO { Id = newRequisite.Id, Value = newRequisite.Value };
        }

        return null;
    }
}
