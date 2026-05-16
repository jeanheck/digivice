using Backend.Domain.Models.Quests;
using Backend.Memory.Resources.Quests;

namespace Backend.Domain.Assemblers
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
