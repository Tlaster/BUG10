using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Bug10.Icon;
using Sample.Annotations;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Sample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private Icons _selectedIcon = Icons.Home;

        public Icons SelectedIcon
        {
            get => _selectedIcon;
            set
            {
                if (value != _selectedIcon)
                {
                    _selectedIcon = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<Icons> IconList { get; } = typeof(Icons).GetFields().Where(it => it.IsStatic).Select(it => (Icons) it.GetValue(null)).ToList();

        public string[] Strings { get; set; } = new[]
        {
            "https://images.dmzj.com/k/%E8%91%B5%E5%90%9B%E6%98%AF%E4%B8%80%E4%B8%AA%E6%83%B3%E6%AD%BB%E7%9A%84%E5%A5%B3%E5%AD%A9%E5%AD%90/%E7%AC%AC01%E8%AF%9D/01.jpg",
            "https://images.dmzj.com/k/%E8%91%B5%E5%90%9B%E6%98%AF%E4%B8%80%E4%B8%AA%E6%83%B3%E6%AD%BB%E7%9A%84%E5%A5%B3%E5%AD%A9%E5%AD%90/%E7%AC%AC01%E8%AF%9D/02.jpg",
            "https://images.dmzj.com/k/%E8%91%B5%E5%90%9B%E6%98%AF%E4%B8%80%E4%B8%AA%E6%83%B3%E6%AD%BB%E7%9A%84%E5%A5%B3%E5%AD%A9%E5%AD%90/%E7%AC%AC01%E8%AF%9D/03.jpg",
            "https://images.dmzj.com/k/%E8%91%B5%E5%90%9B%E6%98%AF%E4%B8%80%E4%B8%AA%E6%83%B3%E6%AD%BB%E7%9A%84%E5%A5%B3%E5%AD%A9%E5%AD%90/%E7%AC%AC01%E8%AF%9D/04.jpg",
            "https://images.dmzj.com/k/%E8%91%B5%E5%90%9B%E6%98%AF%E4%B8%80%E4%B8%AA%E6%83%B3%E6%AD%BB%E7%9A%84%E5%A5%B3%E5%AD%A9%E5%AD%90/%E7%AC%AC01%E8%AF%9D/05.jpg",
            "https://images.dmzj.com/k/%E8%91%B5%E5%90%9B%E6%98%AF%E4%B8%80%E4%B8%AA%E6%83%B3%E6%AD%BB%E7%9A%84%E5%A5%B3%E5%AD%A9%E5%AD%90/%E7%AC%AC01%E8%AF%9D/06.jpg",
            "https://images.dmzj.com/k/%E8%91%B5%E5%90%9B%E6%98%AF%E4%B8%80%E4%B8%AA%E6%83%B3%E6%AD%BB%E7%9A%84%E5%A5%B3%E5%AD%A9%E5%AD%90/%E7%AC%AC01%E8%AF%9D/07.jpg",
            "https://images.dmzj.com/k/%E8%91%B5%E5%90%9B%E6%98%AF%E4%B8%80%E4%B8%AA%E6%83%B3%E6%AD%BB%E7%9A%84%E5%A5%B3%E5%AD%A9%E5%AD%90/%E7%AC%AC01%E8%AF%9D/08.jpg",
            "https://images.dmzj.com/k/%E8%91%B5%E5%90%9B%E6%98%AF%E4%B8%80%E4%B8%AA%E6%83%B3%E6%AD%BB%E7%9A%84%E5%A5%B3%E5%AD%A9%E5%AD%90/%E7%AC%AC01%E8%AF%9D/09.jpg",
            "https://images.dmzj.com/k/%E8%91%B5%E5%90%9B%E6%98%AF%E4%B8%80%E4%B8%AA%E6%83%B3%E6%AD%BB%E7%9A%84%E5%A5%B3%E5%AD%A9%E5%AD%90/%E7%AC%AC01%E8%AF%9D/10.jpg",
            "https://images.dmzj.com/k/%E8%91%B5%E5%90%9B%E6%98%AF%E4%B8%80%E4%B8%AA%E6%83%B3%E6%AD%BB%E7%9A%84%E5%A5%B3%E5%AD%A9%E5%AD%90/%E7%AC%AC01%E8%AF%9D/11.jpg",
            "https://images.dmzj.com/k/%E8%91%B5%E5%90%9B%E6%98%AF%E4%B8%80%E4%B8%AA%E6%83%B3%E6%AD%BB%E7%9A%84%E5%A5%B3%E5%AD%A9%E5%AD%90/%E7%AC%AC01%E8%AF%9D/12.jpg",
            "https://images.dmzj.com/k/%E8%91%B5%E5%90%9B%E6%98%AF%E4%B8%80%E4%B8%AA%E6%83%B3%E6%AD%BB%E7%9A%84%E5%A5%B3%E5%AD%A9%E5%AD%90/%E7%AC%AC01%E8%AF%9D/13.jpg",
            "https://images.dmzj.com/k/%E8%91%B5%E5%90%9B%E6%98%AF%E4%B8%80%E4%B8%AA%E6%83%B3%E6%AD%BB%E7%9A%84%E5%A5%B3%E5%AD%A9%E5%AD%90/%E7%AC%AC01%E8%AF%9D/14.jpg",
            "https://images.dmzj.com/k/%E8%91%B5%E5%90%9B%E6%98%AF%E4%B8%80%E4%B8%AA%E6%83%B3%E6%AD%BB%E7%9A%84%E5%A5%B3%E5%AD%A9%E5%AD%90/%E7%AC%AC01%E8%AF%9D/15.jpg",
            "https://images.dmzj.com/k/%E8%91%B5%E5%90%9B%E6%98%AF%E4%B8%80%E4%B8%AA%E6%83%B3%E6%AD%BB%E7%9A%84%E5%A5%B3%E5%AD%A9%E5%AD%90/%E7%AC%AC01%E8%AF%9D/16.jpg",
            "https://images.dmzj.com/k/%E8%91%B5%E5%90%9B%E6%98%AF%E4%B8%80%E4%B8%AA%E6%83%B3%E6%AD%BB%E7%9A%84%E5%A5%B3%E5%AD%A9%E5%AD%90/%E7%AC%AC01%E8%AF%9D/17.jpg",
        };


        public MainPage()
        {
            this.InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
