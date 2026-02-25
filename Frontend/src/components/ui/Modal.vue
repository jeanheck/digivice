<script setup lang="ts">
import { onMounted, onUnmounted, watch } from 'vue'

const props = defineProps<{
  isOpen: boolean
  title?: string
}>()

const emit = defineEmits<{
  (e: 'close'): void
}>()

const handleKeydown = (e: KeyboardEvent) => {
  if (e.key === 'Escape' && props.isOpen) {
    emit('close')
  }
}

watch(() => props.isOpen, (isOpen) => {
  if (isOpen) {
    document.body.style.overflow = 'hidden'
  } else {
    document.body.style.overflow = ''
  }
})

onMounted(() => {
  window.addEventListener('keydown', handleKeydown)
})

onUnmounted(() => {
  window.removeEventListener('keydown', handleKeydown)
  document.body.style.overflow = ''
})
</script>

<template>
  <Teleport to="body">
    <transition
      enter-active-class="transition duration-200 ease-out"
      enter-from-class="opacity-0"
      enter-to-class="opacity-100"
      leave-active-class="transition duration-150 ease-in"
      leave-from-class="opacity-100"
      leave-to-class="opacity-0"
    >
      <div 
        v-if="isOpen" 
        class="fixed inset-0 z-[100] flex items-center justify-center bg-black bg-opacity-75 p-4"
        @click.self="emit('close')"
      >
        <div class="bg-[#000a2b] border-2 border-[#00154a] rounded shadow-[0_0_15px_rgba(0,0,0,0.8)] flex flex-col w-full max-w-6xl max-h-[90vh] overflow-hidden">
          
          <!-- Modal Header -->
          <div class="flex justify-between items-center bg-[#00154a] px-4 py-2 border-b-2 border-blue-900">
            <h2 class="text-white font-bold tracking-widest uppercase text-sm drop-shadow-md">
              {{ title || 'Modal' }}
            </h2>
            <button 
              @click="emit('close')"
              class="text-blue-300 hover:text-white transition-colors focus:outline-none"
            >
              <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
              </svg>
            </button>
          </div>

          <!-- Modal Body -->
          <div class="flex-1 overflow-auto p-4 bg-gradient-to-b from-[#000e3f] to-[#00071f] text-white custom-scrollbar">
            <slot></slot>
          </div>

        </div>
      </div>
    </transition>
  </Teleport>
</template>

<style scoped>
.custom-scrollbar::-webkit-scrollbar {
  width: 10px;
  height: 10px;
}
.custom-scrollbar::-webkit-scrollbar-track {
  background: #000a2b; 
}
.custom-scrollbar::-webkit-scrollbar-thumb {
  background: #002266; 
  border: 1px solid #00154a;
  border-radius: 2px;
}
.custom-scrollbar::-webkit-scrollbar-thumb:hover {
  background: #003399; 
}
</style>
