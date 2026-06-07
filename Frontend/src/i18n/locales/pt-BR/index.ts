import map from "./map.json";
import enemy from "./enemy/enemy.json";
import species from "./enemy/specie.json";
import conditions from "./enemy/condition.json";
import drops from "./enemy/drop.json";
import regularAttacks from "./enemy/enemy-regular-attack.json";
import enemyTechniques from "./enemy/enemy-technique.json";
import techniqueTypes from "./digievolution/technique-type.json";
import digimon from "./digimon.json";
import stat from "./stat.json";
import equipments from "./equipment/equipment.json";
import equipmentType from "./equipment/equipment-type.json";
import location from "./location.json";
import technique from "./digievolution/technique.json";
import digivolution from "./digievolution/digievolution.json";
import journal from "./journal.json";
import mainQuest from "./quest/main-quest.json";
import fishingPole from "./quest/side-quest/fishing-pole.json";
import folderBag from "./quest/side-quest/folder-bag.json";
import treeBoots from "./quest/side-quest/tree-boots.json";
import eternally from "./quest/legendary-weapons/eternally.json";
import invincible from "./quest/legendary-weapons/invincible.json";
import muramasa from "./quest/legendary-weapons/muramasa.json";
import driAgentGuilmon from "./quest/dri-agents/dri-agent-guilmon.json";
import driAgentAgumon from "./quest/dri-agents/dri-agent-agumon.json";
import player from "./player.json";
import party from "./party.json";
import connection from "./connection.json";

export default {
  ...map,
  ...enemy,
  ...species,
  ...conditions,
  ...drops,
  ...regularAttacks,
  ...enemyTechniques,
  ...techniqueTypes,
  ...digimon,
  ...stat,
  ...equipments,
  ...equipmentType,
  ...location,
  ...technique,
  ...digivolution,
  ...journal,
  ...mainQuest,
  ...fishingPole,
  ...folderBag,
  ...treeBoots,
  ...eternally,
  ...invincible,
  ...muramasa,
  ...driAgentGuilmon,
  ...driAgentAgumon,
  ...player,
  ...party,
  ...connection,
};