using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Microsoft.VisualStudio.ProjectSystem.VS.PropertyPages
{
    public class StatusCalloutSettings : INotifyPropertyChanged
    {
        private Color _backgroundColor;
        private Color _borderColor;
        private Color _dropShadowColor;
        private Color _foregroundColor;
        private Brush _backgroundBrush;
        private Brush _borderBrush;
        private Brush _dropShadowBrush;
        private Brush _foregroundBrush;
        private bool? _areAnimationsAllowed;
        private bool? _areGradientsAllowed;
        internal StatusCalloutSettings()
        {
            UpdateEnvironmentRenderCapabilitiesProperties();
            EnvironmentRenderCapabilities.Current.RenderCapabilitiesChanged += OnEnvironmentRenderCapabilitiesChanged;
            UpdateVSColorThemeProperties();
            VSColorTheme.ThemeChanged += OnVSColorThemeChanged;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public bool? AreAnimationsAllowed
        {
            get
            {
                return _areAnimationsAllowed;
            }
            set
            {
                SetProperty(ref _areAnimationsAllowed, value);
            }
        }
        public bool? AreGradientsAllowed
        {
            get
            {
                return _areGradientsAllowed;
            }
            set
            {
                SetProperty(ref _areGradientsAllowed, value);
            }
        }
        public Brush BackgroundBrush
        {
            get
            {
                return _backgroundBrush;
            }
            set
            {
                SetProperty(ref _backgroundBrush, value);
            }
        }
        public Color BackgroundColor
        {
            get
            {
                return _backgroundColor;
            }
            set
            {
                SetProperty(ref _backgroundColor, value);
            }
        }
        public Brush BorderBrush
        {
            get
            {
                return _borderBrush;
            }
            set
            {
                SetProperty(ref _borderBrush, value);
            }
        }
        public Color BorderColor
        {
            get
            {
                return _borderColor;
            }
            set
            {
                SetProperty(ref _borderColor, value);
            }
        }
        public Brush DropShadowBrush
        {
            get
            {
                return _dropShadowBrush;
            }
            set
            {
                SetProperty(ref _dropShadowBrush, value);
            }
        }
        public Color DropShadowColor
        {
            get
            {
                return _dropShadowColor;
            }
            set
            {
                SetProperty(ref _dropShadowColor, value);
            }
        }
        public Brush ForegroundBrush
        {
            get
            {
                return _foregroundBrush;
            }
            set
            {
                SetProperty(ref _foregroundBrush, value);
            }
        }
        public Color ForegroundColor
        {
            get
            {
                return _foregroundColor;
            }
            set
            {
                SetProperty(ref _foregroundColor, value);
            }
        }
        public double HorizontalOffset => SystemParameters.IsMenuDropRightAligned ? 20 : -20;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void OnEnvironmentRenderCapabilitiesChanged(object sender, System.EventArgs e)
        {
            UpdateEnvironmentRenderCapabilitiesProperties();
        }
        private void OnVSColorThemeChanged(ThemeChangedEventArgs eventArgs)
        {
            UpdateVSColorThemeProperties();
        }
        private void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value))
            {
                return;
            }
            field = value;
            if (propertyName != null)
            {
                OnPropertyChanged(propertyName);
            }
        }
        private void UpdateEnvironmentRenderCapabilitiesProperties()
        {
            AreAnimationsAllowed = EnvironmentRenderCapabilities.Current.AreAnimationsAllowed;
            AreGradientsAllowed = EnvironmentRenderCapabilities.Current.AreGradientsAllowed;
        }
        private void UpdateVSColorThemeProperties()
        {
            var globalServiceProvider = ServiceProvider.GlobalProvider;
            var vsShell = (IVsUIShell5)(globalServiceProvider.GetService(typeof(IVsUIShell)));
            if (vsShell != null)
            {
                BackgroundBrush = Application.Current.Resources[StatusCalloutResourceKeys.BackgroundBrush] as Brush;
                BackgroundColor = VsColors.GetThemedWPFColor(vsShell, StatusCalloutResourceKeys.BackgroundColor);
                BorderBrush = Application.Current.Resources[StatusCalloutResourceKeys.BorderBrush] as Brush;
                BorderColor = VsColors.GetThemedWPFColor(vsShell, StatusCalloutResourceKeys.BorderColor);
                DropShadowBrush = Application.Current.Resources[EnvironmentColors.DropShadowBackgroundBrushKey] as Brush;
                DropShadowColor = VsColors.GetThemedWPFColor(vsShell, EnvironmentColors.DropShadowBackgroundColorKey);
                ForegroundBrush = Application.Current.Resources[StatusCalloutResourceKeys.ForegroundBrush] as Brush;
                ForegroundColor = VsColors.GetThemedWPFColor(vsShell, StatusCalloutResourceKeys.ForegroundColor);
            }
            else // Fall back to some reasonable defaults (these are the current colors for the Light theme)
            {
                BackgroundColor = Color.FromArgb(0xFF, 0xFC, 0xFC, 0xFC);
                BackgroundBrush = new SolidColorBrush(BackgroundColor);

                BorderColor = Color.FromArgb(0xFF, 0x6D, 0xC2, 0xE9);
                BorderBrush = new SolidColorBrush(BorderColor);
                DropShadowColor = Color.FromArgb(0x72, 0x00, 0x00, 0x00);
                DropShadowBrush = new SolidColorBrush(DropShadowColor);
                ForegroundColor = Color.FromArgb(0xFF, 0x1E, 0x1E, 0x1E);
                ForegroundBrush = new SolidColorBrush(ForegroundColor);
            }
        }
    }
}
