import DnaDigievolutionJson from "@/data/game/dna-digievolution.json";
import type { DnaDigievolutionTable } from "@/presentation/repositories/tables/digievolution/dna-digievolution.table";
import type { DnaDigievolutionRaw } from "@/presentation/repositories/tables/raws/digievolution/dna-digievolution.raw";

export class DnaDigievolutionRepository {
  private static readonly dnaDigievolutionTable = DnaDigievolutionJson as DnaDigievolutionTable;

  public static getById(id: string): DnaDigievolutionRaw {
    return this.dnaDigievolutionTable[id]!;
  }
  public static getAll(): { id: string; dnaDigievolution: DnaDigievolutionRaw }[] {
    return Object.entries(this.dnaDigievolutionTable).map(([id, dnaDigievolution]) => {
      return { id, dnaDigievolution };
    });
  }
}
