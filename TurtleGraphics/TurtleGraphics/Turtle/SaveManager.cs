﻿using System;
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
        public const string S_KEY_TURTLE_SPEED = "t_speed";
        public const string S_KEY_LINE_SIZE = "t_psiz";
        public const string S_KEY_BOARD_COLOR = "b_color_idx";

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


        public static int GetLineSize(int def = 1) => (int)LoadOrDefault(S_KEY_LINE_SIZE, def);
        public static int GetTurtleSpeed(int def = 0) => (int)LoadOrDefault(S_KEY_TURTLE_SPEED, def);
        public static int GetCanvasColorIndex(int def = 2) => (int)LoadOrDefault(S_KEY_BOARD_COLOR, def);
        public static SKColor GetCanvasColor(int def = 2) => Views.ColorPicker.GetColorByIndex((int)GetCanvasColorIndex(def));
        public static void SetLineSize(int val) => Save(S_KEY_LINE_SIZE, (object)val);
        public static void SetTurtleSpeed(int val) => Save(S_KEY_TURTLE_SPEED, (object)val);
        public static void SetCanvasColorIndex(int val) => Save(S_KEY_BOARD_COLOR, (object)val);
        
    }
}
