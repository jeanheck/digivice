<script setup lang="ts">
import type { CardBattle, DigimonBattle } from "@/state/models";
import DebugBar from "@/ui/components/debug-monitor/DebugBar.vue";

const props = defineProps<{
  digimonBattle: DigimonBattle;
  cardBattle: CardBattle;
}>();
</script>

<template>
  <section class="flex flex-col gap-0.5">
    <div class="text-cyan-300 font-bold">BATTLE</div>
    <div><span class="text-gray-400">Field: </span>{{ props.digimonBattle.field }}</div>
    <template v-if="props.digimonBattle.enemy !== null">
      <div>
        <span class="text-gray-400">Enemy: </span>
        Id {{ props.digimonBattle.enemy.id }} | Group {{ props.digimonBattle.enemy.groupId }} |
        Condition {{ props.digimonBattle.enemy.condition }}
      </div>
      <div class="pl-2">
        <DebugBar label="HP" :vital="props.digimonBattle.enemy.hp" kind="hp" />
        <div>
          Str {{ props.digimonBattle.enemy.strength }} Def
          {{ props.digimonBattle.enemy.defense }} Spd
          {{ props.digimonBattle.enemy.speed }}
        </div>
      </div>
    </template>
    <div v-else><span class="text-gray-400">Enemy: </span>-</div>
    <div><span class="text-gray-400">Card battle: </span>{{ props.cardBattle.id ?? "-" }}</div>
  </section>
</template>
