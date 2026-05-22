import type { Technique, TechniqueType, TechniqueTypeId } from '@/models';
import TechniquesTableData from '../database/TechniquesTable.json';
import TechniquesTypeTableData from '../database/TechniquesTypeTable.json';

export class TechniqueRepository {
    private static techniquesMap = new Map<string, Technique>();
    private static techniquesTypeMap = new Map<string, TechniqueType>();

    static {
        this.initializeTechniqueTypes();
        this.initializeTechniques();
    }

    private static initializeTechniqueTypes(): void {
        for (const typeData of TechniquesTypeTableData.Types) {
            const resolved: TechniqueType = {
                id: typeData.Id as TechniqueTypeId,
                description: typeData.Description
            };
            this.techniquesTypeMap.set(resolved.id, resolved);
        }
    }

    private static initializeTechniques(): void {
        for (const techniqueData of TechniquesTableData.techniques) {
            const resolvedType = this.techniquesTypeMap.get(techniqueData.type) ?? {
                id: techniqueData.type as TechniqueTypeId,
                description: {}
            };

            const resolved: Technique = {
                id: techniqueData.id,
                name: techniqueData.name,
                type: resolvedType,
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
    }
}
