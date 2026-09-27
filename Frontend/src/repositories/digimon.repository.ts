import DigimonJson from "@/database/digimon.json";
import type { DigimonTable } from "@/repositories/tables/digimon/digimon.table";
import type { DigimonDigievolutionRaw } from "./tables/raws/digimon/digimon-digievolution.raw";
import type { DigimonDigievolutionRequirementRaw } from "./tables/raws/digimon/digimon-digievolution-requirement.raw";

export class DigimonRepository {
  private static readonly digimonTable = DigimonJson as DigimonTable;

  public static getRequiredExperienceForLevel(id: number, level: number): number {
    return this.digimonTable[id]!.experience[String(level)]!;
  }
  public static getNameById(id: number): string {
    return this.digimonTable[id]!.name;
  }
  public static getDigievolutionsById(id: number): DigimonDigievolutionRaw {
    return this.digimonTable[id]!.digievolutions;
  }
  public static getDigievolutionRequirements(
    digimonId: number,
    digievolutionId: number,
  ): DigimonDigievolutionRequirementRaw[] {
    return this.digimonTable[digimonId]!.digievolutions[String(digievolutionId)]!;
  }
}
