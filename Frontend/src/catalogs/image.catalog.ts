const MAP_ASSET_CONFIG = {
  pathSuffix: "/maps/",
  extension: "webp",
} as const;

const DIGIMON_ICON_ASSET_CONFIG = {
  pathSuffix: "/icons/digimons/",
  extension: "png",
} as const;

const ENEMY_ICON_ASSET_CONFIG = {
  pathSuffix: "/icons/enemies/",
  extension: "png",
} as const;

const DIGIEVOLUTION_ICON_ASSET_CONFIG = {
  pathSuffix: "/icons/digievolutions/",
  extension: "png",
} as const;

const mapModules = import.meta.glob<string>("@/assets/maps/*.webp", {
  eager: true,
  as: "url",
});

const digimonIconModules = import.meta.glob<string>("@/assets/icons/digimons/*.png", {
  eager: true,
  as: "url",
});

const enemyIconModules = import.meta.glob<string>("@/assets/icons/enemies/*.png", {
  eager: true,
  as: "url",
});

const digievolutionIconModules = import.meta.glob<string>("@/assets/icons/digievolutions/*.png", {
  eager: true,
  as: "url",
});

function lookupInGlob(modules: Record<string, string>, pathSuffix: string): string | null {
  const normalizedSuffix = pathSuffix.replace(/\\/g, "/");
  const entry = Object.entries(modules).find(([key]) => {
    return key.replace(/\\/g, "/").endsWith(normalizedSuffix);
  });
  return entry?.[1] ?? null;
}

function resolveAssetUrl(
  modules: Record<string, string>,
  pathSuffix: string,
  extension: string,
  fileName: string | null
): string | null {
  if (!fileName || fileName.trim() === "") {
    return null;
  }

  const fullPathSuffix = `${pathSuffix}${fileName}.${extension}`;
  return lookupInGlob(modules, fullPathSuffix);
}

export class ImageCatalog {
  public static getMapImageUrl(imageName: string | null): string | null {
    return resolveAssetUrl(
      mapModules,
      MAP_ASSET_CONFIG.pathSuffix,
      MAP_ASSET_CONFIG.extension,
      imageName
    );
  }

  public static getDigimonIconUrl(digimonName: string | null): string | null {
    return resolveAssetUrl(
      digimonIconModules,
      DIGIMON_ICON_ASSET_CONFIG.pathSuffix,
      DIGIMON_ICON_ASSET_CONFIG.extension,
      digimonName
    );
  }

  public static getEnemyIconUrl(enemyName: string | null): string | null {
    return resolveAssetUrl(
      enemyIconModules,
      ENEMY_ICON_ASSET_CONFIG.pathSuffix,
      ENEMY_ICON_ASSET_CONFIG.extension,
      enemyName
    );
  }

  public static getDigievolutionIconUrl(digievolutionName: string | null): string | null {
    return resolveAssetUrl(
      digievolutionIconModules,
      DIGIEVOLUTION_ICON_ASSET_CONFIG.pathSuffix,
      DIGIEVOLUTION_ICON_ASSET_CONFIG.extension,
      digievolutionName
    );
  }
}
