<script setup lang="ts">
import { onMounted, onUnmounted } from "vue";

const DEFAULT_FOOTER_BAR_CLASS = "bg-linear-to-r from-blue-900 via-cyan-500 to-blue-900";

const props = withDefaults(
  defineProps<{
    isOpen: boolean;
    maxWidth?: string;
    maxHeight?: string;
    panelClass?: string;
    showHexPattern?: boolean;
    showFooterBar?: boolean;
    footerBarClass?: string;
    showCloseButton?: boolean;
  }>(),
  {
    maxWidth: "max-w-5xl",
    maxHeight: "",
    panelClass: "",
    showHexPattern: true,
    showFooterBar: true,
    footerBarClass: DEFAULT_FOOTER_BAR_CLASS,
    showCloseButton: true,
  }
);

const emit = defineEmits<{
  (e: "close"): void;
}>();

const handleClose = () => {
  emit("close");
};

const handleKeydown = (event: KeyboardEvent) => {
  if (event.key === "Escape" && props.isOpen) {
    handleClose();
  }
};

onMounted(() => {
  window.addEventListener("keydown", handleKeydown);
});

onUnmounted(() => {
  window.removeEventListener("keydown", handleKeydown);
});
</script>

<template>
  <Teleport to="body">
    <Transition name="modal-fade">
      <div
        v-if="isOpen"
        class="fixed inset-0 z-100 flex items-center justify-center p-4 bg-black/60 backdrop-blur-sm"
        @click.self="handleClose"
      >
        <div
          class="relative w-full bg-[#001122] border-2 border-[#0055ff] shadow-[0_0_20px_rgba(0,119,255,0.4)] rounded-lg flex flex-col overflow-hidden animate-modal-slide-up"
          :class="[maxWidth, maxHeight, panelClass]"
        >
          <div
            v-if="showHexPattern"
            class="absolute inset-0 opacity-[0.03] pointer-events-none"
            style="background-image: url('data:image/svg+xml,%3Csvg width=\'60\' height=\'60\' viewBox=\'0 0 60 60\' xmlns=\'http://www.w3.org/2000/svg\'%3E%3Cpath d=\'M30 0l25.98 15v30L30 60 4.02 45V15z\' stroke=\'%230077ff\' stroke-width=\'1\' fill=\'none\'/%3E%3C/svg%3E');"
          />

          <header
            class="relative z-30 flex items-center justify-between gap-4 overflow-visible p-3 bg-linear-to-r from-[#002244] to-[#001122] border-b border-[#0055ff]/50 shrink-0"
          >
            <div class="flex min-w-0 flex-1 items-center">
              <slot name="header" />
            </div>

            <button
              v-if="showCloseButton"
              type="button"
              class="text-gray-400 hover:text-red-400 transition-colors bg-black/30 w-7 h-7 flex items-center justify-center rounded border border-gray-700 hover:border-red-500 shrink-0"
              @click="handleClose"
            >
              <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24" class="w-5 h-5">
                <path fill="currentColor" d="M5 5h2v2H5zm4 4H7V7h2zm2 2H9V9h2zm2 0h-2v2H9v2H7v2H5v2h2v-2h2v-2h2v-2h2v2h2v2h2v2h2v-2h-2v-2h-2v-2h-2zm2-2v2h-2V9zm2-2v2h-2V7zm0 0V5h2v2z"/>
              </svg>
            </button>
          </header>

          <div class="relative z-0 flex min-h-0 flex-1 flex-col">
            <slot />
          </div>

          <slot name="footer">
            <div
              v-if="showFooterBar"
              class="relative z-10 h-1.5 w-full shrink-0"
              :class="footerBarClass"
            />
          </slot>
        </div>
      </div>
    </Transition>
  </Teleport>
</template>

<style scoped>
.modal-fade-enter-active,
.modal-fade-leave-active {
  transition: opacity 0.3s ease;
}

.modal-fade-enter-from,
.modal-fade-leave-to {
  opacity: 0;
}

@keyframes modal-slide-up {
  from {
    transform: translateY(20px);
    opacity: 0;
  }

  to {
    transform: translateY(0);
    opacity: 1;
  }
}

.animate-modal-slide-up {
  animation: modal-slide-up 0.3s cubic-bezier(0.16, 1, 0.3, 1) forwards;
}
</style>
