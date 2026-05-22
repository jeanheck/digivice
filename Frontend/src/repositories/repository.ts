import type { Enemy } from '../models';

import DigievolutionTreeData from '../database/DigievolutionTree.json';
import DigivolvingRequirementsTableData from '../database/DigivolvingRequirementsTable.json';
import EnemiesTableData from '../database/EnemiesTable.json';
import FishingPoleTableData from '../database/FishingPoleTable.json';
import FolderBagTableData from '../database/FolderBagTable.json';
import LocationsData from '../database/Locations.json';
import MainQuestTableData from '../database/MainQuestTable.json';
import TechniquesTableData from '../database/TechniquesTable.json';
import TreeBootsTableData from '../database/TreeBootsTable.json';

export interface DbTechnique {
    id: string;
    name: Record<string, string>;
    type: string;
    typeDescription: Record<string, string> | null;
    element: string;
    elementStrength: number;
    mp: number;
    power: number;
    description: Record<string, string>;
}

export interface QuestStep {
    Number: number;
    Description: Record<string, string>;
    LocationOnMap?: Record<string, string>;
    LocationOnMapCoordinates?: { X: number; Y: number };
    Locations?: Array<{
        LocationImage: string;
        Target: Record<string, string>;
        LocationImageCoordinates: { X: number; Y: number };
    }>;
    Prerequisites?: Array<{ Id: string }>;
}

export interface QuestData {
    Id: string;
    Title: Record<string, string>;
    Description: Record<string, string>;
    Prerequisites?: Array<{ Description?: Record<string, string>; Id?: string }>;
    Steps: QuestStep[];
}

export class Repository {
    private static _enemies: Enemy[] = [];
    private static _enemiesMap = new Map<number, Enemy>();

    private static _techniques: DbTechnique[] = [];
    private static _techniquesMap = new Map<string, DbTechnique>();

    private static _mainQuest: QuestData = MainQuestTableData as QuestData;
    private static _sideQuests: QuestData[] = [
        FolderBagTableData as QuestData,
        FishingPoleTableData as QuestData,
        TreeBootsTableData as QuestData
    ];

    static {
        this.initializeEnemies();
    }

    private static initializeEnemies(): void {
        for (const e of EnemiesTableData) {
            const resolved: Enemy = {
                id: e.Id,
                name: e.Name,
                level: e.Level,
                hp: e.HP,
                mp: e.MP,
                species: e.Species,
                itemHeld: e.ItemHeld ? (typeof e.ItemHeld === 'string' ? e.ItemHeld : e.ItemHeld['EN-US'] || e.ItemHeld['PT-BR']) : null,
                rate: e.Rate,
                strength: e.Strength,
                defense: e.Defense,
                spirit: e.Spirit,
                wisdom: e.Wisdom,
                speed: e.Speed,
                fire: e.Fire,
                water: e.Water,
                ice: e.Ice,
                wind: e.Wind,
                thunder: e.Thunder,
                machine: e.Machine,
                dark: e.Dark,
                canPoison: e.CanPoison,
                poison: e.Poison,
                canParalyze: e.CanParalyze,
                paralyze: e.Paralyze,
                canConfuse: e.CanConfuse,
                confuse: e.Confuse,
                canSleep: e.CanSleep,
                sleep: e.Sleep,
                canKO: e.CanKO,
                ko: e.KO,
                canDrain: e.CanDrain,
                canSteal: e.CanSteal,
                strDown: e.STRDown,
                defDown: e.DEFDown,
                spdDown: e.SPDDown,
                canEscape: e.CanEscape,
                dvxp: e.DVXP,
                exp: e.EXP,
                bits: e.BITS,
                location: e.Location || [],
                regularAttack: e.RegularAttack || null,
                technique: e.Technique || null,
                boss: e.Boss || false
            };

            this._enemies.push(resolved);
            this._enemiesMap.set(resolved.id, resolved);
        }
    }

    public static get enemies(): Enemy[] {
        return this._enemies;
    }

    public static getEnemyById(id: number): Enemy | null {
        return this._enemiesMap.get(id) || null;
    }

    public static get techniques(): DbTechnique[] {
        return this._techniques;
    }

    public static getTechniqueById(id: string | null): DbTechnique | null {
        if (id === null || id === undefined) {
            return null;
        }
        return this._techniquesMap.get(id) || null;
    }

    public static get quests() {
        return {
            mainQuest: this._mainQuest,
            sideQuests: this._sideQuests
        };
    }

    public static get digievolutions() {
        return {
            tree: DigievolutionTreeData.digievolutions,
            requirements: DigivolvingRequirementsTableData
        };
    }

    public static get locations(): Record<string, any> {
        return LocationsData;
    }
}
