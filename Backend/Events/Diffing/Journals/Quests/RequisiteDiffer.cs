using Backend.Domain.Models.Journals.Quests;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO.Journals.Quests;

namespace Backend.Events.Diffing.Journals.Quests;

public static class RequisiteDiffer
{
    public static RequisiteDTO? Diff(Requisite? previousRequisite, Requisite newRequisite)
    {
        if (newRequisite.HasNoChanges(previousRequisite))
        {
            return null;
        }

        if (previousRequisite == null || previousRequisite.IsDone != newRequisite.IsDone)
        {
            return new RequisiteDTO { Id = newRequisite.Id, IsDone = newRequisite.IsDone };
        }

        return null;
    }
}
