using Backend.Domain.Models.Journals.Quests;
using Backend.Memory.Resources.Journals.Quests;

namespace Backend.Domain.Assemblers.Journals.Quests
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
