# **Digivice** - *Your Digimon World 2003 second screen*

![GitHub downloads](https://img.shields.io/github/downloads/jeanheck/digivice/total)

Digimon World 2003 hides a lot behind menus and guesswork. Digivice is a second screen for that world: while you play on Duckstation, it shows the live state of your party, the map, your digievolutions, and how far you are in the story — the way a Nintendo 3DS companion screen might have felt, if the PS1 era had one.

- See the EXP each of your Digimons needs to reach the next level
- Discover enemies you can meet on each map
- Have a journal for main quests and sidequests that automatically tracks your progress as you advance
- See digievolution trees and requirements for every Digimon in your party
- Get warned whenever an Auction is happening
- Have tools to help you move around in the Seabed or Mobius Desert
- And more — the details the game makes hard to see, kept beside the game itself

It's the companion you wished you had when you first walked through Asuka Server.

You can see more in the video below:

[![Digivice 4.0.0 (Latest major release)](https://img.youtube.com/vi/UEHe4sAQ5pM/hqdefault.jpg)](https://www.youtube.com/watch?v=UEHe4sAQ5pM)

[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/R5R028MD8Y)

Donations support my YouTube channel. Digivice is and will always be free (see [Donations](#donations)).

## The current state

**Right now, Digivice only fully supports the game up to the West Sector of Asuka Server** (the Third part of the story, until you beat the Game Master). The main quest, map enemies, and related data are only complete through this sector.<br>
Past that point, many features will still work, but others will be missing. I'll fill in the next areas in future updates.

## Roadmap

More Digivice is on the way. Here's a preview of what's planned next — details and timing may change as development continues.

### 4.5 — Interface and memory reading engine update

Changes to the Digivice interface and improvements to the memory reading engine.

### 5.0 — The Digi-Egg of Knowledge

Main quest update up to the point where the player obtains the Digi-Egg of Knowledge, in Amaterasu Server.

## Before you continue

**This only supports Digimon World 2003. It will not work on Digimon World 3.**<br>
Game data here comes from public sources on the web (see [Reference data credits](#reference-data-credits)). For copyright and reuse details, see [Legal notice](#legal-notice) and [License and reuse](#license-and-reuse).

## Legal notice

Digivice is an unofficial, fan-made project. It is not affiliated with, endorsed, sponsored or approved by Bandai Namco, Toei Animation or any other rights holder.

Digimon, Digimon World 2003 and all related names, characters, images, logos and other assets — including the name "Digivice" itself — are trademarks and/or copyrighted material of Bandai Namco, Toei Animation and their respective owners. **All rights belong to them.**

Digivice is a study project, built for fun and for learning, with no commercial purpose.

If you are a rights holder and would like any content removed, please [open an issue](https://github.com/jeanheck/digivice/issues) and I will take it down promptly.

## Donations

**Digivice is, and will always be, completely free.** Every feature is available to everyone.

Donations through Ko-fi or LivePix support my YouTube channel, where I share Digivice's development progress. They are not a payment for the app: donating does not grant any privilege, early access or exclusive feature.

## License and reuse

### Code and game data: free to reuse

The source code of Digivice is licensed under the [MIT License](LICENSE). You're free to use, modify and share it in any project, as long as you keep the copyright notice, giving credit to Digivice.

You're also free to reuse the game data gathered here, such as EXP tables, digievolution requirements and enemy lists. Most of it already exists publicly on the web. All I ask is that you credit Digivice and, where applicable, the original authors listed in [Reference data credits](#reference-data-credits).

### Images and other Bandai Namco assets: not covered

I can't grant any rights over Digimon images, names or other Bandai Namco assets, because I don't own them either. They are **not** covered by the MIT License.

If you decide to use them, I recommend making it clear that your project is non-commercial and fan-made, and crediting the sources you took them from, as I do in [Digimon images credits](#digimon-images-credits) at the end of this file.

Note that although those sources show the name of the user who uploaded each image, many of them describe the images as official Bandai artwork. Those users most likely don't hold the rights either; they just shared the images publicly. Crediting them helps trace where an image came from, but it is not permission to use it.

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

## Streaming and recording (OBS)

Digivice works fine as an OBS source, but a couple of Windows quirks trip most people up on their first try.

1. **Open Digivice before you add the source.** OBS only lists windows that are open and *not minimized*. If Digivice is minimized when you create the **Window Capture** source, it simply won't be in the dropdown — there's no error, it's just missing. Restore the window, then reopen the dropdown.
2. **Don't minimize it during the stream.** Windows stops drawing minimized windows, so the capture freezes on the last frame or goes black. Move Digivice to a second monitor or leave it behind another window instead.
3. **If the capture is black, change the capture method.** In the source properties, set **Capture Method** to **Windows 10 (1903 and up)**. Digivice's interface is GPU-accelerated, and the older BitBlt method can't read it.
4. **Set Window Match Priority** to **Match title, otherwise find window of same executable**, so the source reconnects on its own the next time you open Digivice.

**Game Capture will not work** with Digivice, since it hooks Direct3D/OpenGL directly. Use **Window Capture** or **Display Capture**.

## Reference data credits

This project uses public game data from sources across the web. The originals are listed below if you want to explore them.

None of this would have been possible without the people who dug up and shared that information. Thank you all.

- **Experience levels** *by Mehdi*: [GameFAQs](https://gamefaqs.gamespot.com/boards/562323-digimon-world-3/64473556)
- **Digimon World 3 – Digivolution Guide** *by icevenant*: [GameFAQs](https://gamefaqs.gamespot.com/ps/562323-digimon-world-3/faqs/72629)
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
- **Digimon World 3 – Patch Code Generation Guide** *by LupusArgentum*: [GameFAQs](https://gamefaqs.gamespot.com/ps/562323-digimon-world-3/faqs/41102)
- **Digimon World 3 – Card Battle FAQ** *by CatastrophieX*: [GameFAQs](https://gamefaqs.gamespot.com/ps/562323-digimon-world-3/faqs/18275)
- **Digimon World 3 – FAQ (European)** *by Mykas0*: [GameFAQs](https://gamefaqs.gamespot.com/ps/562323-digimon-world-3/faqs/21889)
- **Digimon World 3 – Guides and FAQs** *by nick1n*: [GameFAQs](https://gamefaqs.gamespot.com/ps/562323-digimon-world-3/map/16028-underground-seabed-map)
- **Digimon World 3 – Detonado Completo (Guia Passo a Passo)** *by Markus Norat*: [revolutionarena](https://revolutionarena.com/pt-br/digimon-world-3-detonado-completo-guia-passo-a-passo/)
- **Digimon World 3 (PS1 Gameplay Português PT-BR)** *by DashGames*: [YouTube](https://www.youtube.com/watch?v=rW8wxZceOjI&list=PLZDYs951OahD_TT4QUByvOEBTRJE-amAi)
- **DRI locations** *by rentz14*: [NeoSeeker](https://www.neoseeker.com/forums/3546/t1102277-dri-locations/)
- **Digimon World 3 – Guides and FAQs** *by HRahman*: [GameFAQs](https://gamefaqs.gamespot.com/ps/562323-digimon-world-3/map/565-mobius-desert-map)
- **Digivolution Experience Points (DVEXP)** *by Rob01m*: [GameFAQs](https://gamefaqs.gamespot.com/boards/562323-digimon-world-3/47657837)
- **Consumable Items**: [StratsWiki](https://stratswiki.com/digimon-world-3/items/consumables/)
- **Digimon World 3 - TCG Cards Sprites** *by metaldodomon*: [The Spriters Resource](https://www.spriters-resource.com/playstation/digiworld3/)
- **Digimon World 3 – FAQ** *by ChaosBeelzemon* [GameFAQs](https://gamefaqs.gamespot.com/ps/562323-digimon-world-3/faqs/51616)
- **Digimon World 3 – Guide and Walkthrough** *by mharbenedict34* [GameFAQs](https://gamefaqs.gamespot.com/ps/562323-digimon-world-3/faqs/33794)
- **Digimon World 2003 - Code Notes** *by Hotscrock and Dreaker* [RetroAchievements](https://retroachievements.org/codenotes.php?g=11328)
- **Digimon World 3 – Opponent's Deck List** *by SSJ2__Gohan* [GameFAQs] (https://gamefaqs.gamespot.com/ps/562323-digimon-world-3/faqs/47836)
- **DW2003 - Booster Packs complete** *by BSkullX* [GameFAQs] (https://gamefaqs.gamespot.com/boards/562323-digimon-world-3/76545344)
- **DV Experience Table** *by bugz_lha* [GameFAQs] (https://gamefaqs.gamespot.com/ps/562323-digimon-world-3/faqs/78509)
- **Circuit Board Guide** *by Shalios* [GameFAQs](https://gamefaqs.gamespot.com/ps/562323-digimon-world-3/faqs/76725/dw3-circuit-board-maps)

## Digimon images credits

Below are the links to where the Digimon images were taken from, as well as the names of the users who posted the respective images (some of them are Bandai Official Arts).

### Enemies

- **Airdramon(Gold)** *by ElissabethHaser* [Deviant Art](https://www.deviantart.com/elissabethhaser/art/Airdramon-Gold-World-3-1001557156)
- **Airdramon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Airdramon/Galeria?file=Airdramon_b.jpg)
- **Andromon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Andromon/Galeria?file=Andromon_b.jpg)
- **Apemon** *by Yuetmoon* [Digimon Wiki](https://digimon.fandom.com/wiki/Apemon?file=Apemon_b.jpg)
- **Bakemon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Bakemon/Galeria?file=Bakemon_b.jpg)
- **Baromon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Baromon?file=Baromon_b.jpg)
- **Betamon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Betamon/Galeria?file=Betamon_b.jpg)
- **Bulbmon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Bulbmon?file=Bulbmon_b.jpg)
- **Cardmon(A)** *by Me, using the follow image as inspiration* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Cardmon_C1?file=Cardmon_C1_%28World_3%29.jpg)
- **Cardmon(B)** *by Me, using the follow image as inspiration* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Cardmon_C2?file=Cardmon_C2_%28World_3%29.jpg)
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
- **Flymon(Pink)** *by Digital Monsters Almanac* [Digital Monsters Almanac](http://dma.wtw-x.net/dexfly2.shtml)
- **Flymon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Flymon?file=Flymon_b.jpg)
- **Gargoylemon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Gargoylemon?file=Gargoylemon_b.jpg)
- **Gekomon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Gekomon/Galeria?file=Gekomon_b.jpg)
- **Gesomon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Gesomon/Galeria?file=Gesomon_b.jpg)
- **Gizamon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Gizamon?file=Gizamon_b.jpg)
- **Goburimon(Red)** *by DoruFlaymDraGon* [Deviant Art](https://www.deviantart.com/doruflaymdragon/art/Goburimon-Red-red-hair-913908979)
- **Goburimon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Goburimon/Galeria?file=Goburimon_b.jpg)
- **Guardromon(Blue)** *by Me, using the follow image as inspiration* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Guardromon/Galeria?file=Guardromon_b.jpg)
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
- **Mammothmon(Silver)** *by Me, using the follow image as inspiration* [Digimon Wiki](https://digimon.fandom.com/wiki/Mammothmon?file=Mammothmon_b.jpg)
- **MasterTyranomon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/MasterTyranomon?file=MasterTyranomon_b.jpg)
- **Minotarumon(Blue)** *by Digital Monsters Almanac* [Digital Monsters Almanac](http://dma.wtw-x.net/dexminotaru.shtml)
- **Minotarumon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Lista_de_Esp%C3%A9cies?file=Minotaurmon_b.jpg)
- **Monzaemon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Monzaemon?file=Monzaemon_b.jpg)
- **Musyamon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Musyamon/Galeria?file=Musyamon_b.jpg)
- **Nanimon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Nanimon?file=Nanimon_b.jpg)
- **Numemon(Blue)** *by Digital Monsters Almanac* [Digital Monsters Almanac](http://dma.wtw-x.net/dexnume.shtml)
- **Numemon(Aqua)** *by Digital Monsters Almanac* [Digital Monsters Almanac](http://dma.wtw-x.net/dexnume.shtml)
- **Numemon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Numemon?file=Numemon_b.jpg)
- **Ogremon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Orgemon/Galeria?file=Orgemon_b.jpg)
- **Persiamon** *by ERROR101USERNOTFOUND* [Digivolutions Wiki](https://static.wikia.nocookie.net/digivolutions-digimon/images/f/f1/Persiamon.jpg/revision/latest?cb=20210504011749)
- **Pharaohmon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Pharaohmon/Galeria?file=Pharaohmon_b.jpg)
- **Raremon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Raremon/Galeria?file=Raremon_b.jpg)
- **RedVegiemon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/RedVegimon/Galeria?file=RedVegimon_b.jpg)
- **Seadramon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Seadramon?file=Seadramon_b.jpg)
- **Shellmon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Shellmon/Galeria?file=Shellmon_b.jpg)
- **ShogunGekomon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/ShogunGekomon?file=ShogunGekomon_b.jpg)
- **ShogunGekomon(Blue)** *by Me, using the follow image as inspiration* [Digimon Wiki](https://digimon.fandom.com/wiki/ShogunGekomon?file=ShogunGekomon_b.jpg)
- **Sukamon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Sukamon?file=Sukamon_b.jpg)
- **Tankmon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Tankmon/Galeria?file=Tankmon_b.jpg)
- **Tapirmon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Tapirmon?file=Tapirmon_b.jpg)
- **Thundermon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Thundermon?file=Thundermon_b.jpg)
- **Triceramon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Triceramon?file=Triceramon_b.jpg)
- **Tuskmon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Tuskmon?file=Tuskmon_b.jpg)
- **Tyrannomon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Tyrannomon?file=Tyrannomon_b.jpg)
- **Vademon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Vademon/Galeria?file=Vademon_b.jpg)
- **Vegiemon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Vegimon/Galeria?file=Vegimon_b.jpg)
- **Vemmon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Bemmon/Galeria?file=Bemmon_b.jpg)
- **WaruMonzaemon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/WaruMonzaemon?file=WaruMonzaemon_b.jpg)
- **Woodmon(Green)** *by Digital Monsters Almanac* [Digital Monsters Almanac](http://dma.wtw-x.net/dexwood.shtml)
- **Woodmon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Woodmon?file=Woodmon_b.jpg)
- **Yanmamon(Green)** *by me (using the Yanmamon image as base, only changing the colors to green tones)*
- **Yanmamon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Yanmamon?file=Yanmamon_b.jpg)
- **Zanbamon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Zanbamon?file=Zanbamon_b.jpg)
- **Kurisarimon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Chrysalimon?file=Chrysalimon_b.jpg)
- **Snimon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Snimon/Galeria?file=Snimon_b.jpg)
- **Blossomon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Blossomon?file=Blossomon_b.jpg)
- **MarineDevimon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/MarinDevimon?file=MarinDevimon_b.jpg)
- **Ebidramon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Ebidramon?file=Ebidramon_b.jpg)
- **Drimogemon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Drimogemon?file=Drimogemon_b.jpg)
- **Flarerizamon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/FlareLizamon?file=FlareLizamon_b.jpg)
- **Meramon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Meramon?file=Meramon_b.jpg)
- **Mojyamon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Mojyamon?file=Mojyamon_b.jpg)
- **Frigimon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Frigimon?file=Frigimon_b.jpg)
- **Icemon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Icemon?file=Icemon_b.jpg)
- **Antylamon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Antylamon_(Good)?file=Antylamon_%28Good%29_b.jpg)
- **Fugamon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Fugamon?file=Fugamon_b.jpg)
- **Kurisarimon(Pink)** *by Lhikan634* [Digimon Wiki](https://digimon.fandom.com/de/wiki/Kurisarimon?file=Kurisarimon_rot.png)
- **Deltamon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Deltamon?file=Deltamon_b.jpg)
- **Vikemon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Vikemon?file=Vikemon_b.jpg)
- **Kimeramon** *by Digi9346* [Digimon Wiki](https://digimon.fandom.com/wiki/Kimeramon?file=Kimeramon_b.jpg)
- **Scorpiomon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Scorpiomon?file=Scorpiomon_b.jpg)
- **MetalEtemon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/MetalEtemon?file=MetalEtemon_b.jpg)
- **Numemon(Red)** *by Digital Monsters Almanac* [Digital Monsters Almanac](http://dma.wtw-x.net/dexnume.shtml)
- **H-Kabuterimon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/HerakleKabuterimon?file=HerakleKabuterimon_b.jpg)
- **BlueMeramon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/BlueMeramon?file=BlueMeramon_b.jpg)
- **Giromon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Giromon?file=Giromon_b.jpg)
- **SkullSatamon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/SkullSatamon?file=SkullSatamon_b.jpg)
- **Devidramon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Devidramon?file=Devidramon_b.jpg)
- **Vilemon(Purple)** *by Digital Monsters Almanac* [Digital Monsters Almanac](http://dma.wtw-x.net/dexvile2.shtml)
- **MetalSeadramon** *by Digigamma* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/MetalSeadramon?file=MetalSeadramon_b.jpg)
- **Dragomon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Dagomon?file=Dagomon_b.jpg)
- **Pukumon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Pukumon?file=Pukumon_b.jpg)
- **Brachiomon** *by Chimera-gui* [Digimon Wiki](https://digimon.fandom.com/wiki/Brachiomon?file=Brachiomon_b.jpg)
- **MetalTyrannomon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/MetalTyrannomon?file=MetalTyrannomon_b.jpg)
- **Vilemon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Vilemon?file=Vilemon_b.jpg)
- **Phantomon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Fantomon?file=Fantomon_b.jpg)
- **Mummymon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Mummymon?file=Mummymon_b.jpg)
- **Okuwamon** *by Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Okuwamon?file=Okuwamon_b.jpg)
- **Tortomon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Tortamon?file=Tortamon_b.jpg)
- **SkullMeramon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/SkullMeramon?file=SkullMeramon_b.jpg)
- **Cherrymon** *by KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Cherrymon?file=Cherrymon_b.jpg)
- **Cherrymon(Red)** *by Me, using the follow image as inspiration* [Digimon Wiki](https://digimon.fandom.com/wiki/Cherrymon?file=Cherrymon_b.jpg)
- **Musyamon(Green)** *by VilmonDigibits* [Deviant Art](https://www.deviantart.com/vilmondigibits/art/Musyamon-(Green)-(World-3)-1203254714)

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

### DNA Digievolutions

- **Agunimon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Agnimon/Galeria?file=Agnimon_b.jpg)
- **AncientGreymon** *by ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/AncientGreymon?file=AncientGreymon_b.jpg)
- **BladeGarurumon** *by Wikimon* [Wikimon](https://wikimon.net/Garummon)