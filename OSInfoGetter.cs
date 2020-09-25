namespace gtkfetch
{
    class OSInfoGetter
    {
        static string osexpr = @"PRETTY_NAME=""(.*)""";
        public static string GetOS()
        {
            return FileReader.ReadFileAndFindGroup("/etc/os-release", osexpr, 1);
        }
    }
}