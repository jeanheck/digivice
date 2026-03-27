import { useI18n } from 'vue-i18n';
import MainQuestTable from '../data/static/MainQuestTable.json';
import FolderBagTable from '../data/static/FolderBagTable.json';
import FishingPoleTable from '../data/static/FishingPoleTable.json';
import TreeBootsTable from '../data/static/TreeBootsTable.json';
import ConsumableItemsTable from '../data/static/ConsumableItemsTable.json';
import ImportantItemsTable from '../data/static/ImportantItemsTable.json';

export function useLocalization() {
  const { locale, t } = useI18n();

  const getLocalized = (obj: any) => {
    if (!obj) return '';
    if (typeof obj === 'string') return obj;
    
    // Suporte para o padrão { "PT-BR": "...", "EN-US": "..." }
    const currentLocale = locale.value.toUpperCase();
    return obj[currentLocale] || obj['EN-US'] || '';
  };

  const getLocalizedItemName = (itemKey: string) => {
    const allItems = [
      ...ConsumableItemsTable.ConsumableItems,
      ...ImportantItemsTable.ImportantItems
    ];
    const item = allItems.find(i => i.Id === itemKey);
    return item ? item.Name : null;
  };

  /**
   * Enriches a quest from the backend with local translations.
   * Relates backend data with frontend JSONs using the 'Id' property.
   */
  const getLocalizedQuest = (backendQuest: any) => {
    if (!backendQuest) return null;

    const enriched = { ...backendQuest };
    
    // Normalizing access to backend ID (SignalR often uses camelCase, JSON files use Pascal)
    const questId = backendQuest.Id || backendQuest.id || backendQuest.QuestId;
    
    // List of available quest translation tables
    const questTables = [MainQuestTable, FolderBagTable, FishingPoleTable, TreeBootsTable];
    
    // Find the matching local table by Id
    const localTable = questTables.find(table => (table as any).Id === questId);

    // Helper to translate prerequisites
    const translatePrereqs = (prereqs: any[]) => {
      if (!prereqs || !Array.isArray(prereqs)) return prereqs;
      return prereqs.map(p => {
        const itemKey = p.ItemKey || p.itemKey;
        if (itemKey) {
          const localizedName = getLocalizedItemName(itemKey);
          if (localizedName) {
            // Apply translation to both casing variations for UI compatibility
            return { 
              ...p, 
              description: localizedName,
              Description: localizedName
            };
          }
        }
        return p;
      });
    };

    // Translate quest-level prerequisites
    const questPrereqs = enriched.prerequisites || enriched.Prerequisites;
    if (questPrereqs) {
      const translated = translatePrereqs(questPrereqs);
      // Update both to be safe
      enriched.prerequisites = translated;
      enriched.Prerequisites = translated;
    }

    if (localTable) {
      // Use PascalCase for localTable properties as seen in JSON files
      enriched.title = (localTable as any).Title;
      enriched.description = (localTable as any).Description;

      const backendSteps = enriched.steps || enriched.Steps;
      const localSteps = (localTable as any).Steps;

      if (backendSteps && Array.isArray(backendSteps)) {
        enriched.steps = backendSteps.map((step: any) => {
          let enrichedStep = { ...step };
          
          // Translate step-level item prerequisites (check both cases)
          const stepPrereqs = enrichedStep.prerequisites || enrichedStep.Prerequisites;
          if (stepPrereqs) {
            const translated = translatePrereqs(stepPrereqs);
            enrichedStep.prerequisites = translated;
            enrichedStep.Prerequisites = translated;
          }

          // Translate step description from local table
          if (localSteps) {
            const stepNum = step.number !== undefined ? step.number : step.Number;
            const localStep = localSteps.find((s: any) => Number(s.Number) === Number(stepNum));
            
            if (localStep) {
              enrichedStep.description = localStep.Description;
              enrichedStep.Description = localStep.Description;

              // Geographic info from local JSON
              enrichedStep.locationOnMap = getLocalized(localStep.LocationOnMap || localStep.locationOnMap);
              enrichedStep.LocationOnMap = enrichedStep.locationOnMap;

              if (localStep.LocationOnMapCoordinates) {
                  const coords = localStep.LocationOnMapCoordinates;
                  enrichedStep.locationOnMapCoordinates = {
                      x: coords.X !== undefined ? coords.X : coords.x,
                      y: coords.Y !== undefined ? coords.Y : coords.y
                  };
                  enrichedStep.LocationOnMapCoordinates = enrichedStep.locationOnMapCoordinates;
              }

              if (localStep.Locations) {
                  enrichedStep.locations = localStep.Locations.map((loc: any) => ({
                      locationImage: loc.LocationImage || loc.locationImage,
                      target: getLocalized(loc.Target || loc.target),
                      locationImageCoordinates: loc.LocationImageCoordinates ? {
                          x: loc.LocationImageCoordinates.X !== undefined ? loc.LocationImageCoordinates.X : loc.LocationImageCoordinates.x,
                          y: loc.LocationImageCoordinates.Y !== undefined ? loc.LocationImageCoordinates.Y : loc.LocationImageCoordinates.y
                      } : null
                  }));
                  enrichedStep.Locations = enrichedStep.locations;
              }
            }
          }
          
          return enrichedStep;
        });
        // Sync enriched.Steps if it exists
        enriched.Steps = enriched.steps;
      }
    } else {
      // Even if no local table, still translate item prerequisites if they exist
      const backendSteps = enriched.steps || enriched.Steps;
      if (backendSteps && Array.isArray(backendSteps)) {
        enriched.steps = backendSteps.map((step: any) => {
          let enrichedStep = { ...step };
          const stepPrereqs = enrichedStep.prerequisites || enrichedStep.Prerequisites;
          if (stepPrereqs) {
            const translated = translatePrereqs(stepPrereqs);
            enrichedStep.prerequisites = translated;
            enrichedStep.Prerequisites = translated;
          }
          return enrichedStep;
        });
        enriched.Steps = enriched.steps;
      }
    }

    return enriched;
  };

  return {
    locale,
    t,
    getLocalized,
    getLocalizedQuest
  };
}
