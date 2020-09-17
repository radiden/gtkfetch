using Gtk;
using System;
using System.Net;
using System.Timers;

namespace gtkfetch
{
    public class InfoLabel
    {
        public Label titleLabel;
        public Label contentLabel;
        public Image icon;
        public int topPos;
        public InfoLabel(string title, string iconname, int top)
        {
            titleLabel = MainWindow.mkLabel(title);
            // align text to left
            titleLabel.Xalign = 0.0f;
            contentLabel = new Label();
            contentLabel.Xalign = 0.0f;
            icon = Image.NewFromIconName(iconname, IconSize.LargeToolbar);
            topPos = top;
        }
        public static void AttachAllToGrid(Grid maingrid, InfoLabel label, int top)
        {
            maingrid.Attach(label.icon, 0, top, 1, 1);
            maingrid.Attach(label.titleLabel, 1, top, 1, 1);
            maingrid.Attach(label.contentLabel, 2, top, 1, 1);
        }
    }
    public class MainWindow 
    {
        // create InfoLabel instances for each label
        static InfoLabel osLabel = new InfoLabel("os", "media-floppy", 0);
        static InfoLabel uptimeLabel = new InfoLabel("uptime", "video-television", 1);
        static InfoLabel cpuLabel = new InfoLabel("cpu", "cpu", 2);
        static InfoLabel memoryLabel = new InfoLabel("mem", "media-memory", 3);
        // create main grid here so it's accessible from other plices
        static Grid maingrid = new Grid();

        public static void InitWindow() 
        {
            Application.Init();

            //iterate over all labels, attach them in order
            InfoLabel[] labels = {osLabel, uptimeLabel, cpuLabel, memoryLabel};
            int iter = 0;
            foreach(InfoLabel label in labels)
            {
                InfoLabel.AttachAllToGrid(maingrid, label, iter);
                iter++;
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
            maingrid.Halign = Align.Center;
            maingrid.Valign = Align.Center;

            // get various info
            LabelUpdate();
            MemInfoGetter.GetMemInfo();
            CPUInfoGetter.GetCPUInfo();
            window.Add(maingrid);

            // display window
            window.ShowAll();

            FSInfoGetter.GetDrives();

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
            osLabel.contentLabel.Text = $"{Environment.OSVersion.ToString()}";
            uptimeLabel.contentLabel.Text = $"{UptimeCalculator.GetUptimeStr()}";
            cpuLabel.contentLabel.Text = $"{CPUInfoGetter.CPU.model} @ {CPUInfoGetter.CPU.speed}";
            memoryLabel.contentLabel.Text = $"{Math.Round(MemInfoGetter.Mem.used/1048576, 2)} GiB/{Math.Round(MemInfoGetter.Mem.total/1048576, 2)} GiB";
        }
        public static Label mkLabel(string content)
        {
            Label newlabel = new Label(content);
            newlabel.MarginStart = 16;
            newlabel.MarginEnd = 16;
            return newlabel;
        }
    }
}