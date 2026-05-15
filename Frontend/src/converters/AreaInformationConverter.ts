import type { AreaInformation } from '../models/AreaInformation';
import type { Enemy } from '../models/Enemy';
import { LocationCalculator } from '../logic/LocationCalculator';
import EnemiesData from '../database/EnemiesTable.json';

export class AreaInformationConverter {
    public static convert(mapId: string | undefined | null): AreaInformation | null {
        if (!mapId) return null;

        const locationEntry = LocationCalculator.getFromMapId(mapId);
        if (!locationEntry) return null;

        const locationTag = locationEntry.location;
        const enemies: Enemy[] = [];

        if (locationTag && Array.isArray(EnemiesData)) {
            // Filtra os inimigos que aparecem neste Location
            const matchingEnemies = EnemiesData.filter(e =>
                e.Location && Array.isArray(e.Location) && e.Location.includes(locationTag)
            );

            // Mapeia do PascalCase do banco para o camelCase do modelo
            for (const e of matchingEnemies) {
                enemies.push({
                    id: e.Id,
                    name: e.Name,
                    level: e.Level,
                    hp: e.HP,
                    mp: e.MP,
                    species: e.Species,
                    itemHeld: null,
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
                    location: e.Location,
                    regularAttack: e.RegularAttack ?? null,
                    technique: e.Technique ?? null,
                    boss: e.Boss ?? false
                });
            }
        }

        return {
            location: locationEntry,
            enemies: enemies
        };
    }
}
