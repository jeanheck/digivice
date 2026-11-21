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

const OnSeabedText = "in-[[data-theme=seabed]]:text-teal-700";
const OnSeabedBorder = "in-[[data-theme=seabed]]:border-teal-700";
const OnSeabedHeaderHover = "in-[[data-theme=seabed]]:hover:bg-teal-700/30";
const OnSeabedGroupHover = "in-[[data-theme=seabed]]:group-hover:text-teal-700";

const JournalSectionPalettes: Record<JournalSectionAccentColor, JournalSectionPalette> = {
  emerald: {
    sectionTitleClass: `text-emerald-400 ${OnSeabedText}`,
    sectionBorderClass: `border-emerald-800 ${OnSeabedBorder}`,
    sectionChevronClass: `text-emerald-400 ${OnSeabedText}`,
    sectionHeaderHoverClass: `hover:bg-emerald-900/30 ${OnSeabedHeaderHover}`,
    stepNumberClass: `text-emerald-600 ${OnSeabedText}`,
    questTitleClass: `text-emerald-400 ${OnSeabedText}`,
    questTitleHoverClass: `group-hover:text-emerald-400 ${OnSeabedGroupHover}`,
  },
  cyan: {
    sectionTitleClass: `text-cyan-400 ${OnSeabedText}`,
    sectionBorderClass: `border-cyan-800 ${OnSeabedBorder}`,
    sectionChevronClass: `text-cyan-400 ${OnSeabedText}`,
    sectionHeaderHoverClass: `hover:bg-cyan-900/30 ${OnSeabedHeaderHover}`,
    stepNumberClass: `text-cyan-600 ${OnSeabedText}`,
    questTitleClass: `text-cyan-400 ${OnSeabedText}`,
    questTitleHoverClass: `group-hover:text-cyan-400 ${OnSeabedGroupHover}`,
  },
  teal: {
    sectionTitleClass: `text-teal-400 ${OnSeabedText}`,
    sectionBorderClass: `border-teal-800 ${OnSeabedBorder}`,
    sectionChevronClass: `text-teal-400 ${OnSeabedText}`,
    sectionHeaderHoverClass: `hover:bg-teal-900/30 ${OnSeabedHeaderHover}`,
    stepNumberClass: `text-teal-600 ${OnSeabedText}`,
    questTitleClass: `text-teal-400 ${OnSeabedText}`,
    questTitleHoverClass: `group-hover:text-teal-400 ${OnSeabedGroupHover}`,
  },
  sky: {
    sectionTitleClass: `text-sky-400 ${OnSeabedText}`,
    sectionBorderClass: `border-sky-800 ${OnSeabedBorder}`,
    sectionChevronClass: `text-sky-400 ${OnSeabedText}`,
    sectionHeaderHoverClass: `hover:bg-sky-900/30 ${OnSeabedHeaderHover}`,
    stepNumberClass: `text-sky-600 ${OnSeabedText}`,
    questTitleClass: `text-sky-400 ${OnSeabedText}`,
    questTitleHoverClass: `group-hover:text-sky-400 ${OnSeabedGroupHover}`,
  },
};

export function getJournalSectionPalette(
  accentColor: JournalSectionAccentColor,
): JournalSectionPalette {
  return JournalSectionPalettes[accentColor];
}

export const journalSectionPaletteKey: InjectionKey<ComputedRef<JournalSectionPalette>> =
  Symbol("journalSectionPalette");
