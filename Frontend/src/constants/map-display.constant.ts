/**
 * Canonical width for quest pin / zoomed-location map rendering.
 * Quest JSON coordinates are calibrated against this size (not MAP_FRAME_WIDTH_PX = 600).
 */
export const MAP_DISPLAY_WIDTH_PX = 512;

/** Left padding (24px) + stable scrollbar gutter (~16px) around the fixed-width map. */
export const MAP_PANEL_HORIZONTAL_GUTTER_PX = 40;

export const MAP_PANEL_MIN_WIDTH_PX = MAP_DISPLAY_WIDTH_PX + MAP_PANEL_HORIZONTAL_GUTTER_PX;
