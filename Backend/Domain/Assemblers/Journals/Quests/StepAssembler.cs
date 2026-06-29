using Backend.Domain.Models.Journals.Quests;
using Backend.Memory.Resources.Journals.Quests;

namespace Backend.Domain.Assemblers.Journals.Quests
{
    public static class StepAssembler
    {
        public static Step Assemble(StepResource resource)
        {
            return new Step
            {
                Number = resource.Number,
                Value = resource.Value,
                Requisites = [.. resource.Requisites.Select(RequisiteAssembler.Assemble)]
            };
        }
    }
}
