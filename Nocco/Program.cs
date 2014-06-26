// The entrance point for the program.  Just run Nocco!

using System;
using System.IO;

using NDesk.Options;

namespace Nocco
{
    class Program
    {
        static void Main(string[] args)
        {
            string prefix = string.Empty;
            bool help = false;
            bool console = false;
            string dir = "docs";
            var options = new OptionSet()
                            {
                                { "c|console", v => console = v != null },
                                { "p|prefix=", v => prefix = v},
                                { "o|output=", v => dir = v},
                                { "h|?|help", v => help = v != null}
                            };

            var notHandledArgs = options.Parse(args);

            var reports = Nocco.Generate(notHandledArgs.ToArray(), prefix);
            if (console)
            {
                foreach (var report in reports)
                {
                    Console.WriteLine(report.Content);
                }
            }
            else
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                foreach (var documentation in reports)
                {
                    var outputFile = Path.Combine(dir, Path.ChangeExtension(Path.GetFileName(documentation.File), "html"));
                    if (File.Exists(outputFile))
                    {
                        Console.WriteLine("Warning: file {0} is being overwritten", outputFile);
                    }
                    else
                    {
                        Console.WriteLine(outputFile);
                    }

                    File.WriteAllText(outputFile, documentation.Content);
                }
            }
        }
    }
}
