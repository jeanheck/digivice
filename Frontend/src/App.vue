<script setup lang="ts">
import Journal from "@/components/journal/Journal.vue";
import Map from "@/components/map/Map.vue";
import Footer from "@/components/footer/Footer.vue";
import Party from "@/components/party/Party.vue";
import AppErrorScreen from "@/components/app-error/AppErrorScreen.vue";
import { useAppBlockingError } from "@/composables/use-app-blocking-error";

const blockingError = useAppBlockingError();
</script>

<template>
  <AppErrorScreen v-if="blockingError" :error="blockingError" />

  <main v-else class="h-screen min-h-0 overflow-hidden bg-transparent p-4 flex flex-col gap-4 max-w-450 mx-auto text-white">
    <div class="flex-1 min-h-0 flex gap-4 overflow-hidden">
      <div class="flex-3 min-h-0 h-full overflow-hidden">
        <Party />
      </div>

      <div class="flex-1 min-w-75 min-h-0 overflow-hidden flex flex-col gap-4">
        <div class="flex-1 min-h-0 overflow-hidden flex flex-col">
          <Journal />
        </div>

        <div class="flex-1 min-h-0 overflow-hidden flex flex-col">
          <Map />
        </div>
      </div>
    </div>

    <Footer class="shrink-0" />
  </main>
</template>
