using Backend.Domain.Models.Quests;
using Backend.Memory.Resources.Quests;

namespace Backend.Domain.Assemblers
{
    public static class StepAssembler
    {
        public static Step Assemble(StepResource resource)
        {
            return new Step
            {
                Number = resource.Number,
                Value = resource.Value,
                Requisites = resource.Requisites.Select(RequisiteAssembler.Assemble).ToList()
            };
        }
    }
}
