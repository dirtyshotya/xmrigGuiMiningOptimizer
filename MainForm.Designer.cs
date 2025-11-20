// File: MainForm.Designer.cs
using System;

namespace Xmrig_Ranch_Launcher
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblHashrate;
        private System.Windows.Forms.Label lblAcceptedShares;
        private System.Windows.Forms.Label lblRejectedShares;
        private System.Windows.Forms.Label lblUptime;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblWorkerId;
        private System.Windows.Forms.Label lblMaxHashrate;
        private System.Windows.Forms.Timer timerUpdate;
        private System.Windows.Forms.Button btnViewLogs;
        private System.Windows.Forms.Button btnClearLogs;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Timer timerSystemStats;
        private System.Windows.Forms.Label lblCPU;
        private System.Windows.Forms.Label lblRAM;
        private System.Windows.Forms.Label lblTemp;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelStats;
        private System.Windows.Forms.Panel panelControls;
        private System.Windows.Forms.Panel gaugeCPUPanel;
        private System.Windows.Forms.Panel gaugeRAMPanel;
        private System.Windows.Forms.Panel gaugeTempPanel;
        private System.Windows.Forms.Label lblCPUValue;
        private System.Windows.Forms.Label lblRAMValue;
        private System.Windows.Forms.Label lblTempValue;
        private System.Windows.Forms.Label lblCPUTitle;
        private System.Windows.Forms.Label lblRAMTitle;
        private System.Windows.Forms.Label lblTempTitle;
        
        // Bar Graph Controls
        private System.Windows.Forms.Panel panelCpuBarBackground;
        private System.Windows.Forms.Panel panelCpuBar;
        private System.Windows.Forms.Panel panelRamBarBackground;
        private System.Windows.Forms.Panel panelRamBar;

        // REMOVED: Dispose method - it's now in MainForm.cs

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));

            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.lblHashrate = new System.Windows.Forms.Label();
            this.lblAcceptedShares = new System.Windows.Forms.Label();
            this.lblRejectedShares = new System.Windows.Forms.Label();
            this.lblUptime = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblWorkerId = new System.Windows.Forms.Label();
            this.lblMaxHashrate = new System.Windows.Forms.Label();
            this.timerUpdate = new System.Windows.Forms.Timer(this.components);
            this.btnViewLogs = new System.Windows.Forms.Button();
            this.btnClearLogs = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelStats = new System.Windows.Forms.Panel();
            this.panelRamBarBackground = new System.Windows.Forms.Panel();
            this.panelRamBar = new System.Windows.Forms.Panel();
            this.panelCpuBarBackground = new System.Windows.Forms.Panel();
            this.panelCpuBar = new System.Windows.Forms.Panel();
            this.lblTempValue = new System.Windows.Forms.Label();
            this.lblRAMValue = new System.Windows.Forms.Label();
            this.lblCPUValue = new System.Windows.Forms.Label();
            this.lblTempTitle = new System.Windows.Forms.Label();
            this.lblRAMTitle = new System.Windows.Forms.Label();
            this.lblCPUTitle = new System.Windows.Forms.Label();
            this.gaugeTempPanel = new System.Windows.Forms.Panel();
            this.gaugeRAMPanel = new System.Windows.Forms.Panel();
            this.gaugeCPUPanel = new System.Windows.Forms.Panel();
            this.panelControls = new System.Windows.Forms.Panel();
            this.btnSettings = new System.Windows.Forms.Button();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.timerSystemStats = new System.Windows.Forms.Timer(this.components);
            this.lblCPU = new System.Windows.Forms.Label();
            this.lblRAM = new System.Windows.Forms.Label();
            this.lblTemp = new System.Windows.Forms.Label();

            // Color Scheme
            System.Drawing.Color cyberBlue = System.Drawing.Color.FromArgb(0, 240, 255);
            System.Drawing.Color darkBackground = System.Drawing.Color.FromArgb(10, 10, 20);
            System.Drawing.Color panelBackground = System.Drawing.Color.FromArgb(20, 20, 35);
            System.Drawing.Color borderColor = System.Drawing.Color.FromArgb(0, 150, 255);

            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelStats.SuspendLayout();
            this.panelRamBarBackground.SuspendLayout();
            this.panelCpuBarBackground.SuspendLayout();
            this.panelControls.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();

            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(30, 30, 50);
            this.btnStart.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(0, 200, 0);
            this.btnStart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(0, 100, 0);
            this.btnStart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(40, 40, 60);
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.btnStart.ForeColor = System.Drawing.Color.FromArgb(0, 255, 0);
            this.btnStart.Location = new System.Drawing.Point(20, 20);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(220, 50);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "▶ START MINING";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.FromArgb(30, 30, 50);
            this.btnStop.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 0, 0);
            this.btnStop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(100, 0, 0);
            this.btnStop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(40, 40, 60);
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.btnStop.ForeColor = System.Drawing.Color.FromArgb(255, 100, 100);
            this.btnStop.Location = new System.Drawing.Point(20, 80);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(220, 50);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "⏹ STOP MINING";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // lblHashrate
            // 
            this.lblHashrate.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.lblHashrate.ForeColor = cyberBlue;
            this.lblHashrate.Location = new System.Drawing.Point(80, 250);
            this.lblHashrate.Name = "lblHashrate";
            this.lblHashrate.Size = new System.Drawing.Size(520, 25);
            this.lblHashrate.TabIndex = 2;
            this.lblHashrate.Text = "HASHRATE: n/a";
            this.lblHashrate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAcceptedShares
            // 
            this.lblAcceptedShares.Font = new System.Drawing.Font("Consolas", 10F);
            this.lblAcceptedShares.ForeColor = System.Drawing.Color.FromArgb(100, 255, 100);
            this.lblAcceptedShares.Location = new System.Drawing.Point(80, 310);
            this.lblAcceptedShares.Name = "lblAcceptedShares";
            this.lblAcceptedShares.Size = new System.Drawing.Size(250, 20);
            this.lblAcceptedShares.TabIndex = 4;
            this.lblAcceptedShares.Text = "ACCEPTED: n/a";
            this.lblAcceptedShares.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRejectedShares
            // 
            this.lblRejectedShares.Font = new System.Drawing.Font("Consolas", 10F);
            this.lblRejectedShares.ForeColor = System.Drawing.Color.FromArgb(255, 100, 100);
            this.lblRejectedShares.Location = new System.Drawing.Point(350, 310);
            this.lblRejectedShares.Name = "lblRejectedShares";
            this.lblRejectedShares.Size = new System.Drawing.Size(250, 20);
            this.lblRejectedShares.TabIndex = 5;
            this.lblRejectedShares.Text = "REJECTED: n/a";
            this.lblRejectedShares.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblUptime
            // 
            this.lblUptime.Font = new System.Drawing.Font("Consolas", 10F);
            this.lblUptime.ForeColor = System.Drawing.Color.FromArgb(200, 200, 255);
            this.lblUptime.Location = new System.Drawing.Point(80, 340);
            this.lblUptime.Name = "lblUptime";
            this.lblUptime.Size = new System.Drawing.Size(250, 20);
            this.lblUptime.TabIndex = 6;
            this.lblUptime.Text = "UPTIME: n/a";
            this.lblUptime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(0, 255, 255);
            this.lblStatus.Location = new System.Drawing.Point(80, 370);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(520, 25);
            this.lblStatus.TabIndex = 8;
            this.lblStatus.Text = "STATUS: IDLE";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblWorkerId
            // 
            this.lblWorkerId.Font = new System.Drawing.Font("Consolas", 10F);
            this.lblWorkerId.ForeColor = System.Drawing.Color.FromArgb(200, 200, 255);
            this.lblWorkerId.Location = new System.Drawing.Point(350, 340);
            this.lblWorkerId.Name = "lblWorkerId";
            this.lblWorkerId.Size = new System.Drawing.Size(250, 20);
            this.lblWorkerId.TabIndex = 7;
            this.lblWorkerId.Text = "WORKER: n/a";
            this.lblWorkerId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMaxHashrate
            // 
            this.lblMaxHashrate.Font = new System.Drawing.Font("Consolas", 10F);
            this.lblMaxHashrate.ForeColor = System.Drawing.Color.FromArgb(200, 200, 255);
            this.lblMaxHashrate.Location = new System.Drawing.Point(80, 280);
            this.lblMaxHashrate.Name = "lblMaxHashrate";
            this.lblMaxHashrate.Size = new System.Drawing.Size(520, 20);
            this.lblMaxHashrate.TabIndex = 3;
            this.lblMaxHashrate.Text = "MAX: n/a";
            this.lblMaxHashrate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // timerUpdate
            // 
            this.timerUpdate.Interval = 1000;
            this.timerUpdate.Tick += new System.EventHandler(this.timerUpdate_Tick);
            // 
            // btnViewLogs
            // 
            this.btnViewLogs.BackColor = System.Drawing.Color.FromArgb(30, 30, 50);
            this.btnViewLogs.FlatAppearance.BorderColor = borderColor;
            this.btnViewLogs.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(0, 80, 120);
            this.btnViewLogs.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(40, 40, 60);
            this.btnViewLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewLogs.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.btnViewLogs.ForeColor = cyberBlue;
            this.btnViewLogs.Location = new System.Drawing.Point(20, 140);
            this.btnViewLogs.Name = "btnViewLogs";
            this.btnViewLogs.Size = new System.Drawing.Size(220, 50);
            this.btnViewLogs.TabIndex = 9;
            this.btnViewLogs.Text = "📄 VIEW LOG FILE";
            this.btnViewLogs.UseVisualStyleBackColor = false;
            this.btnViewLogs.Click += new System.EventHandler(this.btnViewLogs_Click);
            // 
            // btnClearLogs
            // 
            this.btnClearLogs.BackColor = System.Drawing.Color.FromArgb(30, 30, 50);
            this.btnClearLogs.FlatAppearance.BorderColor = borderColor;
            this.btnClearLogs.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(0, 80, 120);
            this.btnClearLogs.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(40, 40, 60);
            this.btnClearLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearLogs.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.btnClearLogs.ForeColor = cyberBlue;
            this.btnClearLogs.Location = new System.Drawing.Point(20, 200);
            this.btnClearLogs.Name = "btnClearLogs";
            this.btnClearLogs.Size = new System.Drawing.Size(220, 50);
            this.btnClearLogs.TabIndex = 10;
            this.btnClearLogs.Text = "🗑️ CLEAR LOGS";
            this.btnClearLogs.UseVisualStyleBackColor = false;
            this.btnClearLogs.Click += new System.EventHandler(this.btnClearLogs_Click);
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.FromArgb(15, 15, 25);
            this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtLog.ForeColor = cyberBlue;
            this.txtLog.Location = new System.Drawing.Point(0, 0);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(984, 218);
            this.txtLog.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 60);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = darkBackground;
            this.splitContainer1.Panel1.Controls.Add(this.panelStats);
            this.splitContainer1.Panel1.Controls.Add(this.panelControls);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = darkBackground;
            this.splitContainer1.Panel2.Controls.Add(this.txtLog);
            this.splitContainer1.Size = new System.Drawing.Size(984, 621);
            this.splitContainer1.SplitterDistance = 400;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 0;
            // 
            // panelStats
            // 
            this.panelStats.BackColor = panelBackground;
            this.panelStats.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelStats.Controls.Add(this.panelRamBarBackground);
            this.panelStats.Controls.Add(this.panelCpuBarBackground);
            this.panelStats.Controls.Add(this.lblTempValue);
            this.panelStats.Controls.Add(this.lblRAMValue);
            this.panelStats.Controls.Add(this.lblCPUValue);
            this.panelStats.Controls.Add(this.lblTempTitle);
            this.panelStats.Controls.Add(this.lblRAMTitle);
            this.panelStats.Controls.Add(this.lblCPUTitle);
            this.panelStats.Controls.Add(this.gaugeTempPanel);
            this.panelStats.Controls.Add(this.gaugeRAMPanel);
            this.panelStats.Controls.Add(this.gaugeCPUPanel);
            this.panelStats.Controls.Add(this.lblHashrate);
            this.panelStats.Controls.Add(this.lblMaxHashrate);
            this.panelStats.Controls.Add(this.lblAcceptedShares);
            this.panelStats.Controls.Add(this.lblRejectedShares);
            this.panelStats.Controls.Add(this.lblUptime);
            this.panelStats.Controls.Add(this.lblWorkerId);
            this.panelStats.Controls.Add(this.lblStatus);
            this.panelStats.Location = new System.Drawing.Point(300, 10);
            this.panelStats.Name = "panelStats";
            this.panelStats.Size = new System.Drawing.Size(674, 380);
            this.panelStats.TabIndex = 18;
            // 
            // panelRamBarBackground
            // 
            this.panelRamBarBackground.BackColor = System.Drawing.Color.FromArgb(40, 40, 60);
            this.panelRamBarBackground.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelRamBarBackground.Controls.Add(this.panelRamBar);
            this.panelRamBarBackground.Location = new System.Drawing.Point(280, 80);
            this.panelRamBarBackground.Name = "panelRamBarBackground";
            this.panelRamBarBackground.Size = new System.Drawing.Size(120, 120);
            this.panelRamBarBackground.TabIndex = 27;
            // 
            // panelRamBar
            // 
            this.panelRamBar.BackColor = System.Drawing.Color.FromArgb(0, 255, 150);
            this.panelRamBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelRamBar.Location = new System.Drawing.Point(0, 120);
            this.panelRamBar.Name = "panelRamBar";
            this.panelRamBar.Size = new System.Drawing.Size(118, 0);
            this.panelRamBar.TabIndex = 0;
            // 
            // panelCpuBarBackground
            // 
            this.panelCpuBarBackground.BackColor = System.Drawing.Color.FromArgb(40, 40, 60);
            this.panelCpuBarBackground.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelCpuBarBackground.Controls.Add(this.panelCpuBar);
            this.panelCpuBarBackground.Location = new System.Drawing.Point(80, 80);
            this.panelCpuBarBackground.Name = "panelCpuBarBackground";
            this.panelCpuBarBackground.Size = new System.Drawing.Size(120, 120);
            this.panelCpuBarBackground.TabIndex = 26;
            // 
            // panelCpuBar
            // 
            this.panelCpuBar.BackColor = cyberBlue;
            this.panelCpuBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelCpuBar.Location = new System.Drawing.Point(0, 120);
            this.panelCpuBar.Name = "panelCpuBar";
            this.panelCpuBar.Size = new System.Drawing.Size(118, 0);
            this.panelCpuBar.TabIndex = 0;
            // 
            // lblTempValue
            // 
            this.lblTempValue.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.lblTempValue.ForeColor = System.Drawing.Color.FromArgb(255, 100, 150);
            this.lblTempValue.Location = new System.Drawing.Point(480, 210);
            this.lblTempValue.Name = "lblTempValue";
            this.lblTempValue.Size = new System.Drawing.Size(120, 20);
            this.lblTempValue.TabIndex = 25;
            this.lblTempValue.Text = "N/A";
            this.lblTempValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRAMValue
            // 
            this.lblRAMValue.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.lblRAMValue.ForeColor = System.Drawing.Color.FromArgb(0, 255, 150);
            this.lblRAMValue.Location = new System.Drawing.Point(280, 210);
            this.lblRAMValue.Name = "lblRAMValue";
            this.lblRAMValue.Size = new System.Drawing.Size(120, 20);
            this.lblRAMValue.TabIndex = 24;
            this.lblRAMValue.Text = "0%";
            this.lblRAMValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCPUValue
            // 
            this.lblCPUValue.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.lblCPUValue.ForeColor = cyberBlue;
            this.lblCPUValue.Location = new System.Drawing.Point(80, 210);
            this.lblCPUValue.Name = "lblCPUValue";
            this.lblCPUValue.Size = new System.Drawing.Size(120, 20);
            this.lblCPUValue.TabIndex = 23;
            this.lblCPUValue.Text = "0%";
            this.lblCPUValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTempTitle
            // 
            this.lblTempTitle.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.lblTempTitle.ForeColor = System.Drawing.Color.FromArgb(255, 100, 150);
            this.lblTempTitle.Location = new System.Drawing.Point(480, 60);
            this.lblTempTitle.Name = "lblTempTitle";
            this.lblTempTitle.Size = new System.Drawing.Size(120, 20);
            this.lblTempTitle.TabIndex = 19;
            this.lblTempTitle.Text = "TEMPERATURE";
            this.lblTempTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRAMTitle
            // 
            this.lblRAMTitle.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.lblRAMTitle.ForeColor = System.Drawing.Color.FromArgb(0, 255, 150);
            this.lblRAMTitle.Location = new System.Drawing.Point(280, 60);
            this.lblRAMTitle.Name = "lblRAMTitle";
            this.lblRAMTitle.Size = new System.Drawing.Size(120, 20);
            this.lblRAMTitle.TabIndex = 18;
            this.lblRAMTitle.Text = "MEMORY";
            this.lblRAMTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCPUTitle
            // 
            this.lblCPUTitle.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.lblCPUTitle.ForeColor = cyberBlue;
            this.lblCPUTitle.Location = new System.Drawing.Point(80, 60);
            this.lblCPUTitle.Name = "lblCPUTitle";
            this.lblCPUTitle.Size = new System.Drawing.Size(120, 20);
            this.lblCPUTitle.TabIndex = 17;
            this.lblCPUTitle.Text = "CPU";
            this.lblCPUTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gaugeTempPanel
            // 
            this.gaugeTempPanel.BackColor = System.Drawing.Color.FromArgb(40, 40, 60);
            this.gaugeTempPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gaugeTempPanel.Location = new System.Drawing.Point(480, 80);
            this.gaugeTempPanel.Name = "gaugeTempPanel";
            this.gaugeTempPanel.Size = new System.Drawing.Size(120, 120);
            this.gaugeTempPanel.TabIndex = 22;
            // 
            // gaugeRAMPanel
            // 
            this.gaugeRAMPanel.BackColor = System.Drawing.Color.FromArgb(40, 40, 60);
            this.gaugeRAMPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gaugeRAMPanel.Location = new System.Drawing.Point(280, 80);
            this.gaugeRAMPanel.Name = "gaugeRAMPanel";
            this.gaugeRAMPanel.Size = new System.Drawing.Size(120, 120);
            this.gaugeRAMPanel.TabIndex = 21;
            // 
            // gaugeCPUPanel
            // 
            this.gaugeCPUPanel.BackColor = System.Drawing.Color.FromArgb(40, 40, 60);
            this.gaugeCPUPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gaugeCPUPanel.Location = new System.Drawing.Point(80, 80);
            this.gaugeCPUPanel.Name = "gaugeCPUPanel";
            this.gaugeCPUPanel.Size = new System.Drawing.Size(120, 120);
            this.gaugeCPUPanel.TabIndex = 20;
            // 
            // panelControls
            // 
            this.panelControls.BackColor = panelBackground;
            this.panelControls.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelControls.Controls.Add(this.btnSettings);
            this.panelControls.Controls.Add(this.btnStart);
            this.panelControls.Controls.Add(this.btnStop);
            this.panelControls.Controls.Add(this.btnViewLogs);
            this.panelControls.Controls.Add(this.btnClearLogs);
            this.panelControls.Location = new System.Drawing.Point(20, 10);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(260, 380);
            this.panelControls.TabIndex = 17;
            // 
            // btnSettings
            // 
            this.btnSettings.BackColor = System.Drawing.Color.FromArgb(30, 30, 50);
            this.btnSettings.FlatAppearance.BorderColor = borderColor;
            this.btnSettings.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(0, 80, 120);
            this.btnSettings.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(40, 40, 60);
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.btnSettings.ForeColor = cyberBlue;
            this.btnSettings.Location = new System.Drawing.Point(20, 280);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(220, 50);
            this.btnSettings.TabIndex = 18;
            this.btnSettings.Text = "⚙ SETTINGS";
            this.btnSettings.UseVisualStyleBackColor = false;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(15, 15, 25);
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(984, 60);
            this.panelHeader.TabIndex = 1;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = cyberBlue;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(984, 60);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "XMRIG RANCH LAUNCHER";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timerSystemStats
            // 
            this.timerSystemStats.Interval = 1000;
            this.timerSystemStats.Tick += new System.EventHandler(this.timerSystemStats_Tick);
            // 
            // lblCPU
            // 
            this.lblCPU.AutoSize = true;
            this.lblCPU.Location = new System.Drawing.Point(0, 0);
            this.lblCPU.Name = "lblCPU";
            this.lblCPU.Size = new System.Drawing.Size(35, 13);
            this.lblCPU.TabIndex = 2;
            this.lblCPU.Text = "label1";
            // 
            // lblRAM
            // 
            this.lblRAM.AutoSize = true;
            this.lblRAM.Location = new System.Drawing.Point(0, 0);
            this.lblRAM.Name = "lblRAM";
            this.lblRAM.Size = new System.Drawing.Size(35, 13);
            this.lblRAM.TabIndex = 3;
            this.lblRAM.Text = "label2";
            // 
            // lblTemp
            // 
            this.lblTemp.AutoSize = true;
            this.lblTemp.Location = new System.Drawing.Point(0, 0);
            this.lblTemp.Name = "lblTemp";
            this.lblTemp.Size = new System.Drawing.Size(35, 13);
            this.lblTemp.TabIndex = 4;
            this.lblTemp.Text = "label3";
            // 
            // MainForm
            // 
            this.BackColor = darkBackground;
            this.ClientSize = new System.Drawing.Size(984, 681);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panelHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.ForeColor = System.Drawing.Color.White;
            // Safe icon load - won't crash if resource missing
            try
            {
                var obj = resources.GetObject("$this.Icon");
                if (obj is System.Drawing.Icon ico) this.Icon = ico;
            }
            catch { /* ignore missing resource */ }
            this.MinimumSize = new System.Drawing.Size(1000, 720);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "XMRig Ranch Launcher - CyberPunk Edition";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);

            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panelStats.ResumeLayout(false);
            this.panelRamBarBackground.ResumeLayout(false);
            this.panelCpuBarBackground.ResumeLayout(false);
            this.panelControls.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
    }
}