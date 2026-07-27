import { ref } from "vue";

export type TooltipPlacement = "above" | "below";
export type TooltipHorizontalAlign = "right" | "left";

/**
 * Cursor-following tooltip position state. Use in the parent that handles mouseenter/move/leave;
 * pass show, x, y to Tooltip.vue or a content wrapper. Does not own title/body content.
 */
export function useTooltipPosition(defaultMaxWidth = 250) {
    const show = ref(false);
    const x = ref(0);
    const y = ref(0);
    const maxWidth = ref(defaultMaxWidth);
    const horizontalAlign = ref<TooltipHorizontalAlign>("right");

    function updatePosition(
        event: MouseEvent,
        width: number,
        placement: TooltipPlacement,
        align: TooltipHorizontalAlign
    ): void {
        let positionX: number;

        if (align === "left") {
            positionX = event.clientX - 15;
        } else {
            positionX = event.clientX + 15;
            if (positionX + width > window.innerWidth) {
                positionX = event.clientX - width - 10;
            }
        }

        x.value = positionX;
        y.value = placement === "above" ? event.clientY - 15 : event.clientY + 15;
    }

    function showAt(
        event: MouseEvent,
        options?: {
            maxWidth?: number;
            placement?: TooltipPlacement;
            align?: TooltipHorizontalAlign;
        }
    ): void {
        show.value = true;

        if (options?.maxWidth !== undefined) {
            maxWidth.value = options.maxWidth;
        }

        horizontalAlign.value = options?.align ?? "right";

        updatePosition(
            event,
            maxWidth.value,
            options?.placement ?? "below",
            horizontalAlign.value
        );
    }

    function move(event: MouseEvent, placement: TooltipPlacement = "below"): void {
        if (!show.value) {
            return;
        }

        updatePosition(event, maxWidth.value, placement, horizontalAlign.value);
    }

    function hide(): void {
        show.value = false;
        horizontalAlign.value = "right";
    }

    return {
        show,
        x,
        y,
        maxWidth,
        horizontalAlign,
        showAt,
        move,
        hide
    };
}
