<script setup lang="ts">
import { computed, ref } from "vue";
import { ImageCatalog } from "@/catalogs/image.catalog.ts";
import Tooltip from "@/components/tooltip/Tooltip.vue";
import { useLocalization } from "@/composables/useLocalization";
import { useTooltipPosition } from "@/composables/use-tooltip-position";
import type { DigimonDigievolutionRequirementViewModel } from '@/viewmodels/digimon/digimon-digievolution-requirement.viewmodel';
import { DigievolutionDetailPanelPresenter } from "@/presenters/digievolutions-modal/digievolution-detail-panel.presenter";
import type { DigimonDigievolutionViewModel } from '@/viewmodels/digimon/digimon-digievolution.viewmodel';

const props = defineProps<{
  evolution: DigimonDigievolutionRequirementViewModel[]
  evolutionName: string | undefined
  allEvolutions: string[]
  derivativeParameter: DigimonDigievolutionViewModel
}>()

const emit = defineEmits<{
  (e: "select-digievolution", name: string): void;
}>();

const { t, getLocalized } = useLocalization()

const evolutionAvatarUrl = computed(() => {
  return ImageCatalog.getDigievolutionIconUrl(props.evolutionName ?? null);
});

const techniques = computed(() => {
  if (!props.evolutionName) {
    return [];
  }

  return DigievolutionDetailPanelPresenter.getTechniquesByEvolutionName(props.evolutionName);
});

function elementColor(element: string): string {
  const map: Record<string, string> = {
    'Fire': 'text-orange-400',
    'Water': 'text-blue-400',
    'Ice': 'text-cyan-300',
    'Wind': 'text-gray-300',
    'Thunder': 'text-yellow-300',
    'Dark': 'text-purple-400',
    'Machine': 'text-gray-400',
    'None': 'text-white/60',
  }
  return map[element] ?? 'text-white/60'
}

function typeIcon(type: string): string {
  if (type === 'Physical') return '👊'
  if (type === 'Magical') return '🧙‍♂️'
  if (type === 'Heal') return '💚'
  if (type === 'Support') return '🟡'
  return '?'
}

const reqEvolutions = computed(() => {
    return props.evolution
        .filter(req => req.type === 'DigievolutionLevel')
        .map(req => req.digievolution!)
})

const derivatives = computed(() => {
  const teste = Object.entries(props.derivativeParameter).map(value => value);

  const result = teste.filter(entry => {
    const requirements = entry[1];

    return requirements.some(req => req.type === 'DigievolutionLevel' && req.digievolution === props.evolutionName)
  })

  return Object.values(result).map(value => value[0]);
})

const tooltipPlacement = "below" as const;
const tooltipPosition = useTooltipPosition(150);
const { show: tooltipShow, x: tooltipX, y: tooltipY, showAt, move, hide } = tooltipPosition;
const tooltipTitle = ref("");

const showTypeTooltip = (event: MouseEvent, type: string) => {
  const normalizedType = type || "Physical";
  tooltipTitle.value = t(`techniqueTypes.${normalizedType.toLowerCase()}`);
  showAt(event, { maxWidth: 150, placement: tooltipPlacement });
};

const hideTypeTooltip = () => {
  hide();
};

const moveTypeTooltip = (event: MouseEvent) => {
  move(event, tooltipPlacement);
};
</script>

