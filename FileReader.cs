using System;
using System.IO;
using System.Text.RegularExpressions;

namespace gtkfetch
{
    public class FileReader
    {
        public static string ReadFileAndFindGroup(string path, string regex, int group)
        {
            try 
            {
                using (StreamReader file = new StreamReader(path)) 
                {
                    string line;

                    while((line = file.ReadLine()) != null)
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