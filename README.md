# Digivice (for Digimon World 2003)

This is a companion app that reads memory from the Digimon World 2003 game running in an emulator and displays real-time data in a beautiful frontend. It was developed to be your "second screen" while playing the game, showing all the data without you need to access any menu.

The Digivice will show to you many information that the game didn't, and others that the game did, but in a easist way, like:
 * The EXP each of your Digimon's need to reach the next level;
 * The enemies that you can find on each map;
 * A journal with the main quests and sidequests, that uppdates according you proceed on the quests;
 * All the digievolutions and the requirements to achieve them for each different Digimon;
 * And more!

It does it by reading the memory data of your emulator, calculating what they meaning, and showing the information on real time on your screen. Every time you level up your digimon and they status increase, or when you equip a new weapon, or when you unlock a new digievolution, everything will be shown on the Digivice in a easist way comparing with the original game.

## The current state

**At this moment, the Digivice is prepared to support ONLY the first part of the game, the Central Sector of Asuka Server**. The main quest, for example, will only contain the steps for this part. The enemies on the maps are only configured correctly on this sector. <br >
When you reach the next sector, many features will still work, but many will be missing. I will update the Digivice with the information of the next areas in the next updates.

## Before you continue

**At this moment, this only supports the game Digimon World 2003.**<br />
I cannot garantee that it will works fine on Digimon World 3.<br /><br />
All the game information used here was taken from many different sources that I found on web (see the Reference Data Credits below). So, as I could have free access to them to do this project, if you are reading this, feel free to get any data that you find here that you think that could be usefull for you develop your things.<br /><br />
Lastly, but no less importantly, I do not own any legal rights to the game, nor to the Digimon work itself. Even the name "Digivice" it's something from the Digimon context. **All copyrights belong to the respective game developers and the copyright holders of Digimon**. This is just a study project, in which I'm combining a learning experience with a game that marked my childhood.

# Requirements

**Plataform**: Windows. The Digivice was a .exe software that you will run on your PC.<br />
**Emulator**: **Must be Duckstation**. In the future, I will try to make it available for others emulators.

## How to use

* Configure your Duckstation emulator at the point that you can run a game;
* Go to the Duckstation Settings > Advanced > On "Interface Settings", mark the option check the option "Show Debug Menu";
* A new menu named "Debugging" will appear on the left. Click on it;
* Find the option "Export Memory Shared" and check it;
* Download the latest version Digivice (see the Releases tab). I recommend you to extract the ZIP content to a new folder;
* Run the Digimon World 2003 on your emulador;
* Advance on the game until you have your Digimon party running with you on the map (when the game send you to the Digimon Lab).
* While the game was running, double-click the "digivice.exe".
* If everything was right, the Digivice window will open working correctly.

## Reference Data Credits

To build this, I used public that from the game that is available along many links around the web. Below you can see the original sources, if you want. And also, nothing that I develop would be possible without those people that grinded all those information in the past. So, thanks for all of them.

- **EXP Tables for Digimon**: Data extracted from GameFAQs thread by user Mehdi. [Link here](https://gamefaqs.gamespot.com/boards/562323-digimon-world-3/64473556)
- **Digivolving Requirements**: Data extracted from GameFAQs Digimon World 3 FAQ by Med Jai. [Link here](https://gamefaqs.gamespot.com/ps/562323-digimon-world-3/faqs/72629)
- **Digimon World 3 Enemies List**: [Fandom Wiki](https://digimon.fandom.com/wiki/Digimon_World_3/Enemy)
- **Digimon World 3 Detailed Enemy Stats**: [GameFAQs Guide](https://gamefaqs.gamespot.com/ps/562323-digimon-world-3/faqs/66315)
- **DMW3 Tools**: Special thanks to [markisha64/dmw3-tools](https://github.com/markisha64/dmw3-tools) for providing the tools and internal data structures used to cross-reference game mechanics.
- **Pixelarticons**: [Pixelarticons](https://pixelarticons.com/)
- **Weather Icons**: [Erik Flowers](https://erikflowers.github.io/weather-icons/)
- **Digimon World 3 Attributes Mechanics**: [StrategyWiki](https://stratswiki.com/digimon-world-3/game-mechanics/attributes/)
- **Digimon World 3 Weapons & Shields**: [StratsWiki](https://stratswiki.com/digimon-world-3/items/weapons-shields/)
- **Digimon World 3 Body Gear**: [StratsWiki](https://stratswiki.com/digimon-world-3/items/body-gear/)
- **Digimon World 3 Head Gear**: [StratsWiki](https://stratswiki.com/digimon-world-3/items/head-gear/)
- **Digimon World 3 Accessories**: [StratsWiki](https://stratswiki.com/digimon-world-3/items/accessories/)
- **Digimon World 3 Stat Discussion**: [GameFAQs](https://gamefaqs.gamespot.com/boards/562323-digimon-world-3/45368300)
- **Digimon World 3 Mechanics & Digivolutions**: [GameFAQs](https://gamefaqs.gamespot.com/boards/562323-digimon-world-3/80568401)
- **Digimon World 3 Stat Calculation**: Data for internal stat calculations. [Link here](https://gamefaqs.gamespot.com/boards/562323-digimon-world-3/62602581)
