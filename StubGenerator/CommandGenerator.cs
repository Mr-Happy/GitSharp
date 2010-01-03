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
            text += "                    cmd.Source = arguments[0];\r\n";
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

        }
    }
}
