import { ref } from "vue";

export interface HighlightedDigievolution {
  digimonId: number;
  digievolutionId: number;
}

const highlightedDigievolutions = ref<HighlightedDigievolution[]>([]);

export function useDnaFusionHighlight() {
  function highlight(digievolutions: HighlightedDigievolution[]): void {
    highlightedDigievolutions.value = digievolutions;
  }

  function clear(): void {
    highlightedDigievolutions.value = [];
  }

  function isHighlighted(digimonId: number, digievolutionId: number): boolean {
    return highlightedDigievolutions.value.some(
      (highlightedDigievolution) =>
        highlightedDigievolution.digimonId === digimonId &&
        highlightedDigievolution.digievolutionId === digievolutionId,
    );
  }

  return { highlight, clear, isHighlighted };
}
