namespace Backend.Constants
{
    public static class LocationAddress
    {
        // True Shared Memory PS1 Offset discovered via intersection scanner
        public const long CurrentMapName = 0x000ACBF4;

        // Let's assume map name has max ~32 chars until we hit the null terminator
        public const int MapNameBufferSize = 32;
    }
}
