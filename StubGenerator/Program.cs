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
