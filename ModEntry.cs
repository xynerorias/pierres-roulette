using GenericModConfigMenu;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace pierres_roulette;

internal sealed class ModEntry : Mod
{
    private ModConfig? _config;

    public override void Entry(IModHelper helper)
    {
        _config = helper.ReadConfig<ModConfig>();
        helper.Events.Display.MenuChanged += MenuChanged;
        helper.Events.GameLoop.GameLaunched += GameLaunched;
    }

    private void GameLaunched(object? sender, GameLaunchedEventArgs eventArgs)
    {
        // get Generic Mod Config Menu's API (if it's installed)
        var configMenu = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
        if (configMenu is null)
            return;

        // register mod
        configMenu.Register(
            ModManifest,
            () => _config = new ModConfig(),
            () => Helper.WriteConfig(_config)
        );

        // add some config options
        configMenu.AddBoolOption(
            ModManifest,
            name: () => "Enable Mod",
            tooltip: () =>
                "Master switch for the mod. Nothing happens if disabled.\n" +
                "Default : true",
            getValue: () => _config!.ModEnabled,
            setValue: value => _config!.ModEnabled = value
        );

        configMenu.AddTextOption(
            ModManifest,
            name: () => "Shop Owners",
            tooltip: () =>
                "Comma-separated list of ShopId values for shops affected by the mod.\n" +
                "See the wiki for a list of available IDs: https://stardewvalleywiki.com/Modding:Shops\n" +
                "Default : SeedShop, Sandy",
            getValue: () => string.Join(", ", _config!.Owners),
            setValue: value => _config!.Owners = value.Split(',').Select(s => s.Trim()).ToArray()
        );

        configMenu.AddBoolOption(
            ModManifest,
            name: () => "Affect Seeds",
            tooltip: () =>
                "Enable/disable seeds being affected.\n" +
                "Default : true",
            getValue: () => _config!.SeedsEnabled,
            setValue: value => _config!.SeedsEnabled = value
        );

        configMenu.AddNumberOption(
            ModManifest,
            name: () => "Seed Stock",
            tooltip: () =>
                "How much seeds does the shops have in stock every day. Does nothing if SeedsEnabled is false.\n" +
                "0 clears the stock completely. Max is 64.\n" +
                "Default : 4",
            getValue: () => _config!.SeedStock,
            setValue: value => _config!.SeedStock = value,
            min: 0,
            max: 64,
            interval: 1
        );

        configMenu.AddBoolOption(
            ModManifest,
            name: () => "Affect Saplings",
            tooltip: () =>
                "Enable/disable saplings being affected.\n" +
                "Default : true",
            getValue: () => _config!.SaplingsEnabled,
            setValue: value => _config!.SaplingsEnabled = value
        );

        configMenu.AddNumberOption(
            ModManifest,
            name: () => "Sapling Stock",
            tooltip: () =>
                "How much saplings does the shops have in stock every day. Does nothing if SaplingsEnabled is false.\n" +
                "0 clears the stock completely. Max is 64.\n" +
                "Default : 2",
            getValue: () => _config!.SaplingStock,
            setValue: value => _config!.SaplingStock = value,
            min: 0,
            max: 64,
            interval: 1
        );
    }

