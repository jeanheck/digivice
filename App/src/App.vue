<script setup lang="ts">
import { defineAsyncComponent } from "vue";
import Journal from "@/ui/components/journal/Journal.vue";
import Map from "@/ui/components/map/Map.vue";
import Footer from "@/ui/components/footer/Footer.vue";
import Party from "@/ui/components/party/Party.vue";
import AppHealthyScreen from "@/ui/components/app-healthy/AppHealthyScreen.vue";
import { useAppHealthyScreen } from "@/ui/composables/use-app-healthy-screen";
import { useBootLoader } from "@/ui/composables/use-boot-loader";

const healthyScreen = useAppHealthyScreen();
const isLoading = useBootLoader();

// Checked inline so the production build drops the monitor chunk entirely.
const DebugMonitor = import.meta.env.DEV
  ? defineAsyncComponent(() => import("@/ui/components/debug-monitor/DebugMonitor.vue"))
  : null;
</script>

<template>
  <AppHealthyScreen v-if="healthyScreen" :view-model="healthyScreen" />

  <main
    v-else-if="!isLoading"
    class="h-screen min-h-0 overflow-hidden bg-transparent p-2 flex flex-col gap-2 max-w-450 mx-auto text-white"
  >
    <div class="flex-1 min-h-0 flex gap-2 overflow-hidden">
      <div class="flex-3 min-h-0 h-full overflow-hidden">
        <Party />
      </div>

      <div class="flex-1 min-w-75 min-h-0 overflow-hidden flex flex-col gap-2">
        <div class="flex-3 min-h-0 overflow-hidden flex flex-col">
          <Journal />
        </div>

        <div class="flex-2 min-h-0 overflow-hidden flex flex-col">
          <Map />
        </div>
      </div>
    </div>

    <Footer class="shrink-0" />
  </main>

  <component :is="DebugMonitor" v-if="DebugMonitor" />
</template>
