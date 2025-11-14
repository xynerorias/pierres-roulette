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
        helper.Events.Input.ButtonPressed += OnButtonPressed;
        helper.Events.Display.MenuChanged += MenuChanged;
    }

    private void OnButtonPressed(object? sender, ButtonPressedEventArgs eventArgs)
    {
        if (!Context.IsWorldReady)
            return;
        Monitor.Log($"{Game1.player.Name} pressed {eventArgs.Button}.", LogLevel.Debug);
    }


    private void MenuChanged(object? sender, MenuChangedEventArgs eventArgs)
    {
        if (_config != null && !_config.ModEnabled)
            return;

        if (eventArgs.NewMenu is ShopMenu shopMenu)
            if (!shopMenu.categoriesToSellHere.Contains(-74)) // -74 is the category for seeds
            {
                var categories = shopMenu.categoriesToSellHere.ToArray();
                Monitor.Log("Shop categories: " + string.Join(", ", categories), LogLevel.Debug);
                Monitor.Log("Shop does not sell seeds, skipping...", LogLevel.Debug);
            }
    }
}