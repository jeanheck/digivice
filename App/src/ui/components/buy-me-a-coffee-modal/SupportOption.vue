<script setup lang="ts">
import { ExternalLinkService } from "@/presentation/services/external-link.service";

const props = defineProps<{
  description: string;
  qrCodeUrl: string;
  qrCodeAlt: string;
  link: string;
}>();

async function openLink(): Promise<void> {
  await ExternalLinkService.open(props.link);
}
</script>

<template>
  <div class="flex h-full flex-col items-center justify-center gap-4 px-8 text-center">
    <p class="text-sm text-white/90 leading-relaxed max-w-md">
      {{ description }}
    </p>

    <img :src="qrCodeUrl" :alt="qrCodeAlt" class="w-56 h-56 rounded bg-white p-2 object-contain" />

    <p class="text-sm text-white/90">
      {{ $t("buyMeACoffee.orClickLink") }}
      <button
        type="button"
        class="text-cyan-300 underline hover:text-cyan-100 transition-colors cursor-pointer"
        @click="openLink"
      >
        {{ link }}
      </button>
    </p>
  </div>
</template>
