using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TurtleGraphics.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewResource : ResourceDictionary
    {
        public ViewResource()
        {
            InitializeComponent();
            
        }

        private void btnPopupClose_Clicked(object sender, EventArgs e)
        {
            Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync(true);
        }
    }
}