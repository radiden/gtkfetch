using Gtk;
using System;
using System.Net;
using System.Timers;
using System.Collections.Generic;

namespace gtkfetch
{
    public class MainWindow 
    {
        static Dictionary<string, Label> labels = new Dictionary<string, Label>();
        string[] labelnames = new string[] {"os", "uptime", "cpu", "mem"};

        public static void InitWindow() 
        {
            Application.Init();

            Grid maingrid = new Grid();


            // init dict with label names and labels for values
            string[] labelnames = new string[] {"os", "uptime", "cpu", "mem"};
            foreach(string label in labelnames)
            {
                labels.Add(label, new Label());
            }
            
            int iteration = 0;
            foreach(KeyValuePair<string, Label> entry in labels)
            {
                // add title labels
                maingrid.Attach(mkLabel(entry.Key), 0, iteration, 1, 1);
                // add content labels
                maingrid.Attach(entry.Value, 1, iteration, 1, 1);
                // add padding to content labels
                entry.Value.MarginStart = 16;
                iteration++;
            }

            // create timer which is used to update the values every second
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Enabled = true;
            timer.Elapsed += timerevent;

            // create main window
            Window window = new Window($"{Environment.UserName}@{Dns.GetHostName()}");
            window.DeleteEvent += delete_event;

            // style main grid
            maingrid.Margin = 8;
            maingrid.RowSpacing = 2;

            // get various info
            LabelUpdate();
            MemInfoGetter.GetMemInfo();
            CPUInfoGetter.GetCPUInfo();
            window.Add(maingrid);

            // display window
            window.ShowAll();

            Application.Run();            
        }
        // handle window exit
        static void delete_event(object obj, DeleteEventArgs args) 
        {
            Application.Quit();
        }
        // ran once timer elapses; updates values on labels
        static void timerevent(object obj, System.Timers.ElapsedEventArgs args)
        {
            LabelUpdate();
        }
        static void LabelUpdate()
        {
            MemInfoGetter.RefreshMemInfo();
            labels["os"].Text = $"{Environment.OSVersion.ToString()}";
            labels["uptime"].Text = $"{UptimeCalculator.GetUptimeStr()}";
            labels["cpu"].Text = $"{CPUInfoGetter.CPU.vendor} {CPUInfoGetter.CPU.model} @ {CPUInfoGetter.CPU.speed}";
            labels["mem"].Text = $"{Math.Round(MemInfoGetter.Mem.used/1048576, 2)} GiB/{Math.Round(MemInfoGetter.Mem.total/1048576, 2)} GiB";
        }
        static Label mkLabel(string content)
        {
            return new Label(content);
        }

    }
}