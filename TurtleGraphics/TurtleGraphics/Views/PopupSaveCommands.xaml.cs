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
    public partial class PopupSaveCommands : PopupPage
    {
        public bool Result { get; private set; }
        public EventHandler Success;

        private readonly List<KeyValuePair<Turtle.SkiaTurtleE.CommandTypes, int>> mCommandList;
        public PopupSaveCommands(List<KeyValuePair<Turtle.SkiaTurtleE.CommandTypes, int>> command_list, EventHandler on_success)
        {
            InitializeComponent();
            this.Success = on_success;

            this.mCommandList = command_list;

            
            this.Appearing += (s, e) => {
                if (this.mCommandList.Count <= 0)
                {
                    this.Close();
                }
            };

            this.btnSave.Clicked += (s, e) => 
            {
                string name = this.txtName.Text;
                if(Turtle.SaveManager.IsCommandListNameAvailable(name))
                {
                    if(Turtle.SaveManager.SaveCommandList(name, this.mCommandList.ToList()))
                    {
                        Turtle.SoundManager.Play(Turtle.SoundManager.SND_CLICK);
                        this.Success?.Invoke(this, EventArgs.Empty);
                        this.Close(true);
                    }
                    else
                    {
                        Turtle.SoundManager.Play(Turtle.SoundManager.SND_ERROR);
                        PopupMessage.Show("Error", $"Failed to save {name}.");
                    }
                }
                else
                {
                    Turtle.SoundManager.Play(Turtle.SoundManager.SND_ERROR);
                    PopupMessage.Show("Error", $"Name '{name}' is already in use.");
                }
            };
        }
    }
}