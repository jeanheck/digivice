using Backend.Domain.Models.Journals.Quests;
using Backend.Memory.Resources.Journal.Quest;

namespace Backend.Domain.Assemblers.Journal.Quest
{
    public static class RequisiteAssembler
    {
        public static Requisite Assemble(RequisiteResource resource)
        {
            return new Requisite
            {
                Id = resource.Id,
                Value = resource.Value
            };
        }
    }
}
