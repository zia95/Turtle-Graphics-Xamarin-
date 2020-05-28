using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TurtleGraphics
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageCommands : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }

        public PageCommands()
        {
            InitializeComponent();

            Items = new ObservableCollection<string>
            {
                "Item 1",
                "Item 2",
                "Item 3",
                "Item 4",
                "Item 5"
            };

            MyListView.ItemsSource = Items;


            this.btnAddCommand.Clicked += async (s, e) => 
            {
                string action = await DisplayActionSheet("Which Command You Want To Add?", "Cancel", null, "Forward", "Backward", "Rotate");
                
                string result = await DisplayPromptAsync("Amount", "For How Long?", initialValue: "10", maxLength: 4, keyboard: Keyboard.Numeric);

                Items = new ObservableCollection<string> { $"{action} -> {result}" };

                MyListView.ItemsSource = Items;
            };
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
