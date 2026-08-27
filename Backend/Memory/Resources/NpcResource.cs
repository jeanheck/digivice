namespace Backend.Memory.Resources
{
    public class NpcResource
    {
        public string Id { get; set; } = string.Empty;
        public List<NpcBattleResource> DigimonBattles { get; set; } = [];
    }
}
