using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Bug10.Icon
{
    public partial class IconView : FontIcon
    {
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            nameof(Icon), typeof(Icons), typeof(IconView), new PropertyMetadata(default(Icons), OnIconsChanged));

        private static void OnIconsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as IconView).OnIconsChanged((Icons) e.NewValue);
        }

        private void OnIconsChanged(Icons newValue)
        {
            Glyph = Mapper[newValue];
        }

        public IconView()
        {
            FontFamily = new FontFamily("Segoe MDL2 Assets");
        }

        public Icons Icon
        {
            get => (Icons) GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
    }
}