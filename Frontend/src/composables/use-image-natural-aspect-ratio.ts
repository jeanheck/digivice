import { ref, watch, type ComputedRef, type Ref } from "vue";

export function useImageNaturalAspectRatio(
  imageUrl: Ref<string | null> | ComputedRef<string | null>,
) {
  const aspectRatio = ref<number | null>(null);

  watch(
    imageUrl,
    (url) => {
      aspectRatio.value = null;

      if (url === null) {
        return;
      }

      const image = new Image();
      image.onload = () => {
        if (image.naturalWidth === 0) {
          return;
        }

        aspectRatio.value = image.naturalHeight / image.naturalWidth;
      };
      image.src = url;
    },
    { immediate: true },
  );

  return {
    aspectRatio,
  };
}
