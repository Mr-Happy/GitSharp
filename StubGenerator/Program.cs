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
            List<OptArg> result = DocumentationParser.Parse(@"c:\code\csharp\git\documentation\git-merge.txt");
            string text = "";

            Console.ReadKey();
        }
    }
}
