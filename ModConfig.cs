namespace pierres_roulette;

public sealed class ModConfig
{
    // Master switch
    // Default: true
    public bool ModEnabled { get; set; } = true;

    // The shop owners to be affected by the mod.
    // Default: ["Pierre"]
    public string[] Owners { get; set; } = { "Pierre" };

    // Enable/disable seeds being affected
    //Default: true
    public bool SeedsEnabled { get; set; } = true;

    // Enable/disable saplings being affected
    // Default: true
    public bool SaplingsEnabled { get; set; } = true;

    // How much seeds does the shops have in stock every day. Set to -1 to disable. Does nothing if SeedsEnabled is false.
    // Default: 5
    public int SeedStock { get; set; } = 5;

    //How much saplings does the shop have in stock every day. Set to -1 to disable. Does nothing if SaplingsEnabled is false.
    // Default: 2
    public int SaplingStock { get; set; } = 2;
}