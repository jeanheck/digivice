import { useI18n } from 'vue-i18n';
import MainQuestTable from '../data/static/MainQuestTable.json';
import FolderBagTable from '../data/static/FolderBagTable.json';
import FishingPoleTable from '../data/static/FishingPoleTable.json';
import TreeBootsTable from '../data/static/TreeBootsTable.json';

export function useLocalization() {
  const { locale, t } = useI18n();

  const getLocalized = (obj: any) => {
    if (!obj) return '';
    if (typeof obj === 'string') return obj;
    
    // Suporte para o padrão { "PT-BR": "...", "EN-US": "..." }
    const currentLocale = locale.value.toUpperCase();
    return obj[currentLocale] || obj['EN-US'] || '';
  };

  /**
   * Enriches a quest from the backend with local translations.
   * Relates backend data with frontend JSONs using the 'Id' property.
   */
  const getLocalizedQuest = (backendQuest: any) => {
    if (!backendQuest) return null;

    const enriched = { ...backendQuest };
    
    // Normalizing access to backend ID (SignalR often uses camelCase)
    const questId = backendQuest.Id || backendQuest.id || backendQuest.QuestId;
    
    // List of available quest translation tables
    const questTables = [MainQuestTable, FolderBagTable, FishingPoleTable, TreeBootsTable];
    
    // Find the matching local table by Id
    const localTable = questTables.find(table => (table as any).Id === questId);

    if (localTable) {
      // Use PascalCase for localTable properties as seen in JSON files
      enriched.title = (localTable as any).Title;
      enriched.description = (localTable as any).Description;

      const backendSteps = enriched.steps || enriched.Steps;
      const localSteps = (localTable as any).Steps;

      if (backendSteps && Array.isArray(backendSteps) && localSteps) {
        enriched.steps = backendSteps.map((step: any) => {
          // Normalize step number access and force numeric comparison
          const stepNum = step.number !== undefined ? step.number : step.Number;
          const localStep = localSteps.find((s: any) => Number(s.Number) === Number(stepNum));
          
          if (localStep) {
            return {
              ...step,
              description: localStep.Description
            };
          }
          return step;
        });
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
