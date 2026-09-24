using Backend.Domain.Models.Journals;
using Backend.Domain.Models.Journals.Quests;
using Backend.Memory.Resources.Journals;

namespace Backend.Domain.Assemblers.Journals
{
    public static class DuelIslandAssembler
    {
        public static Quest Assemble(QuestResource resource)
        {
            Quest quest = QuestAssembler.Assemble(resource);
            NormalizeProgression(quest);
            return quest;
        }

        private static void NormalizeProgression(Quest quest)
        {
            if (quest.Steps.Count == 0)
            {
                return;
            }

            if (!quest.Requisites.All(requisite => requisite.IsDone))
            {
                foreach (Step step in quest.Steps)
                {
                    step.IsDone = false;
                }

                return;
            }

            Step trophyStep = quest.Steps[^1];
            if (!trophyStep.IsDone)
            {
                return;
            }

            for (int i = 0; i < quest.Steps.Count - 1; i++)
            {
                if (!quest.Steps[i].IsDone)
                {
                    quest.Steps[i].IsDone = true;
                }
            }
        }
    }
}
