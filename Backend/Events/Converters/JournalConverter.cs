using System.Linq;
using Backend.Domain.Models;
using Backend.Events.DTO;

namespace Backend.Events.Converters;

public static class JournalConverter
{
    public static JournalDTO ToDTO(Journal journal)
    {
        return new JournalDTO
        {
            MainQuest = journal.MainQuest != null ? QuestConverter.ToDTO(journal.MainQuest) : null,
            SideQuests = journal.SideQuests.Select(QuestConverter.ToDTO).ToList()
        };
    }
}
