namespace pierres_roulette;

public class ModConfig
{
    // Master switch
    public bool ModEnabled { get; set; } = true;

    // Wether or not the mod affects the JojaMart
    //Default: false
    public bool JojaEnabled { get; set; } = false;

    // // The shop owners to be affected by the mod. Their name must be their ShopMenu.portaitPerson.Name
    // // Default: ["Pierre"]
    // public string[] Owners { get; set; } = { "Pierre" };
    //
    // // Cycle between "OnlySeeds" to affect only the seeds, "OnlySaplings" to affect only the saplings or "Both" to affect them both
    // // Default: "Both" 
    // public string Mode { get; set; } = "Both";
    //
    // // How much seeds does the shops have in stock every day. Set to 0 to disable. Ignored if mode is "OnlySaplings"
    // // Default: 5
    // public int SeedStock { get; set; } = 5;
    //
    // //How much saplings does the shop have in stock every day. Set to 0 to disable. Ignored if mode is "OnlyCrops"
    // // Default: 2
    // public int SaplingStock { get; set; } = 2;
}