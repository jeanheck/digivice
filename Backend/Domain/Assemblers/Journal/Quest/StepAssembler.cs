using Backend.Domain.Models.Journals.Quests;
using Backend.Memory.Resources.Journal.Quest;

namespace Backend.Domain.Assemblers.Journal.Quest
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
