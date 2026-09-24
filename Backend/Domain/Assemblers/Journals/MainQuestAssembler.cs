using Backend.Domain.Models.Journals;
using Backend.Memory.Resources.Journals;

namespace Backend.Domain.Assemblers.Journals
{
    public static class MainQuestAssembler
    {
        public static Quest Assemble(QuestResource resource)
        {
            Quest mainQuest = QuestAssembler.Assemble(resource);

            // Completion cascade: if a later step is done, earlier unfinished steps must also be done.
            for (int i = mainQuest.Steps.Count - 2; i >= 0; i--)
            {
                if (!mainQuest.Steps[i].IsDone && mainQuest.Steps[i + 1].IsDone)
                {
                    mainQuest.Steps[i].IsDone = true;
                }
            }

            return mainQuest;
        }
    }
}
