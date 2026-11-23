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

const OnAmaterasuText = "in-[[data-theme=amaterasu]]:text-accent-bright";
const OnAmaterasuBorder = "in-[[data-theme=amaterasu]]:border-accent";
const OnAmaterasuHeaderHover = "in-[[data-theme=amaterasu]]:hover:bg-accent/20";
const OnAmaterasuGroupHover = "in-[[data-theme=amaterasu]]:group-hover:text-accent-bright";

const JournalSectionPalettes: Record<JournalSectionAccentColor, JournalSectionPalette> = {
  emerald: {
    sectionTitleClass: `text-emerald-400 ${OnSeabedText} ${OnAmaterasuText}`,
    sectionBorderClass: `border-emerald-800 ${OnSeabedBorder} ${OnAmaterasuBorder}`,
    sectionChevronClass: `text-emerald-400 ${OnSeabedText} ${OnAmaterasuText}`,
    sectionHeaderHoverClass: `hover:bg-emerald-900/30 ${OnSeabedHeaderHover} ${OnAmaterasuHeaderHover}`,
    stepNumberClass: `text-emerald-600 ${OnSeabedText} ${OnAmaterasuText}`,
    questTitleClass: `text-emerald-400 ${OnSeabedText} ${OnAmaterasuText}`,
    questTitleHoverClass: `group-hover:text-emerald-400 ${OnSeabedGroupHover} ${OnAmaterasuGroupHover}`,
  },
  cyan: {
    sectionTitleClass: `text-cyan-400 ${OnSeabedText} ${OnAmaterasuText}`,
    sectionBorderClass: `border-cyan-800 ${OnSeabedBorder} ${OnAmaterasuBorder}`,
    sectionChevronClass: `text-cyan-400 ${OnSeabedText} ${OnAmaterasuText}`,
    sectionHeaderHoverClass: `hover:bg-cyan-900/30 ${OnSeabedHeaderHover} ${OnAmaterasuHeaderHover}`,
    stepNumberClass: `text-cyan-600 ${OnSeabedText} ${OnAmaterasuText}`,
    questTitleClass: `text-cyan-400 ${OnSeabedText} ${OnAmaterasuText}`,
    questTitleHoverClass: `group-hover:text-cyan-400 ${OnSeabedGroupHover} ${OnAmaterasuGroupHover}`,
  },
  teal: {
    sectionTitleClass: `text-teal-400 ${OnSeabedText} ${OnAmaterasuText}`,
    sectionBorderClass: `border-teal-800 ${OnSeabedBorder} ${OnAmaterasuBorder}`,
    sectionChevronClass: `text-teal-400 ${OnSeabedText} ${OnAmaterasuText}`,
    sectionHeaderHoverClass: `hover:bg-teal-900/30 ${OnSeabedHeaderHover} ${OnAmaterasuHeaderHover}`,
    stepNumberClass: `text-teal-600 ${OnSeabedText} ${OnAmaterasuText}`,
    questTitleClass: `text-teal-400 ${OnSeabedText} ${OnAmaterasuText}`,
    questTitleHoverClass: `group-hover:text-teal-400 ${OnSeabedGroupHover} ${OnAmaterasuGroupHover}`,
  },
  sky: {
    sectionTitleClass: `text-sky-400 ${OnSeabedText} ${OnAmaterasuText}`,
    sectionBorderClass: `border-sky-800 ${OnSeabedBorder} ${OnAmaterasuBorder}`,
    sectionChevronClass: `text-sky-400 ${OnSeabedText} ${OnAmaterasuText}`,
    sectionHeaderHoverClass: `hover:bg-sky-900/30 ${OnSeabedHeaderHover} ${OnAmaterasuHeaderHover}`,
    stepNumberClass: `text-sky-600 ${OnSeabedText} ${OnAmaterasuText}`,
    questTitleClass: `text-sky-400 ${OnSeabedText} ${OnAmaterasuText}`,
    questTitleHoverClass: `group-hover:text-sky-400 ${OnSeabedGroupHover} ${OnAmaterasuGroupHover}`,
  },
};

export function getJournalSectionPalette(
  accentColor: JournalSectionAccentColor,
): JournalSectionPalette {
  return JournalSectionPalettes[accentColor];
}

export const journalSectionPaletteKey: InjectionKey<ComputedRef<JournalSectionPalette>> =
  Symbol("journalSectionPalette");
