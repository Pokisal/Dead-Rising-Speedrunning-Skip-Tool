using System;
using System.Timers;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace DR_RTM
{

    public class Form1 : Form
    {
        private delegate void TimeDisplayUpdateCallback(string text);

        private delegate void PIDUpdateCallback(string text);

        public static System.Timers.Timer ConnectTimer = new System.Timers.Timer(500);

        public static bool skipFlag;

        private IContainer components;

        private Label label2;

        private Label timeDisplay;

        private Label label3;

        private RadioButton radioButton1;

        private RadioButton radioButton2;

        private RadioButton radioButton3;

        private RadioButton radioButton4;

        private RadioButton radioButton5;
        private Label PID;
        private CheckBox checkBox1;

        public Form1()
        {
            InitializeComponent();
            ConnectTimer.Elapsed += Connect;
            ConnectTimer.AutoReset = true;
            ConnectTimer.Enabled = true;
        }

        [DllImport("DwmApi")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);

        protected override void OnHandleCreated(EventArgs e)
        {
            if (DwmSetWindowAttribute(Handle, 19, new[] { 1 }, 4) != 0)
                DwmSetWindowAttribute(Handle, 20, new[] { 1 }, 4);
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        private unsafe static extern bool WriteProcessMemory(IntPtr hProcess, uint lpBaseAddress, byte[] lpBuffer, int nSize, void* lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        private static extern int CloseHandle(IntPtr hObject);

        public static IntPtr MemoryOpen(int ProcessID)
        {
            return OpenProcess(2035711u, bInheritHandle: false, ProcessID);
        }

        public unsafe void Write(uint mAddress, byte[] Buffer, int ProcessID)
        {
            if (!(MemoryOpen(ProcessID) == (IntPtr)0))
            {
                WriteProcessMemory(MemoryOpen(ProcessID), mAddress, Buffer, Buffer.Length, null);
            }
        }

        public void SaveSettings()
        {
            Dead_Rising_Speedrunning_Skip_Tool.Properties.Settings.Default.TimeSkip = radioButton1.Checked;
            Dead_Rising_Speedrunning_Skip_Tool.Properties.Settings.Default.PsychoSkip = radioButton2.Checked;
            Dead_Rising_Speedrunning_Skip_Tool.Properties.Settings.Default.AllBosses = radioButton3.Checked;
            Dead_Rising_Speedrunning_Skip_Tool.Properties.Settings.Default.AllSurvivors = radioButton4.Checked;
            Dead_Rising_Speedrunning_Skip_Tool.Properties.Settings.Default.AllScoops = radioButton5.Checked;
            Dead_Rising_Speedrunning_Skip_Tool.Properties.Settings.Default.BenchSkip = checkBox1.Checked;
        }

        public void TimeDisplayUpdate(string text)
        {
            if (timeDisplay.InvokeRequired)
            {
                TimeDisplayUpdateCallback method = TimeDisplayUpdate;
                try
                {
                    Invoke(method, text);
                    return;
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
            }
            timeDisplay.Text = text;
        }

        public void PIDUpdate(string text)
        {
            if (timeDisplay.InvokeRequired)
            {
                PIDUpdateCallback method = PIDUpdate;
                try
                {
                    Invoke(method, text);
                    return;
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
            }
            PID.Text = text;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            AllSkips.UpdateTimer.Enabled = false;
            ConnectTimer.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bool a = Dead_Rising_Speedrunning_Skip_Tool.Properties.Settings.Default.BenchSkip;
            PID.Text = string.Empty;
            radioButton1.Checked = Dead_Rising_Speedrunning_Skip_Tool.Properties.Settings.Default.TimeSkip;
            radioButton2.Checked = Dead_Rising_Speedrunning_Skip_Tool.Properties.Settings.Default.PsychoSkip;
            radioButton3.Checked = Dead_Rising_Speedrunning_Skip_Tool.Properties.Settings.Default.AllBosses;
            radioButton4.Checked = Dead_Rising_Speedrunning_Skip_Tool.Properties.Settings.Default.AllSurvivors;
            radioButton5.Checked = Dead_Rising_Speedrunning_Skip_Tool.Properties.Settings.Default.AllScoops;
            checkBox1.Checked = a;
        }

        public void disable_Radios(bool flag)
        {
            if (flag)
            {
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
                radioButton3.Enabled = false;
                radioButton4.Enabled = false;
                radioButton5.Enabled = false;
            }
            if (!flag)
            {
                radioButton1.Enabled = true;
                radioButton2.Enabled = true;
                radioButton3.Enabled = true;
                radioButton4.Enabled = true;
                radioButton5.Enabled = true;
            }
        }

        private void Connect(object source, ElapsedEventArgs e)
        {
            Process[] processesByName = Process.GetProcessesByName("DeadRising");
            if (processesByName.Length == 0)
            {
                PID.Text = string.Empty;
                return;
            }
            else if (AllSkips.GameProcess == null || processesByName[0].Id.ToString("X8") != AllSkips.GameProcess.Id.ToString("X8"))
            {
                AllSkips.GameProcess = processesByName[0];
                AllSkips.UpdateTimer.Elapsed += AllSkips.UpdateEvent;
                AllSkips.UpdateTimer.AutoReset = true;
                AllSkips.UpdateTimer.Enabled = true;
                AllSkips.form = this;
                AllSkips.CheckVersion();
                PIDUpdate(string.Format("Connected to PID {0}", processesByName[0].Id.ToString("X8")));
            }
        }
        public void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            AllSkips.skipMode = 0;
            SaveSettings();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            AllSkips.skipMode = 1;
            SaveSettings();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            AllSkips.skipMode = 2;
            SaveSettings();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            AllSkips.skipMode = 3;
            SaveSettings();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            AllSkips.skipMode = 4;
            SaveSettings();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                skipFlag = true;
            }
            else
            {
                skipFlag = false;
            }
            SaveSettings();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DR_RTM.Form1));
            this.label2 = new System.Windows.Forms.Label();
            this.timeDisplay = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.PID = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(12, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "The current time is:";
            // 
            // timeDisplay
            // 
            this.timeDisplay.AutoSize = true;
            this.timeDisplay.ForeColor = System.Drawing.Color.White;
            this.timeDisplay.Location = new System.Drawing.Point(12, 50);
            this.timeDisplay.Name = "timeDisplay";
            this.timeDisplay.Size = new System.Drawing.Size(53, 13);
            this.timeDisplay.TabIndex = 5;
            this.timeDisplay.Text = "<missing>";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(12, 223);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(184, 26);
            this.label3.TabIndex = 6;
            this.label3.Text = "About the only thing to do in this town\nis to kill time at the shopping mall.";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.radioButton1.ForeColor = System.Drawing.Color.White;
            this.radioButton1.Location = new System.Drawing.Point(15, 75);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(69, 17);
            this.radioButton1.TabIndex = 7;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "TimeSkip";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.radioButton2.ForeColor = System.Drawing.Color.White;
            this.radioButton2.Location = new System.Drawing.Point(15, 98);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(81, 17);
            this.radioButton2.TabIndex = 8;
            this.radioButton2.Text = "PsychoSkip";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.radioButton3.ForeColor = System.Drawing.Color.White;
            this.radioButton3.Location = new System.Drawing.Point(15, 121);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(91, 17);
            this.radioButton3.TabIndex = 9;
            this.radioButton3.Text = "AllBossesSkip";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.radioButton4.ForeColor = System.Drawing.Color.White;
            this.radioButton4.Location = new System.Drawing.Point(15, 144);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(85, 17);
            this.radioButton4.TabIndex = 10;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "SurvivorSkip";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.CheckedChanged += new System.EventHandler(this.radioButton4_CheckedChanged);
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.radioButton5.ForeColor = System.Drawing.Color.White;
            this.radioButton5.Location = new System.Drawing.Point(15, 167);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(93, 17);
            this.radioButton5.TabIndex = 11;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "AllScoopsSkip";
            this.radioButton5.UseVisualStyleBackColor = true;
            this.radioButton5.CheckedChanged += new System.EventHandler(this.radioButton5_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.checkBox1.ForeColor = System.Drawing.Color.White;
            this.checkBox1.Location = new System.Drawing.Point(15, 190);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(201, 30);
            this.checkBox1.TabIndex = 12;
            this.checkBox1.Text = "Use BenchSkip / BikeSkip routes\n(Only affects Survivor and AllScoops)";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // PID
            // 
            this.PID.AutoSize = true;
            this.PID.ForeColor = System.Drawing.Color.White;
            this.PID.Location = new System.Drawing.Point(12, 7);
            this.PID.Name = "PID";
            this.PID.Size = new System.Drawing.Size(25, 13);
            this.PID.TabIndex = 13;
            this.PID.Text = "PID";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(228, 258);
            this.Controls.Add(this.PID);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.radioButton5);
            this.Controls.Add(this.radioButton4);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.timeDisplay);
            this.Controls.Add(this.label2);
            base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            this.Name = "Form1";
            this.Text = "Dead Rising Speedrunning Skip Tool";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}