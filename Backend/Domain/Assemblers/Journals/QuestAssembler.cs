using Backend.Domain.Models.Journals;
using Backend.Memory.Resources.Journals;
using Backend.Domain.Assemblers.Journals.Quests;

namespace Backend.Domain.Assemblers.Journals
{
    public static class QuestAssembler
    {
        public static Quest Assemble(QuestResource resource)
        {
            return new Quest
            {
                Id = resource.Id,
                Requisites = [.. resource.Requisites.Select(RequisiteAssembler.Assemble)],
                Steps = [.. resource.Steps.Select(StepAssembler.Assemble)]
            };
        }
    }
}
