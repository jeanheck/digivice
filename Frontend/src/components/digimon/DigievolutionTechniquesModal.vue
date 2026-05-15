<script setup lang="ts">
import { computed, onMounted, onUnmounted } from 'vue'
import IconClose from '../icons/IconClose.vue'
import type { Digievolution } from '../../models'
import { useLocalization } from '../../composables/useLocalization'

const props = defineProps<{
  isOpen: boolean
  digievolution: Digievolution | null
}>()

const emit = defineEmits(['close'])

const { t, getLocalized } = useLocalization()

const closeModal = () => emit('close')

const handleKeydown = (e: KeyboardEvent) => {
  if (e.key === 'Escape' && props.isOpen) {
    closeModal()
  }
}

onMounted(() => {
  window.addEventListener('keydown', handleKeydown)
})

onUnmounted(() => {
  window.removeEventListener('keydown', handleKeydown)
})

const techniques = computed(() => {
  return props.digievolution?.techniques ?? []
})

function elementColor(element: string): string {
  const map: Record<string, string> = {
    'Fire': 'text-orange-400',
    'Water': 'text-blue-400',
    'Ice': 'text-cyan-300',
    'Wind': 'text-gray-300',
    'Thunder': 'text-yellow-300',
    'Dark': 'text-purple-400',
    'Machine': 'text-gray-400',
    'None': 'text-white/60',
  }
  return map[element] ?? 'text-white/60'
}

function typeIcon(type: string): string {
  if (type === 'Physical') return '👊'
  if (type === 'Magical') return '🧙‍♂️'
  if (type === 'Heal') return '💚'
  if (type === 'Support') return '🟡'
  return '?'
}
</script>

<template>
  <Teleport to="body">
    <Transition name="fade">
      <div
        v-if="isOpen && digievolution"
        class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/60 backdrop-blur-sm"
        @click.self="closeModal"
      >
        <div class="relative w-full max-w-md bg-[#001122] border-2 border-[#0055ff] shadow-[0_0_20px_rgba(0,119,255,0.4)] rounded-lg flex flex-col overflow-hidden animate-slide-up">

          <!-- Hexagon background pattern -->
          <div class="absolute inset-0 opacity-[0.03] pointer-events-none"
               style="background-image: url('data:image/svg+xml,%3Csvg width=\'60\' height=\'60\' viewBox=\'0 0 60 60\' xmlns=\'http://www.w3.org/2000/svg\'%3E%3Cpath d=\'M30 0l25.98 15v30L30 60 4.02 45V15z\' stroke=\'%230077ff\' stroke-width=\'1\' fill=\'none\'/%3E%3C/svg%3E');">
          </div>

          <!-- Header -->
          <header class="flex items-center justify-between p-3 bg-gradient-to-r from-[#002244] to-[#001122] border-b border-[#0055ff]/50 relative z-10">
            <h2 class="font-bold tracking-widest text-[#00aaff] text-sm uppercase flex items-center gap-2">
              <span class="text-yellow-400">⚡</span>
              {{ digievolution?.name }}
            </h2>
            <button
              @click="closeModal"
              class="text-white/60 hover:text-white transition-colors ml-2"
              aria-label="Close"
            >
              <IconClose class="w-4 h-4" />
            </button>
          </header>

          <!-- Technique List -->
          <div class="relative z-10 flex flex-col gap-[3px] p-3 max-h-[70vh] overflow-y-auto custom-scroll">
            <div
              v-for="tech in techniques"
              :key="tech.id"
              class="relative rounded px-3 py-2 flex items-start gap-3 border transition-all text-xs"
              :class="{
                // Signature (highest level)
                'bg-yellow-950/30 border-yellow-500/60 shadow-[0_0_8px_rgba(234,179,8,0.2)]': tech.isSignature,
                // Unlocked (not signature)
                'bg-[#001a33]/80 border-[#0055ff]/40': !tech.isSignature && tech.isUnlocked,
                // Locked
                'bg-[#000e1f]/50 border-[#0033aa]/20 opacity-50': !tech.isUnlocked,
              }"
            >
              <!-- Signature badge -->
              <span
                v-if="tech.isSignature"
                class="absolute top-1 right-2 text-[10px] text-yellow-400 font-bold tracking-widest"
              >
                ⭐
              </span>

              <!-- Type icon -->
              <span class="text-base leading-none mt-[1px] flex-shrink-0">
                {{ typeIcon(tech.type?.id ?? '') }}
              </span>

              <!-- Content -->
              <div class="flex-1 min-w-0">
                <!-- Name + lock indicator -->
                <div class="flex items-center gap-1 mb-[2px]">
                  <span
                    class="font-bold tracking-wide"
                    :class="tech.isSignature ? 'text-yellow-300' : tech.isUnlocked ? 'text-white' : 'text-white/40'"
                  >
                    {{ getLocalized(tech.name) }}
                  </span>
                  <span v-if="!tech.isUnlocked" class="text-[10px] text-red-400/80 ml-1">Lv.{{ tech.learnLevel }}</span>
                  <span v-else class="text-[10px] text-green-400/80 ml-1">✓</span>
                </div>

                <!-- Description -->
                <p class="text-white/50 text-[11px] leading-snug">{{ getLocalized(tech.description) }}</p>

                <!-- Stats row -->
                <div class="flex gap-3 mt-1 text-[10px]">
                  <span :class="elementColor(tech.element ?? 'None')">
                    {{ (tech.element ?? 'None') !== 'None' ? t('resistances.' + (tech.element || 'None').toLowerCase()) : t('digievolution.neutral') }}
                  </span>
                  <span class="text-blue-300/70">MP {{ tech.mp }}</span>
                </div>
              </div>
            </div>

            <!-- Empty state -->
            <p v-if="techniques.length === 0" class="text-white/40 text-center py-4 text-xs">
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
