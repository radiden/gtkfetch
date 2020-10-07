using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace gtkfetch
{
    public class FileReader
    {
        /// <summary> Returns first match </summary>
        public static string ReadFileAndFindGroup(string path, string regex, int group)
        {
            try 
            {
                using (StreamReader file = new StreamReader(path)) 
                {
                    string line;

                    while ((line = file.ReadLine()) != null)
                    {
                        if (Regex.IsMatch(line, regex))
                        {
                            Match m = Regex.Match(line, regex);
                            return m.Groups[group].Value.ToString();
                        }
                    }
                }
            }
            catch (Exception e) 
            {
                Console.WriteLine($"something broke: {e}");
                return null;
            }
            return null;

        }
        /// <summary> Reads file and regex matches things on all lines of the file </summary> 
        public static List<string> ReadFileMatchMultiple(string path, string regex, int group)
        {
            List<string> matches = new List<string>();
            try 
            {
                using (StreamReader file = new StreamReader(path)) 
                {
                    string line;

                    while ((line = file.ReadLine()) != null)
                    {
                        if (Regex.IsMatch(line, regex))
                        {
                            Match m = Regex.Match(line, regex);
                            // check if the group even exists, and also checks if its not an empty match
                            if (m.Groups[1] != null && m.Groups[1].Value != "")
                            {
                                matches.Add(m.Groups[group].Value.ToString());
                            }
                        }
                    }
                    return matches;
                }
            }
            catch (Exception e) 
            {
                Console.WriteLine($"something broke: {e}");
                return null;
            }
        }
        /// <summary> Reads single line of file </summary>
        public static string ReadLine(string path)
        {
            try 
            {
                using (StreamReader file = new StreamReader(path)) 
                {
                    return file.ReadLine();
                }
            }
            catch (Exception e) 
            {
                Console.WriteLine($"something broke: {e}");
                return null;
            }
        }
    }
}