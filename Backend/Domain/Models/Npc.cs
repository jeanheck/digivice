using Backend.Domain.Models.Npcs;

namespace Backend.Domain.Models
{
    public record class Npc
    {
        public string Id { get; set; } = string.Empty;
        public List<NpcBattle> DigimonBattles { get; set; } = [];

        public virtual bool Equals(Npc? other)
        {
            if (other is null)
            {
                return false;
            }

            bool digimonBattlesEqual = (DigimonBattles == null && other.DigimonBattles == null) ||
                (DigimonBattles != null && other.DigimonBattles != null &&
                 DigimonBattles.SequenceEqual(other.DigimonBattles));

            return Id == other.Id && digimonBattlesEqual;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Id);
            if (DigimonBattles != null)
            {
                foreach (var digimonBattle in DigimonBattles)
                {
                    hash.Add(digimonBattle);
                }
            }
            return hash.ToHashCode();
        }
    }
}
