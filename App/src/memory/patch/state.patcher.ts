import type { State } from "@/state/models";
import { storeLogger } from "@/core/logger/logger";

type PlainObject = Record<string, unknown>;

const ListKeys: Record<string, string> = {
  slots: "index",
  digievolutions: "index",
  storedDigievolutions: "digievolutionId",
  sideQuests: "id",
  legendaryWeapons: "id",
  driAgents: "id",
  duelIsland: "id",
  requisites: "id",
  battles: "id",
  steps: "number",
};

// Mutates the target in place so Vue only reacts to the values that actually changed.
export class StatePatcher {
  // Paths of digievolution slots whose filled → empty transition was already reported.
  private readonly ignoredEmptyDigievolutionSlots = new Set<string>();

  private static isPlainObject(value: unknown): value is PlainObject {
    return typeof value === "object" && value !== null && !Array.isArray(value);
  }

  private static getItemKey(item: unknown, property: string, position: number): unknown {
    const keyProperty = ListKeys[property];
    if (keyProperty === undefined || !StatePatcher.isPlainObject(item)) {
      return position;
    }

    return item[keyProperty];
  }

  private patchObject(target: PlainObject, source: PlainObject, path: string): void {
    for (const [property, sourceValue] of Object.entries(source)) {
      this.patchProperty(target, property, sourceValue, `${path}.${property}`);
    }
  }

  private patchProperty(
    target: PlainObject,
    property: string,
    sourceValue: unknown,
    path: string,
  ): void {
    const targetValue = target[property];

    if (Array.isArray(targetValue) && Array.isArray(sourceValue)) {
      this.patchList(targetValue, sourceValue, property, path);
      return;
    }

    if (StatePatcher.isPlainObject(targetValue) && StatePatcher.isPlainObject(sourceValue)) {
      this.patchObject(targetValue, sourceValue, path);
      return;
    }

    if (targetValue !== sourceValue) {
      target[property] = sourceValue;
    }
  }

  private removeMissingItems(target: unknown[], sourceKeys: Set<unknown>, property: string): void {
    // Stored digievolutions only grow: an entry missing from the RAM stays in the state.
    if (property === "storedDigievolutions") {
      return;
    }

    for (let position = target.length - 1; position >= 0; position--) {
      if (!sourceKeys.has(StatePatcher.getItemKey(target[position], property, position))) {
        target.splice(position, 1);
      }
    }
  }

  private patchList(target: unknown[], source: unknown[], property: string, path: string): void {
    const sourceKeys = new Set(
      source.map((item, position) => StatePatcher.getItemKey(item, property, position)),
    );
    this.removeMissingItems(target, sourceKeys, property);

    source.forEach((sourceItem, position) => {
      const key = StatePatcher.getItemKey(sourceItem, property, position);
      const targetPosition = target.findIndex(
        (targetItem, itemPosition) =>
          StatePatcher.getItemKey(targetItem, property, itemPosition) === key,
      );

      if (targetPosition === -1) {
        target.push(sourceItem);
        return;
      }

      const targetItem = target[targetPosition];
      const itemPath = `${path}[${String(key)}]`;
      if (StatePatcher.isPlainObject(targetItem) && StatePatcher.isPlainObject(sourceItem)) {
        this.patchListItem(targetItem, sourceItem, property, itemPath);
        return;
      }

      if (targetItem !== sourceItem) {
        target[targetPosition] = sourceItem;
      }
    });
  }

  private patchListItem(
    target: PlainObject,
    source: PlainObject,
    property: string,
    path: string,
  ): void {
    if (property === "digievolutions") {
      this.patchDigievolutionSlot(target, source, path);
      return;
    }

    if (property === "slots" && target.digimonId !== source.digimonId) {
      this.forgetIgnoredDigievolutionSlots(path);
      target.digimonId = source.digimonId;
      target.digimon = source.digimon;
      return;
    }

    this.patchObject(target, source, path);
  }

  private forgetIgnoredDigievolutionSlots(digimonSlotPath: string): void {
    for (const path of this.ignoredEmptyDigievolutionSlots) {
      if (path.startsWith(`${digimonSlotPath}.`)) {
        this.ignoredEmptyDigievolutionSlots.delete(path);
      }
    }
  }

  private patchDigievolutionSlot(target: PlainObject, source: PlainObject, path: string): void {
    if (target.digievolutionId !== null && source.digievolutionId === null) {
      if (!this.ignoredEmptyDigievolutionSlots.has(path)) {
        this.ignoredEmptyDigievolutionSlots.add(path);
        storeLogger.warn(`Digievolution slot ${path}: filled → empty transition ignored.`);
      }
      return;
    }

    this.ignoredEmptyDigievolutionSlots.delete(path);
    this.patchObject(target, source, path);
  }

  public patch(target: State, source: State): void {
    this.patchObject(target as unknown as PlainObject, source as unknown as PlainObject, "state");
  }
}
