import { computed, ref, watch, type ComputedRef, type Ref } from "vue";
import { SEABED_MAP_FRAME_WIDTH_PX } from "@/components/seabed/seabed-map-frame";

export function useSeabedMapFrame(imageUrl: Ref<string | null> | ComputedRef<string | null>) {
  const imageNaturalSize = ref<{ width: number; height: number } | null>(null);

  const displayHeight = computed(() => {
    if (imageNaturalSize.value === null) {
      return Math.round(SEABED_MAP_FRAME_WIDTH_PX * 0.75);
    }

    return Math.round(
      SEABED_MAP_FRAME_WIDTH_PX * (imageNaturalSize.value.height / imageNaturalSize.value.width)
    );
  });

  const mapImageFrameStyle = computed(() => {
    return {
      width: `${SEABED_MAP_FRAME_WIDTH_PX}px`,
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
