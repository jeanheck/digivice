using Backend.Events.DTO;
using Backend.Events.Converters.Journal;

namespace Backend.Events.Converters;

public static class JournalConverter
{
    public static JournalDTO ToDTO(Backend.Domain.Models.Journal journal)
    {
        return new JournalDTO
        {
            MainQuest = QuestConverter.ToDTO(journal.MainQuest),
            SideQuests = journal.SideQuests.Select(QuestConverter.ToDTO).ToList()
        };
    }
}
