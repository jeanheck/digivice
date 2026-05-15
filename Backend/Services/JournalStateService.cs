using Backend.Models;
using Backend.Models.Quests;
using Backend.Interfaces;

namespace Backend.Services
{
    public class JournalStateService(
        IGameDatabase gameDatabase,
        IGameReader gameReader)
    {
        public Journal GetJournal()
        {
            return new()
            {
                MainQuest = (MainQuest)ProcessQuest(CloneQuest(gameDatabase.GetMainQuest())),
                SideQuests = gameDatabase.GetAllSideQuests()
                    .Select(sideQuest => (SideQuest)ProcessQuest(CloneQuest(sideQuest)))
                    .ToList()
            };
        }

        private Quest ProcessQuest(Quest quest)
        {
            var questStepsState = gameReader.ReadQuestSteps(quest);

            // Evaluate Step Completion (Bitmasks + Cascade)
            ApplyQuestStepsLogic(quest, questStepsState);

            return quest;
        }

        private static T CloneQuest<T>(T template) where T : Quest
        {
            // Create a shallow copy of the record
            var cloned = template with { };

            // Deep copy lists to ensure mutations don't affect the cached template
            cloned.Requisites = template.Requisites.Select(r => r with { }).ToList();
            cloned.Steps = template.Steps.Select(s => s with
            {
                Requisites = s.Requisites?.Select(r => r with { }).ToList()
            }).ToList();

            return cloned;
        }

        private static void ApplyQuestStepsLogic(Quest quest, Dictionary<int, byte> questStepsState)
        {
            foreach (var step in quest.Steps)
            {
                if (!questStepsState.TryGetValue(step.Number, out byte state))
                {
                    continue;
                }

                if (string.IsNullOrEmpty(step.BitMask))
                {
                    // Legacy mode: byte == 1 means completed
                    step.IsCompleted = state == 1;
                }
                else
                {
                    // Bitmask mode: parse hex string to int, then check if any bit in the mask is set
                    int maskValue = Convert.ToInt32(step.BitMask, 16);
                    step.IsCompleted = (state & maskValue) != 0;
                }
            }

            // Completion cascade: If the next step is completed, the current step must also be completed.
            for (int i = quest.Steps.Count - 2; i >= 0; i--)
            {
                if (!quest.Steps[i].IsCompleted && quest.Steps[i + 1].IsCompleted)
                {
                    quest.Steps[i].IsCompleted = true;
                }
            }
        }
    }
}
