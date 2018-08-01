using System;
using Microsoft.VisualStudio.Shell;

namespace Microsoft.VisualStudio.ProjectSystem.VS.PropertyPages
{
    internal static class StatusCalloutResourceKeys
    {
        // Use the CodeLensControls GUID so that our color scheme matches theirs
        public static readonly Guid Category = new Guid("DE7B1121-99A4-4708-AEDF-15F40C9B332F");
        public static readonly ThemeResourceKey BackgroundBrush = new ThemeResourceKey(Category, "Popup", ThemeResourceKeyType.BackgroundBrush);
        public static readonly ThemeResourceKey BackgroundColor = new ThemeResourceKey(Category, "Popup", ThemeResourceKeyType.BackgroundColor);
        public static readonly ThemeResourceKey BorderBrush = new ThemeResourceKey(Category, "PopupBorder", ThemeResourceKeyType.BackgroundBrush);
        public static readonly ThemeResourceKey BorderColor = new ThemeResourceKey(Category, "PopupBorder", ThemeResourceKeyType.BackgroundColor);
        public static readonly ThemeResourceKey ForegroundBrush = new ThemeResourceKey(Category, "Popup", ThemeResourceKeyType.ForegroundBrush);
        public static readonly ThemeResourceKey ForegroundColor = new ThemeResourceKey(Category, "Popup", ThemeResourceKeyType.ForegroundColor);
    }
}
