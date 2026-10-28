<template>
  <div class="relative flex flex-col text-white dw3-beveled">
    <div class="absolute inset-0 bg-progress-frame dw3-beveled pointer-events-none"></div>
    <div class="absolute inset-[1.5px] bg-black dw3-beveled overflow-hidden pointer-events-none">
      <slot name="background" />
      <div
        class="absolute inset-x-0 top-0 h-24 bg-linear-to-b from-black/85 via-black/50 to-transparent"
      ></div>
    </div>

    <div class="relative z-10 flex-1 min-h-0 flex flex-col">
      <slot />
    </div>
  </div>
</template>
