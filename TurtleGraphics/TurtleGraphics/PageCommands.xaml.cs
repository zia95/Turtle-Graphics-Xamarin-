using System;
using System.Collections.Generic;
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
        public ObservableCollection<SkiaTurtleE.CommandInfo> ListCommands { get; set; }

        private void add_dummy()
        {
            this.ListCommands.Add(new SkiaTurtleE.CommandInfo() { Command = SkiaTurtleE.CommandTypes.Repeat, Amount = 9 });


            this.ListCommands.Add(new SkiaTurtleE.CommandInfo() { Command = SkiaTurtleE.CommandTypes.Repeat, Amount = 4 });

            this.ListCommands.Add(new SkiaTurtleE.CommandInfo() { Command = SkiaTurtleE.CommandTypes.Repeat, Amount = 2 });
            this.ListCommands.Add(new SkiaTurtleE.CommandInfo() { Command = SkiaTurtleE.CommandTypes.PenUp, Amount = -1 });
            this.ListCommands.Add(new SkiaTurtleE.CommandInfo() { Command = SkiaTurtleE.CommandTypes.Forward, Amount = 10 });
            this.ListCommands.Add(new SkiaTurtleE.CommandInfo() { Command = SkiaTurtleE.CommandTypes.PenDown, Amount = -1 });
            this.ListCommands.Add(new SkiaTurtleE.CommandInfo() { Command = SkiaTurtleE.CommandTypes.Forward, Amount = 50 });
            this.ListCommands.Add(new SkiaTurtleE.CommandInfo() { Command = SkiaTurtleE.CommandTypes.EndRepeat, Amount = -1 });

            this.ListCommands.Add(new SkiaTurtleE.CommandInfo() { Command = SkiaTurtleE.CommandTypes.Left, Amount = -1 });

            this.ListCommands.Add(new SkiaTurtleE.CommandInfo() { Command = SkiaTurtleE.CommandTypes.EndRepeat, Amount = -1 });

            this.ListCommands.Add(new SkiaTurtleE.CommandInfo() { Command = SkiaTurtleE.CommandTypes.Rotate, Amount = 40 });
            this.ListCommands.Add(new SkiaTurtleE.CommandInfo() { Command = SkiaTurtleE.CommandTypes.Forward, Amount = 100 });


            this.ListCommands.Add(new SkiaTurtleE.CommandInfo() { Command = SkiaTurtleE.CommandTypes.EndRepeat, Amount = -1 });
        }
        public PageCommands()
        {
            InitializeComponent();

            if(this.ListCommands == null)
                this.ListCommands = new ObservableCollection<SkiaTurtleE.CommandInfo>();

            lstCommands.ItemsSource = this.ListCommands;

            Settings.PageCommandsInstance = this;

            this.add_dummy();

            this.btnAddCommand.Clicked += async (s, e) => 
            {
                string cmnd = await DisplayActionSheet("Which Command You Want To Add?", "Cancel", null, Settings.Turtle.CommandTypesString);

                int cidx = SkiaTurtleE.GetCommandIndex(cmnd);

                if (cidx == -1)
                    return;

                int amnt = 0;

                if (SkiaTurtleE.DoCommandNeedAmount((SkiaTurtleE.CommandTypes)cidx))
                {
                    string samnt = await DisplayPromptAsync("Amount", "For How Long?", initialValue: "10", maxLength: 4, keyboard: Keyboard.Numeric);

                    if (int.TryParse(samnt, out amnt) == false || amnt <= 0)
                        return;
                }
                else
                    amnt = -1;

                this.ListCommands.Add(new SkiaTurtleE.CommandInfo() { Command = (SkiaTurtleE.CommandTypes)cidx, Amount = amnt });
            };
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", e.Item.ToString(), "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
