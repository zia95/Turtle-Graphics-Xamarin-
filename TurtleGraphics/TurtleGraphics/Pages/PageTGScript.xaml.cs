using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;

namespace TurtleGraphics
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageTGScript : ContentPage
    {

        private ObservableCollection<SkiaTurtleE.CommandInfo> __commands;
        public ObservableCollection<SkiaTurtleE.CommandInfo> Commands 
        {
            get => __commands;
            set
            {
                __commands = value;
                this.edtScript.Text = TGScript.Generate(__commands.ToList());
            }
        }

        public event EventHandler OnResult;


        public PageTGScript()
        {
            InitializeComponent();

            this.btnDone.Clicked += async (s, e) => 
            {
                var res = TGScript.Parse(null, this.edtScript.Text);

                if(res == null)
                {
                    Settings.Sound.Play(Settings.Sound.SND_ID.SEQ_INVALID);
                    if (await DisplayAlert("Errors", "Failed to parse the script. Return back?", "Yes", "No"))
                        return;
                }

                this.Commands = res != null ? new ObservableCollection<SkiaTurtleE.CommandInfo>(res) : null;

                this.OnResult?.Invoke(this, new EventArgs());
                await Navigation.PopModalAsync();
            };
        }
    }
}