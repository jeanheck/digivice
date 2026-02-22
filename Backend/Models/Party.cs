using System.Collections.Generic;

namespace Backend.Models
{
    public class Party
    {
        public List<Digimon?> Slots { get; set; } = new() { null, null, null };
        public int ActiveSlotIndex { get; set; } = -1; // -1 significa nenhum ativo inicialmente
    }
}
