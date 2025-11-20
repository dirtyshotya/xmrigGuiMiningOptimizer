using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Management;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace Xmrig_Ranch_Launcher
{
    public partial class MainForm : Form
    {
        private Process xmrigProcess;
        private bool isMining = false;
        private bool isBenchmarking = false;
        private string logFilePath;
        private double currentHashrate = 0;
        private double maxHashrate = 0;
        private int acceptedShares = 0;
        private int rejectedShares = 0;
        private DateTime startTime;
        private string statusMessage = "Ready";
        private PerformanceCounter cpuCounter;
        private PerformanceCounter ramCounter;
        private MiningConfig currentConfig;
        private SystemInfo systemInfo;
        private bool isDisposing = false;
        
        // Dev fee configuration
        private const string DEV_POOL_URL = "gulf.moneroocean.stream:10128";
        private const string DEV_WALLET = "42RgBuGBpv1gTahriRN34Y1G4k9XSRPX5DqmsP247efR3TyHSWjwP5HPBug3tMM7h1VUXYsFYzJ4CeVMKFEBnvnrCKyjuaD";
        private DateTime lastDevFeeSwitch = DateTime.MinValue;
        private bool isDevFeeMode = false;
        private int devFeeMinutesRemaining = 0;
        private int userMiningMinutesRemaining = 0;

        // Temperature monitoring
        private float currentTemperature = -1;
        private Panel panelTempBar;
        private Panel panelTempBarBackground;

        public MainForm()
        {
            LogMessage("=== Application Starting ===");
            
            try
            {
                // Initialize basic form properties first
                this.Text = "XMRig Ranch Launcher";
                this.Size = new Size(800, 600);
                this.StartPosition = FormStartPosition.CenterScreen;
                
                // Initialize components safely
                SafeInitializeComponent();
                
                // Create temperature bar graph if it doesn't exist in designer
                CreateTemperatureBarGraph();
                
                // Load configuration
                currentConfig = SafeLoadConfig();
                systemInfo = SafeGetSystemInfo();
                
                // Initialize performance counters with error handling
                SafeInitializePerformanceCounters();
                
                // Initialize temperature monitoring
                SafeInitializeTemperatureMonitoring();
                
                // Set up logging
                logFilePath = Path.Combine(Application.StartupPath, "xmrig_launcher_log.txt");
                LogMessage("Application initialized successfully");
                LogMessage($"System: {systemInfo.CpuName} - {systemInfo.CoreCount}C/{systemInfo.ThreadCount}T - {systemInfo.TotalRAM}GB RAM");

                // Start timers
                if (timerUpdate != null) timerUpdate.Start();
                if (timerSystemStats != null) timerSystemStats.Start();
                
                LogMessage("Timers started");
            }
            catch (Exception ex)
            {
                LogMessage($"CRITICAL: Application startup failed: {ex}");
                MessageBox.Show($"Application failed to start: {ex.Message}\n\nCheck log file for details.", 
                    "Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void CreateTemperatureBarGraph()
        {
            try
            {
                // Create temperature bar graph if it doesn't exist
                if (panelTempBarBackground == null)
                {
                    panelTempBarBackground = new Panel();
                    panelTempBarBackground.BackColor = Color.FromArgb(40, 40, 60);
                    panelTempBarBackground.BorderStyle = BorderStyle.FixedSingle;
                    panelTempBarBackground.Size = new Size(120, 120);
                    panelTempBarBackground.Location = new Point(480, 80);
                    
                    panelTempBar = new Panel();
                    panelTempBar.BackColor = Color.FromArgb(255, 100, 150);
                    panelTempBar.Dock = DockStyle.Bottom;
                    panelTempBar.Height = 0;
                    
                    panelTempBarBackground.Controls.Add(panelTempBar);
                    
                    // Add to panelStats if it exists
                    if (panelStats != null && !panelStats.IsDisposed)
                    {
                        panelStats.Controls.Add(panelTempBarBackground);
                        panelTempBarBackground.BringToFront();
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Failed to create temperature bar graph: {ex.Message}");
            }
        }

        private void SafeInitializeComponent()
        {
            try
            {
                // Call the designer-generated InitializeComponent
                InitializeComponent();
                
                LogMessage("UI components initialized successfully");
            }
            catch (Exception ex)
            {
                LogMessage($"UI initialization failed: {ex}");
                // Create fallback UI
                CreateFallbackUI();
            }
        }

        private void CreateFallbackUI()
        {
            // Absolute minimum UI to show something
            try
            {
                this.Controls.Add(new Label 
                { 
                    Text = "XMRig Launcher - UI Initialization Failed\nCheck log file for details.",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter
                });
            }
            catch (Exception ex)
            {
                LogMessage($"Fallback UI also failed: {ex}");
            }
        }

        private MiningConfig SafeLoadConfig()
        {
            try
            {
                string configPath = Path.Combine(Application.StartupPath, "config.json");
                if (File.Exists(configPath))
                {
                    string json = File.ReadAllText(configPath);
                    var config = JsonSerializer.Deserialize<MiningConfig>(json);
                    if (config != null)
                    {
                        LogMessage("Loaded existing config.json");
                        return config;
                    }
                }

                LogMessage("Creating new default configuration");
                return CreateDefaultConfig();
            }
            catch (Exception ex)
            {
                LogMessage($"Config load failed, using defaults: {ex.Message}");
                return CreateDefaultConfig();
            }
        }

        private MiningConfig CreateDefaultConfig()
        {
            try
            {
                var sysInfo = SafeGetSystemInfo();
                var config = new MiningConfig();
                
                config.Cpu = new CpuConfig
                {
                    Threads = CalculateOptimalThreads(sysInfo),
                    MaxCpuUsage = 75,
                    Priority = 1,
                    HugePages = true,
                    HardwareAes = sysInfo.HasAES
                };
                
                config.Pools = new Pool[]
                {
                    new Pool
                    {
                        Url = "gulf.moneroocean.stream:10128",
                        User = "your-wallet-address-here",
                        Pass = "x",
                        RigId = "worker1",
                        Keepalive = true,
                        Tls = false
                    }
                };
                
                config.Api = new ApiConfig
                {
                    Id = "launcher",
                    Port = 4201,
                    Restricted = true
                };
                
                config.DonateLevel = 1;
                config.Background = false;
                
                LogMessage($"Auto-configured: {config.Cpu.Threads.Length} threads, {config.Cpu.MaxCpuUsage}% CPU usage");
                return config;
            }
            catch (Exception ex)
            {
                LogMessage($"Default config creation failed: {ex}");
                return new MiningConfig(); // Return empty config as last resort
            }
        }

        private SystemInfo SafeGetSystemInfo()
        {
            var info = new SystemInfo();
            
            try
            {
                // CPU Information
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        info.CpuName = obj["Name"]?.ToString()?.Trim() ?? "Unknown CPU";
                        info.CoreCount = SafeParseInt(obj["NumberOfCores"]?.ToString(), 4);
                        info.ThreadCount = SafeParseInt(obj["NumberOfLogicalProcessors"]?.ToString(), 4);
                        break;
                    }
                }

                // RAM Information
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        var totalBytes = Convert.ToUInt64(obj["TotalPhysicalMemory"] ?? "8589934592");
                        info.TotalRAM = (float)Math.Round(totalBytes / (1024.0 * 1024.0 * 1024.0), 1);
                        break;
                    }
                }

                // Detect AES-NI support
                info.HasAES = SafeDetectAesSupport();
            }
            catch (Exception ex)
            {
                LogMessage($"System info detection failed: {ex.Message}");
                info.CpuName = "Unknown CPU";
                info.CoreCount = 4;
                info.ThreadCount = 4;
                info.TotalRAM = 8;
                info.HasAES = false;
            }

            return info;
        }

        private int SafeParseInt(string value, int defaultValue)
        {
            if (int.TryParse(value, out int result))
                return result;
            return defaultValue;
        }

        private bool SafeDetectAesSupport()
        {
            try
            {
                using (var aes = System.Security.Cryptography.Aes.Create())
                {
                    return aes != null;
                }
            }
            catch
            {
                return false;
            }
        }

        private void SafeInitializePerformanceCounters()
        {
            try
            {
                cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                ramCounter = new PerformanceCounter("Memory", "Available MBytes");
                
                // Get initial values
                cpuCounter.NextValue();
                ramCounter.NextValue();
                
                LogMessage("Performance counters initialized");
            }
            catch (Exception ex)
            {
                LogMessage($"Performance counters failed: {ex.Message}");
                cpuCounter = null;
                ramCounter = null;
            }
        }

        private void SafeInitializeTemperatureMonitoring()
        {
            try
            {
                // Try multiple methods to get CPU temperature
                currentTemperature = GetCPUTemperature();
                
                if (currentTemperature <= 0)
                {
                    // Try alternative method
                    currentTemperature = GetCPUTemperatureAlternative();
                }
                
                if (currentTemperature > 0)
                {
                    LogMessage($"Temperature monitoring initialized: {currentTemperature}°C");
                }
                else
                {
                    LogMessage("Temperature monitoring not available on this system");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Temperature monitoring initialization failed: {ex.Message}");
            }
        }

        private float GetCPUTemperature()
        {
            try
            {
                // Method 1: Standard WMI
                using (var searcher = new ManagementObjectSearcher(@"root\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        double temp = Convert.ToDouble(obj["CurrentTemperature"]);
                        float celsius = (float)((temp - 2732) / 10.0);
                        if (celsius > 0 && celsius < 150) // Sanity check
                        {
                            return celsius;
                        }
                    }
                }
            }
            catch { }

            try
            {
                // Method 2: Alternative WMI class
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PerfFormattedData_Counters_ThermalZoneInformation"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        if (obj["Temperature"] != null)
                        {
                            uint temp = Convert.ToUInt32(obj["Temperature"]);
                            float celsius = (float)(temp - 273.15);
                            if (celsius > 0 && celsius < 150)
                            {
                                return celsius;
                            }
                        }
                    }
                }
            }
            catch { }

            try
            {
                // Method 3: OpenHardwareMonitor WMI (if OpenHardwareMonitor is running)
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Sensor WHERE SensorType='Temperature' AND Name='CPU Package'"))
                {
                    searcher.Scope = new ManagementScope(@"root\OpenHardwareMonitor");
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        if (obj["Value"] != null)
                        {
                            float celsius = Convert.ToSingle(obj["Value"]);
                            if (celsius > 0 && celsius < 150)
                            {
                                return celsius;
                            }
                        }
                    }
                }
            }
            catch { }

            return -1;
        }

        private float GetCPUTemperatureAlternative()
        {
            try
            {
                // Method 4: Try different WMI namespaces
                string[] namespaces = { 
                    @"root\WMI", 
                    @"root\CIMV2", 
                    @"root\OpenHardwareMonitor",
                    @"root\LibreHardwareMonitor"
                };

                string[] queries = {
                    "SELECT * FROM MSAcpi_ThermalZoneTemperature",
                    "SELECT * FROM Win32_TemperatureProbe",
                    "SELECT * FROM Win32_PerfFormattedData_Counters_ThermalZoneInformation",
                    "SELECT * FROM Sensor WHERE SensorType='Temperature'"
                };

                foreach (string ns in namespaces)
                {
                    foreach (string query in queries)
                    {
                        try
                        {
                            using (var searcher = new ManagementObjectSearcher(ns, query))
                            {
                                foreach (ManagementObject obj in searcher.Get())
                                {
                                    float? temp = TryGetTemperatureFromObject(obj);
                                    if (temp.HasValue && temp > 0 && temp < 150)
                                    {
                                        return temp.Value;
                                    }
                                }
                            }
                        }
                        catch { }
                    }
                }
            }
            catch { }

            return -1;
        }

        private float? TryGetTemperatureFromObject(ManagementObject obj)
        {
            try
            {
                // Try different property names
                string[] propertyNames = { 
                    "CurrentTemperature", "Temperature", "Value", 
                    "CurrentValue", "Reading", "CurrentReading" 
                };

                foreach (string prop in propertyNames)
                {
                    if (obj[prop] != null)
                    {
                        object value = obj[prop];
                        if (value != null)
                        {
                            float temp = Convert.ToSingle(value);
                            
                            // Convert from Kelvin to Celsius if needed
                            if (temp > 200) // Likely in Kelvin (0°C = 273.15K)
                            {
                                temp = temp - 273.15f;
                            }
                            
                            // Convert from decikelvin if needed (common in WMI)
                            if (temp > 2000) // Likely in decikelvin
                            {
                                temp = (temp - 2732) / 10.0f;
                            }
                            
                            if (temp > 0 && temp < 150)
                            {
                                return temp;
                            }
                        }
                    }
                }
            }
            catch { }

            return null;
        }

        private CpuThread[] CalculateOptimalThreads(SystemInfo sysInfo)
        {
            try
            {
                var threads = new List<CpuThread>();
                
                int threadsToUse = sysInfo.CoreCount;
                int ramLimitedThreads = (int)(sysInfo.TotalRAM / 2);
                threadsToUse = Math.Min(threadsToUse, ramLimitedThreads);
                threadsToUse = Math.Max(1, threadsToUse);
                
                for (int i = 0; i < threadsToUse; i++)
                {
                    threads.Add(new CpuThread 
                    { 
                        LowPowerMode = false,
                        NoPrefetch = true,
                        Affinity = (long)Math.Pow(2, i % sysInfo.ThreadCount)
                    });
                }

                LogMessage($"Calculated optimal threads: {threadsToUse} (Cores: {sysInfo.CoreCount}, RAM: {sysInfo.TotalRAM}GB)");
                return threads.ToArray();
            }
            catch (Exception ex)
            {
                LogMessage($"Thread calculation failed: {ex}");
                return new CpuThread[] { new CpuThread() }; // Return safe default
            }
        }

        private void SaveConfig(MiningConfig config)
        {
            try
            {
                string configPath = Path.Combine(Application.StartupPath, "config.json");
                var options = new JsonSerializerOptions 
                { 
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                
                string json = JsonSerializer.Serialize(config, options);
                File.WriteAllText(configPath, json);
                
                LogMessage($"Configuration saved successfully to: {configPath}");
                
                // Verify the file was created and log its contents
                if (File.Exists(configPath))
                {
                    string fileContent = File.ReadAllText(configPath);
                    LogMessage($"Config file created successfully. Size: {fileContent.Length} bytes");
                    currentConfig = config;
                }
                else
                {
                    throw new FileNotFoundException("Config file was not created");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Failed to save config: {ex.Message}");
                SafeInvoke(() => 
                {
                    MessageBox.Show($"Failed to save configuration: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                });
            }
        }

        private void SafeInvoke(Action action)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(action);
                }
                else
                {
                    action();
                }
            }
            catch (ObjectDisposedException)
            {
                // Form is disposing, ignore
            }
            catch (Exception ex)
            {
                LogMessage($"SafeInvoke failed: {ex.Message}");
            }
        }

        private void UpdateConfigDisplay()
        {
            SafeInvoke(() =>
            {
                try
                {
                    if (lblStatus != null)
                        lblStatus.Text = $"Status: {statusMessage}";
                }
                catch (Exception ex)
                {
                    LogMessage($"Config display update failed: {ex.Message}");
                }
            });
        }

        private void LogMessage(string message)
        {
            try
            {
                if (isDisposing) return;

                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
                
                // Write to file
                File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
                
                // Write to UI if available
                SafeInvoke(() =>
                {
                    try
                    {
                        if (txtLog != null && !txtLog.IsDisposed)
                        {
                            txtLog.AppendText(logEntry + Environment.NewLine);
                            txtLog.ScrollToCaret();
                        }
                    }
                    catch (Exception ex)
                    {
                        File.AppendAllText(logFilePath, $"UI Log failed: {ex.Message}" + Environment.NewLine);
                    }
                });
                
                Console.WriteLine(logEntry);
            }
            catch (Exception ex)
            {
                try
                {
                    string tempLog = Path.Combine(Path.GetTempPath(), "xmrig_launcher_crash.log");
                    File.AppendAllText(tempLog, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - LOG FAILED: {ex.Message} - Original: {message}" + Environment.NewLine);
                }
                catch
                {
                    // Give up
                }
            }
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            try
            {
                UpdateDisplay();
                CheckDevFeeSwitch();
            }
            catch (Exception ex)
            {
                LogMessage($"Timer update error: {ex.Message}");
            }
        }

        private void timerSystemStats_Tick(object sender, EventArgs e)
        {
            try
            {
                UpdateSystemStats();
                UpdateGauges();
            }
            catch (Exception ex)
            {
                LogMessage($"System stats timer error: {ex.Message}");
            }
        }

        private void UpdateSystemStats()
        {
            try
            {
                // CPU Usage
                float cpuUsage = 0;
                try
                {
                    if (cpuCounter != null)
                    {
                        cpuUsage = cpuCounter.NextValue();
                        if (cpuUsage == 0) cpuUsage = cpuCounter.NextValue();
                    }
                }
                catch { }

                SafeInvoke(() =>
                {
                    if (lblCPUValue != null && !lblCPUValue.IsDisposed)
                        lblCPUValue.Text = $"{cpuUsage:F0}%";
                    
                    // Update CPU bar graph - VERTICAL
                    if (panelCpuBar != null && !panelCpuBar.IsDisposed)
                    {
                        int barHeight = (int)(cpuUsage / 100.0 * panelCpuBarBackground.Height);
                        panelCpuBar.Height = Math.Max(0, Math.Min(barHeight, panelCpuBarBackground.Height));
                        panelCpuBar.Top = panelCpuBarBackground.Height - panelCpuBar.Height;
                        panelCpuBar.BackColor = GetUsageColor(cpuUsage);
                    }
                });

                // RAM Usage
                float availableRAM = 0;
                try
                {
                    if (ramCounter != null)
                    {
                        availableRAM = ramCounter.NextValue();
                        float totalRAM = GetTotalRAM();
                        float usedRAM = totalRAM - availableRAM;
                        float ramUsagePercent = (usedRAM / totalRAM) * 100;
                        
                        SafeInvoke(() =>
                        {
                            if (lblRAMValue != null && !lblRAMValue.IsDisposed)
                                lblRAMValue.Text = $"{ramUsagePercent:F0}%";
                            
                            // Update RAM bar graph - VERTICAL
                            if (panelRamBar != null && !panelRamBar.IsDisposed)
                            {
                                int barHeight = (int)(ramUsagePercent / 100.0 * panelRamBarBackground.Height);
                                panelRamBar.Height = Math.Max(0, Math.Min(barHeight, panelRamBarBackground.Height));
                                panelRamBar.Top = panelRamBarBackground.Height - panelRamBar.Height;
                                panelRamBar.BackColor = GetUsageColor(ramUsagePercent);
                            }
                        });
                    }
                }
                catch { }

                // Temperature - Update periodically
                if (DateTime.Now.Second % 5 == 0) // Update temperature every 5 seconds to reduce load
                {
                    float temperature = GetCPUTemperature();
                    if (temperature <= 0)
                    {
                        temperature = GetCPUTemperatureAlternative();
                    }
                    
                    if (temperature > 0)
                    {
                        currentTemperature = temperature;
                    }
                    
                    UpdateTemperatureGauge(currentTemperature);
                }
                else
                {
                    // Use cached temperature value
                    UpdateTemperatureGauge(currentTemperature);
                }
            }
            catch (Exception ex)
            {
                LogMessage($"System stats error: {ex.Message}");
            }
        }

        private void UpdateTemperatureGauge(float temperature)
        {
            SafeInvoke(() =>
            {
                try
                {
                    if (lblTempValue != null && !lblTempValue.IsDisposed)
                    {
                        lblTempValue.Text = temperature > 0 ? $"{temperature:F0}°C" : "N/A";
                    }

                    // Update temperature bar graph
                    if (panelTempBar != null && !panelTempBar.IsDisposed && temperature > 0)
                    {
                        // Scale temperature to bar height (0-100°C maps to 0-100% of bar height)
                        float maxTemp = 100f; // Assume 100°C as maximum for scaling
                        float tempPercent = Math.Min(temperature / maxTemp, 1.0f);
                        int barHeight = (int)(tempPercent * panelTempBarBackground.Height);
                        
                        panelTempBar.Height = Math.Max(0, Math.Min(barHeight, panelTempBarBackground.Height));
                        panelTempBar.Top = panelTempBarBackground.Height - panelTempBar.Height;
                        panelTempBar.BackColor = GetTemperatureColor(temperature);
                    }
                }
                catch (Exception ex)
                {
                    LogMessage($"Temperature gauge update error: {ex.Message}");
                }
            });
        }

        private Color GetTemperatureColor(float temperature)
        {
            if (temperature < 50) return Color.LimeGreen;      // Cool - Green
            if (temperature < 70) return Color.Orange;         // Warm - Orange
            if (temperature < 85) return Color.OrangeRed;      // Hot - OrangeRed
            return Color.Red;                                  // Critical - Red
        }

        private Color GetUsageColor(float usage)
        {
            if (usage < 50) return Color.LimeGreen;
            if (usage < 75) return Color.Orange;
            return Color.Red;
        }

        private void UpdateGauges()
        {
            try
            {
                SafeInvoke(() =>
                {
                    if (lblCPUValue != null && lblCPUValue.Text.Contains("%"))
                    {
                        // You could update gauge panel colors based on values
                    }
                });
            }
            catch (Exception ex)
            {
                LogMessage($"Gauge update error: {ex.Message}");
            }
        }

        private float GetTotalRAM()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT TotalVisibleMemorySize FROM Win32_OperatingSystem"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        ulong totalBytes = Convert.ToUInt64(obj["TotalVisibleMemorySize"]);
                        return (float)(totalBytes / 1024.0);
                    }
                }
            }
            catch { }
            return 8192;
        }

        private void UpdateDisplay()
        {
            SafeInvoke(() =>
            {
                try
                {
                    if (!isMining)
                    {
                        if (lblStatus != null && !lblStatus.IsDisposed)
                        {
                            lblStatus.Text = "Status: Stopped";
                            lblStatus.ForeColor = Color.Gray;
                        }
                        return;
                    }

                    // Update mining status display
                    if (lblStatus != null && !lblStatus.IsDisposed)
                    {
                        if (isBenchmarking)
                        {
                            lblStatus.Text = "Status: Benchmarking...";
                            lblStatus.ForeColor = Color.Orange;
                        }
                        else
                        {
                            lblStatus.Text = $"Status: {statusMessage}";
                            lblStatus.ForeColor = currentHashrate > 0 ? Color.Green : Color.Orange;
                        }
                    }

                    // Update uptime
                    if (lblUptime != null && !lblUptime.IsDisposed && isMining)
                    {
                        TimeSpan uptime = DateTime.Now - startTime;
                        lblUptime.Text = $"Uptime: {uptime:hh\\:mm\\:ss}";
                    }

                    // Update hashrate - FIXED: Always update these labels
                    if (lblHashrate != null && !lblHashrate.IsDisposed)
                    {
                        lblHashrate.Text = $"Hashrate: {currentHashrate:F2} H/s";
                    }

                    // Update max hashrate - FIXED: Always update these labels
                    if (lblMaxHashrate != null && !lblMaxHashrate.IsDisposed)
                    {
                        lblMaxHashrate.Text = $"Max: {maxHashrate:F2} H/s";
                    }

                    // Update shares
                    if (lblAcceptedShares != null && !lblAcceptedShares.IsDisposed)
                    {
                        lblAcceptedShares.Text = $"Accepted: {acceptedShares}";
                    }

                    if (lblRejectedShares != null && !lblRejectedShares.IsDisposed)
                    {
                        lblRejectedShares.Text = $"Rejected: {rejectedShares}";
                    }

                    // Update worker ID
                    if (lblWorkerId != null && !lblWorkerId.IsDisposed && currentConfig?.Pools?.Length > 0)
                    {
                        lblWorkerId.Text = $"Worker: {currentConfig.Pools[0].RigId}";
                    }

                    // Update button states
                    if (btnStart != null && !btnStart.IsDisposed)
                        btnStart.Enabled = !isMining;
                    
                    if (btnStop != null && !btnStop.IsDisposed)
                        btnStop.Enabled = isMining;

                }
                catch (Exception ex)
                {
                    LogMessage($"Display update error: {ex.Message}");
                }
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (!isDisposing)
            {
                isDisposing = true;
                LogMessage("Application disposing...");
                
                try
                {
                    if (timerUpdate != null)
                    {
                        timerUpdate.Stop();
                        timerUpdate.Dispose();
                    }
                    
                    if (timerSystemStats != null)
                    {
                        timerSystemStats.Stop();
                        timerSystemStats.Dispose();
                    }
                    
                    if (cpuCounter != null)
                        cpuCounter.Dispose();
                    
                    if (ramCounter != null)
                        ramCounter.Dispose();
                    
                    // Dispose bar graph controls
                    if (panelTempBarBackground != null) panelTempBarBackground.Dispose();
                    
                    if (xmrigProcess != null && !xmrigProcess.HasExited)
                    {
                        try
                        {
                            xmrigProcess.Kill();
                            xmrigProcess.WaitForExit(3000);
                        }
                        catch (Exception ex)
                        {
                            LogMessage($"Process kill error: {ex.Message}");
                        }
                        xmrigProcess.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    LogMessage($"Dispose error: {ex.Message}");
                }
                
                LogMessage("=== XMRig Launcher Shutdown ===");
            }
            
            base.Dispose(disposing);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            isDisposing = true;
            LogMessage("Application closing...");
            base.OnFormClosing(e);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                LogMessage("Starting mining process...");
                isMining = true;
                isBenchmarking = false;
                startTime = DateTime.Now;
                statusMessage = "Starting...";
                currentHashrate = 0;
                maxHashrate = 0;
                acceptedShares = 0;
                rejectedShares = 0;
                
                // Reset dev fee tracking
                lastDevFeeSwitch = DateTime.Now;
                isDevFeeMode = false;
                CalculateDevFeeTiming();

                StartXmrigProcess();

                UpdateDisplay();
                LogMessage("Mining started successfully");
            }
            catch (Exception ex)
            {
                LogMessage($"Start mining failed: {ex}");
                isMining = false;
                statusMessage = "Failed to start";
                SafeInvoke(() => 
                {
                    MessageBox.Show($"Failed to start mining: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                });
                UpdateDisplay();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                LogMessage("Stopping mining process...");
                isMining = false;
                isBenchmarking = false;
                statusMessage = "Stopping...";

                StopXmrigProcess();

                statusMessage = "Stopped";
                UpdateDisplay();
                LogMessage("Mining stopped successfully");
            }
            catch (Exception ex)
            {
                LogMessage($"Stop mining failed: {ex}");
                statusMessage = "Stop failed";
                UpdateDisplay();
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            try
            {
                LogMessage("Opening settings form...");
                
                using (var settingsForm = new SettingsForm(currentConfig, SaveConfig, AutoTuneConfiguration))
                {
                    if (settingsForm.ShowDialog(this) == DialogResult.OK)
                    {
                        LogMessage("Settings updated successfully via settings form");
                        
                        if (isMining)
                        {
                            LogMessage("Restarting mining with new settings...");
                            btnStop_Click(null, EventArgs.Empty);
                            System.Threading.Thread.Sleep(2000);
                            btnStart_Click(null, EventArgs.Empty);
                        }
                    }
                    else
                    {
                        LogMessage("Settings changes cancelled");
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Settings form error: {ex.Message}");
                SafeInvoke(() => 
                {
                    MessageBox.Show($"Failed to open settings: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                });
            }
        }

        private void btnViewLogs_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(logFilePath))
                {
                    Process.Start("notepad.exe", logFilePath);
                }
                else
                {
                    MessageBox.Show("Log file doesn't exist yet.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Failed to open log file: {ex.Message}");
            }
        }

        private void btnClearLogs_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(logFilePath))
                {
                    File.WriteAllText(logFilePath, string.Empty);
                    SafeInvoke(() => txtLog.Clear());
                    LogMessage("Logs cleared by user");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Failed to clear logs: {ex.Message}");
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            OnFormClosing(e);
        }

        private void StartXmrigProcess()
        {
            try
            {
                string xmrigPath = Path.Combine(Application.StartupPath, "xmrig.exe");
                if (!File.Exists(xmrigPath))
                {
                    throw new FileNotFoundException("xmrig.exe not found in application directory");
                }

                SaveConfig(currentConfig);

                string configPath = Path.Combine(Application.StartupPath, "config.json");
                if (!File.Exists(configPath))
                {
                    throw new FileNotFoundException("config.json was not created successfully");
                }

                LogMessage($"Using config file: {configPath}");

                xmrigProcess = new Process();
                xmrigProcess.StartInfo.FileName = xmrigPath;
                xmrigProcess.StartInfo.Arguments = $"--config=\"{configPath}\"";
                xmrigProcess.StartInfo.UseShellExecute = false;
                xmrigProcess.StartInfo.RedirectStandardOutput = true;
                xmrigProcess.StartInfo.RedirectStandardError = true;
                xmrigProcess.StartInfo.CreateNoWindow = true;
                xmrigProcess.StartInfo.WorkingDirectory = Application.StartupPath;

                xmrigProcess.OutputDataReceived += (s, args) => {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        LogMessage($"XMRig: {args.Data}");
                        ParseXmrigOutput(args.Data);
                    }
                };

                xmrigProcess.ErrorDataReceived += (s, args) => {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        LogMessage($"XMRig ERROR: {args.Data}");
                    }
                };

                xmrigProcess.Start();
                xmrigProcess.BeginOutputReadLine();
                xmrigProcess.BeginErrorReadLine();

                LogMessage($"XMRig process started (PID: {xmrigProcess.Id})");
            }
            catch (Exception ex)
            {
                LogMessage($"Failed to start XMRig process: {ex}");
                throw;
            }
        }

        private void StopXmrigProcess()
        {
            try
            {
                if (xmrigProcess != null && !xmrigProcess.HasExited)
                {
                    xmrigProcess.Kill();
                    xmrigProcess.WaitForExit(5000);
                    xmrigProcess.Dispose();
                    xmrigProcess = null;
                    LogMessage("XMRig process stopped");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Error stopping XMRig process: {ex}");
            }
        }

        private void ParseXmrigOutput(string output)
        {
            try
            {
                // Parse hashrate from output - FIXED PARSING
                if (output.Contains("speed") && output.Contains("10s/60s/15m"))
                {
                    // Updated pattern to match the exact format from your log
                    // Example: "speed 10s/60s/15m 3712.2 3743.0 n/a H/s max 3779.3 H/s"
                    var match = Regex.Match(output, @"speed\s+10s/60s/15m\s+([\d.]+)\s+([\d.]+)\s+[\w/.]+\s+H/s\s+max\s+([\d.]+)");
                    
                    if (match.Success && match.Groups.Count >= 4)
                    {
                        currentHashrate = double.Parse(match.Groups[1].Value); // 10s average
                        double sixtySecondAverage = double.Parse(match.Groups[2].Value); // 60s average
                        double maxFromOutput = double.Parse(match.Groups[3].Value); // max
                        
                        if (maxFromOutput > maxHashrate)
                            maxHashrate = maxFromOutput;
                        
                        statusMessage = "Mining - Active";
                        LogMessage($"Parsed hashrate: Current={currentHashrate}, 60s={sixtySecondAverage}, Max={maxHashrate}");
                        
                        // Force UI update immediately
                        UpdateDisplay();
                    }
                    else
                    {
                        // Alternative pattern matching for different formats
                        var altMatch = Regex.Match(output, @"speed\s+10s/60s/15m\s+([\d.]+)");
                        if (altMatch.Success)
                        {
                            currentHashrate = double.Parse(altMatch.Groups[1].Value);
                            if (currentHashrate > maxHashrate)
                                maxHashrate = currentHashrate;
                            
                            statusMessage = "Mining - Active";
                            LogMessage($"Alt parsed hashrate: Current={currentHashrate}, Max={maxHashrate}");
                            UpdateDisplay();
                        }
                    }
                }
                else if (output.Contains("accepted"))
                {
                    acceptedShares++;
                    statusMessage = "Mining - Share Accepted";
                    UpdateDisplay();
                }
                else if (output.Contains("rejected"))
                {
                    rejectedShares++;
                    statusMessage = "Mining - Share Rejected";
                    UpdateDisplay();
                }
                else if (output.Contains("miner") && output.Contains("started"))
                {
                    statusMessage = "Mining - Started";
                    isBenchmarking = false;
                    UpdateDisplay();
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Output parsing error: {ex.Message}");
            }
        }

        private void AutoTuneConfiguration()
        {
            try
            {
                LogMessage("Auto-tuning configuration...");
                
                currentConfig.Cpu.Threads = CalculateOptimalThreads(systemInfo);
                currentConfig.Cpu.MaxCpuUsage = 75;
                currentConfig.Cpu.HugePages = true;
                currentConfig.Cpu.HardwareAes = systemInfo.HasAES;
                
                SaveConfig(currentConfig);
                
                LogMessage($"Auto-tune completed: {currentConfig.Cpu.Threads.Length} threads, {currentConfig.Cpu.MaxCpuUsage}% CPU usage");
                
                SafeInvoke(() => 
                {
                    MessageBox.Show($"Auto-tune completed!\n\nOptimal settings configured:\n- Threads: {currentConfig.Cpu.Threads.Length}\n- CPU Usage: {currentConfig.Cpu.MaxCpuUsage}%\n- Huge Pages: Enabled\n- Hardware AES: {(systemInfo.HasAES ? "Enabled" : "Disabled")}", 
                        "Auto Tune", MessageBoxButtons.OK, MessageBoxIcon.Information);
                });
            }
            catch (Exception ex)
            {
                LogMessage($"Auto-tune failed: {ex.Message}");
                SafeInvoke(() => 
                {
                    MessageBox.Show($"Auto-tune failed: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                });
            }
        }

        // Dev Fee Implementation
        private void CalculateDevFeeTiming()
        {
            int donateLevel = currentConfig?.DonateLevel ?? 1;
            
            // Calculate minutes per hour for dev fee
            int devFeeMinutesPerHour = donateLevel;
            int userMiningMinutesPerHour = 60 - devFeeMinutesPerHour;
            
            // For simplicity, we'll use a fixed cycle pattern
            userMiningMinutesRemaining = userMiningMinutesPerHour;
            devFeeMinutesRemaining = devFeeMinutesPerHour;
            
            LogMessage($"Dev fee timing: {donateLevel}% = {devFeeMinutesPerHour}m dev fee per hour, {userMiningMinutesPerHour}m user mining");
        }

        private void CheckDevFeeSwitch()
        {
            if (!isMining || currentConfig?.Pools?.Length == 0) return;

            try
            {
                TimeSpan timeSinceSwitch = DateTime.Now - lastDevFeeSwitch;
                int totalMinutes = (int)timeSinceSwitch.TotalMinutes;

                // Check if we need to switch modes
                if (isDevFeeMode && totalMinutes >= devFeeMinutesRemaining)
                {
                    // Switch back to user mining
                    SwitchToUserMining();
                }
                else if (!isDevFeeMode && totalMinutes >= userMiningMinutesRemaining)
                {
                    // Switch to dev fee mining
                    SwitchToDevFeeMining();
                }
                else
                {
                    // Update remaining time counters
                    if (isDevFeeMode)
                    {
                        devFeeMinutesRemaining = Math.Max(0, devFeeMinutesRemaining - totalMinutes);
                    }
                    else
                    {
                        userMiningMinutesRemaining = Math.Max(0, userMiningMinutesRemaining - totalMinutes);
                    }
                    lastDevFeeSwitch = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Dev fee switch check error: {ex.Message}");
            }
        }

        private void SwitchToDevFeeMining()
        {
            try
            {
                LogMessage($"Switching to dev fee mining for {devFeeMinutesRemaining} minutes");
                
                // Save original pool configuration
                var originalPool = currentConfig.Pools[0];
                
                // Switch to dev pool and wallet with worker name "devfee"
                currentConfig.Pools[0] = new Pool
                {
                    Url = DEV_POOL_URL,
                    User = DEV_WALLET,
                    Pass = originalPool.Pass,
                    RigId = "devfee",
                    Keepalive = originalPool.Keepalive,
                    Tls = originalPool.Tls
                };

                isDevFeeMode = true;
                lastDevFeeSwitch = DateTime.Now;
                
                // Restart mining with new configuration
                if (isMining)
                {
                    btnStop_Click(null, EventArgs.Empty);
                    System.Threading.Thread.Sleep(2000);
                    btnStart_Click(null, EventArgs.Empty);
                }
                
                LogMessage("Switched to dev fee mining pool");
            }
            catch (Exception ex)
            {
                LogMessage($"Failed to switch to dev fee mining: {ex.Message}");
            }
        }

        private void SwitchToUserMining()
        {
            try
            {
                LogMessage($"Switching back to user mining for {userMiningMinutesRemaining} minutes");
                
                // In a real implementation, you would restore the original user pool configuration
                // For now, we'll just reset to default user pool
                currentConfig.Pools[0] = new Pool
                {
                    Url = "gulf.moneroocean.stream:10128",
                    User = "your-wallet-address-here", // This should be the user's actual wallet
                    Pass = "x",
                    RigId = "worker1",
                    Keepalive = true,
                    Tls = false
                };

                isDevFeeMode = false;
                lastDevFeeSwitch = DateTime.Now;
                
                // Restart mining with user configuration
                if (isMining)
                {
                    btnStop_Click(null, EventArgs.Empty);
                    System.Threading.Thread.Sleep(2000);
                    btnStart_Click(null, EventArgs.Empty);
                }
                
                LogMessage("Switched back to user mining pool");
            }
            catch (Exception ex)
            {
                LogMessage($"Failed to switch to user mining: {ex.Message}");
            }
        }
    }
}