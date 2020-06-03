using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TurtleGraphics
{
    public class TGScript
    {

        public static string ToJson<T>(T obj)
        {
            return JsonSerializer.Serialize(obj);
        }
        public static T FromJson<T>(string json_str)
        {
            return JsonSerializer.Deserialize<T>(json_str);
        }    



        public static List<SkiaTurtleE.CommandInfo> Parse(string tag, params string[] tgscript_lines)
        {
            if (tgscript_lines == null || tgscript_lines.Length <= 0) return null;

            var parsed_list = new List<SkiaTurtleE.CommandInfo>();

            var cmd_type_strs = Enum.GetNames(typeof(SkiaTurtleE.CommandTypes)).ToList();

            //int expecting_end_repeat = 0;

            foreach(string ln in tgscript_lines)
            {
                var itm = new SkiaTurtleE.CommandInfo();
                itm.Tag = tag;
                itm.Amount = -1;


                string trmd_ln = ln.Trim();

                if (trmd_ln.StartsWith(";") || String.IsNullOrWhiteSpace(trmd_ln))
                    continue;

                int c_idx = cmd_type_strs.FindIndex(x => trmd_ln.StartsWith(x, StringComparison.CurrentCultureIgnoreCase));

                if (trmd_ln == "]")
                    c_idx = (int)SkiaTurtleE.CommandTypes.EndRepeat;

                if (c_idx == -1) return null;

                itm.Command = (SkiaTurtleE.CommandTypes)c_idx;

                if (SkiaTurtleE.DoCommandNeedAmount((SkiaTurtleE.CommandTypes)c_idx))
                {
                    string[] splt = trmd_ln.Split(' ');
                    if (splt.Length < 2) return null;
                    int amnt = -1;
                    if (!int.TryParse(splt[1], out amnt)) return null;

                    itm.Amount = amnt;
                }

                parsed_list.Add(itm);
            }

            return parsed_list;
        }
        public static List<SkiaTurtleE.CommandInfo> Parse(string tag, string tgscript, char separator = '\n')
        {
            if (string.IsNullOrWhiteSpace(tgscript)) return null;

            return Parse(tag, tgscript.Split(separator));
        }

        public static string Generate(List<SkiaTurtleE.CommandInfo> commands, char separator = '\n')
        {
            if (commands == null || commands.Count <= 0) return null;
            //string gen = $";name:{commands[0].Tag ?? "unnamed"}{separator}";
            string gen = "";

            //uint expected_end_repeat = 0;
            foreach(var c in commands)
            {
                if(c.Command == SkiaTurtleE.CommandTypes.Repeat)
                {
                    gen += $"{c.Command} {c.Amount} [";
                }
                else if(c.Command == SkiaTurtleE.CommandTypes.EndRepeat)
                {
                    gen += "]";
                }
                else
                {
                    gen += c.Amount == -1 ? $"{c.Command}" : $"{c.Command} {c.Amount}";
                }
                gen += $"{separator}";
            }

            return gen;
        }
    }
}