    private void MenuChanged(object? sender, MenuChangedEventArgs eventArgs)
    {
        if (eventArgs.NewMenu is not ShopMenu shopMenu)
            return;

        if (_config is not { ModEnabled: true })
            return;

        if (shopMenu.forSale.OfType<Item>().All(i => i.Category != -74)) // -74 is the category for seeds
        {
            var categories = shopMenu.categoriesToSellHere.ToArray();
            Monitor.Log("[Pierre's Roulette] Shop categories: " + string.Join(", ", categories), LogLevel.Debug);
            Monitor.Log("[Pierre's Roulette] Shop does not sell seeds, skipping...", LogLevel.Debug);
            return;
        }

        var seeds = shopMenu.forSale.FindAll(seed =>
            seed is Item { Category: -74 } item && !item.Name.Contains("Sapling"));
        var saplings = shopMenu.forSale.FindAll(sapling =>
            sapling is Item { Category: -74 } item && item.Name.Contains("Sapling"));
        var shopId = shopMenu.ShopId;
        var rnd = new Random(Game1.Date.TotalDays + Game1.GetSaveGameName().GetHashCode());

        Monitor.Log("[Pierre's Roulette] Shop: " + shopId, LogLevel.Debug);
        Monitor.Log("[Pierre's Roulette] Shop seeds: " + string.Join(", ", seeds.Select(s => (s as Item)?.Name)),
            LogLevel.Debug);
        Monitor.Log("[Pierre's Roulette] Shop saplings: " + string.Join(", ", saplings.Select(s => (s as Item)?.Name)),
            LogLevel.Debug);

        if (!_config.Owners.Contains(shopId))
            return;

        var removedItems = false;
        if (_config.SeedsEnabled)
            removedItems = removedItems || RemoveRandomItems(rnd, _config.SeedStock, seeds, "Seeds");
        if (_config.SaplingsEnabled)
            removedItems = removedItems || RemoveRandomItems(rnd, _config.SaplingStock, saplings, "Saplings");
        if (!removedItems)
            return;

        Monitor.Log("[Pierre's Roulette] Remaining seeds: " +
                    string.Join(", ", seeds.Select(s => (s as Item)?.Name)), LogLevel.Debug);
        Monitor.Log("[Pierre's Roulette] Remaining saplings: " +
                    string.Join(", ", saplings.Select(s => (s as Item)?.Name)), LogLevel.Debug);
        Monitor.Log("[Pierre's Roulette] Old stock information: " +
                    string.Join(", ", shopMenu.itemPriceAndStock.Keys.Select(i => (i as Item)?.Name)), LogLevel.Debug);

        var stockDict = new Dictionary<ISalable, ItemStockInformation>(shopMenu.itemPriceAndStock);
        var stockList = new List<ISalable>(shopMenu.forSale);
        var toRemove = new List<ISalable>(stockList.Where(i =>
            i is Item { Category: -74 } && !seeds.Contains(i) && !saplings.Contains(i)));

        Monitor.Log("[Pierre's Roulette] Items to remove: " +
                    string.Join(", ", toRemove.Select(i => (i as Item)?.Name)), LogLevel.Debug);

        foreach (var item in toRemove)
        {
            stockDict.Remove(item);
            stockList.Remove(item);
        }

        shopMenu.itemPriceAndStock = stockDict;
        shopMenu.forSale = stockList;

        Monitor.Log("[Pierre's Roulette] New stock information: " + string.Join(", ",
            shopMenu.itemPriceAndStock.Keys.Select(i => (i as Item)?.Name)), LogLevel.Debug);
    }

    private bool RemoveRandomItems(Random rnd, int amount, List<ISalable> items, string category)
    {
        if (amount < 0)
        {
            Monitor.Log(
                "[Pierre's Roulette] Stock amount for " +
                category + " is invalid : " + amount, LogLevel.Error);
            return false;
        }

        if (amount >= items.Count)
        {
            Monitor.Log("Pierres' Roulette] Stock config for " +
                        category + " is greater or equal to merchant's stock. Skipping...", LogLevel.Debug);
            return false;
        }

        if (amount == 0)
        {
            Monitor.Log("[Pierre's Roulette] Stock config for " +
                        category + " is set to 0, removing entire stock...", LogLevel.Debug);
            items.Clear();
        }
        else
        {
            while (items.Count > amount)
            {
                var indexToRemove = rnd.Next(items.Count);
                Monitor.Log($"Removed item: {items[indexToRemove].Name}", LogLevel.Debug);
                items.RemoveAt(indexToRemove);
            }
        }

        return true;
    }
}