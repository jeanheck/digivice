<script setup lang="ts">
import { computed } from "vue";
import WikiNpcBattleStatusLabel from "@/components/wiki-modal/wiki-npc-panel/WikiNpcBattleStatusLabel.vue";
import type { NpcBattleStatus } from "@/services/npc.service";

const props = defineProps<{
  exp: number;
  dvexp: number;
  bits: number;
  memberCount: number;
  modelValue: number;
  battleStatus: NpcBattleStatus;
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
        <span class="font-bold text-blue-500 tracking-wider uppercase">
          {{ $t("enemy.baseExp") }}:
        </span>
        <span class="font-bold text-gray-300">{{ exp }}</span>
      </div>

      <div class="flex items-center gap-2">
        <span class="font-bold text-blue-500 tracking-wider uppercase">
          {{ $t("enemy.baseDvexp") }}:
        </span>
        <span class="font-bold text-gray-300">{{ dvexp }}</span>
      </div>

      <div class="flex items-center gap-2">
        <span class="font-bold text-blue-500 tracking-wider uppercase">
          {{ $t("enemy.baseBits") }}:
        </span>
        <span class="font-bold text-gray-300">{{ bits }}</span>
      </div>
    </div>

    <div
      v-if="showPagination"
      class="absolute left-1/2 -translate-x-1/2 flex items-center justify-center gap-3"
    >
      <button
        type="button"
        class="w-7 h-7 rounded bg-black/80 border border-blue-800 flex items-center justify-center text-blue-500 hover:bg-blue-900/80 hover:border-blue-400 hover:text-white transition-all font-bold text-sm shadow-[0_0_10px_rgba(0,170,255,0.2)] cursor-pointer"
        @click.prevent="showPreviousMember"
      >
        &lt;
      </button>

      <div
        class="flex gap-2 px-3 py-1.5 bg-black/80 rounded border border-blue-900/80 shadow-[0_0_10px_rgba(0,170,255,0.2)]"
      >
        <button
          v-for="memberIndex in memberCount"
          :key="memberIndex - 1"
          type="button"
          class="w-2 h-2 rounded-full transition-all cursor-pointer"
          :class="
            memberIndex - 1 === modelValue
              ? 'bg-blue-500 shadow-[0_0_8px_rgba(59,130,246,1)] scale-110'
              : 'bg-blue-900 hover:bg-blue-600'
          "
          @click.prevent="selectMember(memberIndex - 1)"
        />
      </div>

      <button
        type="button"
        class="w-7 h-7 rounded bg-black/80 border border-blue-800 flex items-center justify-center text-blue-500 hover:bg-blue-900/80 hover:border-blue-400 hover:text-white transition-all font-bold text-sm shadow-[0_0_10px_rgba(0,170,255,0.2)] cursor-pointer"
        @click.prevent="showNextMember"
      >
        &gt;
      </button>
    </div>

    <WikiNpcBattleStatusLabel
      class="absolute right-0 top-1/2 -translate-y-1/2"
      :battle-status="battleStatus"
    />
  </div>
</template>
