﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml.Linq;
using System.IO;
using System.Xml.Serialization;
using WorkhubForWindows.Functions;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WorkhubForWindows
{
    public class Executable
    {
        public Executable(Executable executable)
        {
            Name = executable.Name;
            Path = executable.Path;
            Argments = executable.Argments;
            point = executable.point;
        }

        public Executable()
        {

        }

        public string Name;
        public string Path;
        public string Argments;
        public Point point;
    }

    public struct WorkhubWindowHandler
    {
        public WorkhubWindowHandler(int HWnd, string name)
        {
            hWnd = HWnd;
            Name = name;
        }
        public int hWnd { get; }
        public string Name { get; }
    }

    public class OwnFont
    {
        public OwnFont(string name, float size)
        {
            Name = name;
            Size = size;
        }

        public OwnFont()
        {

        }

        public string Name { get; set; }
        public float Size { get; set; }
    }
    
    public enum HomeMode
    {
        FullScreen=0,
        HalfHome=1,
    }

    public class LanguagePack
    {
        public __Mainwindow Mainwindow = new __Mainwindow();
        public class __Mainwindow
        {
            public string AdditemButton;
            public string EdititemButton;
            public string StartButton;

            public __RibbonFiles RibbonFiles = new __RibbonFiles();
            public class __RibbonFiles
            {
                public string Text;
                public string Settings;
                public string ShowWidget;
                public string Quit;
            }
            public __ApplistRC ApplistRC = new __ApplistRC();
            public class __ApplistRC
            {
                public string Add;
                public string Edit;
                public string Delete;
                public string RunAdmin;
            }
            public __TasktrayIcon TasktrayIcon = new __TasktrayIcon();
            public class __TasktrayIcon
            {
                public string ShowMainWindow;
                public string ShowWidget;
                public string Settings;
                public string AddItem;
                public string Quit;
            }
            public string MB_Quit_Verif;
        }
        public __AddItem AddItem = new __AddItem();
        public class __AddItem
        {
            public string WindowTitle;
            public string Label_Name;
            public string Label_Path;
            public string Label_Args;

            public string CancelButton;
            public string ApplyButton;
            public string RefButton;
        }
        public __EditItem EditItem = new __EditItem();
        public class __EditItem
        {
            public string WindowTitle;
            public string Label_Name;
            public string Label_Path;
            public string Label_Args;

            public string CloseButton;
            public string ApplyButton;
            public string OKButton;
        }
        public __Widget Widget = new __Widget();
        public class __Widget
        {
            public __WidgetRC WidgetRC = new __WidgetRC();
            public class __WidgetRC
            {
                public string FixWidgetPos;
            }
        }
        public __Settings Settings = new __Settings();
        public class __Settings
        {
            public string WindowTitle;
            public string Label_Font;
            public string Label_MainBackimgPath;
            public string Label_WidgetBackimgPath;
            public string Label_Language;
            public string Label_Opacity;
            public string Label_WidgetTextColor;
            public string Label_WidgetBackColor;
            public string Label_WidgetSCKey;
            public string Tab_General;
            public string Tab_Widget;
            public string Button_Apply;
            public string Button_Cancel;
            public string Startup_Add;
            public string Startup_Remove;
        }

        public void LoadLanguagePack(string Path,ref LanguagePack Lpack)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(LanguagePack));

            var xmlSettings = new System.Xml.XmlReaderSettings
            {
                CheckCharacters = false,
            };
            using (var streamReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + Path, Encoding.UTF8))
            using (var xmlReader = System.Xml.XmlReader.Create(streamReader, xmlSettings))
            {
                Lpack = (LanguagePack)serializer.Deserialize(xmlReader);
            }


            return;

        }

    }

    public class Configure
    {

        public OwnFont font = new OwnFont("MS UI Gothic", 9);

        private string __backimgpath;
        public string backimgpath
        {
            get
            {
                return this.__backimgpath;
            }
            set
            {
                this.__backimgpath = value;
            }
        }
        private bool __LockWidget;
        public bool LockWidget
        {
            get
            {
                return this.__LockWidget;
            }
            set
            {
                this.__LockWidget = value;
                if (StaticClasses.AppStatus.Started)
                {
                    SendWidgetConfigChanged();
                }
            }
        }
        private string __Widgetbackimg;
        public string Widgetbackimg
        {
            get
            {
                return this.__Widgetbackimg;
            }
            set
            {
                this.__Widgetbackimg = value;
                foreach (var i in StaticClasses.WindowHandler.WindowHandlers)
                {
                    if (i.Name == "Widget")
                    {
                        PostMessage(i.hWnd, StaticClasses.WorkHubMessages.WidgetBackgroundSet, 0, 0);
                        break;
                    }
                }

            }
        }
        private bool __ShowWidget;
        public bool ShowWidget
        {
            get
            {
                return this.__ShowWidget;
            }
            set
            {
                this.__ShowWidget = value;

                SendWidgetConfigChanged();
            }
        }
        public int __MainWindowBackColor;
        [XmlIgnore]
        public Color MainWindowBackColor
        {
            get
            {
                return Color.FromArgb(__MainWindowBackColor);
            }
            set
            {
                __MainWindowBackColor = value.ToArgb();
                this.SendWidgetConfigChanged();
            }
        }
        public int __WidgetBackColor;
        [XmlIgnore]
        public Color WidgetBackColor
        {
            get
            {
                return Color.FromArgb(__WidgetBackColor);
            }
            set
            {
                __WidgetBackColor = value.ToArgb();
                this.SendWidgetConfigChanged();
            }
        }
        private double __WidgetOpacity = 1;
        public double WidgetOpacity
        {
            get
            {
                return this.__WidgetOpacity;
            }
            set
            {
                __WidgetOpacity = value;
                SendWidgetConfigChanged();
            }
        }
        public int __WidgetForeColor;
        [XmlIgnore]
        public Color WidgetForeColor
        {
            get
            {
                return Color.FromArgb(__WidgetForeColor);
            }
            set
            {
                __WidgetForeColor = value.ToArgb();
                this.SendWidgetConfigChanged();
            }
        }
        private Size __WidgetSize = new Size(300, 300);
        public Size WidgetSize
        {
            get
            {
                return this.__WidgetSize;
            }
            set
            {
                __WidgetSize = value;
            }
        }
        private Point __WidgetPosition = new Point(0, 0);
        public Point WidgetPosition
        {
            get
            {
                return __WidgetPosition;
            }
            set
            {
                __WidgetPosition = value;
            }
        }
        private bool __WidgetAlwaysDisplayTop = false;
        public bool WidgetAlwaysDisplayTop
        {
            get
            {
                return __WidgetAlwaysDisplayTop;
            }
            set
            {
                __WidgetAlwaysDisplayTop = value;
                SendWidgetConfigChanged();
            }
        }
        private HomeMode __Homemode = HomeMode.HalfHome;
        public HomeMode Homemode
        {
            get
            {
                return this.__Homemode;
            }
            set
            {
                this.__Homemode = value;
            }
        }
        private string __Language = "English";
        public string Language
        {
            get
            {
                return __Language;
            }
            set
            {
                __Language = value;
            }
        }
        //public string LogoffSound;
        //public string ShutdownSound;



        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        extern static Int32 PostMessage(Int32 hwnd, Int32 msg, Int32 wParam, Int32 lParam);

        public void SendConfigChanged()
        {
            foreach (WorkhubWindowHandler i in StaticClasses.WindowHandler.WindowHandlers)
            {
                PostMessage(i.hWnd, StaticClasses.WorkHubMessages.ConfigChanged, 0, 0);
            }
        }

        public void ApplyConfig()
        {
            this.SendConfigChanged();
        }

        private void SendWidgetConfigChanged()
        {
            foreach (WorkhubWindowHandler i in StaticClasses.WindowHandler.WindowHandlers)
            {
                if (i.Name == "Widget")
                {
                    PostMessage(i.hWnd, StaticClasses.WorkHubMessages.WidgetConfigChanged, 0, 0);
                }
            }
        }

        public void SaveConfig(Configure cfg)
        {
            XmlSerializer Serialize = new XmlSerializer(typeof(Configure));
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Config\\"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Config");
            }
            using (var Streamwriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "Config\\Config.xml", false, new System.Text.UTF8Encoding(false)))
            {
                Serialize.Serialize(Streamwriter, cfg);
                Streamwriter.Flush();
            }
            return;
        }

        public void SaveConfig()
        {
            Configure cfg = this;
            XmlSerializer Serialize = new XmlSerializer(typeof(Configure));
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Config\\"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Config");
            }
            using (var Streamwriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "Config\\Config.xml", false, new System.Text.UTF8Encoding(false)))
            {
                Serialize.Serialize(Streamwriter, cfg);
                Streamwriter.Flush();
            }
            return;
        }
        public Configure LoadConfig()
        {
            Configure cfg = new Configure();
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Config"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Config");
            }
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Config\\Config.xml"))
            {
                return cfg;
            }
            XmlSerializer serializer = new XmlSerializer(typeof(Configure));

            var xmlSettings = new System.Xml.XmlReaderSettings
            {
                CheckCharacters = false,
            };
            using (var streamReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "Config\\Config.xml", Encoding.UTF8))
            using (var xmlReader = System.Xml.XmlReader.Create(streamReader, xmlSettings))
            {
                cfg = (Configure)serializer.Deserialize(xmlReader);
            }
            return cfg;
        }
    }

    public static class StaticClasses
    {
        public static List<Executable> Executables { get; set; } = new List<Executable>();
        public static string ConfigFoldor { get; set; }
        public static Configure Config { get; set; } = new Configure();
        public static class WorkHubMessages
        {
            public const int ConfigChanged = 0x2500;
            public const int AppListChanged = 0x2501;
            public const int WidgetConfigChanged = 0x2502;
            public const int WidgetBackgroundSet = 0x2503;
            public const int LanguagePackLoad = 0x2504;
            public const int ApplicationQuit = 0x2510;
        }

        public static class WindowHandler
        {
            public static List<WorkhubWindowHandler> WindowHandlers = new List<WorkhubWindowHandler>();
        }



        public static class AppStatus
        {
            public static bool Started = false;
        }

        public static LanguagePack Langs = new LanguagePack();
    }
}
