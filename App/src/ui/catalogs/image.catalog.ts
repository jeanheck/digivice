import { resolveFieldAssetName } from "@/shared/constants/field.constant";
import type { DigimonStat } from "@/shared/types/digimon-stat.type";

const MapAssetConfig = {
  pathSuffix: "/maps/",
  extension: "webp",
} as const;

const DigimonIconAssetConfig = {
  pathSuffix: "/digimons/",
  extension: "webp",
} as const;

const EnemyIconAssetConfig = {
  pathSuffix: "/enemies/",
  extension: "webp",
} as const;

const DigievolutionIconAssetConfig = {
  pathSuffix: "/digievolutions/",
  extension: "webp",
} as const;

const CardAssetConfig = {
  pathSuffix: "/cards/",
  extension: "webp",
} as const;

const BattleFieldAssetConfig = {
  pathSuffix: "/battle/",
  extension: "webp",
} as const;

const BattleJuniorAssetConfig = {
  pathSuffix: "/battle/",
  extension: "webp",
} as const;

const NpcAssetConfig = {
  pathSuffix: "/npcs/",
  extension: "webp",
} as const;

const TamerAssetConfig = {
  pathSuffix: "/tamers/",
  extension: "webp",
} as const;

const DuelIslandAssetConfig = {
  pathSuffix: "/duel-island/",
  extension: "webp",
} as const;

const BossAssetConfig = {
  pathSuffix: "/bosses/",
  extension: "webp",
} as const;

const StoreAssetConfig = {
  pathSuffix: "/stores/",
  extension: "webp",
} as const;

const StatAssetConfig = {
  pathSuffix: "/stats/",
  extension: "webp",
} as const;

const mapModules = import.meta.glob<string>("@/assets/maps/*.webp", {
  eager: true,
  query: "?url",
  import: "default",
});

const digimonIconModules = import.meta.glob<string>("@/assets/digimons/*.webp", {
  eager: true,
  query: "?url",
  import: "default",
});

const enemyIconModules = import.meta.glob<string>("@/assets/enemies/*.webp", {
  eager: true,
  query: "?url",
  import: "default",
});

const digievolutionIconModules = import.meta.glob<string>("@/assets/digievolutions/*.webp", {
  eager: true,
  query: "?url",
  import: "default",
});

const cardModules = import.meta.glob<string>("@/assets/cards/*.webp", {
  eager: true,
  query: "?url",
  import: "default",
});

const battleModules = import.meta.glob<string>("@/assets/battle/*.webp", {
  eager: true,
  query: "?url",
  import: "default",
});

const npcModules = import.meta.glob<string>("@/assets/npcs/*.webp", {
  eager: true,
  query: "?url",
  import: "default",
});

const tamerModules = import.meta.glob<string>("@/assets/tamers/*.webp", {
  eager: true,
  query: "?url",
  import: "default",
});

const duelIslandModules = import.meta.glob<string>("@/assets/duel-island/*.webp", {
  eager: true,
  query: "?url",
  import: "default",
});

const bossModules = import.meta.glob<string>("@/assets/bosses/*.webp", {
  eager: true,
  query: "?url",
  import: "default",
});

const storeModules = import.meta.glob<string>("@/assets/stores/*.webp", {
  eager: true,
  query: "?url",
  import: "default",
});

const statModules = import.meta.glob<string>("@/assets/stats/*.webp", {
  eager: true,
  query: "?url",
  import: "default",
});

function lookupInGlob(modules: Record<string, string>, pathSuffix: string): string | null {
  const normalizedSuffix = pathSuffix.replace(/\\/g, "/");
  const entry = Object.entries(modules).find(([key]) => {
    return key.replace(/\\/g, "/").endsWith(normalizedSuffix);
  });
  return entry?.[1] ?? null;
}

function getImageUrl(
  modules: Record<string, string>,
  pathSuffix: string,
  extension: string,
  fileName: string | null | undefined,
): string | null {
  if (fileName === null || fileName === undefined || fileName.trim() === "") {
    return null;
  }

  const fullPathSuffix = `${pathSuffix}${fileName}.${extension}`;
  return lookupInGlob(modules, fullPathSuffix);
}

export class ImageCatalog {
  public static getLocationImageUrl(imageName: string | null): string | null {
    return getImageUrl(mapModules, MapAssetConfig.pathSuffix, MapAssetConfig.extension, imageName);
  }

  public static getDigimonImageUrl(digimonName: string | null): string | null {
    return getImageUrl(
      digimonIconModules,
      DigimonIconAssetConfig.pathSuffix,
      DigimonIconAssetConfig.extension,
      digimonName,
    );
  }

  public static getEnemyImageUrl(enemyName: string | null): string | null {
    return getImageUrl(
      enemyIconModules,
      EnemyIconAssetConfig.pathSuffix,
      EnemyIconAssetConfig.extension,
      enemyName,
    );
  }

  public static getDigievolutionImageUrl(digievolutionName: string | null): string | null {
    return getImageUrl(
      digievolutionIconModules,
      DigievolutionIconAssetConfig.pathSuffix,
      DigievolutionIconAssetConfig.extension,
      digievolutionName,
    );
  }

  public static getCardImageUrl(cardName: string | null): string | null {
    return getImageUrl(
      cardModules,
      CardAssetConfig.pathSuffix,
      CardAssetConfig.extension,
      cardName,
    );
  }

  public static getFieldImageUrl(fieldId: number): string | null {
    const assetName = resolveFieldAssetName(fieldId);
    return getImageUrl(
      battleModules,
      BattleFieldAssetConfig.pathSuffix,
      BattleFieldAssetConfig.extension,
      assetName,
    );
  }

  public static getJuniorImageUrl(): string | null {
    return getImageUrl(
      battleModules,
      BattleJuniorAssetConfig.pathSuffix,
      BattleJuniorAssetConfig.extension,
      "Junior",
    );
  }

  public static getTamerImageUrl(imageName: string | null | undefined): string | null {
    return getImageUrl(
      tamerModules,
      TamerAssetConfig.pathSuffix,
      TamerAssetConfig.extension,
      imageName,
    );
  }

  public static getNpcImageUrl(imageName: string | null | undefined): string | null {
    return getImageUrl(npcModules, NpcAssetConfig.pathSuffix, NpcAssetConfig.extension, imageName);
  }

  public static getDuelIslandImageUrl(imageName: string | null | undefined): string | null {
    return getImageUrl(
      duelIslandModules,
      DuelIslandAssetConfig.pathSuffix,
      DuelIslandAssetConfig.extension,
      imageName,
    );
  }

  public static getBossImageUrl(imageName: string | null | undefined): string | null {
    return getImageUrl(
      bossModules,
      BossAssetConfig.pathSuffix,
      BossAssetConfig.extension,
      imageName,
    );
  }

  public static getCardShopImageUrl(imageName: string | null | undefined): string | null {
    return getImageUrl(
      storeModules,
      StoreAssetConfig.pathSuffix,
      StoreAssetConfig.extension,
      imageName,
    );
  }

  public static getStatImageUrl(stat: DigimonStat): string | null {
    const assetName = stat.charAt(0).toUpperCase() + stat.slice(1);
    return getImageUrl(statModules, StatAssetConfig.pathSuffix, StatAssetConfig.extension, assetName);
  }
}
