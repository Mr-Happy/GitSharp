using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StubGenerator
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
          //  @"c:\code\csharp\git\documentation\git-clone.txt"

            string[] fileList = Directory.GetFiles(@"c:\code\csharp\gitsharp\stubgenerator\resources\");

            string text = "";
            foreach(string file in fileList)
            {
                List<OptArg> result = DocumentationParser.Parse(file);
                string clazz = new FileInfo(file).Name;
                clazz = clazz.Replace(".txt", "").Replace("-", "").Substring(3);
                clazz = clazz.ToUpper()[0] + clazz.Substring(1);
                text += CommandGenerator.GenerateCLI(clazz, result);
                text += "\n\n---------------------------\n\n";
                text += CommandGenerator.GenerateAPI(clazz, result);
            }
            
            
            System.Windows.Forms.Clipboard.SetText(text);

            Console.ReadKey();
        }


    }
}
