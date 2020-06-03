﻿using System;
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


        private PageTGScript pg_script = null;
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

            this.lstCommands.ItemsSource = this.ListCommands;

            Settings.PageCommandsInstance = this;

            this.add_dummy();

            this.pg_script = new PageTGScript();

            
            this.lstCommands.ItemTapped += async (s, e) =>
            {
                string cmnd = await DisplayActionSheet($"{e.Item}", "Cancel", null, "Remove", "Move Up", "Move Down");

                if (cmnd == "Remove")
                {
                    this.ListCommands.RemoveAt(e.ItemIndex);
                }
                else if (cmnd == "Move Up")
                {
                    int new_idx = e.ItemIndex - 1;
                    if (new_idx >= 0)
                    {
                        this.ListCommands.Move(e.ItemIndex, new_idx);
                    }
                }
                else if (cmnd == "Move Down")
                {
                    int new_idx = e.ItemIndex + 1;
                    if(new_idx < ListCommands.Count)
                    {
                        this.ListCommands.Move(e.ItemIndex, new_idx);
                    }
                }


                

                lstCommands.SelectedItem = null;
            };

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
                    {
                        Settings.Sound.Play(Settings.Sound.SND_ID.SEQ_INVALID);
                        return;
                    }
                }
                else
                    amnt = -1;


                this.ListCommands.Add(new SkiaTurtleE.CommandInfo() { Command = (SkiaTurtleE.CommandTypes)cidx, Amount = amnt });
            };

            pg_script.OnResult += (s, e) =>
            {
                if (pg_script.Commands == null)
                    return;
                this.ListCommands = new ObservableCollection<SkiaTurtleE.CommandInfo>(pg_script.Commands);
                this.lstCommands.ItemsSource = this.ListCommands;
            };
            this.btnFromScript.Clicked += async (s, e) =>
            {
                this.pg_script.Commands = this.ListCommands;
                await Navigation.PushModalAsync(pg_script);
            };


            this.btnSaveSheet.Clicked += async (s, e) =>
            {
                if (this.ListCommands.Count <= 0)
                    return;

                string name = await DisplayPromptAsync("Save", "What name you want to assign this list?", placeholder: "List Name", maxLength: 30, keyboard: Keyboard.Text);

                if (String.IsNullOrWhiteSpace(name))
                    return;

                Settings.CommandsListSave(this.ListCommands, name);
            };
            this.btnLoadSheet.Clicked += async (s, e) =>
            {

                var lst = Settings.CommandsListGet();
                if (lst == null || lst.Count <= 0)
                    return;

                var tags = lst.ConvertAll((x) => { return (string)(x[0].Tag ?? "<unnamed>"); });


                string cmnd = await DisplayActionSheet("Command List", "Cancel", null, tags.ToArray());

                this.ListCommands = cmnd != "Cancel" ? lst.Find(x => x[0].Tag == cmnd) : this.ListCommands;
                this.lstCommands.ItemsSource = this.ListCommands;
            };
        }
    }
}
