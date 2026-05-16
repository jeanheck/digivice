using Backend.Domain.Models;
using Backend.Memory.Resources;

namespace Backend.Domain.Assemblers
{
    public static class QuestAssembler
    {
        public static Quest Assemble(QuestResource resource)
        {
            return new Quest
            {
                Id = resource.Id,
                Requisites = resource.Requisites.Select(RequisiteAssembler.Assemble).ToList(),
                Steps = resource.Steps.Select(StepAssembler.Assemble).ToList()
            };
        }
    }
}
