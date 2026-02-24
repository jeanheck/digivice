namespace Backend.Constants
{
    internal enum DigimonIds : byte
    {
        Kotemon = 0,
        Kumamon = 1,
        Monmon = 2,
        Agumon = 3,
        Veemon = 4,
        Guilmon = 5,
        Renamon = 6,
        Patamon = 7
    }

    internal static class DigimonAddresses
    {
        public const byte EmptySlotId = 0xFF;

        public static readonly Dictionary<byte, (string Name, int Address)> Digimons = new()
        {
            { (byte)DigimonIds.Kotemon, (nameof(DigimonIds.Kotemon), 0x0004949C) },
            { (byte)DigimonIds.Kumamon, (nameof(DigimonIds.Kumamon), 0x00049878) },
            { (byte)DigimonIds.Monmon,  (nameof(DigimonIds.Monmon),  0x00049C54) },
            { (byte)DigimonIds.Agumon,  (nameof(DigimonIds.Agumon),  0x0004A030) },
            { (byte)DigimonIds.Veemon,  (nameof(DigimonIds.Veemon),  0x0004A40C) },
            { (byte)DigimonIds.Guilmon, (nameof(DigimonIds.Guilmon), 0x0004A7E8) },
            { (byte)DigimonIds.Renamon, (nameof(DigimonIds.Renamon), 0x0004ABC4) },
            { (byte)DigimonIds.Patamon, (nameof(DigimonIds.Patamon), 0x0004AFA0) }
        };

        public static class BasicInfo
        {
            public const int Experience = 0x18;
            public const int Level = 0x1C;
            public const int CurrentHP = 0x20;
            public const int MaxHP = 0x22;
            public const int CurrentMP = 0x24;
            public const int MaxMP = 0x26;
        }

        public static class Attributes
        {
            public const int Strength = 0x28;
            public const int Defense = 0x2A;
            public const int Spirit = 0x2C;
            public const int Wisdom = 0x2E;
            public const int Speed = 0x30;
            public const int Charisma = 0x32;
        }

        public static class Resistances
        {
            public const int Fire = 0x34;
            public const int Water = 0x36;
            public const int Ice = 0x38;
            public const int Wind = 0x3A;
            public const int Thunder = 0x3C;
            public const int Machine = 0x3E;
            public const int Dark = 0x40;
        }

        public static class Equipments
        {
            public const int Head = 0x3C0;
            public const int Body = 0x3C2;
            public const int RightHand = 0x3C4;
            public const int LeftHand = 0x3C6;
            public const int Accessory1 = 0x3C8;
            public const int Accessory2 = 0x3CA;
        }
    }
}
