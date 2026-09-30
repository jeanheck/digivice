import DigievolutionJson from "@/database/digievolution/digievolution.json";
import DigievolutionTreeJson from "@/database/digievolution/digievolution-tree.json";
import TechniqueJson from "@/database/digievolution/technique.json";
import type { DigievolutionTable } from "@/repositories/tables/digievolution/digievolution.table";
import type { DigievolutionTreeTable } from "@/repositories/tables/digievolution/digievolution-tree.table";
import type { TechniqueTable } from "@/repositories/tables/digievolution/technique.table";
import type { DigievolutionRaw } from "./tables/raws/digievolution/digievolution.raw";
import type { DigievolutionTechniqueRaw } from "./tables/raws/digievolution/digievolution-technique.raw";
import type { TechniqueRaw } from "./tables/raws/digievolution/technique.raw";
import type { DigievolutionTier } from "@/types/digievolution-tier.type";

export class DigievolutionRepository {
  private static readonly digievolutionTable = DigievolutionJson as DigievolutionTable;
  private static readonly digievolutionTreeTable = DigievolutionTreeJson as DigievolutionTreeTable;
  private static readonly techniqueTable = TechniqueJson as TechniqueTable;

  public static getNameById(id: number): string {
    return this.digievolutionTable[String(id)]!.name;
  }
  public static getTierById(id: number): DigievolutionTier {
    return this.digievolutionTable[String(id)]!.tier;
  }
  public static getAllDigievolutionsNames(): string[] {
    return Object.values(this.digievolutionTable).map((digievolution) => digievolution.name);
  }
  public static getAllDigievolutions(): { id: number; name: string }[] {
    return Object.entries(this.digievolutionTable).map(([id, digievolution]) => {
      return {
        id: Number(id),
        name: digievolution.name,
      };
    });
  }
  public static getIdByName(name: string): number {
    const entry = Object.entries(this.digievolutionTable).find(
      ([, digievolution]) => digievolution.name === name,
    );

    return Number(entry![0]);
  }
  public static getRawDigievolutionById(id: number): DigievolutionRaw {
    return this.digievolutionTable[id]!;
  }
  public static getRawDigievolutionTechniquesById(id: number): DigievolutionTechniqueRaw[] {
    return this.digievolutionTable[id]!.techniques;
  }
  public static getDigievolutionTree(): DigievolutionTreeTable {
    return this.digievolutionTreeTable;
  }
  public static getTechniqueById(id: string): TechniqueRaw {
    return this.techniqueTable[id]!;
  }
}
