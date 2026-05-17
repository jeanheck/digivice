using Backend.Domain.Models.Quests;
using Backend.Memory.Resources.Journal.Quest;

namespace Backend.Domain.Assemblers.Quests
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
