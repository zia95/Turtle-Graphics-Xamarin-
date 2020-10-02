using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using Xamarin.Forms;
using TurtleCommand = System.Collections.Generic.KeyValuePair<TurtleGraphics.Turtle.SkiaTurtleE.CommandTypes, int>;
using TurtleCommandList = System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<TurtleGraphics.Turtle.SkiaTurtleE.CommandTypes, int>>;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using SkiaSharp;
using TurtleGraphics.Views;

namespace TurtleGraphics.Turtle
{
    public static class SaveManager
    {
        public const string S_KEY_COMMANDS_LIST_PREFIX = "clst_";
        public const string S_KEY_COMMANDS_LIST_PRESET_PREFIX = "clstpre_";

        public const string S_KEY_TURTLE_SPEED = "t_speed";
        public const string S_KEY_LINE_SIZE = "t_psiz";
        public const string S_KEY_BOARD_COLOR = "b_color_idx";

        private static KeyValuePair<string, TurtleCommandList>[] __presets = null;
        public static KeyValuePair<string, TurtleCommandList>[] PresetCommands 
        { 
            get 
            {
                if (__presets != null)
                    return __presets;

                __presets = new KeyValuePair<string, TurtleCommandList>[]
                {
                    new KeyValuePair<string, TurtleCommandList>("Square", new TurtleCommandList()
                    {
                        new TurtleCommand(SkiaTurtleE.CommandTypes.Repeat, 2),
                        new TurtleCommand(SkiaTurtleE.CommandTypes.Forward, 200),
                        new TurtleCommand(SkiaTurtleE.CommandTypes.Right, 90),
                        new TurtleCommand(SkiaTurtleE.CommandTypes.Forward, 200),
                        new TurtleCommand(SkiaTurtleE.CommandTypes.Right, 90),
                        new TurtleCommand(SkiaTurtleE.CommandTypes.End, 0),
                    }),
                    new KeyValuePair<string, TurtleCommandList>("Star", new TurtleCommandList()
                    {
                        new TurtleCommand(SkiaTurtleE.CommandTypes.Repeat, 5),
                        new TurtleCommand(SkiaTurtleE.CommandTypes.Forward, 300),
                        new TurtleCommand(SkiaTurtleE.CommandTypes.Right, 144),
                        new TurtleCommand(SkiaTurtleE.CommandTypes.End, 0),
                    }),
                    new KeyValuePair<string, TurtleCommandList>("Lots of stars", new TurtleCommandList()
                    {
                        new TurtleCommand(SkiaTurtleE.CommandTypes.Repeat, 5),
                            
                        new TurtleCommand(SkiaTurtleE.CommandTypes.Repeat, 5),
                        new TurtleCommand(SkiaTurtleE.CommandTypes.Forward, 300),
                        new TurtleCommand(SkiaTurtleE.CommandTypes.Right, 144),
                        new TurtleCommand(SkiaTurtleE.CommandTypes.End, 0),
                            
                        new TurtleCommand(SkiaTurtleE.CommandTypes.Left, 144),
                        new TurtleCommand(SkiaTurtleE.CommandTypes.Forward, 100),
                        new TurtleCommand(SkiaTurtleE.CommandTypes.End, 0),
                    }),
                    new KeyValuePair<string, TurtleCommandList>("Polygon", new TurtleCommandList()
                    {
                        new TurtleCommand(SkiaTurtleE.CommandTypes.Repeat, 8),
                        new TurtleCommand(SkiaTurtleE.CommandTypes.Forward, 200),
                        new TurtleCommand(SkiaTurtleE.CommandTypes.Right, 45),
                        new TurtleCommand(SkiaTurtleE.CommandTypes.End, 0),
                    }),
                    new KeyValuePair<string, TurtleCommandList>("Wave", new TurtleCommandList()
                    {
                        new TurtleCommand(SkiaTurtleE.CommandTypes.Repeat, 3),
                        new TurtleCommand(SkiaTurtleE.CommandTypes.Forward, 500),
                        new TurtleCommand(SkiaTurtleE.CommandTypes.Right, 90),
                        new TurtleCommand(SkiaTurtleE.CommandTypes.Forward, 50),
                        new TurtleCommand(SkiaTurtleE.CommandTypes.Right, 90),
                        new TurtleCommand(SkiaTurtleE.CommandTypes.Forward, 500),
                        new TurtleCommand(SkiaTurtleE.CommandTypes.Left, 90),
                        new TurtleCommand(SkiaTurtleE.CommandTypes.Forward, 50),
                        new TurtleCommand(SkiaTurtleE.CommandTypes.Left, 90),
                        new TurtleCommand(SkiaTurtleE.CommandTypes.End, 0),
                    })
                };


                return __presets;
            }
        }


