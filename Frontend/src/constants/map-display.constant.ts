/**
 * Canonical width for quest pin / zoomed-location map rendering.
 * Quest JSON coordinates are calibrated against this size (not MAP_FRAME_WIDTH_PX = 600).
 */
export const MAP_DISPLAY_WIDTH_PX = 512;

/** Default width for MapFrame / Seabed / Mobius / Wiki detail maps. */
export const MAP_FRAME_WIDTH_PX = 600;

/** Default max height for MapFrame when the parent wants internal scroll. */
export const MAP_FRAME_MAX_HEIGHT_PX = 520;

export const MAP_FRAME_DEFAULT_PIN_WRAPPER_SIZE_PX = 48;
export const MAP_FRAME_DEFAULT_PIN_DOT_SIZE_PX = 16;
export const MAP_FRAME_DEFAULT_PIN_LABEL_VERTICAL_OFFSET_PX = 40;
export const MAP_FRAME_DEFAULT_PIN_LABEL_VERTICAL_THRESHOLD_PERCENT = 25;

export const MAP_FRAME_QUEST_WORLD_PIN_WRAPPER_SIZE_PX = 32;
export const MAP_FRAME_QUEST_WORLD_PIN_DOT_SIZE_PX = 10;
export const MAP_FRAME_QUEST_WORLD_PIN_LABEL_VERTICAL_OFFSET_PX = 26;
export const MAP_FRAME_QUEST_WORLD_PIN_LABEL_VERTICAL_THRESHOLD_PERCENT = 20;

export const MAP_FRAME_QUEST_LOCAL_PIN_WRAPPER_SIZE_PX = 24;
export const MAP_FRAME_QUEST_LOCAL_PIN_DOT_SIZE_PX = 8;
export const MAP_FRAME_QUEST_LOCAL_PIN_LABEL_VERTICAL_OFFSET_PX = 25;

/** Left padding (24px) + stable scrollbar gutter (~16px) around the fixed-width map. */
export const MAP_PANEL_HORIZONTAL_GUTTER_PX = 40;

export const MAP_PANEL_MIN_WIDTH_PX = MAP_DISPLAY_WIDTH_PX + MAP_PANEL_HORIZONTAL_GUTTER_PX;
