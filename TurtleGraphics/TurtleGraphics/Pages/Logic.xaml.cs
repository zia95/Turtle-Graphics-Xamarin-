using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurtleGraphics.Turtle;
using TurtleGraphics.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TurtleGraphics.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Logic : ContentPage
    {
        public Logic()
        {
            InitializeComponent();

            //this.navbar.PageChanging += (s, e) =>
            //{
            //    Settings.Commands = this.lstCommands.ToEnumerable();
            //};
            this.Disappearing += (s, e) =>
            {
                Turtle.Settings.Commands = this.lstCommands.ToEnumerable();
            };
            //add button clicked
            this.btnAddCommand.Clicked += (s, e) =>
            {
                Turtle.SoundManager.Play(Turtle.SoundManager.SND_CLICK);
                PopupNavigation.Instance.PushAsync(new Views.PopupAddCommand(
                    (s, e) => 
                    {
                        var pop = (Views.PopupAddCommand)s;
                        this.lstCommands.Add(pop.ResultCommand, pop.ResultUnits);
                    }), 
                    true);
            };

            //clear button clicked
            this.btnClearCommands.Clicked += (s, e) =>
            {
                Turtle.SoundManager.Play(Turtle.SoundManager.SND_CLICK);
                this.lstCommands.Clear();
            };

            //save button clicked
            this.btnSaveCommands.Clicked += (s, e) =>
            {
                var cmds = this.lstCommands.ToEnumerable().ToList();
                if(cmds.Count > 0)
                {
                    Turtle.SoundManager.Play(Turtle.SoundManager.SND_CLICK);
                    PopupNavigation.Instance.PushAsync(new PopupSaveCommands(cmds, (s, e) => { }));
                }
                else
                {
                    Turtle.SoundManager.Play(Turtle.SoundManager.SND_ERROR);
                    PopupMessage.Show("Save", "Empty list can't be saved.");
                }
            };

            //load button clicked
            this.btnLoadCommands.Clicked += (s, e) =>
            {
                if(SaveManager.GetSavedCommandListCount() > 0 || SaveManager.PresetCommands?.Length > 0)
                {
                    Turtle.SoundManager.Play(Turtle.SoundManager.SND_CLICK);
                    PopupNavigation.Instance.PushAsync(
                        new PopupLoadCommands((s, e) => { ((PopupLoadCommands)s).Result.ForEach(x => this.lstCommands.Add(x.Key, x.Value)); } ));
                }
                else
                {
                    Turtle.SoundManager.Play(Turtle.SoundManager.SND_ERROR);
                    PopupMessage.Show("Load", "No saved list to load.");
                }
            };
        }
    }
}