using System.Windows;
using System.Windows.Controls.Primitives;

namespace Microsoft.VisualStudio.ProjectSystem.VS.PropertyPages
{
    /// <summary>
    /// Interaction logic for StatusCallout.xaml
    /// </summary>
    internal partial class StatusCallout : Popup
    {
        public static readonly DependencyProperty SettingsProperty = DependencyProperty.Register(
            name: nameof(Settings),
            propertyType: typeof(StatusCalloutSettings),
            ownerType: typeof(StatusCallout),
            typeMetadata: new FrameworkPropertyMetadata(
                defaultValue: new StatusCalloutSettings(),
                flags: FrameworkPropertyMetadataOptions.Inherits,
                propertyChangedCallback: null,
                coerceValueCallback: null
            ),
            validateValueCallback: null
        );
        public StatusCallout()
        {
            InitializeComponent();
        }
        public StatusCalloutSettings Settings
        {
            get
            {
                return (StatusCalloutSettings)GetValue(SettingsProperty);
            }
            set
            {
                SetValue(SettingsProperty, value);
            }
        }
    }
}
