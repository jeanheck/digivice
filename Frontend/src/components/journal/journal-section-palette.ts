import type { ComputedRef, InjectionKey } from "vue";

export type JournalSectionAccentColor = "cyan" | "red";

export interface JournalSectionPalette {
    sectionTitleClass: string;
    sectionBorderClass: string;
    sectionChevronClass: string;
    sectionHeaderHoverClass: string;
    stepNumberClass: string;
    questTitleClass: string;
    questTitleHoverClass: string;
}

const JOURNAL_SECTION_PALETTES: Record<JournalSectionAccentColor, JournalSectionPalette> = {
    cyan: {
        sectionTitleClass: "text-cyan-400",
        sectionBorderClass: "border-cyan-800",
        sectionChevronClass: "text-cyan-400",
        sectionHeaderHoverClass: "hover:bg-cyan-900/30",
        stepNumberClass: "text-cyan-600",
        questTitleClass: "text-cyan-400",
        questTitleHoverClass: "group-hover:text-cyan-400",
    },
    red: {
        sectionTitleClass: "text-red-400",
        sectionBorderClass: "border-red-800",
        sectionChevronClass: "text-red-400",
        sectionHeaderHoverClass: "hover:bg-red-900/30",
        stepNumberClass: "text-red-600",
        questTitleClass: "text-red-400",
        questTitleHoverClass: "group-hover:text-red-400",
    },
};

export function getJournalSectionPalette(accentColor: JournalSectionAccentColor): JournalSectionPalette {
    return JOURNAL_SECTION_PALETTES[accentColor];
}

export const journalSectionPaletteKey: InjectionKey<ComputedRef<JournalSectionPalette>> = Symbol("journalSectionPalette");
