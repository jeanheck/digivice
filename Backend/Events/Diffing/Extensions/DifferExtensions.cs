using System.Diagnostics.CodeAnalysis;

namespace Backend.Events.Diffing.Extensions;

public static class DifferExtensions
{
    public static bool HasNoChanges<T>([NotNullWhen(false)] this T? newObj, T? previousObj) where T : class
    {
        if (newObj == null)
        {
            return true;
        }

        return EqualityComparer<T>.Default.Equals(newObj, previousObj);
    }
}
