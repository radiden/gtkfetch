using Gtk;
using System;
using System.Timers;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace gtkfetch 
{
    /// <summary> Class containing a label with all information needed to make an info element </summary>
    public class InfoLabel
    {
        public Label titleLabel;
        public Label contentLabel;
        public Image icon;
        public int topPos;
        /// <summary> InfoLabel with non-static content, so don't assign a default content value </summary>
        public InfoLabel(string title, string iconname)
        {
            titleLabel = Labels.mkLabel(title);
            // align text to left
            titleLabel.Xalign = 0.0f;
            contentLabel = new Label();
            contentLabel.Xalign = 0.0f;
            icon = Image.NewFromIconName(iconname, IconSize.LargeToolbar);
        }
        /// <summary> InfoLabel with static content </summary>
        public InfoLabel(string title, string iconname, string content)
        {
            titleLabel = Labels.mkLabel(title);
            // align text to left
            titleLabel.Xalign = 0.0f;
            contentLabel = new Label(content);
            contentLabel.Xalign = 0.0f;
            icon = Image.NewFromIconName(iconname, IconSize.LargeToolbar);
        }
        /// <summary> Attaches all elements of an InfoLabel to grid </summary>
        public static void AttachAllToGrid(Grid maingrid, InfoLabel label, int top)
        {
            maingrid.Attach(label.icon, 0, top, 1, 1);
            maingrid.Attach(label.titleLabel, 1, top, 1, 1);
            maingrid.Attach(label.contentLabel, 2, top, 1, 1);
        }
    }
    public class Labels {
        // create InfoLabel instances for each label
        static InfoLabel osLabel = new InfoLabel("os", "media-floppy", OSInfoGetter.GetOS());
        static InfoLabel kernelLabel = new InfoLabel("kernel", "tux", RuntimeInformation.OSDescription);
        static InfoLabel shellLabel = new InfoLabel("shell", "terminal", Environment.GetEnvironmentVariable("SHELL"));
        static InfoLabel uptimeLabel = new InfoLabel("uptime", "video-television");
        public static InfoLabel cpuLabel;
        static InfoLabel memoryLabel = new InfoLabel("mem", "media-memory");
        public static List<InfoLabel> labels = new List<InfoLabel>(){osLabel, kernelLabel, shellLabel, uptimeLabel, cpuLabel, memoryLabel};
        public static void Init() {
            // create timer which is used to update the values every second
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Enabled = true;
            timer.Elapsed += timerevent;

            LabelUpdate();
            MemInfoGetter.GetMemInfo();
            CPUInfoGetter.GetCPUInfo();
            GPUInfoGetter.GetGPUInstances();

            int iter2 = 0;
            switch(GPUInfoGetter.GPUs.Count)
            {
                case 0:
                    break;
                case 1:
                    // if theres only one gpu only get one element because there isn't gonna be more and dont add any numbers
                    InfoLabel gpuLabel = new InfoLabel("gpu", "device_pci", GPUInfoGetter.GetGPUInfo()[0]);
                    Labels.labels.Add(gpuLabel);
                    break;
                default:
                    foreach(string gpuname in GPUInfoGetter.GetGPUInfo())
                    {
                        InfoLabel gpuLabels = new InfoLabel($"gpu{iter2}", "device_pci", gpuname);
                        Labels.labels.Add(gpuLabels);
                        iter2++;
                    }
                    break;
            }

            FSInfoGetter.GetDrives();

            PopulateWindow();
        }

        /// <summary> Attaches all miscellaneous labels that don't have separate files and styles the main grid </summary>
        static void PopulateWindow() {
            // iterate over all labels, attach them in order
            int iter = 0;
            foreach(InfoLabel label in Labels.labels)
            {
                InfoLabel.AttachAllToGrid(MainWindow.maingrid, label, iter);
                iter++;
            }

            // style main grid
            MainWindow.maingrid.Margin = 8;
            MainWindow.maingrid.RowSpacing = 2;
            MainWindow.maingrid.Halign = Align.Center;
            MainWindow.maingrid.Valign = Align.Center;
        }
        /// <summary> Function called when the timer elapses that updates contencts for dynamic labels </summary>
        public static void LabelUpdate()
        {
            MemInfoGetter.RefreshMemInfo();
            uptimeLabel.contentLabel.Text = $"{UptimeCalculator.GetUptimeStr()}";
            memoryLabel.contentLabel.Text = $"{Math.Round(MemInfoGetter.Mem.used/1048576, 2)} GiB/{Math.Round(MemInfoGetter.Mem.total/1048576, 2)} GiB";
        }
        /// <summary> Creates label with set content </summary>
        public static Label mkLabel(string content)
        {
            Label newlabel = new Label(content);
            newlabel.MarginStart = 16;
            newlabel.MarginEnd = 16;
            return newlabel;
        }
        /// <summary> Ran once timer elapses; updates values on labels </summary>
        static void timerevent(object obj, System.Timers.ElapsedEventArgs args)
        {
            Labels.LabelUpdate();
        }

    }
}