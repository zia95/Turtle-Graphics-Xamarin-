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
using System.Runtime.CompilerServices;

namespace TurtleGraphics.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupMessage : PopupPage
    {
        public static void Show(string title, string message, bool animate = true)
        {
            var pop = new PopupMessage(title, message);
            PopupNavigation.Instance.PushAsync(pop, animate);
        }
        public PopupMessage(string title, string message)
        {
            InitializeComponent();

            this.Title = title;
            this.lblMessage.Text = message;
        }
    }
}