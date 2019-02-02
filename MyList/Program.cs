using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace MyList
{
    internal class Program
    {
        private static readonly Regex title = new Regex(@"# File: (?<AnimeName>.*) - (?<EpisodeNumber>.*) - (?<EpisodeName>.*) - (?<GroupName>.*).(?<FileExtension>.{3,})");
        private static readonly Regex ed2k = new Regex(@"# UID: (?<FileSize>[0-9]+):(?<ED2K>.+)");
        private static readonly Regex mylist = new Regex(@"(221 MYLIST)+");
        private static readonly Regex aid = new Regex(@"(\|+\d*)+");

        private static List<Anime> animes = new List<Anime>();

        private static void Main(string[] args)
        {
            //Code to handling command line launch
            string path = "";
            if (args.Length > 0)
            {
                path = args[0];
            }
            //Command to handle manual launch
            else
            {
                Console.WriteLine("Insert file path:");
                Console.Write(">");
                path = Console.ReadLine();
            }

            if (File.Exists(path))
                createCleanJson(path);
            else
                Console.WriteLine("File not found");

            Console.WriteLine("Done");
            Console.ReadKey();
        }

        private static void createCleanJson(string file)
        {
            Anime current = new Anime();

            var lines = File.ReadLines(file);
            foreach (var line in lines)
            {
                //Tries to get the title part of the export => # File: ED2K file name
                var matchTitle = title.Match(line);
                if (matchTitle.Length > 0)
                {
                    try
                    {
                        current.Name = matchTitle.Groups["AnimeName"].ToString();
                        current.EpisodeNumber = matchTitle.Groups["EpisodeNumber"].ToString();
                        current.EpisodeTitle = matchTitle.Groups["EpisodeName"].ToString();
                        current.GroupName = matchTitle.Groups["GroupName"].ToString();
                    }
                    catch { }
                }

                //Tries to get the  ED2K part of the export => # UID: file size in bytes:ED2K hash
                var matchEd2k = ed2k.Match(line);
                if (matchEd2k.Length > 0)
                    try { current.ED2K = matchEd2k.Groups["ED2K"].ToString(); } catch { }

                //Tries to get the mylist UDP success code => 221 MYLIST
                var matchMylist = mylist.Match(line);
                if (matchMylist.Length > 0)
                    try { Console.WriteLine("MyList code ignored"); } catch { } //Just to see if this is working

                //Tries to get the anime id from the string => {int4 lid}|{int4 fid}|{int4 eid}|{int4 aid}|{int4 gid}|{int4 date}|{int2 state}|{int4 viewdate}|{str storage}|{str source}|{str other}|{int2 filestate}
                var matchAid = aid.Match(line);
                if (matchAid.Length > 0)
                    try { current.ID = matchAid.Value.Split('|')[3].ToString(); } catch { /*TODO : A better regex pattern...*/ }

                //If all fields are filled then procced to next anime
                if (current.isComplete())
                {
                    animes.Add(current);
                    current = new Anime();
                }
            }

            //Write the new file
            File.WriteAllText(Path.ChangeExtension(file, "_clean.json"), JsonConvert.SerializeObject(animes));
        }
    }
}