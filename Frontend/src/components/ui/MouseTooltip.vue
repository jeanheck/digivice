<script setup lang="ts">
import { ref } from 'vue'

const isVisible = ref(false)
const x = ref(0)
const y = ref(0)
const tooltipTitle = ref('')
const tooltipText = ref('')

const show = (event: MouseEvent, title: string, text: string) => {
  isVisible.value = true
  tooltipTitle.value = title
  tooltipText.value = text
  move(event)
}

const hide = () => {
  isVisible.value = false
}

const move = (event: MouseEvent) => {
  if (!isVisible.value) return
  
  const tooltipWidth = 250
  let posX = event.clientX + 15
  if (posX + tooltipWidth > window.innerWidth) {
    posX = event.clientX - tooltipWidth - 10
  }
  
  x.value = posX
  y.value = event.clientY - 15
}

defineExpose({
  show,
  hide,
  move
})
</script>

<template>
  <Teleport to="body">
    <Transition name="fade">
      <div 
        v-if="isVisible"
        class="fixed z-[9999] pointer-events-none p-3 max-w-[250px] bg-[#001133ee] border-[2px] border-[#0066cc] rounded-sm shadow-[0_4px_12px_rgba(0,0,0,0.8)] flex flex-col gap-1 backdrop-blur-sm"
        :style="{ top: `${y}px`, left: `${x}px`, transform: 'translateY(-100%)' }"
      >
        <div>
           <div class="font-bold text-yellow-300 text-sm border-b border-[#0066cc]/50 pb-1 mb-1 shadow-black text-shadow-sm uppercase tracking-wider">
              {{ tooltipTitle }}
           </div>
           <div class="text-gray-100 text-xs leading-relaxed shadow-black text-shadow-sm">
              {{ tooltipText }}
           </div>
        </div>
      </div>
    </Transition>
  </Teleport>
</template>

<style scoped>
.text-shadow-sm {
  text-shadow: 1px 1px 0 #000;
}
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.15s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
