<script setup lang="ts">
import { computed, ref, watch } from "vue";
import Modal from "@/components/modal/Modal.vue";
import Tooltip from "@/components/tooltip/Tooltip.vue";
import { useLocalization } from "@/composables/useLocalization";
import { useTooltipPosition } from "@/composables/use-tooltip-position";
import { ImageCatalog } from "@/catalogs/image.catalog.ts";
import { EnemyRepository } from "@/repositories/enemy.repository";

const props = defineProps<{
  isOpen: boolean;
  enemyId: string | null;
}>();

const emit = defineEmits<{
  (e: "close"): void;
}>();

const { t, getLocalized } = useLocalization();

const isModalOpen = computed(() => {
  return props.isOpen && props.enemyId !== null;
});

const handleClose = () => {
  emit("close");
};

const enemy = computed(() => {
  return EnemyRepository.getEnemyById(props.enemyId!);
});

const attributesList = computed(() => {
  if (!props.enemyId) {
    return [];
  }
  return [
    { label: t("attribute.strength"), val: enemy.value.strength, icon: "👊", color: "text-[#fcd883]" },
    { label: t("attribute.defense"), val: enemy.value.defense, icon: "🛡️", color: "text-gray-400" },
    { label: t("attribute.spirit"), val: enemy.value.spirit, icon: "🧙‍♂️", color: "text-pink-300" },
    { label: t("attribute.wisdom"), val: enemy.value.wisdom, icon: "📖", color: "text-yellow-600" },
    { label: t("attribute.speed"), val: enemy.value.speed, icon: "🏃", color: "text-green-400" },
  ];
});

const elemTolsList = computed(() => {
  if (!props.enemyId) {
    return [];
  }
  return [
    { label: t("element.fire"), val: enemy.value.fire, icon: "🔥", color: "text-orange-500" },
    { label: t("element.water"), val: enemy.value.water, icon: "💧", color: "text-blue-400" },
    { label: t("element.ice"), val: enemy.value.ice, icon: "🧊", color: "text-cyan-200" },
    { label: t("element.wind"), val: enemy.value.wind, icon: "🍃", color: "text-gray-100" },
    { label: t("element.thunder"), val: enemy.value.thunder, icon: "⚡", color: "text-[#ffffcc]" },
    { label: t("element.machine"), val: enemy.value.machine, icon: "⚙️", color: "text-gray-500" },
    { label: t("element.dark"), val: enemy.value.dark, icon: "🌑", color: "text-purple-500" },
  ];
});

const statusTolsList = computed(() => {
  if (!props.enemyId) {
    return [];
  }
  return [
    { label: enemy.value.canPoison ? t("conditions.resistancePoison") : t("conditions.canPoison"), val: enemy.value.canPoison ? `${enemy.value.poison}%` : "No", icon: "☠️", color: "text-green-500", valColor: enemy.value.canPoison ? "text-white" : "text-red-400" },
    { label: enemy.value.canParalyze ? t("conditions.resistanceParalyze") : t("conditions.canParalyze"), val: enemy.value.canParalyze ? `${enemy.value.paralyze}%` : "No", icon: "⚡", color: "text-yellow-300", valColor: enemy.value.canParalyze ? "text-white" : "text-red-400" },
    { label: enemy.value.canConfuse ? t("conditions.resistanceConfuse") : t("conditions.canConfuse"), val: enemy.value.canConfuse ? `${enemy.value.confuse}%` : "No", icon: "😵", color: "text-pink-400", valColor: enemy.value.canConfuse ? "text-white" : "text-red-400" },
    { label: enemy.value.canSleep ? t("conditions.resistanceSleep") : t("conditions.canSleep"), val: enemy.value.canSleep ? `${enemy.value.sleep}%` : "No", icon: "💤", color: "text-blue-300", valColor: enemy.value.canSleep ? "text-white" : "text-red-400" },
    { label: enemy.value.canKO ? t("conditions.resistanceKo") : t("conditions.canKo"), val: enemy.value.canKO ? `${enemy.value.ko ?? 0}%` : "No", icon: "💀", color: "text-gray-500", valColor: enemy.value.canKO ? "text-white" : "text-red-400" },
    { label: t("conditions.drain"), val: enemy.value.canDrain ? "Yes" : "No", icon: "🧛", color: "text-red-500", valColor: enemy.value.canDrain ? "text-green-400" : "text-red-400" },
    { label: t("conditions.steal"), val: enemy.value.canSteal ? "Yes" : "No", icon: "🦝", color: "text-amber-500", valColor: enemy.value.canSteal ? "text-green-400" : "text-red-400" },
    { label: t("conditions.escape"), val: enemy.value.canEscape ? "Yes" : "No", icon: "🏃", color: "text-gray-400", valColor: enemy.value.canEscape ? "text-green-400" : "text-red-400" },
  ];
});

