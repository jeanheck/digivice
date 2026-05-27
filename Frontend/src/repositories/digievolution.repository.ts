import DigievolutionJson from "@/database/digievolution/digievolution.json";
import DigievolutionNameJson from "@/database/digievolution/digievolution-name.json";
import DigievolutionTechniquesJson from "@/database/digievolution/digievolution-technique.json";
import DigievolutionTreeJson from "@/database/digievolution/digievolution-tree.json";
import TechniqueJson from "@/database/digievolution/technique.json";
import type { DigievolutionTable } from "@/repositories/tables/digievolution/digievolution-table";
import type { DigievolutionNameTable } from "@/repositories/tables/digievolution/digievolution-name-table";
import type { DigievolutionTechniqueTable } from "@/repositories/tables/digievolution/digievolution-technique-table";
import type { DigievolutionTreeTable } from "@/repositories/tables/digievolution/digievolution-tree-table";
import type { TechniqueTable } from "@/repositories/tables/digievolution/technique-table";
import type { DigievolutionRaw } from "./tables/raws/digievolution/digievolution-raw";
import type { DigievolutionTechniqueRaw } from "./tables/raws/digievolution/digievolution-technique-raw";
import type { DigievolutionTreeRaw } from "./tables/raws/digievolution/digievolution-tree-raw";
import type { TechniqueRaw } from "./tables/raws/digievolution/technique-raw";

export class DigievolutionRepository {
    private static readonly digievolutionTable = DigievolutionJson as DigievolutionTable;
    private static readonly digievolutionNameTable = DigievolutionNameJson as DigievolutionNameTable;
    private static readonly digievolutionTechniqueTable = DigievolutionTechniquesJson as DigievolutionTechniqueTable;
    private static readonly digievolutionTreeTable = DigievolutionTreeJson as DigievolutionTreeTable;
    private static readonly techniqueTable = TechniqueJson as TechniqueTable;

    public static getNameById(id: number): string {
        return this.digievolutionNameTable[id]!;
    }
    public static getIdByName(name: string): number {
        const entry = Object.entries(this.digievolutionNameTable).find(([, digievolutionName]) => digievolutionName === name);

        return Number(entry![0]);
    }
    public static getRawDigievolutionById(id: number): DigievolutionRaw {
        return this.digievolutionTable[id]!;
    }
    public static getRawDigievolutionTechniquesById(id: number): DigievolutionTechniqueRaw[] {
        return this.digievolutionTechniqueTable[id]!;
    }
    public static getRawDigievolutionTechniquesByName(digievolutionName: string): DigievolutionTechniqueRaw[] {
        const digievolutionId = this.getIdByName(digievolutionName);
        return this.digievolutionTechniqueTable[digievolutionId]!;
    }
    public static getDigievolutionTreeByFamily(family: string): DigievolutionTreeRaw[] {
        return this.digievolutionTreeTable[family]!;
    }
    public static getDigievolutionTree(): DigievolutionTreeTable {
        return this.digievolutionTreeTable;
    }
    public static getTechniqueById(id: string): TechniqueRaw {
        return this.techniqueTable[id]!;
    }
}
