import { useI18n } from 'vue-i18n';
import MainQuestTable from '../database/MainQuestTable.json';
import FolderBagTable from '../database/FolderBagTable.json';
import FishingPoleTable from '../database/FishingPoleTable.json';
import TreeBootsTable from '../database/TreeBootsTable.json';
import ConsumableItemsTable from '../database/ConsumableItemsTable.json';
import ImportantItemsTable from '../database/ImportantItemsTable.json';

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

    // Helper to translate requisites
    const translateRequisites = (requisites: any[], localRequisites?: any[]) => {
      if (!requisites || !Array.isArray(requisites)) return requisites;
      return requisites.map((p, index) => {
        const id = p.id || p.Id;
        if (id) {
          const localizedName = getLocalizedItemName(id);
          if (localizedName) {
            // Apply translation to both casing variations for UI compatibility
            return { 
              ...p, 
              description: localizedName,
              Description: localizedName
            };
          }
        } else if (localRequisites && localRequisites[index]) {
          const localDescription = localRequisites[index].Description || localRequisites[index].description;
          if (localDescription) {
            return {
              ...p,
              description: localDescription,
              Description: localDescription
            };
          }
        }
        return p;
      });
    };

    // Translate quest-level requisites
    const questRequisites = enriched.requisites || enriched.Requisites || enriched.prerequisites || enriched.Prerequisites;
    if (questRequisites) {
      const localQuestRequisites = localTable ? ((localTable as any).Requisites || (localTable as any).requisites || (localTable as any).Prerequisites || (localTable as any).prerequisites) : undefined;
      const translated = translateRequisites(questRequisites, localQuestRequisites);
      // Update both to be safe
      enriched.requisites = translated;
      enriched.Requisites = translated;
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

          let localStepObj = null;
          if (localSteps) {
            const stepNum = step.number !== undefined ? step.number : step.Number;
            localStepObj = localSteps.find((s: any) => Number(s.Number) === Number(stepNum));
          }

          // Ensure both isDone and isCompleted are set for backwards compatibility / UI robustness
          const stepDone = step.isDone !== undefined ? step.isDone : (step.IsDone || step.isCompleted || step.IsCompleted || false);
          enrichedStep.isDone = stepDone;
          enrichedStep.IsDone = stepDone;
          enrichedStep.isCompleted = stepDone;
          enrichedStep.IsCompleted = stepDone;

          // Translate step-level item requisites (check both cases)
          const stepRequisites = enrichedStep.requisites || enrichedStep.Requisites || enrichedStep.prerequisites || enrichedStep.Prerequisites;
          if (stepRequisites) {
            const localStepRequisites = localStepObj ? (localStepObj.Requisites || localStepObj.requisites || localStepObj.Prerequisites || localStepObj.prerequisites) : undefined;
            const translated = translateRequisites(stepRequisites, localStepRequisites);
            enrichedStep.requisites = translated;
            enrichedStep.Requisites = translated;
          }

          // Translate step description from local table
          if (localStepObj) {
            enrichedStep.description = localStepObj.Description;
            enrichedStep.Description = localStepObj.Description;

            // Geographic info from local JSON
            enrichedStep.locationOnMap = getLocalized(localStepObj.LocationOnMap || localStepObj.locationOnMap);
            enrichedStep.LocationOnMap = enrichedStep.locationOnMap;

            if (localStepObj.LocationOnMapCoordinates) {
              const coords = localStepObj.LocationOnMapCoordinates;
              enrichedStep.locationOnMapCoordinates = {
                x: coords.X !== undefined ? coords.X : coords.x,
                y: coords.Y !== undefined ? coords.Y : coords.y
              };
              enrichedStep.LocationOnMapCoordinates = enrichedStep.locationOnMapCoordinates;
            }

            if (localStepObj.Locations) {
              enrichedStep.locations = localStepObj.Locations.map((loc: any) => ({
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

          return enrichedStep;
        });
        // Sync enriched.Steps if it exists
        enriched.Steps = enriched.steps;
      }
    } else {
      // Even if no local table, still translate item requisites if they exist
      const backendSteps = enriched.steps || enriched.Steps;
      if (backendSteps && Array.isArray(backendSteps)) {
        enriched.steps = backendSteps.map((step: any) => {
          let enrichedStep = { ...step };

          // Ensure both isDone and isCompleted are set for backwards compatibility / UI robustness
          const stepDone = step.isDone !== undefined ? step.isDone : (step.IsDone || step.isCompleted || step.IsCompleted || false);
          enrichedStep.isDone = stepDone;
          enrichedStep.IsDone = stepDone;
          enrichedStep.isCompleted = stepDone;
          enrichedStep.IsCompleted = stepDone;

          const stepRequisites = enrichedStep.requisites || enrichedStep.Requisites || enrichedStep.prerequisites || enrichedStep.Prerequisites;
          if (stepRequisites) {
            const translated = translateRequisites(stepRequisites);
            enrichedStep.requisites = translated;
            enrichedStep.Requisites = translated;
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