const tooltipPlacement = "below" as const;
const tooltipPosition = useTooltipPosition(150);
const { show: tooltipShow, x: tooltipX, y: tooltipY, showAt, move, hide } = tooltipPosition;
const tooltipTitle = ref("");

const showEnemyStatTooltip = (event: MouseEvent, label: string) => {
  tooltipTitle.value = label;
  showAt(event, { maxWidth: 150, placement: tooltipPlacement });
};

const hideEnemyStatTooltip = () => {
  hide();
};

const moveEnemyStatTooltip = (event: MouseEvent) => {
  move(event, tooltipPlacement);
};

watch(() => props.isOpen, (open) => {
  if (!open) {
    hide();
  }
});

const enemyImageUrl = computed(() => {
  if (!props.enemyId) {
    return null;
  }
  return ImageCatalog.getEnemyIconUrl(enemy.value.name);
});
</script>

<template>
  <Modal
    :is-open="isModalOpen"
    max-width="max-w-250"
    @close="handleClose"
  >
    <template #header>
      <h2 class="text-white font-bold tracking-widest drop-shadow flex items-center gap-2">
        {{ enemy.name || $t("enemy.detailsTitle") }}
      </h2>
    </template>

    <div class="p-4 flex flex-col sm:flex-row gap-4 max-h-[70vh] overflow-y-auto custom-scroll">
      <div class="flex flex-col gap-4 flex-1">
        <div class="flex gap-4">
          <div class="w-1/2 aspect-square bg-[#000a1a] border border-blue-900/50 rounded flex items-center justify-center shadow-inner relative group overflow-hidden">
            <div class="absolute inset-0 opacity-10 bg-linear-to-br from-blue-500/20 to-transparent pointer-events-none"></div>
            <img
              v-if="enemyImageUrl"
              :src="enemyImageUrl"
              class="w-full h-full object-cover drop-shadow-[0_0_15px_rgba(0,170,255,0.4)] group-hover:scale-110 transition-transform duration-500"
              :alt="enemy.name"
            />
            <div v-else class="text-4xl opacity-30 select-none">❓</div>
          </div>

          <div class="w-1/2 bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner flex flex-col justify-between gap-1.5 py-4">
            <div class="flex items-center justify-between text-[11px]">
              <span class="font-bold text-blue-500 tracking-wider uppercase">{{ $t("enemy.specie") }}:</span>
              <span class="font-bold text-gray-300 capitalize">{{ $t(`species.${enemy.species}`) }}</span>
            </div>

            <div class="flex items-center justify-between text-[11px]">
              <span class="font-bold text-blue-500 tracking-wider uppercase">{{ $t("enemy.level") }}:</span>
              <span class="font-bold text-gray-300">{{ enemy.level }}</span>
            </div>

            <div class="flex items-center justify-between text-[11px]">
              <span class="font-bold text-blue-500 tracking-wider uppercase">HP:</span>
              <span class="font-bold text-white">{{ enemy.hp }}</span>
            </div>

            <div class="flex items-center justify-between text-[11px]">
              <span class="font-bold text-blue-500 tracking-wider uppercase">MP:</span>
              <span class="font-bold text-white">{{ enemy.mp }}</span>
            </div>

            <div class="flex items-center justify-between text-[11px]">
              <span class="font-bold text-blue-500 tracking-wider uppercase">{{ $t("enemy.baseExp") }}:</span>
              <span class="font-bold text-gray-300">{{ enemy.exp }}</span>
            </div>

            <div class="flex items-center justify-between text-[11px]">
              <span class="font-bold text-blue-500 tracking-wider uppercase">DVEXP:</span>
              <span class="font-bold text-gray-300">{{ enemy.dvxp }}</span>
            </div>

            <div class="flex items-center justify-between text-[11px]">
              <span class="font-bold text-blue-500 tracking-wider uppercase">{{ $t("enemy.baseBits") }}:</span>
              <span class="font-bold text-gray-300">{{ enemy.bits }}</span>
            </div>

            <div class="flex flex-col text-[11px] mt-1">
              <span class="font-bold text-blue-500 tracking-wider uppercase mb-1">{{ $t("enemy.possibleDrop") }}:</span>
              <span
                class="font-bold text-gray-300 text-left"
                :title="enemy.drops && getLocalized(enemy.drops) !== 'N/A' ? getLocalized(enemy.drops) : t('drops.none')"
              >
                {{ enemy.drops && getLocalized(enemy.drops) !== "N/A" ? getLocalized(enemy.drops) : t("drops.None") }}
              </span>
            </div>
          </div>
        </div>

        <div class="bg-[#000a1a] border border-red-900/30 rounded p-4 shadow-inner text-sm mt-auto">
          <h4 class="text-[10px] uppercase font-bold tracking-[0.2em] text-red-500 mb-4 border-b border-red-900/30 pb-1">{{ $t("enemy.combatActions") }}</h4>

          <div class="flex gap-6">
            <div class="flex-1 flex flex-col gap-1.5">
              <span class="text-[9px] text-gray-500 uppercase font-bold tracking-wider">{{ $t("enemy.regularAttack") }}</span>
              <span class="text-gray-200 text-xs">
                {{ enemy.regularAttack ? getLocalized(enemy.regularAttack) : "Unknown" }}
              </span>
            </div>

            <div class="flex-1 flex flex-col gap-1.5">
              <span class="text-[9px] text-gray-500 uppercase font-bold tracking-wider">{{ $t("enemy.technique") }}</span>
              <span class="text-gray-200 text-xs">
                {{ enemy.technique ? getLocalized(enemy.technique) : "None" }}
              </span>
            </div>
          </div>
        </div>
      </div>

      <div class="flex-1">
        <div class="bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner flex flex-row justify-around gap-6 h-full items-start">
          <div class="flex-1 flex flex-col items-center gap-1.5">
            <h4 class="text-[10px] uppercase font-bold tracking-widest text-blue-500 mb-2 border-b border-blue-900/30 pb-1 w-full text-center">{{ $t("enemy.attr") }}</h4>
            <div v-for="attr in attributesList" :key="attr.label" class="flex items-center justify-center gap-2 w-full">
              <div
                class="flex items-center w-5 justify-center cursor-help select-none z-20"
                @mouseenter="showEnemyStatTooltip($event, attr.label)"
                @mousemove="moveEnemyStatTooltip"
                @mouseleave="hideEnemyStatTooltip"
              >
                <span class="text-[16px] font-emoji opacity-90 drop-shadow-[0_0_2px_rgba(255,255,255,0.7)] -translate-y-[2.5px]" :class="attr.color">{{ attr.icon }}</span>
              </div>
              <div class="font-bold tracking-widest text-shadow text-white text-sm w-12 text-center">
                {{ attr.val }}
              </div>
            </div>
          </div>

          <div class="flex-1 flex flex-col items-center gap-1.5 border-l border-blue-900/10 pl-3">
            <h4 class="text-[10px] uppercase font-bold tracking-widest text-blue-500 mb-2 border-b border-blue-900/30 pb-1 w-full text-center">{{ $t("enemy.elem") }}</h4>
            <div v-for="res in elemTolsList" :key="res.label" class="flex items-center justify-center gap-2 w-full">
              <div
                class="flex items-center w-5 justify-center cursor-help select-none z-20"
                @mouseenter="showEnemyStatTooltip($event, res.label)"
                @mousemove="moveEnemyStatTooltip"
                @mouseleave="hideEnemyStatTooltip"
              >
                <span class="text-[16px] font-emoji opacity-90 drop-shadow-[0_0_2px_rgba(255,255,255,0.7)] -translate-y-[2.5px]" :class="res.color">{{ res.icon }}</span>
              </div>
              <div class="font-bold tracking-widest text-shadow text-white text-sm w-12 text-center">
                {{ res.val }}
              </div>
            </div>
          </div>

          <div class="flex-1 flex flex-col items-center gap-1.5 border-l border-blue-900/10 pl-3">
            <h4 class="text-[10px] uppercase font-bold tracking-widest text-blue-500 mb-2 border-b border-blue-900/30 pb-1 w-full text-center">{{ $t("enemy.status") }}</h4>
            <div v-for="st in statusTolsList" :key="st.label" class="flex items-center justify-center gap-2 w-full">
              <div
                class="flex items-center w-5 justify-center cursor-help select-none z-20"
                @mouseenter="showEnemyStatTooltip($event, st.label)"
                @mousemove="moveEnemyStatTooltip"
                @mouseleave="hideEnemyStatTooltip"
              >
                <span class="text-[16px] font-emoji opacity-90 drop-shadow-[0_0_2px_rgba(255,255,255,0.7)] -translate-y-[2.5px]" :class="st.color">{{ st.icon }}</span>
              </div>
              <div class="font-bold tracking-widest text-shadow text-sm w-12 text-center" :class="st.valColor">
                {{ st.val }}
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </Modal>

  <Tooltip
    :show="tooltipShow"
    :x="tooltipX"
    :y="tooltipY"
    :title="tooltipTitle"
    :max-width="150"
    placement="below"
  />
</template>
