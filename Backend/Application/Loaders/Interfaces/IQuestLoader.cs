using Backend.Memory.Resources.Journals;

namespace Backend.Application.Loaders.Interfaces
{
    public interface IQuestLoader
    {
        QuestResource LoadMainQuest();
        List<QuestResource> LoadSideQuests();
        List<QuestResource> LoadLegendaryWeapons();
        List<QuestResource> LoadDriAgents();
        List<QuestResource> LoadDuelIsland();
    }
}
