import { computed, ref, watch, type ComputedRef, type Ref } from "vue";

export function useMapFrame(
  imageUrl: Ref<string | null> | ComputedRef<string | null>,
  width: Ref<number> | ComputedRef<number>,
) {
  const imageNaturalSize = ref<{ width: number; height: number } | null>(null);

  const displayHeight = computed(() => {
    if (imageNaturalSize.value === null) {
      return Math.round(width.value * 0.75);
    }

    return Math.round(
      width.value * (imageNaturalSize.value.height / imageNaturalSize.value.width),
    );
  });

  const mapImageFrameStyle = computed(() => {
    return {
      width: `${width.value}px`,
      height: `${displayHeight.value}px`,
    };
  });

  function onImageLoad(event: Event): void {
    const imageElement = event.target as HTMLImageElement;

    if (imageElement.naturalWidth === 0) {
      return;
    }

    imageNaturalSize.value = {
      width: imageElement.naturalWidth,
      height: imageElement.naturalHeight,
    };
  }

  watch(imageUrl, () => {
    imageNaturalSize.value = null;
  });

  return {
    displayHeight,
    mapImageFrameStyle,
    onImageLoad,
  };
}
