using Gtk;
using System;
using System.Net;

namespace gtkfetch
{
    public class MainWindow 
    {
        public static Grid maingrid = new Grid();
        public static void InitWindow() 
        {
            Application.Init();

            // create main window
            Window window = new Window($"{Environment.UserName}@{Dns.GetHostName()}");
            window.DeleteEvent += delete_event;

            // add main grid to window
            window.Add(maingrid);
            Labels.Init();

            // display window
            window.ShowAll();

            Application.Run();            
        }
        // handle window exit
        static void delete_event(object obj, DeleteEventArgs args) 
        {
            Application.Quit();
        }
    }
}