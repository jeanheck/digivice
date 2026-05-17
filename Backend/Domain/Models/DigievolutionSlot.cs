using Backend.Domain.Models.Digimons;

namespace Backend.Domain.Models
{
    public class DigievolutionSlot
    {
        public int Index { get; set; }
        public int DigievolutionId { get; set; }
        public Digievolution? Digievolution { get; set; }
    }
}
