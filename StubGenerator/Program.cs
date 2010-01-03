using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StubGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    /// <summary>
    /// This class will try to parse the documentation files found in the
    /// original git documentation folder.
    /// Currently it only contains the Parse method.
    /// </summary>
    public static class DocumentationParser
    {
        /// <summary>
        /// Used for possible inclusions in the documentation.
        /// </summary>
        private static string directory;

        /// <summary>
        /// This method will try to parse one file and will return all
        /// options/arguments found.
        /// </summary>
        /// <param name="file">The file which should be parsed.</param>
        /// <returns>List of OptArgs containing all options/arguments found.</returns>
        public static List<OptArg> Parse(string file)
        {
            directory = new FileInfo(file).DirectoryName;
            List<string> optionPart = GetOptionPart(File.ReadAllLines(file));
            
            



            return new List<OptArg>();
        }

        /// <summary>
        /// This method scans each line of the file-to-be-parsed, when it finds
        /// the "OPTIONS" caption, it reads every following line into a new list.
        /// Untill it finds the beginning of the next paragraph.
        /// </summary>
        /// <param name="content">The content of the file (usually File.ReadAllLines())</param>
        /// <returns>a List containig each line of the option paragraph.</returns>
        private static List<string> GetOptionPart(string[] content)
        {
            List<string> optionPart = new List<string>();

            bool inContent = false;

            // A line with 3 or more dashes denotes a new paragraph, and thus the end
            // of the options paragraph.
            for (int i = 0; !(inContent && content[i + 2].Contains("---")); i++)
            {
                if (inContent)
                {
                    optionPart.Add(content[i]);
                }
                if (content[i] == "OPTIONS" && content[i + 1] == "-------")
                {
                    inContent = true;
                    i++;
                }
            }
            
            // Insert extra options if defined and found...
            List<string> extraOptions = new List<string>();

            foreach(string toBeIncluded in optionPart.Where<string>( o => o.StartsWith("include") && o.Contains("options")))
            {
                extraOptions.AddRange(File.ReadAllLines(directory + "\\" + toBeIncluded.Replace("[]", "").Replace("include::", "")));
            }

            optionPart.RemoveAll(o => o.StartsWith("include"));
            optionPart.AddRange(extraOptions);
            
            return optionPart;
        }
    }

    /// <summary>
    /// A struct containing the name, description and suggested delegate-to-be of the
    /// argument/option
    /// </summary>
    public struct OptArg
    {
		/// <summary>
		/// The command/option used on the CLI
		/// </summary>
        public string Name { get; set; }
		
		/// <summary>
		/// The description from the original GIT documentation.
		/// </summary>
        public string Descr { get; set; }
		
		/// <summary>
		/// A suggestion delegate  which can be used by nDesk;
		/// </summary>
        public string Deleg { get; set; }
    }
}
