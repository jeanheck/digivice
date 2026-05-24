<script setup lang="ts">
import { computed, ref } from "vue";
import Tooltip from "@/components/tooltip/Tooltip.vue";
import { useTooltipPosition, type TooltipPlacement } from "@/composables/use-tooltip-position";

const props = withDefaults(
    defineProps<{
        show?: boolean;
        x?: number;
        y?: number;
        title?: string;
        text?: string;
        maxWidth?: number;
        placement?: TooltipPlacement;
    }>(),
    {
        maxWidth: 250,
        placement: "below"
    }
);

const isInternalMode = ref(false);
const internalPosition = useTooltipPosition(props.maxWidth);
const internalTitle = ref("");
const internalText = ref("");

const resolvedShow = computed(() => {
    if (isInternalMode.value) {
        return internalPosition.show.value;
    }

    return props.show ?? false;
});

const resolvedX = computed(() => {
    if (isInternalMode.value) {
        return internalPosition.x.value;
    }

    return props.x ?? 0;
});

const resolvedY = computed(() => {
    if (isInternalMode.value) {
        return internalPosition.y.value;
    }

    return props.y ?? 0;
});

const resolvedTitle = computed(() => {
    if (isInternalMode.value) {
        return internalTitle.value;
    }

    return props.title ?? "";
});

const resolvedText = computed(() => {
    if (isInternalMode.value) {
        return internalText.value;
    }

    return props.text ?? "";
});

function show(event: MouseEvent, title: string, text: string): void {
    isInternalMode.value = true;
    internalTitle.value = title;
    internalText.value = text;
    internalPosition.showAt(event, { maxWidth: props.maxWidth, placement: props.placement });
}

function hide(): void {
    isInternalMode.value = false;
    internalPosition.hide();
}

function move(event: MouseEvent): void {
    internalPosition.move(event, props.placement);
}

defineExpose({
    show,
    hide,
    move
});
</script>

<template>
  <Tooltip
    :show="resolvedShow"
    :x="resolvedX"
    :y="resolvedY"
    :title="resolvedTitle"
    :max-width="maxWidth"
    :placement="placement"
  >
    <div class="text-gray-100 text-xs leading-relaxed shadow-black text-shadow-sm">
      {{ resolvedText }}
    </div>
  </Tooltip>
</template>

<style scoped>
.text-shadow-sm {
  text-shadow: 1px 1px 0 #000;
}
</style>
