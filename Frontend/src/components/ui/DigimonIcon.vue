<script setup lang="ts">
const props = defineProps<{
  digimonName: string
}>()

const getIconUrl = (name: string) => {
  try {
    return new URL(`../../assets/icons/digimons/${name}.png`, import.meta.url).href
  } catch (e) {
    // Falha silenciosa: retorna string vazia ou placeholder caso nÃ£o encontre.
    return ''
  }
}
</script>

<template>
  <div class="bg-[#000e3f] rounded overflow-hidden shadow flex-shrink-0 flex items-center justify-center border-2 border-[#00154a]">
    <img 
      :src="getIconUrl(digimonName)" 
      :alt="digimonName"
      class="w-full h-full object-cover rendering-pixelated"
      @error="(e) => (e.target as HTMLImageElement).style.display = 'none'"
    />
  </div>
</template>
