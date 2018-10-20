using System;
using Windows.Devices.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Animation;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Controls;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Bug10.Card
{
    [ContentProperty(Name = nameof(Content))]
    public sealed class CardView : Control
    {
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            nameof(Content), typeof(UIElement), typeof(CardView), new PropertyMetadata(default(UIElement)));

        public CardView()
        {
            DefaultStyleKey = typeof(CardView);
        }

        public UIElement Content
        {
            get => (UIElement) GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            RootGrid = GetTemplateChild("RootGrid") as Grid;
            ShadowPanel = GetTemplateChild("ShadowPanel") as DropShadowPanel;
        }

        private DropShadowPanel ShadowPanel { get; set; }

        private Grid RootGrid { get; set; }

        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            base.OnPointerExited(e);
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse ||
                e.Pointer.PointerDeviceType == PointerDeviceType.Pen)
            {
                ShadowPanel.Fade(0F, 150D, easingMode: EasingMode.EaseIn,
                    easingType: EasingType.Cubic).Start();
                RootGrid.Scale(1F, 1F, Convert.ToSingle(RootGrid.ActualWidth / 2F),
                    Convert.ToSingle(RootGrid.ActualHeight / 2F), 150D,
                    easingMode: EasingMode.EaseIn,
                    easingType: EasingType.Cubic).Start();
            }
        }

        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse ||
                e.Pointer.PointerDeviceType == PointerDeviceType.Pen)
            {
                ShadowPanel.Visibility = Visibility.Visible;
                ShadowPanel.Fade(1F, 300D, easingMode: EasingMode.EaseOut,
                    easingType: EasingType.Cubic).Start();
                RootGrid.Scale(1.1F, 1.1F, Convert.ToSingle(RootGrid.ActualWidth / 2F),
                    Convert.ToSingle(RootGrid.ActualHeight / 2F), 300D,
                    easingMode: EasingMode.EaseOut,
                    easingType: EasingType.Cubic).Start();
            }
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            base.OnPointerPressed(e);
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Touch)
                RootGrid.Scale(0.9F, 0.9F, Convert.ToSingle(RootGrid.ActualWidth / 2F),
                    Convert.ToSingle(RootGrid.ActualHeight / 2F), 300D,
                    easingMode: EasingMode.EaseOut,
                    easingType: EasingType.Cubic).Start();
            else
                RootGrid.Scale(1F, 1F, Convert.ToSingle(RootGrid.ActualWidth / 2F),
                    Convert.ToSingle(RootGrid.ActualHeight / 2F), 300D,
                    easingMode: EasingMode.EaseOut,
                    easingType: EasingType.Cubic).Start();
        }

        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            base.OnPointerReleased(e);
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Touch)
                RootGrid.Scale(1F, 1F, Convert.ToSingle(RootGrid.ActualWidth / 2F),
                    Convert.ToSingle(RootGrid.ActualHeight / 2F), 150D,
                    easingMode: EasingMode.EaseIn,
                    easingType: EasingType.Cubic).Start();
            else
                RootGrid.Scale(1.1F, 1.1F, Convert.ToSingle(RootGrid.ActualWidth / 2F),
                    Convert.ToSingle(RootGrid.ActualHeight / 2F), 150D,
                    easingMode: EasingMode.EaseIn,
                    easingType: EasingType.Cubic).Start();
        }
    }
}