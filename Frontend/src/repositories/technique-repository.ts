import type { TechniqueTable } from '@/repositories/tables/technique-table';
import TechniqueJson from '@/tables/digievolution/technique.json';
import type { Technique, TechniqueType } from '@/models';

export class TechniqueRepository {
    private static readonly TechniqueTable = TechniqueJson as TechniqueTable;

    /*static {
        this.initializeTechniques();
    }

    private static initializeTechniques(): void {
        for (const techniqueData of TechniquesTableData.techniques) {
            const resolved: Technique = {
                id: techniqueData.id,
                name: techniqueData.name,
                type: techniqueData.type as TechniqueType,
                element: techniqueData.element,
                elementStrength: techniqueData.elementStrength,
                mp: techniqueData.mp,
                power: techniqueData.power,
                description: techniqueData.description,
                learnLevel: 0,
                isSignature: false,
                isUnlocked: false
            };

            this.techniquesMap.set(resolved.id, resolved);
        }
    }

    public static getTechniqueById(id: string): Technique | null {
        return this.techniquesMap.get(id) ?? null;
    }*/
}
