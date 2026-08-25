namespace Backend.Memory.Addresses
{
    public class NpcAddresses
    {
        public Dictionary<string, NpcBattleAddresses> DigimonBattles { get; set; } = [];
        public Dictionary<string, NpcBattleAddresses> CardBattles { get; set; } = [];
    }
}
