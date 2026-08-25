<script setup lang="ts">
import { computed } from "vue";

const props = defineProps<{
  exp: number;
  dvexp: number;
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
        class="w-7 h-7 rounded bg-black/80 border border-cyan-800 flex items-center justify-center text-cyan-400 hover:bg-cyan-900/80 hover:border-cyan-400 hover:text-white transition-all font-bold text-sm shadow-[0_0_10px_rgba(0,170,255,0.2)] cursor-pointer"
        @click.prevent="showPreviousMember"
      >
        &lt;
      </button>

      <div
        class="flex gap-2 px-3 py-1.5 bg-black/80 rounded border border-cyan-900/80 shadow-[0_0_10px_rgba(0,170,255,0.2)]"
      >
        <button
          v-for="memberIndex in memberCount"
          :key="memberIndex - 1"
          type="button"
          class="w-2 h-2 rounded-full transition-all cursor-pointer"
          :class="
            memberIndex - 1 === modelValue
              ? 'bg-cyan-400 shadow-[0_0_8px_rgba(0,255,255,1)] scale-110'
              : 'bg-cyan-900 hover:bg-cyan-600'
          "
          @click.prevent="selectMember(memberIndex - 1)"
        />
      </div>

      <button
        type="button"
        class="w-7 h-7 rounded bg-black/80 border border-cyan-800 flex items-center justify-center text-cyan-400 hover:bg-cyan-900/80 hover:border-cyan-400 hover:text-white transition-all font-bold text-sm shadow-[0_0_10px_rgba(0,170,255,0.2)] cursor-pointer"
        @click.prevent="showNextMember"
      >
        &gt;
      </button>
    </div>
  </div>
</template>
