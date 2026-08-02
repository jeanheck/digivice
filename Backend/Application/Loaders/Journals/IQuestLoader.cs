using Backend.Memory.Resources.Journals;

namespace Backend.Application.Loaders.Journals
{
    public interface IQuestLoader
    {
        QuestResource LoadMainQuest();
        List<QuestResource> LoadSideQuests();
        List<QuestResource> LoadLegendaryWeapons();
        List<QuestResource> LoadDriAgents();
    }
}
