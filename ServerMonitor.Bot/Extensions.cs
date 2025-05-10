namespace ServerMonitor.Bot;

using Humanizer;

public static class Extensions
{
    public static string Quantize(this string text, int amount) => text.ToQuantity(amount, ShowQuantityAs.None);
}