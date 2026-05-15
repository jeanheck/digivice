import TechniquesTable from '../database/TechniquesTable.json';
import DigievolutionTechniques from '../database/DigievolutionTechniques.json';
import TechniquesTypeTable from '../database/TechniquesTypeTable.json';
import type { Technique, TechniqueType } from '../models/Digimon';
import { DigievolutionRegistry } from './DigievolutionRegistry';

export class TechniqueCalculator {
    private static techniqueTypesMap: Map<string, TechniqueType> | null = null;

    private static getTechniqueType(id: string): TechniqueType {
        if (!this.techniqueTypesMap) {
            this.techniqueTypesMap = new Map();
            TechniquesTypeTable.Types.forEach(t => {
                this.techniqueTypesMap!.set(t.Id, {
                    id: t.Id,
                    description: t.Description
                });
            });
        }
        return this.techniqueTypesMap.get(id) ?? { id: id, description: {} };
    }

    public static getTechniquesForDigievolution(digievolutionId: number, currentLevel: number): Technique[] {
        const digievolutionName = DigievolutionRegistry.getDigievolutionNameById(digievolutionId);
        
        const entry = DigievolutionTechniques.digievolutions.find(d => d.name === digievolutionName);
        if (!entry) return [];

        const maxLearnLevel = Math.max(...entry.techniques.map(t => t.learnLevel));

        return entry.techniques.map(t => {
            const baseTech = TechniquesTable.techniques.find(tech => tech.id === t.techniqueId);
            if (!baseTech) return null;

            return {
                id: baseTech.id,
                name: baseTech.name,
                type: this.getTechniqueType(baseTech.type),
                element: baseTech.element,
                elementStrength: baseTech.elementStrength,
                mp: baseTech.mp,
                power: baseTech.power,
                description: baseTech.description,
                learnLevel: t.learnLevel,
                isSignature: t.learnLevel === maxLearnLevel,
                isUnlocked: currentLevel >= t.learnLevel
            } as Technique;
        }).filter((t): t is Technique => t !== null);
    }
}
