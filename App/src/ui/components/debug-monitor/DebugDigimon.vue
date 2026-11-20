<script setup lang="ts">
import { computed } from "vue";
import type { Digimon } from "@/state/models";
import { DigimonBattleSelector } from "@/state/stores/selectors/digimon-battle.selector";
import DebugBar from "@/ui/components/debug-monitor/DebugBar.vue";

const props = defineProps<{
  slotIndex: number;
  digimonId: number;
  digimon: Digimon;
  mapId: string;
}>();

const isInBattle = computed(() =>
  DigimonBattleSelector.isInBattle(props.mapId, props.digimon.inBattle),
);
</script>

<template>
  <div class="flex flex-col gap-0.5 border-l border-cyan-900 pl-2">
    <div class="text-yellow-300">
      Slot {{ props.slotIndex }} (ID: {{ props.digimonId }})
      <span class="text-gray-200">
        Lv {{ props.digimon.level }} | TP {{ props.digimon.tp }} | EXP
        {{ props.digimon.experience }} | Blast {{ props.digimon.blast }}
      </span>
    </div>

    <DebugBar label="HP" :vital="props.digimon.hp" kind="hp" />
    <DebugBar label="MP" :vital="props.digimon.mp" kind="mp" />

    <div>
      <span class="text-gray-400">Stats: </span>
      Str {{ props.digimon.attributes.strength }} Def {{ props.digimon.attributes.defense }} Spt
      {{ props.digimon.attributes.spirit }} Wis {{ props.digimon.attributes.wisdom }} Spd
      {{ props.digimon.attributes.speed }} Cha {{ props.digimon.attributes.charisma }}
    </div>
    <div>
      <span class="text-gray-400">Elem: </span>
      Fir {{ props.digimon.elements.fire }} Wat {{ props.digimon.elements.water }} Ice
      {{ props.digimon.elements.ice }} Wnd {{ props.digimon.elements.wind }} Thu
      {{ props.digimon.elements.thunder }} Mac {{ props.digimon.elements.machine }} Drk
      {{ props.digimon.elements.dark }}
    </div>
    <div>
      <span class="text-gray-400">Equips: </span>
      H {{ props.digimon.equipments.head ?? "-" }} B {{ props.digimon.equipments.body ?? "-" }} R
      {{ props.digimon.equipments.right ?? "-" }} L {{ props.digimon.equipments.left ?? "-" }} A1
      {{ props.digimon.equipments.accessory1 ?? "-" }} A2
      {{ props.digimon.equipments.accessory2 ?? "-" }}
    </div>
    <div>
      <span class="text-gray-400">Evos: </span>
      <span v-for="slot in props.digimon.digievolutions" :key="slot.index" class="mr-2">
        S{{ slot.index }}:
        <span
          v-if="slot.digievolutionId !== null && slot.digievolution !== null"
          class="text-yellow-300"
        >
          [{{ slot.digievolutionId }} Lv {{ slot.digievolution.level }} DVEXP
          {{ slot.digievolution.dvexp }}]
        </span>
        <span v-else class="text-gray-500">[Empty]</span>
      </span>
    </div>
    <div>
      <span class="text-gray-400">Active evo: </span
      >{{ props.digimon.activeDigievolutionId ?? "-" }}
    </div>
    <div>
      <span class="text-gray-400">In battle: </span>
      <span :class="isInBattle ? 'text-green-400' : 'text-gray-500'">{{ isInBattle }}</span>
      <span class="text-gray-400"> | Condition: </span>{{ props.digimon.inBattle.condition }}
    </div>
    <div class="pl-2">
      <DebugBar label="HP" :vital="props.digimon.inBattle.hp" kind="hp" />
      <DebugBar label="MP" :vital="props.digimon.inBattle.mp" kind="mp" />
      <div>
        Str {{ props.digimon.inBattle.strength }} Def {{ props.digimon.inBattle.defense }} Spd
        {{ props.digimon.inBattle.speed }}
      </div>
    </div>
  </div>
</template>
