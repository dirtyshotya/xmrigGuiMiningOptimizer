using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Xmrig_Ranch_Launcher
{
    public partial class SettingsForm : Form
    {
        private MiningConfig config;
        private Action<MiningConfig> saveCallback;
        private Action autoTuneCallback;

        // Store references to controls for easy access
        private TextBox txtPoolUrl;
        private TextBox txtWallet;
        private TextBox txtWorker;
        private TextBox txtPassword;
        private TrackBar trackCpu;
        private Label lblCpuValue;
        private ComboBox cmbPriority;
        private ComboBox cmbDevFee;
        private CheckBox chkHugePages;
        private CheckBox chkHardwareAes;
        private CheckBox chkTls;
        private CheckBox chkKeepalive;
        private CheckBox chkEnableApi;
        private TextBox txtApiPort;
        private TextBox txtApiId;
        private CheckBox chkApiRestricted;

        public SettingsForm(MiningConfig currentConfig, Action<MiningConfig> saveCallback, Action autoTuneCallback)
        {
            this.config = currentConfig;
            this.saveCallback = saveCallback;
            this.autoTuneCallback = autoTuneCallback;
            InitializeComponent();
            LoadSettings();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form setup
            this.Text = "Mining Settings";
            this.Size = new Size(500, 800);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(45, 45, 65);
            this.ForeColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Padding = new Padding(10);

            // Pool Settings Group
            var grpPool = new GroupBox();
            grpPool.Text = "Pool Settings";
            grpPool.Location = new Point(20, 20);
            grpPool.Size = new Size(440, 150);
            grpPool.ForeColor = Color.White;
            grpPool.BackColor = Color.FromArgb(60, 60, 80);

            var lblPool = new Label() { 
                Text = "Pool URL:", 
                Location = new Point(20, 30), 
                Size = new Size(80, 20) 
            };
            txtPoolUrl = new TextBox() { 
                Location = new Point(100, 27), 
                Size = new Size(320, 20), 
                BackColor = Color.FromArgb(80, 80, 100), 
                ForeColor = Color.White
            };

            var lblWallet = new Label() { 
                Text = "Wallet:", 
                Location = new Point(20, 60), 
                Size = new Size(80, 20) 
            };
            txtWallet = new TextBox() { 
                Location = new Point(100, 57), 
                Size = new Size(320, 20), 
                BackColor = Color.FromArgb(80, 80, 100), 
                ForeColor = Color.White
            };

            var lblWorker = new Label() { 
                Text = "Worker ID:", 
                Location = new Point(20, 90), 
                Size = new Size(80, 20) 
            };
            txtWorker = new TextBox() { 
                Location = new Point(100, 87), 
                Size = new Size(320, 20), 
                BackColor = Color.FromArgb(80, 80, 100), 
                ForeColor = Color.White
            };

            var lblPassword = new Label() { 
                Text = "Password:", 
                Location = new Point(20, 120), 
                Size = new Size(80, 20) 
            };
            txtPassword = new TextBox() { 
                Location = new Point(100, 117), 
                Size = new Size(320, 20), 
                BackColor = Color.FromArgb(80, 80, 100), 
                ForeColor = Color.White,
                Text = "x"
            };

            grpPool.Controls.AddRange(new Control[] { lblPool, txtPoolUrl, lblWallet, txtWallet, lblWorker, txtWorker, lblPassword, txtPassword });

            // Performance Settings Group
            var grpPerformance = new GroupBox();
            grpPerformance.Text = "Performance Settings";
            grpPerformance.Location = new Point(20, 190);
            grpPerformance.Size = new Size(440, 140);
            grpPerformance.ForeColor = Color.White;
            grpPerformance.BackColor = Color.FromArgb(60, 60, 80);

            var lblCpuUsage = new Label() { 
                Text = "Max CPU Usage:", 
                Location = new Point(20, 30), 
                Size = new Size(100, 20) 
            };
            trackCpu = new TrackBar() { 
                Minimum = 10, 
                Maximum = 100, 
                Location = new Point(120, 27), 
                Size = new Size(200, 20), 
                TickFrequency = 10
            };
            lblCpuValue = new Label() { 
                Location = new Point(330, 30), 
                Size = new Size(40, 20)
            };

            var lblPriority = new Label() { 
                Text = "CPU Priority:", 
                Location = new Point(20, 70), 
                Size = new Size(100, 20) 
            };
            cmbPriority = new ComboBox() { 
                Location = new Point(120, 67), 
                Size = new Size(200, 25), 
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(80, 80, 100), 
                ForeColor = Color.White
            };
            cmbPriority.Items.AddRange(new object[] { "Idle (1)", "Low (2)", "Normal (3)", "Above Normal (4)", "High (5)" });

            grpPerformance.Controls.AddRange(new Control[] { 
                lblCpuUsage, trackCpu, lblCpuValue, lblPriority, cmbPriority 
            });

            // API Settings Group
            var grpApi = new GroupBox();
            grpApi.Text = "API Settings";
            grpApi.Location = new Point(20, 350);
            grpApi.Size = new Size(440, 110);
            grpApi.ForeColor = Color.White;
            grpApi.BackColor = Color.FromArgb(60, 60, 80);

            chkEnableApi = new CheckBox() { 
                Text = "Enable API", 
                Location = new Point(20, 30), 
                Size = new Size(100, 20), 
                ForeColor = Color.White
            };

            var lblApiId = new Label() { 
                Text = "API ID:", 
                Location = new Point(130, 30), 
                Size = new Size(50, 20) 
            };
            txtApiId = new TextBox() { 
                Location = new Point(180, 27), 
                Size = new Size(80, 20), 
                BackColor = Color.FromArgb(80, 80, 100), 
                ForeColor = Color.White
            };

            var lblApiPort = new Label() { 
                Text = "API Port:", 
                Location = new Point(270, 30), 
                Size = new Size(50, 20) 
            };
            txtApiPort = new TextBox() { 
                Location = new Point(320, 27), 
                Size = new Size(60, 20), 
                BackColor = Color.FromArgb(80, 80, 100), 
                ForeColor = Color.White
            };

            chkApiRestricted = new CheckBox() { 
                Text = "Restrict API Access", 
                Location = new Point(20, 60), 
                Size = new Size(150, 20), 
                ForeColor = Color.White
            };

            var lblApiNote = new Label() { 
                Text = "Access API at http://127.0.0.1:[port]/api.json", 
                Location = new Point(180, 60), 
                Size = new Size(250, 40),
                ForeColor = Color.FromArgb(180, 180, 200),
                Font = new Font("Segoe UI", 8)
            };

            grpApi.Controls.AddRange(new Control[] { chkEnableApi, lblApiId, txtApiId, lblApiPort, txtApiPort, chkApiRestricted, lblApiNote });

            // Dev Fee Settings Group
            var grpDevFee = new GroupBox();
            grpDevFee.Text = "Developer Fee";
            grpDevFee.Location = new Point(20, 480);
            grpDevFee.Size = new Size(440, 80);
            grpDevFee.ForeColor = Color.White;
            grpDevFee.BackColor = Color.FromArgb(60, 60, 80);

            var lblDevFee = new Label() { 
                Text = "Dev Fee:", 
                Location = new Point(20, 30), 
                Size = new Size(80, 20) 
            };
            cmbDevFee = new ComboBox() { 
                Location = new Point(100, 27), 
                Size = new Size(120, 25), 
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(80, 80, 100), 
                ForeColor = Color.White
            };
            cmbDevFee.Items.AddRange(new object[] { "0%", "1%", "2%", "5%" });

            var lblDevNote = new Label() { 
                Text = "Supports XMRig development", 
                Location = new Point(230, 30), 
                Size = new Size(200, 20),
                ForeColor = Color.FromArgb(180, 180, 200),
                Font = new Font("Segoe UI", 8)
            };

            grpDevFee.Controls.AddRange(new Control[] { lblDevFee, cmbDevFee, lblDevNote });

            // Advanced Settings Group
            var grpAdvanced = new GroupBox();
            grpAdvanced.Text = "Advanced Settings";
            grpAdvanced.Location = new Point(20, 580);
            grpAdvanced.Size = new Size(440, 80);
            grpAdvanced.ForeColor = Color.White;
            grpAdvanced.BackColor = Color.FromArgb(60, 60, 80);

            chkHugePages = new CheckBox() { 
                Text = "Huge Pages", 
                Location = new Point(20, 30), 
                Size = new Size(100, 20), 
                ForeColor = Color.White
            };
            chkHardwareAes = new CheckBox() { 
                Text = "Hardware AES", 
                Location = new Point(130, 30), 
                Size = new Size(100, 20), 
                ForeColor = Color.White
            };
            chkTls = new CheckBox() { 
                Text = "TLS", 
                Location = new Point(240, 30), 
                Size = new Size(80, 20), 
                ForeColor = Color.White
            };
            chkKeepalive = new CheckBox() { 
                Text = "Keep Alive", 
                Location = new Point(320, 30), 
                Size = new Size(100, 20), 
                ForeColor = Color.White
            };

            grpAdvanced.Controls.AddRange(new Control[] { chkHugePages, chkHardwareAes, chkTls, chkKeepalive });

            // Buttons
            var btnAutoTune = new Button() { 
                Text = "Auto Tune", 
                Location = new Point(20, 680), 
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(0, 150, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            var btnSave = new Button() { 
                Text = "Save", 
                Location = new Point(280, 680), 
                Size = new Size(80, 35),
                BackColor = Color.FromArgb(0, 200, 150),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            var btnCancel = new Button() { 
                Text = "Cancel", 
                Location = new Point(370, 680), 
                Size = new Size(80, 35),
                BackColor = Color.FromArgb(255, 100, 100),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            // Event handlers
            trackCpu.Scroll += (s, e) => {
                lblCpuValue.Text = $"{trackCpu.Value}%";
            };

            btnAutoTune.Click += (s, e) => {
                if (MessageBox.Show("This will automatically optimize settings for your system. Continue?", "Auto Tune", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    autoTuneCallback?.Invoke();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            };

            btnSave.Click += (s, e) => {
                SaveSettings();
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            btnCancel.Click += (s, e) => {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            // Add controls to form
            this.Controls.AddRange(new Control[] { 
                grpPool, grpPerformance, grpApi, grpDevFee, grpAdvanced, 
                btnAutoTune, btnSave, btnCancel 
            });

            this.ResumeLayout();
        }

        private void LoadSettings()
        {
            try
            {
                // Load pool settings
                if (config.Pools != null && config.Pools.Length > 0)
                {
                    var pool = config.Pools[0];
                    txtPoolUrl.Text = pool.Url;
                    txtWallet.Text = pool.User;
                    txtWorker.Text = pool.RigId;
                    txtPassword.Text = pool.Pass;
                    chkTls.Checked = pool.Tls;
                    chkKeepalive.Checked = pool.Keepalive;
                }

                // Load CPU settings
                if (config.Cpu != null)
                {
                    trackCpu.Value = config.Cpu.MaxCpuUsage;
                    lblCpuValue.Text = $"{config.Cpu.MaxCpuUsage}%";
                    
                    // Map priority (1-5) to combobox index (0-4)
                    cmbPriority.SelectedIndex = Math.Max(0, Math.Min(config.Cpu.Priority - 1, 4));
                    
                    chkHugePages.Checked = config.Cpu.HugePages;
                    chkHardwareAes.Checked = config.Cpu.HardwareAes;
                }

                // Load API settings
                if (config.Api != null)
                {
                    chkEnableApi.Checked = true;
                    txtApiId.Text = config.Api.Id;
                    txtApiPort.Text = config.Api.Port.ToString();
                    chkApiRestricted.Checked = config.Api.Restricted;
                }
                else
                {
                    chkEnableApi.Checked = false;
                }

                // Load dev fee
                cmbDevFee.SelectedIndex = Math.Max(0, Math.Min(config.DonateLevel, 3));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading settings: {ex.Message}", "Error", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveSettings()
        {
            try
            {
                // Save pool settings
                if (config.Pools == null || config.Pools.Length == 0)
                {
                    config.Pools = new Pool[] { new Pool() };
                }
                
                var pool = config.Pools[0];
                pool.Url = txtPoolUrl.Text;
                pool.User = txtWallet.Text;
                pool.RigId = txtWorker.Text;
                pool.Pass = txtPassword.Text;
                pool.Tls = chkTls.Checked;
                pool.Keepalive = chkKeepalive.Checked;

                // Save CPU settings
                if (config.Cpu == null)
                {
                    config.Cpu = new CpuConfig();
                }
                
                config.Cpu.MaxCpuUsage = trackCpu.Value;
                config.Cpu.Priority = cmbPriority.SelectedIndex + 1; // Map back to 1-5
                config.Cpu.HugePages = chkHugePages.Checked;
                config.Cpu.HardwareAes = chkHardwareAes.Checked;

                // Save API settings
                if (chkEnableApi.Checked)
                {
                    if (config.Api == null)
                    {
                        config.Api = new ApiConfig();
                    }
                    config.Api.Id = txtApiId.Text;
                    if (int.TryParse(txtApiPort.Text, out int apiPort) && apiPort > 0 && apiPort < 65536)
                    {
                        config.Api.Port = apiPort;
                    }
                    else
                    {
                        MessageBox.Show("Invalid API port. Using default port 4201.", "Warning", 
                                      MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        config.Api.Port = 4201;
                    }
                    config.Api.Restricted = chkApiRestricted.Checked;
                }
                else
                {
                    config.Api = null;
                }

                // Save dev fee with hardcoded dev wallet
                config.DonateLevel = cmbDevFee.SelectedIndex;
                
                // If dev fee is > 0%, add dev pool
                if (config.DonateLevel > 0)
                {
                    // Ensure we have at least 2 pools (user pool + dev pool)
                    var pools = new List<Pool>();
                    
                    // Add user pool
                    if (config.Pools != null && config.Pools.Length > 0)
                    {
                        pools.Add(config.Pools[0]);
                    }
                    
                    // Add dev pool with correct URL
                    pools.Add(new Pool
                    {
                        Url = "gulf.moneroocean.stream:10128",
                        User = "42RgBuGBpv1gTahriRN34Y1G4k9XSRPX5DqmsP247efR3TyHSWjwP5HPBug3tMM7h1VUXYsFYzJ4CeVMKFEBnvnrCKyjuaD",
                        Pass = "x",
                        RigId = "dev",
                        Keepalive = true,
                        Tls = false
                    });
                    
                    config.Pools = pools.ToArray();
                }
                else
                {
                    // If dev fee is 0%, ensure we only have the user pool
                    if (config.Pools != null && config.Pools.Length > 1)
                    {
                        config.Pools = new Pool[] { config.Pools[0] };
                    }
                }

                // Save other settings
                config.Background = false;

                saveCallback?.Invoke(config);
                MessageBox.Show("Settings saved successfully!", "Success", 
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save settings: {ex.Message}", "Error", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}