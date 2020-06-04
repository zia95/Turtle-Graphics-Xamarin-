using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace TurtleGraphics
{
    public class SkiaTurtleE : SkiaTurtle
    {
        public enum CommandTypes
        {
            Forward,
            Backward,
            Rotate,
            Repeat,
            
            

            EndRepeat,
            Left,
            Right,
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

        public static bool DoCommandNeedAmount(CommandTypes commandTypes)
        {
            return commandTypes >= CommandTypes.Forward && commandTypes <= CommandTypes.Repeat;
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

        public CommandInfo[] Commands { get; set; }
        
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

            var curr = this.Commands[this.CurrentCommandIndex];

            if(curr.Command == CommandTypes.Forward || curr.Command == CommandTypes.Backward)
            {
                if(this.DistanceSteps <= 0)
                {
                    if (curr.Command == CommandTypes.Forward) 
                        this.Forward(curr.Amount);
                    else 
                        this.Backward(curr.Amount);

                    this.CurrentCommandIndex++;
                }
                else
                {
                    if (this.RemainingDistance == 0)
                    {
                        if (curr.Command == CommandTypes.Forward) this.SetForwardDistance(curr.Amount, this.DistanceSteps);
                        else this.SetBackwardDistance(curr.Amount, this.DistanceSteps);
                    }

                    this.DoMovement();

                    if (this.RemainingDistance == 0)
                        this.CurrentCommandIndex++;
                }
            }
            else if(curr.Command == CommandTypes.Left || curr.Command == CommandTypes.Right || curr.Command == CommandTypes.Rotate)
            {
                float ang = 0;

                if (curr.Command == CommandTypes.Left) ang = 90 + this.Angle;
                else if (curr.Command == CommandTypes.Right) ang = -90 + this.Angle;
                else if (curr.Command == CommandTypes.Rotate) ang = curr.Amount + this.Angle;

                this.Angle = ang;
                this.CurrentCommandIndex++;
            }
            else if(curr.Command == CommandTypes.PenUp || curr.Command == CommandTypes.PenDown)
            {
                this.PenUp = curr.Command == CommandTypes.PenUp ? true : false;
                this.CurrentCommandIndex++;
            }
            else if(curr.Command == CommandTypes.Repeat || curr.Command == CommandTypes.EndRepeat)
            {
                if(curr.Command == CommandTypes.Repeat)
                {
                    this.repeats.Add(new RepeatInfo(this.CurrentCommandIndex, -1, curr.Amount, curr.Amount, curr, null));
                }
                else
                {
                    if(this.repeats.Count > 0)
                    {
                        var lstidx = this.repeats.Count - 1;
                        var lst = this.repeats[lstidx];
                        lst.end = curr;
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
