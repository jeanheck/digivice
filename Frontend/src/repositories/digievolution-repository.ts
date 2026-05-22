import type { EnrichedDigievolution } from '../models';
import type { Attributes } from '../models/attributes';
import type { Resistances } from '../models/resistances';
import DigievolutionData from '../database/Digievolution.json';
import { TechniqueCalculator } from '../logic/TechniqueCalculator';

export class DigievolutionRepository {
    private static _digievolutionNameById = new Map<number, string>();
    private static _isInitialized = false;

    static {
        this.initializeRegistry();
    }

    private static initializeRegistry(): void {
        if (this._isInitialized) {
            return;
        }

        for (const digievolution of DigievolutionData.digievolutions) {
            if (digievolution.id !== null && digievolution.id !== undefined) {
                this._digievolutionNameById.set(digievolution.id, digievolution.name);
            }
        }

        this._isInitialized = true;
    }

    public static getDigievolutionNameById(id: number): string {
        return this._digievolutionNameById.get(id) ?? `Unknown (${id})`;
    }

    public static getEnrichedDigievolution(id: number, level: number): EnrichedDigievolution | null {
        const entry = DigievolutionData.digievolutions.find((d) => {
            return d.id === id;
        });

        if (!entry) {
            return null;
        }

        const mappedAttributes: Attributes = {
            strength: entry.Attributes?.Strength ?? 0,
            defense: entry.Attributes?.Defense ?? 0,
            spirit: entry.Attributes?.Spirit ?? 0,
            wisdom: entry.Attributes?.Wisdom ?? 0,
            speed: entry.Attributes?.Speed ?? 0,
            charisma: entry.Attributes?.Charisma ?? 0
        };

        const mappedResistances: Resistances = {
            fire: entry.Resistances?.Fire ?? 0,
            water: entry.Resistances?.Water ?? 0,
            ice: entry.Resistances?.Ice ?? 0,
            wind: entry.Resistances?.Wind ?? 0,
            thunder: entry.Resistances?.Thunder ?? 0,
            machine: entry.Resistances?.Machine ?? 0,
            dark: entry.Resistances?.Dark ?? 0
        };

        const resolvedTechniques = TechniqueCalculator.getTechniquesForDigievolution(id, level);

        return {
            id: entry.id,
            name: entry.name,
            level: level,
            attributes: mappedAttributes,
            resistances: mappedResistances,
            techniques: resolvedTechniques
        };
    }
}
