<template>
  <button
    type="button"
    class="h-6 bg-track border-2 border-progress-frame rounded flex items-center justify-center cursor-pointer hover:bg-track-hover transition-colors"
    :title="$t('digimon.digievolutions')"
    :aria-label="$t('digimon.digievolutions')"
  >
    <span
      class="leading-none text-[16px] font-emoji bg-linear-to-br from-pink-400 to-green-400 bg-clip-text text-transparent"
      >&#x1F9EC;</span
    >
  </button>
</template>
