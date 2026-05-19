using Backend.Domain.Models.Journals;
using Backend.Events.DTO.Journal;

using Backend.Events.Converters.Journals.Quests;

namespace Backend.Events.Converters.Journals;

public static class QuestConverter
{
    public static QuestDTO ToDTO(Quest quest) => new()
    {
        Id = quest.Id,
        Requisites = quest.Requisites.Select(RequisiteConverter.ToDTO).ToList(),
        Steps = quest.Steps.Select(QuestStepConverter.ToDTO).ToList()
    };
}
