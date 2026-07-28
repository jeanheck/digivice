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

[![Digivice 2.0 (Latest major release)](https://img.youtube.com/vi/a_r7qz0TodQ/hqdefault.jpg)](https://www.youtube.com/watch?v=a_r7qz0TodQ)

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

## Reference Data Credits

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
