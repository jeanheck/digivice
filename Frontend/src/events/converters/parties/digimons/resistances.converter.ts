import type { ResistancesDTO } from "@/events/dto/parties/digimons/resistances.dto";
import type { Resistances } from "@/models/party/digimon/resistances";

export class ResistancesConverter {
    public static convert(newResistancesDto: ResistancesDTO | null): Resistances {
        return {
            fire: newResistancesDto?.fire ?? 0,
            water: newResistancesDto?.water ?? 0,
            ice: newResistancesDto?.ice ?? 0,
            wind: newResistancesDto?.wind ?? 0,
            thunder: newResistancesDto?.thunder ?? 0,
            machine: newResistancesDto?.machine ?? 0,
            dark: newResistancesDto?.dark ?? 0
        };
    }
}
