# **Digivice** - *Your Digimon World 2003 second screen*

![GitHub downloads](https://img.shields.io/github/downloads/jeanheck/digivice-backend/total)

Digimon World 2003 hides a lot behind menus and guesswork. Digivice is a second screen for that world: while you play on Duckstation, it shows the live state of your party, the map, your digievolutions, and how far you are in the story — the way a Nintendo 3DS companion screen might have felt, if the PS1 era had one.

- See the EXP each of your Digimons needs to reach the next level
- Discover enemies you can meet on each map
- Have a journal for main quests and sidequests that automatically tracks your progress as you advance
- See digievolution trees and requirements for every Digimon in your party
- Get warned whenever an Auction is happening
- Have tools to help you move around in the Seabed or Mobius Desert
- And more — the details the game makes hard to see, kept beside the game itself

It reads the PS1 memory Duckstation shares and updates in real time as you play.

It's the companion you wished you had when you first walked through Asuka Server.

[![Digivice 3.0.0 (Latest major release)](https://img.youtube.com/vi/a_r7qz0TodQ/hqdefault.jpg)](https://www.youtube.com/watch?v=a_r7qz0TodQ)

## The current state

**Right now, Digivice only fully supports the game up to the West Sector of Asuka Server** (the Third part of the story). The main quest, map enemies, and related data are only complete through this sector.

Past that point, many features will still work, but others will be missing. I'll fill in the next areas in future updates.

## Roadmap

More Digivice is on the way. Here's a preview of what's planned next — details and timing may change as development continues.

### 3.5 — Card battles & the world around them

Digimon World 2003's card game, finally with a second screen to match.

- **TCG tools** — support for the in-game card game, right inside Digivice
- **Richer battle data** — deeper insight into Digimon vs Digimon fights
- **Trainer finder** — locate trainers ready to battle with cards or with their Digimon
- **And more** — smaller quality-of-life wins along the way

### 4.0 — The story catches up to Asuka City

A major step forward for anyone playing through the main quest.

- **Main quest through A.o.A** — journal and tracking mapped up to the A.o.A battle in Asuka City
- **DVEXP bars that make sense** — fixed to reflect each digievolution's Tier correctly
- **And more** — the next layer of polish once the South Sector ceiling is gone

## Before you continue

**This only supports Digimon World 2003. It will not work on Digimon World 3.**

Game data here comes from public sources on the web (see Reference Data Credits below). You're free to reuse any of it in your own projects.

I don't own any rights to the game or to Digimon. Even the name "Digivice" comes from that universe. **All copyrights belong to their respective owners.** This is a study project — learning combined with a game that marked my childhood.

## Requirements

**Platform:** Windows. Digivice is an `.exe` you run on your PC.  
**Emulator:** **Must be Duckstation.** Support for other emulators may come later.

## How to use

1. Set up Duckstation so you can run a game.
2. Go to **Settings → Advanced → Interface Settings** and enable **Show Debug Menu**.
3. A **Debugging** menu will appear. Open it.
4. Enable **Export Memory Shared**.
5. Download the latest Digivice from the **Releases** tab and extract the ZIP into its own folder.
6. Start Digimon World 2003 in Duckstation.
7. Play until your Digimon party is with you on the map (on a new game, after you're sent to the Digimon Lab).
8. With the game running, double-click `digivice.exe`.
9. Allow Windows to run it if prompted.
10. If everything is set up correctly, the Digivice window should open and work.

## Reference data credits

This project uses public game data from sources across the web. The originals are listed below if you want to explore them.

None of this would have been possible without the people who dug up and shared that information. Thank you all.

- **Experience levels** *by Mehdi*: [GameFAQs](https://gamefaqs.gamespot.com/boards/562323-digimon-world-3/64473556)
- **Digimon World 3 – Digivolution Guide** *by dudeice997*: [GameFAQs](https://gamefaqs.gamespot.com/ps/562323-digimon-world-3/faqs/72629)
- **Digimon World 3/Enemy**: [DigimonWiki](https://digimon.fandom.com/wiki/Digimon_World_3/Enemy)
- **Digimon World 3 – Bestiary** *by TheFulgorah*: [GameFAQs](https://gamefaqs.gamespot.com/ps/562323-digimon-world-3/faqs/66315)
- **dmw3-tools** *by markisha64* [GitHub](https://github.com/markisha64/dmw3-tools)
- **Attributes**: [StratsWiki](https://stratswiki.com/digimon-world-3/game-mechanics/attributes/)
- **All Weapons & Shields**: [StratsWiki](https://stratswiki.com/digimon-world-3/items/weapons-shields/)
- **Body Gear**: [StratsWiki](https://stratswiki.com/digimon-world-3/items/body-gear/)
- **Head Gear**: [StratsWiki](https://stratswiki.com/digimon-world-3/items/head-gear/)
- **Accessories**: [StratsWiki](https://stratswiki.com/digimon-world-3/items/accessories/)
- **GameShark Codes for Digimon World 2003 (PAL VERSION)** *by Iamhim*: [GameFAQs](https://gamefaqs.gamespot.com/boards/562323-digimon-world-3/80568401)
- **Digimon World 2003 PAL VERSION GAMESHARK CODES (CONVERTED)** *by splakappa*: [GameFAQs](https://gamefaqs.gamespot.com/boards/562323-digimon-world-3/62602581)
- **ENTENDA AS FRAQUEZAS E TOLERÂNCIAS NO DIGIMON WORLD 3** *by Battle Dice*: [YouTube](https://www.youtube.com/watch?v=6UbWt7AyHMI)
- **Digimon World 3 – Item List** *by Med_Jai*: [GameFAQs](https://gamefaqs.gamespot.com/ps/562323-digimon-world-3/faqs/24593)
- **Digimon World 3 – FAQ (European)** *by Mykas0*: [GameFAQs](https://gamefaqs.gamespot.com/ps/562323-digimon-world-3/faqs/21889)
- **Digimon World 3 – Guides and FAQs** *by nick1n*: [GameFAQs](https://gamefaqs.gamespot.com/ps/562323-digimon-world-3/map/16028-underground-seabed-map)
- **Digimon World 3 – Detonado Completo (Guia Passo a Passo)** *by Markus Norat*: [revolutionarena](https://revolutionarena.com/pt-br/digimon-world-3-detonado-completo-guia-passo-a-passo/)
- **Digimon World 3 (PS1 Gameplay Português PT-BR)** *by DashGames*: [YouTube](https://www.youtube.com/watch?v=rW8wxZceOjI&list=PLZDYs951OahD_TT4QUByvOEBTRJE-amAi)
- **DRI locations** *by rentz14*: [NeoSeeker](https://www.neoseeker.com/forums/3546/t1102277-dri-locations/)
- **Digimon World 3 – Guides and FAQs** *by HRahman*: [GameFAQs](https://gamefaqs.gamespot.com/ps/562323-digimon-world-3/map/565-mobius-desert-map)
- **Digivolution Experience Points (DVEXP)** *by Rob01m*: [GameFAQs](https://gamefaqs.gamespot.com/boards/562323-digimon-world-3/47657837)

## Digimons images credits

Below are the links to where the Digimon images were taken from, as well as the names of the users who posted the respective images (some of them ara Bandai Official Arts).

### Enemies

- **Airdramon(Gold)** *by ElissabethHaser* [Deviant Art](https://www.deviantart.com/elissabethhaser/art/Airdramon-Gold-World-3-1001557156)
- **Airdramon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Airdramon/Galeria?file=Airdramon_b.jpg)
- **Andromon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Andromon/Galeria?file=Andromon_b.jpg)
- **Apemon** *by Yuetmoon* [Digimon Wiki](https://digimon.fandom.com/wiki/Apemon?file=Apemon_b.jpg)
- **Bakemon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Bakemon/Galeria?file=Bakemon_b.jpg)
- **Baromon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Baromon?file=Baromon_b.jpg)
- **Betamon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Betamon/Galeria?file=Betamon_b.jpg)
- **Bulbmon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Bulbmon?file=Bulbmon_b.jpg)
- **Clockmon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Clockmon/Galeria?file=Clockmon_b.jpg)
- **Coelamon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Coelamon?file=Coelamon_b.jpg)
- **Crabmon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Crabmon?file=Crabmon_b.jpg)
- **Cyclonemon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Cyclonemon?file=Cyclonemon_b.jpg)
- **Datamon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Datamon?file=Datamon_b.jpg)
- **DemiDevimon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/DemiDevimon?file=DemiDevimon_b.jpg)
- **Divermon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Divermon?file=Divermon_b.jpg)
- **Dokugumon(Brown)** *by me (using the Dokugumon image as base, only changing the colors to brown tones)*
- **Dokugumon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Dokugumon?file=Dokugumon_b.jpg)
- **Dolphmon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Dolphmon?file=Dolphmon_b.jpg)
- **Etemon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Etemon?file=Etemon_b.jpg)
- **Flymon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Flymon?file=Flymon_b.jpg)
- **Gekomon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Gekomon/Galeria?file=Gekomon_b.jpg)
- **Gesomon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Gesomon/Galeria?file=Gesomon_b.jpg)
- **Gizamon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Gizamon?file=Gizamon_b.jpg)
- **Goburimon(Red)** *by DoruFlaymDraGon* [Deviant Art](https://www.deviantart.com/doruflaymdragon/art/Goburimon-Red-red-hair-913908979)
- **Goburimon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Goburimon/Galeria?file=Goburimon_b.jpg)
- **Hagurumon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Hagurumon?file=Hagurumon_b.jpg)
- **HiAndromon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Hi-Andromon/Galeria?file=Hi-Andromon_b.jpg)
- **Kiwimon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Kiwimon/Galeria?file=Kiwimon_b.jpg)
- **Kokatorimon(Brown)** *by Digital Monsters Almanac* [Digital Monsters Almanac](http://dma.wtw-x.net/dexkokatori.shtml)
- **Kokatorimon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Kokatorimon?file=Kokatorimon_b.jpg)
- **Kunemon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Kunemon/Galeria?file=Kunemon_b.jpg)
- **Kuwagamon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Kuwagamon?file=Kuwagamon_b.jpg)
- **Maildramon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Maildramon?file=Maildramon_b.jpg)
- **Mamemon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Mamemon/Galeria?file=Mamemon_b.jpg)
- **Mammothmon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Mammothmon?file=Mammothmon_b.jpg)
- **MasterTyranomon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/MasterTyranomon?file=MasterTyranomon_b.jpg)
- **Minotarumon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Lista_de_Esp%C3%A9cies?file=Minotaurmon_b.jpg)
- **Musyamon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Musyamon/Galeria?file=Musyamon_b.jpg)
- **Numemon(Blue)** *by Digital Monsters Almanac* [Digital Monsters Almanac](http://dma.wtw-x.net/dexnume.shtml)
- **Numemon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Numemon?file=Numemon_b.jpg)
- **Ogremon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Orgemon/Galeria?file=Orgemon_b.jpg)
- **Pharaohmon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Pharaohmon/Galeria?file=Pharaohmon_b.jpg)
- **Raremon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Raremon/Galeria?file=Raremon_b.jpg)
- **RedVegiemon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/RedVegimon/Galeria?file=RedVegimon_b.jpg)
- **Seadramon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Seadramon?file=Seadramon_b.jpg)
- **Shellmon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Shellmon/Galeria?file=Shellmon_b.jpg)
- **ShogunGekomon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/ShogunGekomon?file=ShogunGekomon_b.jpg)
- **Sukamon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Sukamon?file=Sukamon_b.jpg)
- **Tapirmon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Tapirmon?file=Tapirmon_b.jpg)
- **Thundermon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Thundermon?file=Thundermon_b.jpg)
- **Triceramon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Triceramon?file=Triceramon_b.jpg)
- **Tuskmon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Tuskmon?file=Tuskmon_b.jpg)
- **Tyrannomon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Tyrannomon?file=Tyrannomon_b.jpg)
- **Vademon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Vademon/Galeria?file=Vademon_b.jpg)
- **Vegiemon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Vegimon/Galeria?file=Vegimon_b.jpg)
- **WaruMonzaemon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/WaruMonzaemon?file=WaruMonzaemon_b.jpg)
- **Woodmon(Green)** *by Digital Monsters Almanac* [Digital Monsters Almanac](http://dma.wtw-x.net/dexwood.shtml)
- **Woodmon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Woodmon?file=Woodmon_b.jpg)
- **Yanmamon(Green)** *by me (using the Yanmamon image as base, only changing the colors to green tones)*
- **Yanmamon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Yanmamon?file=Yanmamon_b.jpg)
- **Zanbamon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Zanbamon?file=Zanbamon_b.jpg)

