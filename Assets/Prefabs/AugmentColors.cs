using UnityEngine;

public static class AugmentColors
{
    public const string MaxHealth = "#C96A5E";
    public const string BaseDamage = "#A56B4E";
    public const string CritChance = "#D9AE52";
    public const string CritDamage = "#E39A3A";
    public const string Timing = "#8B9AA5";
    public const string Body = "#C9C4B8";

    private static readonly string[] tierHex =
    {
        "#B8AC8C", // 1 bone
        "#5F7A6E", // 2 grave
        "#57C4C0", // 3 wisp
        "#A97BFF", // 4 soulfire
        "#F2C3FF"  // 5 ascendant
    };

    public static string TierHex(int tier) => tierHex[Mathf.Clamp(tier, 1, tierHex.Length) - 1];

    public static Color TierColor(int tier) => Hex(TierHex(tier));

    public static Color Hex(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color c);
        return c;
    }

    public static string Wrap(string text, string hex) => $"<color={hex}>{text}</color>";
}