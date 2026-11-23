import type { ComputedRef, InjectionKey } from "vue";

export type JournalSectionAccentColor = "emerald" | "teal" | "cyan" | "sky";

export interface JournalSectionPalette {
  sectionTitleClass: string;
  sectionBorderClass: string;
  sectionChevronClass: string;
  sectionHeaderHoverClass: string;
  stepNumberClass: string;
  questTitleClass: string;
  questTitleHoverClass: string;
}

const JournalSectionPalettes: Record<JournalSectionAccentColor, JournalSectionPalette> = {
  emerald: {
    sectionTitleClass: "text-journal-emerald",
    sectionBorderClass: "border-journal-emerald-border",
    sectionChevronClass: "text-journal-emerald",
    sectionHeaderHoverClass: "hover:bg-journal-emerald-hover",
    stepNumberClass: "text-journal-emerald",
    questTitleClass: "text-journal-emerald",
    questTitleHoverClass: "group-hover:text-journal-emerald",
  },
  cyan: {
    sectionTitleClass: "text-journal-cyan",
    sectionBorderClass: "border-journal-cyan-border",
    sectionChevronClass: "text-journal-cyan",
    sectionHeaderHoverClass: "hover:bg-journal-cyan-hover",
    stepNumberClass: "text-journal-cyan",
    questTitleClass: "text-journal-cyan",
    questTitleHoverClass: "group-hover:text-journal-cyan",
  },
  teal: {
    sectionTitleClass: "text-journal-teal",
    sectionBorderClass: "border-journal-teal-border",
    sectionChevronClass: "text-journal-teal",
    sectionHeaderHoverClass: "hover:bg-journal-teal-hover",
    stepNumberClass: "text-journal-teal",
    questTitleClass: "text-journal-teal",
    questTitleHoverClass: "group-hover:text-journal-teal",
  },
  sky: {
    sectionTitleClass: "text-journal-sky",
    sectionBorderClass: "border-journal-sky-border",
    sectionChevronClass: "text-journal-sky",
    sectionHeaderHoverClass: "hover:bg-journal-sky-hover",
    stepNumberClass: "text-journal-sky",
    questTitleClass: "text-journal-sky",
    questTitleHoverClass: "group-hover:text-journal-sky",
  },
};

export function getJournalSectionPalette(
  accentColor: JournalSectionAccentColor,
): JournalSectionPalette {
  return JournalSectionPalettes[accentColor];
}

export const journalSectionPaletteKey: InjectionKey<ComputedRef<JournalSectionPalette>> =
  Symbol("journalSectionPalette");
