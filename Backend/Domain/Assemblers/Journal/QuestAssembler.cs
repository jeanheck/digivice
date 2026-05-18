using Backend.Memory.Resources.Journal;
using Backend.Domain.Assemblers.Journal.Quest;

namespace Backend.Domain.Assemblers.Journal
{
    public static class QuestAssembler
    {
        public static Backend.Domain.Models.Journals.Quest Assemble(QuestResource resource)
        {
            return new Backend.Domain.Models.Journals.Quest
            {
                Id = resource.Id,
                Requisites = [.. resource.Requisites.Select(RequisiteAssembler.Assemble)],
                Steps = [.. resource.Steps.Select(StepAssembler.Assemble)]
            };
        }
    }
}
