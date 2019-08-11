using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Microsoft.UI.Xaml.Controls;
using RefreshContainer = Microsoft.UI.Xaml.Controls.RefreshContainer;
using RefreshRequestedEventArgs = Microsoft.UI.Xaml.Controls.RefreshRequestedEventArgs;
using ScrollViewer = Windows.UI.Xaml.Controls.ScrollViewer;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Bug10.Repeater
{
    public sealed class RepeaterView : Control
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource", typeof(object), typeof(RepeaterView),
            new PropertyMetadata(default, OnItemsSourceChanged));

        public static readonly DependencyProperty LayoutProperty = DependencyProperty.Register(
            "Layout", typeof(VirtualizingLayout), typeof(RepeaterView),
            new PropertyMetadata(default(VirtualizingLayout)));

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
            "ItemTemplate", typeof(DataTemplate), typeof(RepeaterView),
            new PropertyMetadata(default(DataTemplate)));

        private RefreshContainer _rootRefreshContainer;
        private ScrollViewer _rootScrollViewer;
        private bool _isLoading;
        private Microsoft.UI.Xaml.Controls.ItemsRepeater _contentRepeater;

        public RepeaterView()
        {
            DefaultStyleKey = typeof(RepeaterView);
        }

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate) GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public object ItemsSource
        {
            get => GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
        
        public VirtualizingLayout Layout
        {
            get => (VirtualizingLayout) GetValue(LayoutProperty);
            set => SetValue(LayoutProperty, value);
        }
        

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as RepeaterView)?.OnItemsSourceChanged(e.NewValue, e.OldValue);
        }

        private void OnItemsSourceChanged(object newValue, object oldValue)
        {
            if (newValue is ISupportIncrementalLoading newIncrementalLoading) _rootRefreshContainer?.RequestRefresh();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _rootScrollViewer = GetTemplateChild("RootScrollViewer") as ScrollViewer;
            _rootRefreshContainer = GetTemplateChild("RootRefreshContainer") as RefreshContainer;
            _contentRepeater = GetTemplateChild("ContentRepeater") as Microsoft.UI.Xaml.Controls.ItemsRepeater;
            _rootScrollViewer.ViewChanging += RootScrollViewerOnViewChanging;
            _rootRefreshContainer.RefreshRequested += RootRefreshContainerOnRefreshRequested;
            Dispatcher.TryRunAsync(CoreDispatcherPriority.Normal, () => _rootRefreshContainer.RequestRefresh());
        }

        private void RootRefreshContainerOnRefreshRequested(RefreshContainer sender, RefreshRequestedEventArgs args)
        {
            LoadItems(args.GetDeferral());
        }

        private void LoadItems(Deferral def = null)
        {
            if (!_isLoading && ItemsSource is ISupportIncrementalLoading loading && loading.HasMoreItems)
            {
                _isLoading = true;
                Dispatcher.TryRunAsync(CoreDispatcherPriority.Normal,
                    async () =>
                    {
                        await loading.LoadMoreItemsAsync(20);
                        def?.Complete();
                        _isLoading = false;
                        TryLoadIfNotFill();
                    });
            }
            else
            {
                def?.Complete();
            }
        }

        private void TryLoadIfNotFill()
        {
            if (_isLoading)
            {
                return;
            }

            if (_rootScrollViewer.ScrollableHeight > ActualHeight)
            {
                return;
            }
            LoadItems();
        }

        private void RootScrollViewerOnViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            var vOffset = _rootScrollViewer.VerticalOffset;
            var vSize = _rootScrollViewer.ScrollableHeight - 100D;
            if (vOffset >= vSize) LoadItems();
        }
    }
}