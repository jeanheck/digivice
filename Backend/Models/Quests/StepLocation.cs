namespace Backend.Models.Quests
{
    public class StepLocation
    {
        public string? LocationImage { get; set; }
        public string? Target { get; set; }
        public MapCoordinates? LocationImageCoordinates { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not StepLocation other) return false;

            bool locImgCoordEqual = (LocationImageCoordinates == null && other.LocationImageCoordinates == null) ||
                                    (LocationImageCoordinates != null && LocationImageCoordinates.Equals(other.LocationImageCoordinates));

            return LocationImage == other.LocationImage &&
                   Target == other.Target &&
                   locImgCoordEqual;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(LocationImage);
            hash.Add(Target);
            if (LocationImageCoordinates != null) hash.Add(LocationImageCoordinates);
            return hash.ToHashCode();
        }
    }
}
