import type { ComputedRef, InjectionKey } from "vue";

export type JournalSectionAccentColor = "cyan" | "teal" | "sky";

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
    teal: {
        sectionTitleClass: "text-teal-400",
        sectionBorderClass: "border-teal-800",
        sectionChevronClass: "text-teal-400",
        sectionHeaderHoverClass: "hover:bg-teal-900/30",
        stepNumberClass: "text-teal-600",
        questTitleClass: "text-teal-400",
        questTitleHoverClass: "group-hover:text-teal-400",
    },
    sky: {
        sectionTitleClass: "text-sky-400",
        sectionBorderClass: "border-sky-800",
        sectionChevronClass: "text-sky-400",
        sectionHeaderHoverClass: "hover:bg-sky-900/30",
        stepNumberClass: "text-sky-600",
        questTitleClass: "text-sky-400",
        questTitleHoverClass: "group-hover:text-sky-400",
    },
};

export function getJournalSectionPalette(accentColor: JournalSectionAccentColor): JournalSectionPalette {
    return JOURNAL_SECTION_PALETTES[accentColor];
}

export const journalSectionPaletteKey: InjectionKey<ComputedRef<JournalSectionPalette>> = Symbol("journalSectionPalette");
