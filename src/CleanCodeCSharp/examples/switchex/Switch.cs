using System.Collections.Generic;

namespace CleanCode.SwitchEx;

public class Switch {
    Dictionary<string, string> powerToHeroMap = new Dictionary<string, string>
    {
        {"wind", "Storm"},
        {"laser", "Cyclops"},
        {"DEFAULT", "Aquaman"}
    };

    public string GetHero(string power) {
        return powerToHeroMap.TryGetValue(power, out var hero) ? hero : powerToHeroMap["DEFAULT"];
    }
}