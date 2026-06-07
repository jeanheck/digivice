import type { ComputedRef, InjectionKey } from "vue";

export type JournalSectionAccentColor = "cyan" | "purple" | "rose" | "emerald" | "teal" | "sky";

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
    purple: {
        sectionTitleClass: "text-purple-400",
        sectionBorderClass: "border-purple-800",
        sectionChevronClass: "text-purple-400",
        sectionHeaderHoverClass: "hover:bg-purple-900/30",
        stepNumberClass: "text-purple-600",
        questTitleClass: "text-purple-400",
        questTitleHoverClass: "group-hover:text-purple-400",
    },
    rose: {
        sectionTitleClass: "text-rose-400",
        sectionBorderClass: "border-rose-800",
        sectionChevronClass: "text-rose-400",
        sectionHeaderHoverClass: "hover:bg-rose-900/30",
        stepNumberClass: "text-rose-600",
        questTitleClass: "text-rose-400",
        questTitleHoverClass: "group-hover:text-rose-400",
    },
    emerald: {
        sectionTitleClass: "text-emerald-400",
        sectionBorderClass: "border-emerald-800",
        sectionChevronClass: "text-emerald-400",
        sectionHeaderHoverClass: "hover:bg-emerald-900/30",
        stepNumberClass: "text-emerald-600",
        questTitleClass: "text-emerald-400",
        questTitleHoverClass: "group-hover:text-emerald-400",
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
