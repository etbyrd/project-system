﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Microsoft.VisualStudio.ProjectSystem.VS.PropertyPages
{
    public class PropPageSettings : INotifyPropertyChanged
    {
        //SystemColorHighlightTextColor	Selected Text (foreground)	HighlightAlt	#FFFFFFFF
        private Color _highlightedTextColor;

        //SystemColorHighlightColor Selected Text(background)  Highlight	#FF3399FF
        private SolidColorBrush _highlightedColor;

        internal PropPageSettings()
        {
            UpdateVSColorThemeProperties();
            VSColorTheme.ThemeChanged += OnVSColorThemeChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        public Color HighlightedTextColor
        {
            get
            {
                return _highlightedTextColor;
            }
            set
            {
                SetProperty(ref _highlightedTextColor, value);
            }
        }

        public SolidColorBrush HighlightedColor
        {
            get
            {
                return _highlightedColor;
            }
            set
            {
                SetProperty(ref _highlightedColor, value);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnVSColorThemeChanged(ThemeChangedEventArgs e)
        {
            // If unsupported theme, fallback
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

        private void UpdateVSColorThemeProperties()
        {
            var globalServiceProvider = ServiceProvider.GlobalProvider;
            var vsShell = (IVsUIShell5)(globalServiceProvider.GetService(typeof(IVsUIShell)));

            if (vsShell != null)
            {
                //This will work, so we know the bindining is working correctly
                //HighlightedColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("ff1aeb"));

                //Works, with the correct color, but WPF sets the opacity to .4 and changing that reveals that WPF
                //Draws the brush over the text, meaning it will just be a solid block
                var correctBrush = Application.Current.Resources[EnvironmentColors.SystemHighlightBrushKey] as SolidColorBrush;
                Color colorOfBrush = correctBrush.Color;
                HighlightedColor = new SolidColorBrush(colorOfBrush);
               
                HighlightedTextColor = VsColors.GetThemedWPFColor(vsShell, EnvironmentColors.SystemHighlightTextColorKey);
    
            }
            else // Fall back to some reasonable defaults (these are the current colors for the Light theme)
            {
                //TODO 
            }
        }
    }
}
