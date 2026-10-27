<script setup lang="ts">
import EmulatorSlot from "@/ui-next/components/emulator-slot/EmulatorSlot.vue";
import MainPanel from "@/ui-next/components/main-panel/MainPanel.vue";
import Footer from "@/ui-next/components/footer/Footer.vue";
import { useServerThemeRoot } from "@/ui-next/composables/use-server-theme";

useServerThemeRoot();
</script>

<template>
  <main class="h-screen overflow-hidden flex text-white">
    <div class="h-full shrink-0 flex flex-col">
      <EmulatorSlot class="h-[calc(100%-3rem)] aspect-4/3" />
      <Footer class="shrink-0" />
    </div>

    <MainPanel class="flex-1 min-w-0" />
  </main>
</template>
