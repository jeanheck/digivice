namespace Digivice.Archival
{
    /* 
    Backup da lógica de rastreamento do Kotemon na ROM PT-BR que estava no Program.cs.
    Estes offsets foram mapeados com base no endereço: 0x494B4.
    
    // O Ponto Zero definitivo do Kotemon na sua ROM PT-BR!
    int baseAddr = 0x494B4;

    while (true)
    {
        try
        {
            // Lendo com o novo alinhamento perfeito
            int xp = reader.ReadInt32(baseAddr);        // +0
            short lvl = reader.ReadInt16(baseAddr + 0x4);  // +4
            short curHp = reader.ReadInt16(baseAddr + 0x8);  // +8
            short maxHp = reader.ReadInt16(baseAddr + 0xA);  // +A
            short curMp = reader.ReadInt16(baseAddr + 0xC);  // +C
            short maxMp = reader.ReadInt16(baseAddr + 0xE);  // +E

            Console.Clear();
            Console.WriteLine("================================================");
            Console.WriteLine("     DIGI-TRACKER - KOTEMON (ROM PT-BR)         ");
            Console.WriteLine("================================================");
            Console.WriteLine($" Endereço Base: 0x{baseAddr:X}                ");
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine($" NÍVEL:       {lvl}");
            Console.WriteLine($" XP TOTAL:    {xp}");
            Console.WriteLine($" HP:          {curHp} / {maxHp}");
            Console.WriteLine($" MP:          {curMp} / {maxMp}");
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("  Vá lutar e veja o HP/XP mudar em tempo real!  ");
            Console.WriteLine("================================================");

            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q) break;
        }
        catch (Exception)
        {
            // Ignora erros momentâneos de leitura
        }

        Thread.Sleep(200); // Roda bem rápido para ser instantâneo
    }
    */
}
