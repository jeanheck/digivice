using Backend.Domain.Assemblers.Journals;
using Backend.Domain.Models;
using Backend.Domain.Models.Journals;
using Backend.Memory.Resources;

namespace Backend.Domain.Assemblers
{
    public static class JournalAssembler
    {
        public static Journal Assemble(JournalResource resource)
        {
            var mainQuest = QuestAssembler.Assemble(resource.MainQuest);
            List<Quest> sideQuests = [.. resource.SideQuests.Select(QuestAssembler.Assemble)];

            NormalizeMainQuestProgression(mainQuest);

            return new Journal
            {
                MainQuest = mainQuest,
                SideQuests = sideQuests
            };
        }

        private static void NormalizeMainQuestProgression(Quest mainQuest)
        {
            // Completion cascade: If the next step is completed (> 0), 
            // the current step must also be considered completed.
            for (int i = mainQuest.Steps.Count - 2; i >= 0; i--)
            {
                if (mainQuest.Steps[i].Value == 0 && mainQuest.Steps[i + 1].Value > 0)
                {
                    mainQuest.Steps[i].Value = 1;
                }
            }
        }
    }
}
