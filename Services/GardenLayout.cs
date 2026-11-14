namespace BodyQuest.Services;

/// <summary>
/// Positions of the six worlds on the garden map. Coordinates use the same
/// 1000 x 2200 space as the artwork in Components/Shared/GardenScene.razor.
/// </summary>
public static class GardenLayout
{
    public const double Width = 1000;
    public const double Height = 2200;

    public static readonly (double X, double Y)[] Nodes =
    {
        (300, 340), (700, 650), (300, 960), (700, 1270), (300, 1580), (700, 1890)
    };

    /// <summary>Where the treasure chest stands at the end of the path.</summary>
    public static readonly (double X, double Y) Finish = (500, 2110);

    public static (double X, double Y) NodeAt(int index) =>
        Nodes[Math.Clamp(index, 0, Nodes.Length - 1)];

    public static string Left(int index) => Pct(NodeAt(index).X / Width);
    public static string Top(int index) => Pct(NodeAt(index).Y / Height);

    private static string Pct(double v) =>
        (v * 100).ToString("0.###", System.Globalization.CultureInfo.InvariantCulture) + "%";
}
