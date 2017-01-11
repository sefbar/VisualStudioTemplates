using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildAnalyzer
{
    class Program
    {

        const string errorText = ": error ";
        public static int Main(string[] args)
        {
            List<string> projectPath = new List<string>();
            string buildLogFileName;

            if (args.Length == 0)
                buildLogFileName = "buildDebug.log";
            else
                buildLogFileName = args[0];

            
            var counter = 0;
            try
            {
                using (var sw = new StreamWriter(buildLogFileName + ".errors.txt"))
                {
                    try
                    {
                        using (var sr = new StreamReader(buildLogFileName))
                        {
                            string l;

                            bool foundFailed = false;
                            while ((l = sr.ReadLine()) != null)
                            {
                                if (l.Contains("Build FAILED."))
                                    foundFailed = true;
                                if (foundFailed)
                                {
                                    int position = l.IndexOf(errorText);
                                    if (position >= 0)
                                    {
                                        string projectFolder = GetProjectFolder(l);
                                        string error = l.Substring(position + 2);

                                        // removing [ ] from the project path
                                        string proj = l.Substring(l.LastIndexOf('[') + 1);
                                        proj = proj.Remove(proj.Length - 1);
                                        
                                        //check if the project already exist in the list
                                        if(!projectPath.Contains(proj))
                                            projectPath.Add(proj);

                                        string codeFileName = l.Remove(l.IndexOf('(')).Trim();
                                        var helper = l.Substring(l.IndexOf('(') + 1);
                                        helper = helper.Remove(helper.IndexOf(')'));
                                        var parts = helper.Split(',');
                                        int line = int.Parse(parts[0]);
                                        int column = int.Parse(parts[1]);

                                        string fullFileName = Path.GetFullPath( Path.Combine(Path.GetDirectoryName(buildLogFileName), Path.GetFileName(projectFolder), codeFileName));
                                        
                                        string lastProgram = "";

                                        try
                                        {
                                            using (var codeReader = new StreamReader(fullFileName))
                                            {
                                                string codeLine;
                                                int i = 0;

                                                while ((codeLine = codeReader.ReadLine()) != null)
                                                {
                                                    var progPos = codeLine.IndexOf("(P#");
                                                    if (progPos > 0)
                                                    {
                                                        lastProgram = codeLine.Substring(progPos);
                                                        lastProgram = lastProgram.Remove(lastProgram.IndexOf(')') + 1);
                                                    }
                                                    i++;
                                                    if (i == line)
                                                    {
                                                        codeLine = codeLine.Replace("\t", " ");
                                                        sw.WriteLine(i.ToString() + ": " + codeLine);
                                                        sw.WriteLine(new string('-', column - 1 + i.ToString().Length + 2) + '^' + "  " + error.Substring(error.IndexOf(':') + 2));
                                                        sw.WriteLine(fullFileName + " " + lastProgram);
                                                    }
                                                }
                                            }
                                        }
                                        catch { }

                                        sw.WriteLine();
                                        counter++;



                                    }
                                }
                            }
                            if (counter > 0)
                            {
                                sw.WriteLine("========================");
                                sw.WriteLine("Total errors: " + counter);
                                sw.WriteLine("========================");
                                sw.WriteLine("");
                                sw.WriteLine("Projects:");

                                foreach (var item in projectPath)
                                {
                                    sw.WriteLine(item);
                                }


                                return -1;
                            }


                            return 0;
                        }
                    }
                    catch (Exception e)
                    {

                        sw.WriteLine("Build Analyzer Error:");
                        sw.WriteLine(e.Message);
                        sw.WriteLine(e.StackTrace);
                        counter = 1;
                        return 0;
                    }

                }
            }
            finally
            {
                if (File.Exists(buildLogFileName + ".NoErrors.txt"))
                    File.Delete(buildLogFileName + ".NoErrors.txt");
                if (counter == 0)
                    File.Move(buildLogFileName + ".errors.txt", buildLogFileName + ".NoErrors.txt");
                
                    
            }

        }

        private static string GetProjectFolder(string l)
        {
            int lastOpenBraket = l.LastIndexOf('[');
            if (lastOpenBraket > 0)
            {
                l = l.Substring(lastOpenBraket + 1);
                l = l.Remove(l.IndexOf(']'));
                return Path.GetDirectoryName(l);
            }
            return "The project file could not be loaded";

        }
    }
}
