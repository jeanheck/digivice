<script setup lang="ts">
import { computed, onBeforeUnmount, onMounted, ref } from "vue";
import {
  MapThemeOptions,
  useMapThemeDevOverride,
} from "@/ui/composables/use-map-theme";
import type { MapTheme } from "@/shared/types/map-theme.type";

const MenuGapPx = 8;

const emit = defineEmits<{
  (e: "show-tooltip", event: MouseEvent, title: string): void;
  (e: "move-tooltip", event: MouseEvent): void;
  (e: "hide-tooltip"): void;
}>();

const { effectiveTheme, setTheme } = useMapThemeDevOverride();

const isMenuOpen = ref(false);
const triggerElement = ref<HTMLButtonElement | null>(null);
const menuElement = ref<HTMLDivElement | null>(null);
const menuPosition = ref({ x: 0, y: 0 });

const activeOption = computed(() => {
  return (
    MapThemeOptions.find((option) => option.theme === effectiveTheme.value) ?? MapThemeOptions[0]
  );
});

const otherOptions = computed(() => {
  return MapThemeOptions.filter((option) => option.theme !== effectiveTheme.value);
});

function toggleMenu(): void {
  if (isMenuOpen.value) {
    isMenuOpen.value = false;
    return;
  }

  emit("hide-tooltip");

  const triggerRect = triggerElement.value!.getBoundingClientRect();
  menuPosition.value = {
    x: triggerRect.left + triggerRect.width / 2,
    y: triggerRect.top - MenuGapPx,
  };
  isMenuOpen.value = true;
}

function selectTheme(theme: MapTheme): void {
  setTheme(theme);
  isMenuOpen.value = false;
  emit("hide-tooltip");
}

function onShowTooltip(event: MouseEvent, title: string): void {
  emit("show-tooltip", event, title);
}

function onMoveTooltip(event: MouseEvent): void {
  emit("move-tooltip", event);
}

function onHideTooltip(): void {
  emit("hide-tooltip");
}

function onPointerDown(event: PointerEvent): void {
  const target = event.target as Node;
  if (triggerElement.value?.contains(target) || menuElement.value?.contains(target)) {
    return;
  }

  isMenuOpen.value = false;
}

function onKeyDown(event: KeyboardEvent): void {
  if (event.key === "Escape") {
    isMenuOpen.value = false;
  }
}

onMounted(() => {
  window.addEventListener("pointerdown", onPointerDown);
  window.addEventListener("keydown", onKeyDown);
});

onBeforeUnmount(() => {
  window.removeEventListener("pointerdown", onPointerDown);
  window.removeEventListener("keydown", onKeyDown);
});
</script>

<template>
  <button
    ref="triggerElement"
    type="button"
    class="bg-transparent border-0 px-1 py-0.5 leading-none hover:scale-110 transition-transform cursor-pointer flex items-center justify-center rounded-sm"
    @click="toggleMenu"
    @mouseenter="onShowTooltip($event, activeOption.label)"
    @mousemove="onMoveTooltip"
    @mouseleave="onHideTooltip"
  >
    <span class="font-emoji-color text-lg leading-none">{{ activeOption.emoji }}</span>
  </button>

  <Teleport to="body">
    <Transition name="fade">
      <div
        v-if="isMenuOpen"
        ref="menuElement"
        class="fixed z-9999 flex flex-col gap-1 p-1.5 bg-[#001133ee] border-2 border-[#0066cc] rounded-sm shadow-[0_4px_12px_rgba(0,0,0,0.8)] -translate-x-1/2 -translate-y-full"
        :style="{ left: `${menuPosition.x}px`, top: `${menuPosition.y}px` }"
      >
        <button
          v-for="option in otherOptions"
          :key="option.theme"
          type="button"
          class="px-1 py-0.5 leading-none rounded-sm cursor-pointer hover:bg-[#0055ff]/30 transition-colors"
          @click="selectTheme(option.theme)"
          @mouseenter="onShowTooltip($event, option.label)"
          @mousemove="onMoveTooltip"
          @mouseleave="onHideTooltip"
        >
          <span class="font-emoji-color text-lg leading-none">{{ option.emoji }}</span>
        </button>
      </div>
    </Transition>
  </Teleport>
</template>
