namespace Backend.Constants
{
    public static class LocationAddress
    {
        // User provided address 1F70C64DA00. This is a 64-bit absolute address space pointer.
        public const long CurrentMapName = 0x1F70C64DA00;

        // Let's assume map name has max ~32 chars until we hit the null terminator
        public const int MapNameBufferSize = 32;
    }
}