<template>
  <div class="flex flex-col h-full bg-[#0c0d1b] rounded overflow-hidden relative">
    <!-- Header Hero -->
    <div class="relative flex-none pt-4 px-4 pb-2 flex flex-col items-center justify-center bg-linear-to-b from-[#00051a] to-transparent shrink-0">
        <div class="relative w-full h-32 bg-linear-to-r from-cyan-950/40 to-[#001533] rounded-lg border border-cyan-800/50 shadow-inner overflow-hidden group">
            
            <img v-if="evolutionAvatarUrl" 
                 :src="evolutionAvatarUrl" 
                 class="absolute inset-0 w-full h-full object-cover object-[center_15%] pointer-events-none drop-shadow-[0_0_15px_rgba(0,170,255,0.4)] transition-opacity duration-500" 
                 alt="Avatar Overlay" />

            <!-- Pattern Overlay -->
            <h2 class="absolute top-3 left-4 text-lg sm:text-xl font-bold font-cyber text-white tracking-widest drop-shadow-[0_2px_4px_rgba(0,0,0,0.9)] z-10">
                {{ evolutionName }}
            </h2>
        </div>
    </div>

    <div class="px-4 pb-4 pt-2 flex-1 flex flex-col gap-4 overflow-y-auto custom-scroll">
        <!-- Compact Base Requirements Section -->
        <div v-if="reqEvolutions.length > 0" class="shrink-0">
            <div class="text-[9px] text-indigo-400/80 mb-2 flex items-center font-cyber uppercase tracking-widest">
                <span class="text-[11px] mr-2 opacity-80 drop-shadow-[0_0_2px_rgba(255,255,255,0.7)] -translate-y-0.5">🧬</span>
                {{ $t('digievolution.requirementDigievolutions') }}
            </div>
            
            <div class="flex flex-wrap gap-2">
                <button v-for="reqEvo in reqEvolutions" :key="reqEvo"
                      @click="emit('select-digievolution', reqEvo)"
                      class="cursor-pointer hover:bg-indigo-700/50 transition-colors text-[10px] px-2 py-1.5 bg-indigo-950/30 text-indigo-200 border border-indigo-900/40 rounded font-cyber flex items-center">
                    {{ getLocalized(reqEvo) }}
                </button>
            </div>
        </div>

        <!-- Techniques Section -->
        <div class="flex flex-col">
            <div class="text-[9px] text-indigo-400/80 mb-2 flex items-center font-cyber uppercase tracking-widest">
                <span class="text-[11px] mr-2 opacity-80 drop-shadow-[0_0_2px_rgba(255,255,255,0.7)] -translate-y-0.5">⚔️</span>
                {{ $t('digievolution.techniques') }}
            </div>
            <div class="flex flex-col gap-0.75 pr-1">
                <div
                  v-for="techniqueViewModel in techniques"
                  :key="techniqueViewModel.id"
                  class="relative rounded px-3 py-2 flex items-start gap-3 border transition-all text-xs border-[#0055ff]/40 bg-[#001a33]/60"
                  :class="{ 'bg-yellow-950/30 border-yellow-500/60 shadow-[0_0_8px_rgba(234,179,8,0.2)]': techniqueViewModel.isSignature }"
                >
                  <span
                    v-if="techniqueViewModel.isSignature"
                    class="absolute top-1 right-2 text-[10px] text-yellow-400 font-bold tracking-widest"
                  >
                    ⭐
                  </span>

                  <span class="text-base leading-none mt-px shrink-0 cursor-help tooltip-anchor"
                        @mouseenter="showTypeTooltip($event, techniqueViewModel.type ?? '')"
                        @mousemove="moveTypeTooltip"
                        @mouseleave="hideTypeTooltip">
                    {{ typeIcon(techniqueViewModel.type ?? '') }}
                  </span>

                  <div class="flex-1 min-w-0">
                    <div class="flex items-center gap-1 mb-0.5">
                      <span class="font-bold tracking-wide" :class="techniqueViewModel.isSignature ? 'text-yellow-300' : 'text-white'">
                        {{ t(`${techniqueViewModel.id}.name`) }}
                      </span>
                      <span class="text-[10px] text-cyan-400/80 ml-1">{{ $t('digievolution.lv') }}.{{ techniqueViewModel.learnLevel }}</span>
                    </div>

                    <p class="text-white/50 text-[10px] leading-snug">{{ t(`${techniqueViewModel.id}.description`) }}</p>

                    <div class="flex gap-3 mt-1 text-[9px] uppercase tracking-wider">
                      <span :class="elementColor(techniqueViewModel.element ?? 'None')">
                        {{ (techniqueViewModel.element ?? 'None') !== 'None' ? t('resistances.' + (techniqueViewModel.element || 'None').toLowerCase()) : t('digievolution.neutral') }}
                      </span>
                      <span class="text-blue-300/70">MP {{ techniqueViewModel.mp }}</span>
                    </div>
                  </div>
                </div>
                
                <p v-if="techniques.length === 0" class="text-white/40 text-center py-4 text-[10px] italic font-cyber border border-white/5 rounded">
                  {{ $t('digievolution.noTechData') }}
                </p>
            </div>
        </div>

        <!-- Derivatives Section -->
        <div v-if="derivatives.length > 0" class="shrink-0">
            <div class="text-[9px] text-indigo-400/80 mb-2 flex items-center font-cyber uppercase tracking-widest">
                <span class="text-[11px] mr-2 opacity-80 drop-shadow-[0_0_2px_rgba(255,255,255,0.7)] -translate-y-0.5">🧬</span>
                {{ $t('digievolution.nextDigievolutions') }}
            </div>
            <div class="flex flex-wrap gap-2">
                <button v-for="deriv in derivatives" :key="deriv"
                      @click="emit('select-digievolution', deriv)"
                      class="cursor-pointer hover:bg-indigo-700/50 transition-colors text-[10px] px-2 py-1.5 bg-indigo-950/30 text-indigo-200 border border-indigo-900/40 rounded font-cyber flex items-center">
                    {{ getLocalized(deriv) }}
                </button>
            </div>
        </div>
        </div>
    <Tooltip
      :show="tooltipShow"
      :x="tooltipX"
      :y="tooltipY"
      :title="tooltipTitle"
      :max-width="150"
      placement="below"
    />
  </div>
</template>
