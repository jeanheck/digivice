using Backend.Domain.Models;
using Backend.Events.DTO;
using Backend.Events.Converters.Journals;

namespace Backend.Events.Converters;

public static class JournalConverter
{
    public static JournalDTO ToDTO(Journal journal)
    {
        return new JournalDTO
        {
            MainQuest = QuestConverter.ToDTO(journal.MainQuest),
            SideQuests = journal.SideQuests.Select(QuestConverter.ToDTO).ToList(),
            LegendaryWeapons = journal.LegendaryWeapons.Select(QuestConverter.ToDTO).ToList()
        };
    }
}
