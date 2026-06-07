import type { Quest } from './quest';

export interface Journal {
    mainQuest: Quest | null;
    sideQuests: Quest[];
    legendaryWeapons: Quest[];
}
