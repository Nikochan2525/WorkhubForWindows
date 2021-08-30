﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorkhubForWindows.Forms;

namespace WorkhubForWindows
{
    public partial class Mainwindow : Form
    {
        public Mainwindow()
        {
            InitializeComponent();
            initalizeApps();
            Apps.View = View.LargeIcon;

        }


        private void Additem(object sender, EventArgs e)
        {
            AddItemForm additemform = new AddItemForm();
            if (additemform.ShowDialog() == DialogResult.OK)
            {
                initalizeApps();
            }
        }





        /// <summary>
        /// アプリケーションの読み込み
        /// Load Applications
        /// </summary>
        void initalizeApps()
        {
            StaticClasses.Executables = Functions.Config.Applications.Load();

            IconList.Images.Clear();
            foreach(Executable exe in StaticClasses.Executables)
            {
                Bitmap bmp = Icon.ExtractAssociatedIcon(exe.Path).ToBitmap();
                IconList.Images.Add(exe.Name, bmp);
                Apps.Items.Add(exe.Name);
                Apps.Items[Apps.Items.Count - 1].ImageIndex = Apps.Items.Count - 1;
            }


        }
    }
}
