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

  <main v-else class="min-h-screen bg-transparent p-4 flex flex-col gap-4 max-w-450 mx-auto text-white">
    <div class="flex-1 flex gap-4 min-h-150">
      <Party />

      <div class="flex-1 min-w-75 min-h-0 overflow-hidden flex flex-col gap-4">
        <div class="flex-3 min-h-0 overflow-hidden flex flex-col">
          <Journal class="flex-1" />
        </div>

        <div class="flex-2 min-h-50 flex flex-col">
          <Map class="flex-1" />
        </div>
      </div>
    </div>

    <Footer />
  </main>
</template>
