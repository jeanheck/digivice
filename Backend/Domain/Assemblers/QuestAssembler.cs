using Backend.Domain.Models;
using Backend.Domain.Models.Quests;
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
                Requisites = resource.Requisites.Select(r => new Requisite
                {
                    Id = r.Id,
                    Value = r.Value
                }).ToList(),
                Steps = resource.Steps.Select(s => new Step
                {
                    Number = s.Number,
                    Value = s.Value,
                    Requisites = s.Requisites.Select(r => new Requisite
                    {
                        Id = r.Id,
                        Value = r.Value
                    }).ToList()
                }).ToList()
            };
        }
    }
}
