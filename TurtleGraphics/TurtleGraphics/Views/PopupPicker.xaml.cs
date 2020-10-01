using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Rg.Plugins.Popup;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

namespace TurtleGraphics.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupPicker : PopupPage
    {
        public static void Show(string[] options, EventHandler<int> on_success, bool animate = true)
        {
            PopupNavigation.Instance.PushAsync(new PopupPicker(options, on_success), animate);
        }
        public event EventHandler<int> Success;
        public PopupPicker(string[] options, EventHandler<int> on_success)
        {
            InitializeComponent();

            this.Success = on_success;

            this.lstOptions.ItemsSource = options;

            this.lstOptions.ItemSelected += (s, e) =>
            {
                this.Success?.Invoke(this, e.SelectedItemIndex);
                this.Close();
            };
        }
    }
}