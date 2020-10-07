namespace gtkfetch
{
    class OSInfoGetter
    {
        static string osexpr = @"PRETTY_NAME=""(.*)""";
        /// <summary> Reads PRETTY_NAME from /etc/fstab and returns the contents </summary>
        public static string GetOS()
        {
            return FileReader.ReadFileAndFindGroup("/etc/os-release", osexpr, 1);
        }
    }
}