# Pierre's Roulette Shop

This is a small Stardew Valley mod that adds a daily rotation to the seed shop in an effort to declutter it on heavier modlists.<br/> 
It is highly configurable and supports [GMCM](https://github.com/spacechase0/StardewValleyMods/tree/develop/framework/GenericModConfigMenu).<br/>

## Contents

* [Description](#description)

* [Installation](#installation)

* [Configurations](#configurations)

* [Compatibility](#compatibility)

## Description

The premise of this mod is pretty simple. When you have a lot of mods adding crops and fruit trees (like the [Cornucopia mods](https://next.nexusmods.com/profile/MizuJakkaru/mods?gameId=1303) for example, your shops become really crowded and it's a pain to scroll through a all the seeds at Pierre's or the JojaMart.<br/>
There is no dependencies for this mod outside of SMAPI. There is however [GMCM](https://github.com/spacechase0/StardewValleyMods/tree/develop/framework/GenericModConfigMenu) integration for the configuration of the mod.<br/>
The mod uses a pseudo-random to refresh the shop's stock everyday so that it is different every in-game day, and on every.<br/>

Visit the [Nexus page](https://www.nexusmods.com/stardewvalley/mods/10826) for more informations.<br/>

## Installation

* Install [SMAPI](https://smapi.io/).
* Install the mod by downloading it either from [the Nexus](https://www.nexusmods.com/stardewvalley/mods/10826), or by building it through source.
<br/>

If you do not know how to install mods, [refer to this guide](https://stardewvalleywiki.com/Modding:Player_Guide/Getting_Started)<br/>

## Configurations

It is recommended to use the [Generic Mod Config Menu](https://www.nexusmods.com/stardewvalley/mods/5098) to configure the mod. It is however possible to edit the <code>config.json</code> file manually.<br/>
Here is a quick rundown of the available settings : <br/>

|Configuration|What it does|Available values|
|---|---|---|
|<code>ModEnabled</code>|Master switch for the whole mod.|*boolean*<br/> default: true|
|<code>Owners[]</code>|The list of shop owners the mod applies itself to. <br/> To add a shop, you must add the shop ID from the game or the mod and seperate them with commas as shown in the default configuration. More informations on the shop IDs is available [on the wiki](https://stardewvalleywiki.com/Modding:Shops).|*string array*<br/> default: [ "SeedShop", "Sandy" ]|
|<code>SeedsEnabled</code>|Enables or not the mod for the seeds.|*boolean*<br/> default: true|
|<code>SeedStock</code>|The number of seeds the shops should have daily in stock. Set to 0 clear the shop from seeds. Range is 0 to 64.|*integer*<br/> default: 4|
|<code>SaplingsEnabled</code>|Enables or not the mod for the saplings.|*boolean*<br/> default: true|
|<code>SaplingStock</code>|The number of saplings the shops should have daily in stock. Set to 0 clear the shop from seeds. Range is 0 to 64.|*integer*<br/> default: 2|

## Compatibility

Last time I made the mod, I said it should always be compatible with the latest game version unless there was a drastic change in the shop's code which was unlikely.<br/>
This was in fact, not so unlikely...<br/>
So for now, this mod is compatible with Stardew Valley 1.6.x and SMAPI 4.x.x. You should always check if a mod is updated to the newer version on the [SMAPI website](https://smapi.io/mods/).<br/>

As for mod compatibility, the mod here should once again be compatible with most other mods. I do not yet know how it behaves with other SMAPI mods altering shops' stock like [SVBO](https://www.nexusmods.com/stardewvalley/mods/13426). If you find an incompatibility, please report the issue here and/or on [the Nexus page](https://www.nexusmods.com/stardewvalley/mods/10826). <br/>

