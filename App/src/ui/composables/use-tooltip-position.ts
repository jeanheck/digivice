import { ref } from "vue";

export type TooltipPlacement = "above" | "below";
export type TooltipHorizontalAlign = "right" | "left";

const TooltipFlipWidthPx = 520;

function getFlipWidthEstimate(): number {
  return Math.min(window.innerWidth * 0.9, TooltipFlipWidthPx);
}

/**
 * Cursor-following tooltip position state. Use in the parent that handles mouseenter/move/leave;
 * pass show, x, y to Tooltip.vue or a content wrapper. Does not own title/body content.
 */
export function useTooltipPosition() {
  const show = ref(false);
  const x = ref(0);
  const y = ref(0);
  const horizontalAlign = ref<TooltipHorizontalAlign>("right");

  function updatePosition(
    event: MouseEvent,
    width: number,
    placement: TooltipPlacement,
    align: TooltipHorizontalAlign,
  ): void {
    let positionX: number;

    if (align === "left") {
      positionX = event.clientX - 10;
    } else {
      positionX = event.clientX + 10;
      if (positionX + width > window.innerWidth) {
        positionX = event.clientX - width - 10;
      }
    }

    x.value = positionX;
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

    updatePosition(
      event,
      getFlipWidthEstimate(),
      options?.placement ?? "below",
      horizontalAlign.value,
    );
  }

  function move(event: MouseEvent, placement: TooltipPlacement = "below"): void {
    if (!show.value) {
      return;
    }

    updatePosition(event, getFlipWidthEstimate(), placement, horizontalAlign.value);
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
