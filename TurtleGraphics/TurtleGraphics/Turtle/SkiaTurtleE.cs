using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace TurtleGraphics.Turtle
{
    public class SkiaTurtleE : SkiaTurtle
    {
        public static bool GetCommandTypeInfo(CommandTypes type, out ImageSource Icon, out string Text)
        {
            switch (type)
            {
                case SkiaTurtleE.CommandTypes.Forward:
                    Icon = ImageSource.FromResource("TurtleGraphics.Resources.images.btn_command_ic_forward.png");
                    Text = "Forward";
                    break;
                case SkiaTurtleE.CommandTypes.Backward:
                    Icon = ImageSource.FromResource("TurtleGraphics.Resources.images.btn_command_ic_backward.png");
                    Text = "Backward";
                    break;
                case SkiaTurtleE.CommandTypes.Left:
                    Icon = ImageSource.FromResource("TurtleGraphics.Resources.images.btn_command_ic_left.png");
                    Text = "Left";
                    break;
                case SkiaTurtleE.CommandTypes.Right:
                    Icon = ImageSource.FromResource("TurtleGraphics.Resources.images.btn_command_ic_right.png");
                    Text = "Right";
                    break;
                case SkiaTurtleE.CommandTypes.Repeat:
                    Icon = ImageSource.FromResource("TurtleGraphics.Resources.images.btn_command_ic_repeat.png");
                    Text = "Repeat";
                    break;
                case SkiaTurtleE.CommandTypes.PenColor:
                    Icon = ImageSource.FromResource("TurtleGraphics.Resources.images.btn_command_ic_color.png");
                    Text = "Pen Color";
                    break;
                case SkiaTurtleE.CommandTypes.End:
                    Icon = ImageSource.FromResource("TurtleGraphics.Resources.images.btn_command_ic_end.png");
                    Text = "End";
                    break;
                case SkiaTurtleE.CommandTypes.PenUp:
                    Icon = ImageSource.FromResource("TurtleGraphics.Resources.images.btn_command_ic_penupdown.png");
                    Text = "Pen Up";
                    break;
                case SkiaTurtleE.CommandTypes.PenDown:
                    Icon = ImageSource.FromResource("TurtleGraphics.Resources.images.btn_command_ic_penupdown.png");
                    Text = "Pen Down";
                    break;
                default:
                    Icon = null;
                    Text = null;
                    return false;
            }
            return true;
        }


        public enum CommandTypes
        {
            Forward,
            Backward,
            Repeat,
            PenColor,
            Left,
            Right,


            End,
            PenUp,
            PenDown,
        }
        public string[] CommandTypesString = null;
        public static int GetCommandIndex(string str_cmnd)
        {
            for (int i = 0; i < Settings.Turtle.CommandTypesString.Length; i++)
            {
                if (str_cmnd == Settings.Turtle.CommandTypesString[i])
                    return i;
            }
            return -1;
        }

        public static bool DoCommandNeedExtra(CommandTypes commandTypes)
        {
            return commandTypes >= CommandTypes.Forward && commandTypes <= CommandTypes.Right;
        }

        public struct CommandInfo
        {
            public int ID { get; set; }

            public CommandTypes Command { get; set; }
            public int Amount { get; set; }

            public string Tag { get; set; }

            public override string ToString()
            {
                if (this.Amount > 0)
                    return $"{this.Command} {this.Amount}";
                else
                    return $"{this.Command}";
            }
        }

        public KeyValuePair<SkiaTurtleE.CommandTypes, int>[] Commands { get; set; }
        
        public int CurrentCommandIndex { get; private set; }

        //public bool InstantMovementSpeed { get; set; }

        private struct RepeatInfo
        {
            public CommandInfo? start;
            public CommandInfo? end;

            public int start_idx;
            public int end_idx;
            public int total;
            public int remain;

            public RepeatInfo(int sidx, int eidx, int tot, int rem, CommandInfo? s, CommandInfo? e)
            {
                this.start_idx = sidx;
                this.end_idx = eidx;
                this.total = tot;
                this.remain = rem;
                this.start = s;
                this.end = e;
            }
        }
        private List<RepeatInfo> repeats = null;


        public SkiaTurtleE(SKPoint pos, float ang, SKPaint pnt) : base(pos, ang, pnt) 
        {
            this.CommandTypesString = Enum.GetNames(typeof(CommandTypes));
            this.Reset(true);
        }

        public void Reset(bool clear_commands = false)
        {
            if(clear_commands)
                this.Commands = null;

            this.repeats = new List<RepeatInfo>();
            this.CurrentCommandIndex = 0;
        }

        public bool RunCommands()
        {
            if (this.Commands == null)
                return false;
            if (this.Commands.Length <= 0) 
                return false;

            if (this.CurrentCommandIndex >= this.Commands.Length) 
                return false;

            var curr_type = this.Commands[this.CurrentCommandIndex].Key;
            var curr_units = this.Commands[this.CurrentCommandIndex].Value;


            if (curr_type == CommandTypes.Forward || curr_type == CommandTypes.Backward)
            {
                if(this.DistanceSteps <= 0)
                {
                    if (curr_type == CommandTypes.Forward) 
                        this.Forward(curr_units);
                    else 
                        this.Backward(curr_units);

                    this.CurrentCommandIndex++;
                }
                else
                {
                    if (this.RemainingDistance == 0)
                    {
                        if (curr_type == CommandTypes.Forward) this.SetForwardDistance(curr_units, this.DistanceSteps);
                        else this.SetBackwardDistance(curr_units, this.DistanceSteps);
                    }

                    this.DoMovement();

                    if (this.RemainingDistance == 0)
                        this.CurrentCommandIndex++;
                }
            }
            else if(curr_type == CommandTypes.Left || curr_type == CommandTypes.Right)
            {
                this.Angle += (curr_type == CommandTypes.Left) ? +curr_units: -curr_units;
                this.CurrentCommandIndex++;
            }
            else if(curr_type == CommandTypes.PenUp || curr_type == CommandTypes.PenDown)
            {
                this.PenUp = curr_type == CommandTypes.PenUp ? true : false;
                this.CurrentCommandIndex++;
            }
            else if(curr_type == CommandTypes.PenColor)
            {
                this.Paint.Color = Views.ColorPicker.GetColorByIndex(curr_units);
                this.CurrentCommandIndex++;
            }
            else if(curr_type == CommandTypes.Repeat || curr_type == CommandTypes.End)
            {
                if(curr_type == CommandTypes.Repeat)
                {
                    this.repeats.Add(new RepeatInfo(this.CurrentCommandIndex, -1, curr_units, curr_units, null, null));
                }
                else
                {
                    if(this.repeats.Count > 0)
                    {
                        var lstidx = this.repeats.Count - 1;
                        var lst = this.repeats[lstidx];
                        //lst.end = curr;
                        lst.end_idx = this.CurrentCommandIndex;

                        lst.remain--;

                        if (lst.remain > 0)
                        {
                            this.CurrentCommandIndex = lst.start_idx;
                            this.repeats[lstidx] = lst;
                        }
                        else
                            this.repeats.RemoveAt(lstidx);
                    }
                }
                this.CurrentCommandIndex++;
            }
            return true;

        }

        

    }
}
