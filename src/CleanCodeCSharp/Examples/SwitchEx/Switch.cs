namespace CleanCodeCSharp.Examples.SwitchEx;

public class Switch
{
    public string GetHero(string power)
    {
        switch (power)
        {
            case "wind":
                return "Storm";
            case "laser":
                return "Cyclops";
            default:
                return "Aquaman";
        }
    }

    private readonly Dictionary<string, string> powerToHeroMap = new()
    {
        ["wind"] = "Storm",
        ["laser"] = "Cyclops",
        ["DEFAULT"] = "Aquaman",
    };

    public string GetHero2(string power)
    {
        return powerToHeroMap.GetValueOrDefault(power, powerToHeroMap["DEFAULT"]);
    }
}
