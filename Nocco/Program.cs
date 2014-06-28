// The entrance point for the program.  Just run Nocco!

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            bool verbose = false;
            var options = new OptionSet()
                            {
                                { "c|console", v => console = v != null },
                                { "p|prefix=", v => prefix = v},
                                { "o|output=", v => dir = v},
                                { "h|?|help", v => help = v != null},
                                { "v|verbose", v => verbose = v != null }
                            };

            var notHandledArgs = options.Parse(args);

            if (verbose)
            {
                Console.WriteLine("Prefix: {0}, Output dir: {1}", prefix, dir);
            }

            if (!notHandledArgs.Any())
            {
                List<string> lines = new List<string>();
                string inputLine = string.Empty;

                do
                {
                    inputLine = Console.ReadLine();
                    if (!string.IsNullOrEmpty(inputLine))
                    {
                        lines.Add(inputLine);
                    }
                    else
                    {
                        break;
                    }
                }
                while (true);

                notHandledArgs = lines;
            }

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