        public static IEnumerable<KeyValuePair<string, TurtleCommandList>> LoadCommandLists()
        {
            foreach(var s in Application.Current.Properties)
            {
                if(s.Key.StartsWith(S_KEY_COMMANDS_LIST_PREFIX))
                {
                    yield return 
                        new KeyValuePair<string, TurtleCommandList>(
                        s.Key.Remove(0, S_KEY_COMMANDS_LIST_PREFIX.Length),
                        JsonSerializer.Deserialize<TurtleCommandList>(s.Value.ToString())
                        );
                }
            }
        }
        public static bool SaveCommandList(string cmd_list_name, TurtleCommandList cmds_list)
        {
            if (cmds_list == null || cmds_list.Count <= 0 || !IsCommandListNameAvailable(cmd_list_name))
                return false;

            Application.Current.Properties.Add($"{S_KEY_COMMANDS_LIST_PREFIX}{cmd_list_name}", JsonSerializer.Serialize(cmds_list));
            Application.Current.SavePropertiesAsync();
            return true;
        }
        
        public static bool IsCommandListNameAvailable(string cmd_list_name)
        {
            if (string.IsNullOrWhiteSpace(cmd_list_name))
                return false;

            foreach (var s in Application.Current.Properties)
            {
                if (s.Key == $"{S_KEY_COMMANDS_LIST_PREFIX}{cmd_list_name}")
                    return false;
            }
            foreach (var s in PresetCommands)
            {
                if (s.Key == cmd_list_name)
                    return false;
            }
            return true;
        }

        public static int GetSavedCommandListCount()
        {
            int c = 0;
            foreach (var s in Application.Current.Properties)
            {
                if (s.Key.StartsWith(S_KEY_COMMANDS_LIST_PREFIX))
                    c++;
            }
            return c;
        }

        //**************************************GENERAL*********************************************//

        public static bool Load(string key, out object val)
        {
            if(Application.Current.Properties.TryGetValue(key, out object _val))
            {
                val = _val;
                return true;
            }
            val = null;
            return false;
        }
        public static void Save(string key, object val)
        {
            Application.Current.Properties[key] = val;
            Application.Current.SavePropertiesAsync();
        }

        public static object LoadOrDefault(string key, object def)
        {
            if(Load(key, out object v))
            {
                return v;
            }
            return def;
        }

        //************************SETTINGS*****************************************//


        public const int DEFAULT_LINE_SIZE = 6;
        public const int DEFAULT_TURTLE_SPEED = 0;
        public const int DEFAULT_CANVAS_COLOR_INDEX = 2;



        public static int GetLineSize(int def = DEFAULT_LINE_SIZE) => (int)LoadOrDefault(S_KEY_LINE_SIZE, def);
        public static int GetTurtleSpeed(int def = DEFAULT_TURTLE_SPEED) => (int)LoadOrDefault(S_KEY_TURTLE_SPEED, def);
        public static int GetCanvasColorIndex(int def = DEFAULT_CANVAS_COLOR_INDEX) => (int)LoadOrDefault(S_KEY_BOARD_COLOR, def);
        public static SKColor GetCanvasColor(int def = DEFAULT_CANVAS_COLOR_INDEX) => Views.ColorPicker.GetColorByIndex((int)GetCanvasColorIndex(def));
        public static void SetLineSize(int val) => Save(S_KEY_LINE_SIZE, (object)val);
        public static void SetTurtleSpeed(int val) => Save(S_KEY_TURTLE_SPEED, (object)val);
        public static void SetCanvasColorIndex(int val) => Save(S_KEY_BOARD_COLOR, (object)val);
        
    }
}
