<script setup lang="ts">
import Modal from "@/ui/components/modal/Modal.vue";
import SupportOption from "@/ui/components/buy-me-a-coffee-modal/SupportOption.vue";
import { ExternalLink } from "@/shared/constants/external-link.constant";
import livePixQrCode from "@/assets/buy-me-a-coffee/LivePixQrCode.webp";
import koFiQrCode from "@/assets/buy-me-a-coffee/KoFiQrCode.webp";

defineProps<{
  isOpen: boolean;
}>();

const emit = defineEmits<{
  (e: "close"): void;
}>();

function closeModal(): void {
  emit("close");
}
</script>

<template>
  <Modal
    :is-open="isOpen"
    max-width="max-w-[1300px]"
    max-height="h-[650px] max-h-[650px]"
    panel-class="w-[1300px]"
    @close="closeModal"
  >
    <template #header>
      <h2 class="text-white font-bold tracking-widest drop-shadow whitespace-nowrap shrink-0">
        {{ $t("buyMeACoffee.title") }}
      </h2>
    </template>

    <div class="flex flex-col gap-3 px-12 pt-6 pb-4 text-center text-white">
      <p class="font-bold">{{ $t("buyMeACoffee.introFree") }}</p>
      <p class="text-sm text-white/90 leading-relaxed">{{ $t("buyMeACoffee.introSupport") }}</p>
    </div>

    <div class="grid grid-cols-2 flex-1 min-h-0 py-4">
      <div class="border-r border-[#0055ff]/50">
        <SupportOption
          :description="$t('buyMeACoffee.livePixDescription')"
          :qr-code-url="livePixQrCode"
          qr-code-alt="LivePix QR Code"
          :link="ExternalLink.livePix"
        />
      </div>
      <SupportOption
        :description="$t('buyMeACoffee.koFiDescription')"
        :qr-code-url="koFiQrCode"
        qr-code-alt="Ko-fi QR Code"
        :link="ExternalLink.koFi"
      />
    </div>
  </Modal>
</template>
