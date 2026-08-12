<script setup lang="ts">
import BestiaryProfileImage from "@/components/map/bestiary-modal/bestiary-profile-panel/BestiaryProfileImage.vue";
import BestiaryProfileResume from "@/components/map/bestiary-modal/bestiary-profile-panel/BestiaryProfileResume.vue";
import BestiaryProfileTechniques from "@/components/map/bestiary-modal/bestiary-profile-panel/BestiaryProfileTechniques.vue";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

defineProps<{
  enemy: EnemyViewModel;
  enemyImageUrl: string | null;
}>();

const emit = defineEmits<{
  (e: "open-drops", dropId: string): void;
}>();
</script>

<template>
  <div class="flex flex-col gap-4 flex-1">
    <div class="flex gap-4 h-72 shrink-0">
      <BestiaryProfileImage
        class="w-1/2 shrink-0"
        :enemy-image-url="enemyImageUrl"
        :enemy-name="enemy.name"
      />
      <BestiaryProfileResume
        class="w-1/2"
        :enemy="enemy"
        @open-drops="emit('open-drops', $event)"
      />
    </div>

    <BestiaryProfileTechniques :enemy="enemy" />
  </div>
</template>
