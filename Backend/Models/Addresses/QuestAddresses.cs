using System.Collections.Generic;
using Backend.Models.Quests;

namespace Backend.Models.Addresses
{
    public class QuestAddresses
    {
        public List<QuestAddressStep> Steps { get; set; } = new List<QuestAddressStep>();
    }
}
