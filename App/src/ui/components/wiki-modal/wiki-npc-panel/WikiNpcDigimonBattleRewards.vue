<script setup lang="ts">
import { computed } from "vue";
import WikiPartyDvexp from "@/ui/components/wiki-modal/wiki-party-dvexp/WikiPartyDvexp.vue";
import type { PartyDvexpViewModel } from "@/presentation/viewmodels/dvexp/party-dvexp.viewmodel";

const props = defineProps<{
  exp: number | null;
  partyDvexps: PartyDvexpViewModel[];
  bits: number;
  memberCount: number;
  modelValue: number;
}>();

const emit = defineEmits<{
  (e: "update:modelValue", memberIndex: number): void;
}>();

const showPagination = computed(() => {
  return props.memberCount > 1;
});

const showPreviousMember = () => {
  if (props.memberCount === 0) {
    return;
  }

  const nextIndex = (props.modelValue - 1 + props.memberCount) % props.memberCount;
  emit("update:modelValue", nextIndex);
};

const showNextMember = () => {
  if (props.memberCount === 0) {
    return;
  }

  const nextIndex = (props.modelValue + 1) % props.memberCount;
  emit("update:modelValue", nextIndex);
};

const selectMember = (memberIndex: number) => {
  emit("update:modelValue", memberIndex);
};
</script>

<template>
  <div class="relative w-full shrink-0 flex items-center min-h-7">
    <div class="z-10 flex items-center justify-start gap-4 text-xs">
      <div class="flex items-center gap-2">
        <span class="font-bold text-accent tracking-wider uppercase">
          {{ $t("enemy.baseExp") }}:
        </span>
        <span class="font-bold text-ink">{{ exp ?? "-" }}</span>
      </div>

      <div class="flex items-center gap-2">
        <span class="font-bold text-accent tracking-wider uppercase">
          {{ $t("enemy.baseDvexp") }}:
        </span>
        <WikiPartyDvexp :party-dvexps="partyDvexps" />
      </div>
    </div>

    <div class="z-10 ml-auto flex items-center gap-2 text-xs">
      <span class="font-bold text-accent tracking-wider uppercase">
        {{ $t("enemy.bits") }}:
      </span>
      <span class="font-bold text-ink">{{ bits }}</span>
    </div>

    <div
      v-if="showPagination"
      class="absolute left-1/2 -translate-x-1/2 flex items-center justify-center gap-3"
    >
      <button
        type="button"
        class="w-7 h-7 rounded bg-surface border border-frame flex items-center justify-center text-accent hover:bg-accent/30 hover:border-frame hover:text-ink transition-all font-bold text-sm shadow-[0_0_10px_color-mix(in_srgb,var(--color-accent)_20%,transparent)] cursor-pointer"
        @click.prevent="showPreviousMember"
      >
        &lt;
      </button>

      <div
        class="flex gap-2 px-3 py-1.5 bg-surface rounded border border-frame/60 shadow-[0_0_10px_color-mix(in_srgb,var(--color-accent)_20%,transparent)]"
      >
        <button
          v-for="memberIndex in memberCount"
          :key="memberIndex - 1"
          type="button"
          class="w-2 h-2 rounded-full transition-all cursor-pointer"
          :class="
            memberIndex - 1 === modelValue
              ? 'bg-accent scale-110'
              : 'bg-frame/40 hover:bg-accent/50'
          "
          @click.prevent="selectMember(memberIndex - 1)"
        />
      </div>

      <button
        type="button"
        class="w-7 h-7 rounded bg-surface border border-frame flex items-center justify-center text-accent hover:bg-accent/30 hover:border-frame hover:text-ink transition-all font-bold text-sm shadow-[0_0_10px_color-mix(in_srgb,var(--color-accent)_20%,transparent)] cursor-pointer"
        @click.prevent="showNextMember"
      >
        &gt;
      </button>
    </div>
  </div>
</template>
