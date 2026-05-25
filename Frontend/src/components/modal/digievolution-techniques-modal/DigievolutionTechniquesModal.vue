<script setup lang="ts">
import { watch, onUnmounted, computed } from "vue";
import IconClose from "@/components/modal/IconClose.vue";
import Technique from "@/components/modal/digievolution-techniques-modal/Technique.vue";
import { DigievolutionTechniquesModalPresenter } from "@/presenters/digievolution-techniques-modal.presenter";

const props = defineProps<{
  isOpen: boolean;
  digievolutionId: number;
  digievolutionName: string;
  digievolutionLevel: number;
}>();

const emit = defineEmits(['close']);

const closeModal = () => {
  emit('close');
};

const handleKeydown = (e: KeyboardEvent) => {
  if (e.key === 'Escape') {
    closeModal();
  }
};

watch(() => props.isOpen, (open) => {
  if (open) {
    window.addEventListener('keydown', handleKeydown);
  } else {
    window.removeEventListener('keydown', handleKeydown);
  }
});

onUnmounted(() => {
  window.removeEventListener('keydown', handleKeydown);
});

const digievolutionTechniquesRaw = computed(() => {
  return DigievolutionTechniquesModalPresenter.getTechniquesByDigievolutionId(props.digievolutionId);
});
const signatureTechniqueId = computed(() => {
  return DigievolutionTechniquesModalPresenter.getSignatureTechnique(digievolutionTechniquesRaw.value);
});
</script>

<template>
  <Teleport to="body">
    <Transition name="fade">
      <div
        v-if="isOpen"
        class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/60 backdrop-blur-sm"
        @click.self="closeModal"
      >
        <div class="relative w-full max-w-md bg-[#001122] border-2 border-[#0055ff] shadow-[0_0_20px_rgba(0,119,255,0.4)] rounded-lg flex flex-col overflow-hidden animate-slide-up">

          <div class="absolute inset-0 opacity-[0.03] pointer-events-none"
               style="background-image: url('data:image/svg+xml,%3Csvg width=\'60\' height=\'60\' viewBox=\'0 0 60 60\' xmlns=\'http://www.w3.org/2000/svg\'%3E%3Cpath d=\'M30 0l25.98 15v30L30 60 4.02 45V15z\' stroke=\'%230077ff\' stroke-width=\'1\' fill=\'none\'/%3E%3C/svg%3E');">
          </div>

          <header class="flex items-center justify-between p-3 bg-linear-to-r from-[#002244] to-[#001122] border-b border-[#0055ff]/50 relative z-10">
            <h2 class="font-bold tracking-widest text-[#00aaff] text-sm uppercase flex items-center gap-2">
              <span class="text-yellow-400">⚡</span>
              {{ digievolutionName }}
            </h2>
            <button
              @click="closeModal"
              class="text-white/60 hover:text-white transition-colors ml-2"
              aria-label="Close"
            >
              <IconClose class="w-4 h-4" />
            </button>
          </header>

          <div class="relative z-10 flex flex-col gap-0.75 p-3 max-h-[70vh] overflow-y-auto custom-scroll">
            <Technique
              v-for="technique in digievolutionTechniquesRaw"
              :key="technique.id"
              :technique-id="technique.id"
              :learn-level="technique.learnLevel"
              :loaded-level="technique.loadedLevel"
              :digievolution-level="digievolutionLevel"
              :is-signature="signatureTechniqueId === technique.id"
            />

            <p v-if="digievolutionTechniquesRaw.length === 0" class="text-white/40 text-center py-4 text-xs">
              {{ $t('digievolution.noTechData') }}
            </p>
          </div>
        </div>
      </div>
    </Transition>
  </Teleport>
</template>

<style scoped>
.custom-scroll::-webkit-scrollbar { width: 4px; }
.custom-scroll::-webkit-scrollbar-track { background: transparent; }
.custom-scroll::-webkit-scrollbar-thumb { background: #0055ff55; border-radius: 2px; }
.custom-scroll::-webkit-scrollbar-thumb:hover { background: #0077ff88; }

.fade-enter-active, .fade-leave-active { transition: opacity 0.2s ease; }
.fade-enter-from, .fade-leave-to { opacity: 0; }

.animate-slide-up {
  animation: slideUp 0.2s ease-out;
}
@keyframes slideUp {
  from { transform: translateY(12px); opacity: 0; }
  to   { transform: translateY(0);    opacity: 1; }
}
</style>
