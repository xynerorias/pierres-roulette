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
        var owners = shopMenu.ShopData.Owners;
        owners.Remove(owners.FirstOrDefault()); // Remove default owner (always "None")
        var rnd = new Random(Game1.Date.TotalDays + Game1.GetSaveGameName().GetHashCode());

        Monitor.Log("[Pierre's Roulette] Shop seeds: " + string.Join(", ", seeds.Select(s => (s as Item)?.Name)),
            LogLevel.Debug);
        Monitor.Log("[Pierre's Roulette] Shop saplings: " + string.Join(", ", saplings.Select(s => (s as Item)?.Name)),
            LogLevel.Debug);
        Monitor.Log("[Pierre's Roulette] Shop owners: " + string.Join(", ", owners.Select(o => o.Name)),
            LogLevel.Debug);

        if (!owners.Any(o => _config.Owners.Contains(o.Name)))
            return;

        if (_config.SeedsEnabled)
        {
            RemoveRandomItems(rnd, _config.SeedStock, seeds, "Seeds");
            Monitor.Log(
                "[Pierre's Roulette] Remaining seeds: " + string.Join(", ", seeds.Select(s => (s as Item)?.Name)),
                LogLevel.Debug);
        }

        if (_config.SaplingsEnabled)
        {
            RemoveRandomItems(rnd, _config.SaplingStock, saplings, "Saplings");
            Monitor.Log(
                "[Pierre's Roulette] Remaining saplings: " +
                string.Join(", ", saplings.Select(s => (s as Item)?.Name)),
                LogLevel.Debug);
        }

        Monitor.Log("[Pierre's Roulette] Old stock information: " + string.Join(", ",
            shopMenu.itemPriceAndStock.Keys.Select(i => (i as Item)?.Name)), LogLevel.Debug);

        var stockDict = new Dictionary<ISalable, ItemStockInformation>(shopMenu.itemPriceAndStock);
        var stockList = new List<ISalable>(shopMenu.forSale);
        var toRemove = new List<ISalable>(stockList.Where(i =>
            i is Item { Category: -74 } && !seeds.Contains(i) && !saplings.Contains(i)));

        Monitor.Log("[Pierre's Roulette] Items to remove: " + string.Join(", ",
            toRemove.Select(i => (i as Item)?.Name)), LogLevel.Debug);
        Monitor.Log("[Pierre's Roulette] Removing items...", LogLevel.Debug);

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

    private void RemoveRandomItems(Random rnd, int amount, List<ISalable> items, string category)
    {
        if (amount < 0)
        {
            Monitor.Log(
                "[Pierre's Roulette] Not removing items of category " + category, LogLevel.Debug);
            return;
        }

        if (amount > items.Count)
        {
            Monitor.Log("[Pierre's Roulette] More items to remove than stock, removing entire stock", LogLevel.Debug);
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
    }
}