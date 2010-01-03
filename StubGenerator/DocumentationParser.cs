using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StubGenerator
{
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

            int begin = 0;

            List<OptArg> options = new List<OptArg>();

            for (int i = 0; i < optionPart.Count; i++)
            {
                if (optionPart[i] != "")
                {
                    begin = i;
                    for (; i < optionPart.Count - 1 && optionPart[i] != ""; i++) ;
                    options.AddRange(ParseOption(optionPart, begin, i));
                }

            }
            return options;
        }

        // ([Mr Happy] Note/keep in mind that the word "option" is used in a few (potentially confusing) different ways.
        //             It is meant as the input, as well as a specific option which is something else than an argument.
        //             I just suck at naming stuff ^^.

        /// <summary>
        /// This method tries to categorize the input as an Option or Argument.
        /// It also tries to appoint a delegate which can be used w/ nDesk.
        /// 
        /// </summary>
        /// <param name="options">A List containing all options</param>
        /// <param name="startIndex">The index of the starting entry in the List of the option which should be parsed</param>
        /// <param name="endIndex">The index of the ending entry in the List of the option which should be parsed</param>
        /// <returns>An OptArg struct containing the parsed option.</returns>
        public static List<OptArg> ParseOption(List<string> options, int startIndex, int endIndex)
        {
            List<OptArg> resultingOptArgs = new List<OptArg>();

            string description = "";

            for (int i = startIndex; i <= endIndex; i++)
            {
                if (options[i].StartsWith("<"))
                {
                    //ignore it.
                }
                else if (options[i].StartsWith("-"))
                {
                    // In some documentation files the short options is listed
                    // before the long version. But in others it's the other way
                    // around.

                    if (options[i][1] == '-') //If it is a long option try to find a the short one
                    {
                        OptArg tempOpt = new OptArg();
                        string firstLetter = options[i][2].ToString();
                        string longName = options[i].Replace("=", " ").Replace("::", "").Substring(2).Split(' ')[0]; // Only use the option name itself.

                        if (options[i + 1].StartsWith("-") && !options[1 + 1].StartsWith("--")) // Found short version.
                        {
                            if (options[i + 1].Contains("<") || options[i].Contains("<") || options[i].Contains("=")) //the option takes an argument
                            {
                                tempOpt.Name = firstLetter + "|";
                                tempOpt.Name += longName;
                                tempOpt.Name += "=";

                                longName = longName.ToUpper()[0] + longName.Substring(1).Replace("-", "");
                                tempOpt.Deleg = "v => cmd." + longName + " = v";
                            }
                            else
                            {
                                tempOpt.Name = firstLetter + "|";
                                tempOpt.Name += longName;
                                longName = longName.ToUpper()[0] + longName.Substring(1).Replace("-", "");
                                tempOpt.Deleg = "v => cmd." + longName + " = true";
                            }
                            resultingOptArgs.Add(tempOpt);
                            i++; // Skip parsing the short version.
                        }
                        else // No short version available.
                        {
                            if (options[i + 1].Contains("<") || options[i].Contains("<") || options[i].Contains("=")) //the option takes an argument
                            {
                                tempOpt.Name = longName;
                                tempOpt.Name += "=";

                                longName = longName.ToUpper()[0] + longName.Substring(1).Replace("-", "");
                                tempOpt.Deleg = "v => cmd." + longName + " = v";
                            }
                            else
                            {
                                tempOpt.Name = longName;
                                longName = longName.ToUpper()[0] + longName.Substring(1).Replace("-", "");
                                tempOpt.Deleg = "v => cmd." + longName + " = true";
                            }
                            resultingOptArgs.Add(tempOpt);
                        }
                    }
                    else // If it is the short option try to find the long one.
                    {
                        OptArg tempOpt = new OptArg();
                        string firstLetter = options[i][1].ToString();

                        if (options[i + 1].StartsWith("--")) // Found the long version.
                        {
                            string longName = options[i + 1].Replace("=", " ").Replace("::", "").Substring(2).Split(' ')[0]; // Only use the option name itself.
                            if (options[i + 1].Contains("<") || options[i].Contains("<") || options[i + 1].Contains("=")) //the option takes an argument
                            {
                                tempOpt.Name = firstLetter + "|";
                                tempOpt.Name += longName;
                                tempOpt.Name += "=";

                                longName = longName.ToUpper()[0] + longName.Substring(1).Replace("-", "");
                                tempOpt.Deleg = "v => cmd." + longName + " = v";
                            }
                            else
                            {
                                tempOpt.Name = firstLetter + "|";
                                tempOpt.Name += longName;
                                longName = longName.ToUpper()[0] + longName.Substring(1).Replace("-", "");
                                tempOpt.Deleg = "v => cmd." + longName + " = true";
                            }
                            resultingOptArgs.Add(tempOpt);
                            i++; // Skip parsing the long version.
                        }
                        else // No long version available.
                        {
                            if (options[i + 1].Contains("<") || options[i].Contains("<") || options[i].Contains("=")) //the option takes an argument
                            {
                                tempOpt.Name = firstLetter;
                                tempOpt.Name += "=";

                                tempOpt.Deleg = "v => cmd." + firstLetter.ToUpper() + " = v";
                            }
                            else
                            {
                                tempOpt.Name = firstLetter;
                                tempOpt.Deleg = "v => cmd." + firstLetter.ToUpper() + " = true";
                            }
                            resultingOptArgs.Add(tempOpt);
                        }
                    }
                    //resultingOptArgs.Add(new op
                }
                else
                {
                    description += options[i] + "\n";
                }
            }

            resultingOptArgs.ForEach(oa => oa.Descr = description);

            //for (int i = 0; i < resultingOptArgs.Count; i++)
            //{
            //    resultingOptArgs.ElementAt<OptArg>(i).Descr = description;
            //}

            return resultingOptArgs;
        }

        //private static OptArg 
        /// <summary>
        /// This method scans each line of the file-to-be-parsed, when it finds
        /// the "OPTIONS" caption, it reads every following line into a new list.
        /// Untill it finds the beginning of the next paragraph.
        /// </summary>
        /// <param name="content">The content of the file (usually File.ReadAllLines())</param>
        /// <returns>a List containig each line of the option paragraph</returns>
        public static List<string> GetOptionPart(string[] content)
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

            foreach (string toBeIncluded in optionPart.Where<string>(o => o.StartsWith("include") && o.Contains("options")))
            {
                extraOptions.AddRange(File.ReadAllLines(directory + "\\" + toBeIncluded.Replace("[]", "").Replace("include::", "")));
            }

            optionPart.RemoveAll(o => o.StartsWith("include"));
            optionPart.AddRange(extraOptions);

            return optionPart;
        }
    }

    /// <summary>
    /// A class containing the name, description and suggested delegate-to-be of the
    /// argument/option
    /// </summary>
    public class OptArg
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
