<script setup lang="ts">
import bgPatternUrl from '../../assets/bg-pattern.svg'
</script>

<template>
  <div class="fixed inset-0 w-full h-full -z-10 bg-[#000030] pointer-events-none">
    <div 
      class="absolute inset-0 w-full h-full repeating-bg"
      :style="{ backgroundImage: `url(${bgPatternUrl})` }"
    ></div>
  </div>
</template>

<style scoped>
.repeating-bg {
  /* Tamanho exato do grid do SVG, mas agora totalmente estático como você pediu */
  background-size: 128px 128px;
  background-repeat: repeat;
  background-position: top left;
}
</style>
