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
        /// This method will try to parse one file and will return all
        /// options/arguments found.
        /// </summary>
        /// <param name="file">The file which should be parsed.</param>
        /// <returns>List of OptArgs containing all options/arguments found.</returns>
        public static List<OptArg> Parse(string file)
        {

            return new List<OptArg>();
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
