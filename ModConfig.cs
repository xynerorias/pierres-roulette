namespace pierres_roulette;

public sealed class ModConfig
{
    // Master switch
    // Default: true
    public bool ModEnabled { get; set; } = true;

    // Choose from the ShopId values in the game's data files.
    // Check the wiki for a list of available IDs: https://stardewvalleywiki.com/Modding:Shops
    // Default: ["SeedShop", "Sandy"]
    public string[] Owners { get; set; } = { "SeedShop", "Sandy" };

    // Enable/disable seeds being affected
    //Default: true
    public bool SeedsEnabled { get; set; } = true;

    // How much seeds does the shops have in stock every day. Does nothing if SeedsEnabled is false.
    // Default: 4
    public int SeedStock { get; set; } = 4;

    // Enable/disable saplings being affected
    // Default: true
    public bool SaplingsEnabled { get; set; } = true;

    //How much saplings does the shop have in stock every day. Does nothing if SaplingsEnabled is false.
    // Default: 2
    public int SaplingStock { get; set; } = 2;
}