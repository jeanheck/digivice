<script setup lang="ts">
import type { PartyDvexpViewModel } from "@/viewmodels/dvexp/party-dvexp.viewmodel";

defineProps<{
  partyDvexps: PartyDvexpViewModel[];
}>();
</script>

<template>
  <div class="flex items-center gap-3">
    <span
      v-for="partyDvexp in partyDvexps"
      :key="partyDvexp.digimonId"
      class="flex items-center gap-1"
    >
      <img
        v-if="partyDvexp.imageUrl"
        :src="partyDvexp.imageUrl"
        :alt="partyDvexp.digimonName"
        class="w-7.5 h-7.5 object-cover rendering-pixelated"
      />
      <span class="font-bold text-gray-300">{{ partyDvexp.dvexp ?? "-" }}</span>
    </span>
  </div>
</template>
