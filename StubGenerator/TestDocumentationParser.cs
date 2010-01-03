using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace StubGenerator
{
    [TestFixture]
    public class TestDocumentationParser
    {
        [Test]
        public void TestParseOption()
        {
            List<string> options = new List<string>();
            options.Add("--local::");
            options.Add("-l::");
            options.Add("\tWhen the repository to clone from is on a local machine,");
            options.Add("\tthis flag bypasses the normal \"git aware\" transport");
            options.Add("\tmechanism and clones the repository by making a copy of");
            options.Add("\tHEAD and everything under objects and refs directories.");
            options.Add("\tThe files under `.git/objects/` directory are hardlinked");
            options.Add("\tto save space when possible.  This is now the default when");
            options.Add("\tthe source repository is specified with `/path/to/repo`");
            options.Add("\tsyntax, so it essentially is a no-op option.  To force");
            options.Add("\tcopying instead of hardlinking (which may be desirable");
            options.Add("\tif you are trying to make a back-up of your repository),");
            options.Add("\tbut still avoid the usual \"git aware\" transport");
            options.Add("\tmechanism, `--no-hardlinks` can be used.");
            options.Add("");
            options.Add("-s::");
            options.Add("--shared::");
            options.Add("\tWhen the repository to clone is on the local machine,");
            options.Add("\tinstead of using hard links, automatically setup");
            options.Add("\t`.git/objects/info/alternates` to share the objects");
            options.Add("\twith the source repository.  The resulting repository");
            options.Add("\tstarts out without any object of its own.");
            List<OptArg> result = DocumentationParser.ParseOption(options, 0, 13);
            
            Assert.AreEqual("l|local", result[0].Name);
            Assert.AreEqual("\tWhen the repository to clone from is on a local machine,\n\tthis flag bypasses the normal \"git aware\" transport\n\tmechanism and clones the repository by making a copy of\n\tHEAD and everything under objects and refs directories.\n\tThe files under `.git/objects/` directory are hardlinked\n\tto save space when possible.  This is now the default when\n\tthe source repository is specified with `/path/to/repo`\n\tsyntax, so it essentially is a no-op option.  To force\n\tcopying instead of hardlinking (which may be desirable\n\tif you are trying to make a back-up of your repository),\n\tbut still avoid the usual \"git aware\" transport\n\tmechanism, `--no-hardlinks` can be used.\n", result[0].Descr);
            Assert.AreEqual("v => cmd.Local = true", result[0].Deleg);

            result = DocumentationParser.ParseOption(options, 15, 21);
            
            Assert.AreEqual("s|shared", result[0].Name);
            Assert.AreEqual("\tWhen the repository to clone is on the local machine,\n\tinstead of using hard links, automatically setup\n\t`.git/objects/info/alternates` to share the objects\n\twith the source repository.  The resulting repository\n\tstarts out without any object of its own.\n", result[0].Descr);
            Assert.AreEqual("v => cmd.Shared = true", result[0].Deleg);
        }

        [Test]
        public void TestParseOption2()
        {
            List<string> options = new List<string>();

            options.Add("--no-hardlinks::");
            options.Add("\tOptimize the cloning process from a repository on a");
            options.Add("\tlocal filesystem by copying files under `.git/objects`");
            options.Add("\tdirectory.");

            List<OptArg> result = DocumentationParser.ParseOption(options, 0, 3);
            
            Assert.AreEqual("no-hardlinks", result[0].Name);
            Assert.AreEqual("\tOptimize the cloning process from a repository on a\n\tlocal filesystem by copying files under `.git/objects`\n\tdirectory.\n", result[0].Descr);
            Assert.AreEqual("v => cmd.Nohardlinks = true", result[0].Deleg);

        }

        [Test]
        public void TestParseOption3()
        {
            List<string> options = new List<string>();

            options.Add("--reference <repository>::");
            options.Add("\tIf the reference repository is on the local machine,");
            options.Add("\tautomatically setup `.git/objects/info/alternates` to");
            options.Add("\tobtain objects from the reference repository.  Using");
            options.Add("\tan already existing repository as an alternate will");
            options.Add("\trequire fewer objects to be copied from the repository");
            options.Add("\tbeing cloned, reducing network and local storage costs.");

            List<OptArg> result = DocumentationParser.ParseOption(options, 0, 6);

            Assert.AreEqual("reference=", result[0].Name);
            Assert.AreEqual("\tIf the reference repository is on the local machine,\n\tautomatically setup `.git/objects/info/alternates` to\n\tobtain objects from the reference repository.  Using\n\tan already existing repository as an alternate will\n\trequire fewer objects to be copied from the repository\n\tbeing cloned, reducing network and local storage costs.\n", result[0].Descr);
            Assert.AreEqual("v => cmd.Reference = v", result[0].Deleg);
        }

        [Test]
        public void TestParseOption4()
        {
            List<string> options = new List<string>();

            options.Add("--origin <name>::");
            options.Add("-o <name>::");
            options.Add("\tInstead of using the remote name `origin` to keep track");
            options.Add("\tof the upstream repository, use `<name>`.");

            List<OptArg> result = DocumentationParser.ParseOption(options, 0, 3);

            Assert.AreEqual("o|origin=", result[0].Name);
            Assert.AreEqual("\tInstead of using the remote name `origin` to keep track\n\tof the upstream repository, use `<name>`.\n", result[0].Descr);
            Assert.AreEqual("v => cmd.Origin = v", result[0].Deleg);
        }
    }
}
