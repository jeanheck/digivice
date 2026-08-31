import map from "./map.json";
import enemy from "./enemy/enemy.json";
import species from "./enemy/specie.json";
import conditions from "./enemy/condition.json";
import drops from "./enemy/drop.json";
import boosters from "./tcg/booster.json";
import stores from "./tcg/stores.json";
import decks from "./tcg/deck.json";
import cards from "./tcg/cards.json";
import cardType from "./tcg/card-type.json";
import regularAttacks from "./enemy/enemy-regular-attack.json";
import enemyTechniques from "./enemy/enemy-technique.json";
import techniqueTypes from "./digievolution/technique-type.json";
import digimon from "./digimon.json";
import stat from "./stat.json";
import equipments from "./equipment/equipment.json";
import equipmentType from "./equipment/equipment-type.json";
import consumableItems from "./consumable-item/consumable-item.json";
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
import superNova from "./quest/legendary-weapons/super-nova.json";
import punishment from "./quest/legendary-weapons/punishment.json";
import driAgentGuilmon from "./quest/dri-agents/dri-agent-guilmon.json";
import driAgentAgumon from "./quest/dri-agents/dri-agent-agumon.json";
import driAgentVeemon from "./quest/dri-agents/dri-agent-veemon.json";
import driAgentKumamon from "./quest/dri-agents/dri-agent-kumamon.json";
import driAgentMonmon from "./quest/dri-agents/dri-agent-monmon.json";
import driAgentKotemon from "./quest/dri-agents/dri-agent-kotemon.json";
import driAgentRenamon from "./quest/dri-agents/dri-agent-renamon.json";
import driAgentPatamon from "./quest/dri-agents/dri-agent-patamon.json";
import asukaTrophy from "./quest/duel-island/asuka-trophy.json";
import player from "./player.json";
import party from "./party.json";
import connection from "./connection.json";
import auction from "./auction.json";
import npc from "./npc.json";
import tamer from "./tamer.json";
import duelIsland from "./duel-island.json";
import npcs from "./npcs.json";

export default {
  ...map,
  ...enemy,
  ...species,
  ...conditions,
  ...drops,
  ...boosters,
  ...stores,
  ...decks,
  ...cards,
  ...cardType,
  ...regularAttacks,
  ...enemyTechniques,
  ...techniqueTypes,
  ...digimon,
  ...stat,
  ...equipments,
  ...equipmentType,
  ...consumableItems,
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
  ...superNova,
  ...punishment,
  ...driAgentGuilmon,
  ...driAgentAgumon,
  ...driAgentVeemon,
  ...driAgentKumamon,
  ...driAgentMonmon,
  ...driAgentKotemon,
  ...driAgentRenamon,
  ...driAgentPatamon,
  ...asukaTrophy,
  ...player,
  ...party,
  ...connection,
  ...auction,
  ...npc,
  ...tamer,
  ...duelIsland,
  ...npcs,
};
