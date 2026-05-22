import type { EnrichedDigievolution, Technique } from '@/models';
import type { Attributes } from '@/models/attributes';
import type { Resistances } from '@/models/resistances';
import DigievolutionData from '../database/Digievolution.json';
import DigievolutionTechniquesData from '../database/DigievolutionTechniques.json';
import { TechniqueRepository } from './technique-repository';

export class DigievolutionRepository {
    private static digievolutionMap = new Map<number, any>();
    private static digievolutionTechniquesMap = new Map<string, any[]>();
    private static isInitialized = false;

    static {
        this.initializeRegistry();
    }

    private static initializeRegistry(): void {
        if (this.isInitialized) {
            return;
        }

        for (const digievolution of DigievolutionData.digievolutions) {
            if (digievolution.id !== null && digievolution.id !== undefined) {
                this.digievolutionMap.set(digievolution.id, digievolution);
            }
        }

        for (const digievolution of DigievolutionTechniquesData.digievolutions) {
            this.digievolutionTechniquesMap.set(digievolution.name, digievolution.techniques);
        }

        this.isInitialized = true;
    }

    public static getDigievolutionNameById(id: number): string {
        const digievolution = this.digievolutionMap.get(id);
        return digievolution ? digievolution.name : `Unknown (${id})`;
    }

    public static getEnrichedDigievolution(id: number, level: number): EnrichedDigievolution | null {
        const digievolution = this.digievolutionMap.get(id);
        if (!digievolution) {
            return null;
        }

        const mappedAttributes: Attributes = {
            strength: digievolution.Attributes?.Strength ?? 0,
            defense: digievolution.Attributes?.Defense ?? 0,
            spirit: digievolution.Attributes?.Spirit ?? 0,
            wisdom: digievolution.Attributes?.Wisdom ?? 0,
            speed: digievolution.Attributes?.Speed ?? 0,
            charisma: digievolution.Attributes?.Charisma ?? 0
        };

        const mappedResistances: Resistances = {
            fire: digievolution.Resistances?.Fire ?? 0,
            water: digievolution.Resistances?.Water ?? 0,
            ice: digievolution.Resistances?.Ice ?? 0,
            wind: digievolution.Resistances?.Wind ?? 0,
            thunder: digievolution.Resistances?.Thunder ?? 0,
            machine: digievolution.Resistances?.Machine ?? 0,
            dark: digievolution.Resistances?.Dark ?? 0
        };

        const techniquesData = this.digievolutionTechniquesMap.get(digievolution.name) ?? [];
        const maxLearnLevel = techniquesData.length > 0 ? Math.max(...techniquesData.map((t) => t.learnLevel)) : 0;

        const resolvedTechniques = techniquesData
            .map((t) => {
                const baseTech = TechniqueRepository.getTechniqueById(t.techniqueId);
                if (!baseTech) {
                    return null;
                }

                return {
                    ...baseTech,
                    learnLevel: t.learnLevel,
                    isSignature: t.learnLevel === maxLearnLevel,
                    isUnlocked: level >= t.learnLevel
                } as Technique;
            })
            .filter((t): t is Technique => t !== null);

        return {
            id: digievolution.id,
            name: digievolution.name,
            level: level,
            attributes: mappedAttributes,
            resistances: mappedResistances,
            techniques: resolvedTechniques
        };
    }
}
