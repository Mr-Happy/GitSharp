using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StubGenerator
{
    public static class CommandGenerator
    {
        public static string GenerateCLI(string commandName, List<OptArg> options)
        {
            string text = "";
            text += "using System;\r\n";
            text += "using System.Collections.Generic;\r\n";
            text += "using NDesk.Options;\r\n";
            text += "\r\n";
            text += "namespace GitSharp.CLI\r\n";
            text += "{\r\n";
            text += "\r\n";
            text += "    [Command(common=true, requiresRepository=true, usage = \"\")]\r\n";
            text += "    public class " + commandName + " : TextBuiltin\r\n";
            text += "    {\r\n";
            text += "        private " + commandName + "Command cmd = new " + commandName + "Command();\r\n";
            text += "        private static Boolean isHelp;\r\n";
            text += "        public override void Run(string[] args)\r\n";
            text += "        {\r\n";
            text += "            cmd.Quiet = false;\r\n";
            text += "			\r\n";
            text += "            options = new CmdParserOptionSet()\r\n";
            text += "            {\r\n";
            
            foreach(OptArg oa in options)
            {
                text += "               { \"" + oa.Name + "\", \"" + oa.Descr.Replace("\t", "").Replace("\n", " ").Split('.')[0].Trim() + "\", " + oa.Deleg + " },\r\n";
            }

            text += "            };\r\n";
            text += "\r\n";
            text += "            try\r\n";
            text += "            {\r\n";
            text += "                List<String> arguments = ParseOptions(args);\r\n";
            text += "                if (arguments.Count > 0)\r\n";
            text += "                {\r\n";
            text += "                    cmd.arguments = arguments;\r\n";
            text += "                    cmd.Execute();\r\n";
            text += "                }\r\n";
            text += "                else\r\n";
            text += "                {\r\n";
            text += "                    OfflineHelp();\r\n";
            text += "                }\r\n";
            text += "            }\r\n";
            text += "            catch (Exception e)            \r\n";
            text += "            {\r\n";
            text += "                cmd.OutputStream.WriteLine(e.Message);\r\n";
            text += "            }\r\n";
            text += "        }\r\n";
            text += "\r\n";
            text += "        private void OfflineHelp()\r\n";
            text += "        {\r\n";
            text += "            if (!isHelp)\r\n";
            text += "            {\r\n";
            text += "                isHelp = true;\r\n";
            text += "                cmd.OutputStream.WriteLine(\"/*Usage*/\");\r\n";
            text += "                cmd.OutputStream.WriteLine();\r\n";
            text += "                options.WriteOptionDescriptions(Console.Out);\r\n";
            text += "                cmd.OutputStream.WriteLine();\r\n";
            text += "            }\r\n";
            text += "        }\r\n";
            text += "    }\r\n";
            text += "}\r\n";

            return text;
        }

        public static string GenerateAPI(string commandName, List<OptArg> options)
        {
            string text = "";
            text += "using System;\r\n";
            text += "using System.Collections.Generic;\r\n";
            text += "using System.IO;\r\n";
            text += "using System.Linq;\r\n";
            text += "using System.Text;\r\n";
            text += "using GitSharp.Core.Transport;\r\n";
            text += "using GitSharp.Core;\r\n";
            text += "\r\n";
            text += "namespace GitSharp\r\n";
            text += "{\r\n";
            text += "    public class " + commandName + "Command\r\n";
            text += "        : AbstractCommand\r\n";
            text += "    {\r\n";
            text += "\r\n";
            text += "        public " + commandName + "Command() {\r\n";
            text += "        }\r\n";
            text += "\r\n";
            text += "        // note: the naming of command parameters is not following .NET conventions in favour of git command line parameter naming conventions.\r\n";
            text += "\r\n";
            text += "        public List<string> arguments { get; set; }";
            
            foreach(OptArg oa in options)
            {
                text += "\r\n";
                text += "        /// <summary>\r\n";
                text += "        /// Not implemented\r\n";
                text += "        /// \r\n";
                text += "        /// " + oa.Descr.Replace("\t", "").Replace("\n", "\r\n        /// ");
                text += "</summary>\r\n";

                if(oa.Name.EndsWith("="))
                {
                    string propertyName = oa.Name.Remove(oa.Name.Length - 1);

                    if(propertyName.Contains("|"))
                    {
                        propertyName = propertyName.Split('|')[1];
                    }

                    propertyName = propertyName.Replace("-", "");
                    propertyName = propertyName.ToUpper()[0] + propertyName.Substring(1);
                    text += "        public string " + propertyName + " { get; set; }\r\n";
                }
                else
                {
                    string propertyName = oa.Name;
                    if(propertyName.Contains("|"))
                    {
                        propertyName = propertyName.Split('|')[1];
                    }

                    propertyName = propertyName.Replace("-", "");
                    propertyName = propertyName.ToUpper()[0] + propertyName.Substring(1);
                    text += "        public bool " + propertyName + " { get; set; }\r\n";
                }
            }
            text += "\r\n";
            text += "        public override void Execute()\r\n";
            text += "        {\r\n";
            text += "            throw new NotImplementedException();\r\n";
            text += "        }\r\n";
            text += "    }\r\n";
            text += "}\r\n";

            return text;
        }
    }
}
