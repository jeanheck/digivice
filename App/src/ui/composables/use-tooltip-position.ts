import { ref } from "vue";

export type TooltipPlacement = "above" | "below";
export type TooltipHorizontalAlign = "right" | "left";

/**
 * Cursor-following tooltip position state. Use in the parent that handles mouseenter/move/leave;
 * pass show, x, y to Tooltip.vue or a content wrapper. Does not own title/body content.
 *
 * Edge flip is temporarily disabled for positioning tests: x always tracks the cursor offset.
 */
export function useTooltipPosition() {
  const show = ref(false);
  const x = ref(0);
  const y = ref(0);
  const horizontalAlign = ref<TooltipHorizontalAlign>("right");

  function updatePosition(
    event: MouseEvent,
    placement: TooltipPlacement,
    align: TooltipHorizontalAlign,
  ): void {
    if (align === "left") {
      x.value = event.clientX - 10;
    } else {
      x.value = event.clientX + 10;
    }

    y.value = placement === "above" ? event.clientY - 10 : event.clientY + 10;
  }

  function showAt(
    event: MouseEvent,
    options?: {
      placement?: TooltipPlacement;
      align?: TooltipHorizontalAlign;
    },
  ): void {
    show.value = true;
    horizontalAlign.value = options?.align ?? "right";

    updatePosition(event, options?.placement ?? "below", horizontalAlign.value);
  }

  function move(event: MouseEvent, placement: TooltipPlacement = "below"): void {
    if (!show.value) {
      return;
    }

    updatePosition(event, placement, horizontalAlign.value);
  }

  function hide(): void {
    show.value = false;
    horizontalAlign.value = "right";
  }

  return {
    show,
    x,
    y,
    horizontalAlign,
    showAt,
    move,
    hide,
  };
}
