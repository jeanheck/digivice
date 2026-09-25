import type { Digimon } from "@/models";
import type { DeepRequired } from "@/events/dto/deep-required";
import type { DigimonDTO } from "@/events/dto/parties/digimon.dto";
import { VitalConverter } from "./digimons/vital.converter";
import { InBattleConverter } from "./digimons/in-battle.converter";
import { AttributesConverter } from "./digimons/attributes.converter";
import { ResistancesConverter } from "./digimons/resistances.converter";
import { EquipmentsConverter } from "./digimons/equipments.converter";
import { DigievolutionSlotConverter } from "./digimons/digievolution-slot.converter";
import { StoredDigievolutionConverter } from "./digimons/stored-digievolution.converter";

export class DigimonConverter {
  public static convert(digimonDto: DeepRequired<DigimonDTO>): Digimon {
    return {
      level: digimonDto.level,
      tp: digimonDto.tp,
      blast: digimonDto.blast,
      experience: digimonDto.experience,
      activeDigievolutionId: digimonDto.activeDigievolutionId,
      hp: VitalConverter.convert(digimonDto.hp),
      mp: VitalConverter.convert(digimonDto.mp),
      inBattle: InBattleConverter.convert(digimonDto.inBattle),
      attributes: AttributesConverter.convert(digimonDto.attributes),
      resistances: ResistancesConverter.convert(digimonDto.resistances),
      equipments: EquipmentsConverter.convert(digimonDto.equipments),
      digievolutions: digimonDto.digievolutions.map((digievolutionSlotDto) =>
        DigievolutionSlotConverter.convert(digievolutionSlotDto),
      ),
      storedDigievolutions: digimonDto.storedDigievolutions.map((storedDigievolutionDto) =>
        StoredDigievolutionConverter.convert(storedDigievolutionDto),
      ),
    };
  }
}