### Digievolutions

- **Angemon** *by G-SANtos* [Digimon Wiki](https://digimon.fandom.com/wiki/Angemon?file=Angemon_b.jpg)
- **Angewomon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Angewomon/Galeria?file=Angewomon_b.jpg)
- **Armormon** *by Chimera-gui* [Digimon Wiki](https://digimon.fandom.com/wiki/Armormon?file=Armormon_b.jpg)
- **Beelzemon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Beelzemon?file=Beelzemon+b.jpg)
- **BK WarGreymon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/BlackWarGreymon?file=BlackWarGreymon_b.jpg)
- **Cannondramon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Cannondramon/Galeria?file=Cannondramon_b.jpg)
- **Devimon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Devimon?file=Devimon+b.jpg)
- **Diaboromon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Diablomon?file=Diablomon_b.jpg)
- **Digitamamon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Digitamamon?file=Digitamamon_b.jpg)
- **Dinohumon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Dinohumon?file=Dinohumon_b.jpg)
- **ExVeemon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/ExVeemon?file=ExVeemon_b.jpg)
- **Gallantmon** *by Lanate* [Digimon Wiki](https://digimon.fandom.com/wiki/Gallantmon?file=Gallantmon+b.jpg)
- **GranKuwagamon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/GranKuwagamon?file=GranKuwagamon_b.jpg)
- **GrapLeomon** *by Alfius* [Digimon Wiki](https://digimon.fandom.com/wiki/GrapLeomon?file=GrapLeomon_b.jpg)
- **Greymon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Greymon?file=Greymon_b.jpg)
- **Grizzlymon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Grizzlymon?file=Grizzlymon_b.jpg)
- **Growlmon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Growlmon?file=Growlmon_b.jpg)
- **Guardiangemon** *by Magnaillusion CM* [NeoSeeker](https://digimon.neoseeker.com/wiki/File:Slashangemon.jpg)
- **Hookmon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Hookmon?file=Hookmon_b.jpg)
- **Imperialdramon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Imperialdramon:_Dragon_Mode?file=Imperialdramon_Dragon_Mode_b.jpg)
- **Imperialdramon-F** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Imperialdramon:_Fighter_Mode?file=Imperialdramon_Fighter_Mode_b.jpg)
- **Imperialdramon-P** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Imperialdramon:_Paladin_Mode?file=Imperialdramon_Paladin_Mode_b.jpg)
- **Kabuterimon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Kabuterimon?file=Kabuterimon_b.jpg)
- **Kyubimon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Kyubimon?file=Kyubimon_b.jpg)
- **Kyukimon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Kyukimon?file=Kyukimon_b.jpg)
- **MaloMyotismon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/MaloMyotismon?file=MaloMyotismon_b.jpg)
- **Marsmon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Marsmon?file=Marsmon_b.jpg)
- **MetalGarurumon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/MetalGarurumon?file=MetalGarurumon_b.jpg)
- **MetalGreymon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/MetalGreymon_(Vaccine)?file=MetalGreymon_%28Vaccine%29_b.jpg)
- **MetalMamemon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/MetalMamemon?file=MetalMamemon_b.jpg)
- **Myotismon** *by G-SANtos* [Digimon Wiki](https://digimon.fandom.com/wiki/Myotismon?file=Myotismon_b.jpg)
- **Omnimon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Omnimon?file=Omnimon_b.jpg)
- **Paildramon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Paildramon?file=Paildramon_b.jpg)
- **Phoenixmon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Phoenixmon?file=Phoenixmon_b.jpg)
- **Rosemon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Rosemon?file=Rosemon_b.jpg)
- **Sakuyamon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Sakuyamon?file=Sakuyamon_b.jpg)
- **Seraphimon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Seraphimon?file=Seraphimon_b.jpg)
- **SkullGreymon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/SkullGreymon?file=SkullGreymon_b.jpg)
- **Stingmon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Stingmon?file=Stingmon_b.jpg)
- **Taomon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Taomon?file=Taomon_b.jpg)
- **WarGreymon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/WarGreymon?file=WarGreymon_b.jpg)
- **WarGrowlmon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/WarGrowlmon?file=WarGrowlmon_b.jpg)