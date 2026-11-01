<script setup lang="ts">
import { computed, onBeforeUnmount, onMounted, ref } from "vue";
import { useI18n } from "vue-i18n";

const LanguageOptions = [
  { locale: "en-US", flag: "\u{1F1FA}\u{1F1F8}", label: "English" },
  { locale: "pt-BR", flag: "\u{1F1E7}\u{1F1F7}", label: "Português" },
  { locale: "es-ES", flag: "\u{1F1EA}\u{1F1F8}", label: "Español" },
] as const;

const MenuGapPx = 8;

const { locale } = useI18n();

const isMenuOpen = ref(false);
const triggerElement = ref<HTMLButtonElement | null>(null);
const menuElement = ref<HTMLDivElement | null>(null);
const menuPosition = ref({ x: 0, y: 0 });

const activeOption = computed(() => {
  return LanguageOptions.find((option) => option.locale === locale.value) ?? LanguageOptions[0];
});

const otherOptions = computed(() => {
  return LanguageOptions.filter((option) => option.locale !== locale.value);
});

function toggleMenu(): void {
  if (isMenuOpen.value) {
    isMenuOpen.value = false;
    return;
  }

  const triggerRect = triggerElement.value!.getBoundingClientRect();
  menuPosition.value = {
    x: triggerRect.left + triggerRect.width / 2,
    y: triggerRect.top - MenuGapPx,
  };
  isMenuOpen.value = true;
}

function setLocale(newLocale: string): void {
  locale.value = newLocale;
  localStorage.setItem("user-locale", newLocale);
  isMenuOpen.value = false;
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
    :title="activeOption.label"
    @click="toggleMenu"
  >
    <span class="font-emoji-color text-lg leading-none">{{ activeOption.flag }}</span>
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
          :key="option.locale"
          type="button"
          class="px-1 py-0.5 leading-none rounded-sm cursor-pointer hover:bg-[#0055ff]/30 transition-colors"
          :title="option.label"
          @click="setLocale(option.locale)"
        >
          <span class="font-emoji-color text-lg leading-none">{{ option.flag }}</span>
        </button>
      </div>
    </Transition>
  </Teleport>
</template>
