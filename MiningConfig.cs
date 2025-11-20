using System.Text.Json.Serialization;

namespace Xmrig_Ranch_Launcher
{
    public class MiningConfig
    {
        [JsonPropertyName("api")]
        public ApiConfig Api { get; set; } = new ApiConfig();
        
        [JsonPropertyName("background")]
        public bool Background { get; set; } = false;
        
        [JsonPropertyName("cpu")]
        public CpuConfig Cpu { get; set; } = new CpuConfig();
        
        [JsonPropertyName("pools")]
        public Pool[] Pools { get; set; } = new Pool[1] { new Pool() };
        
        [JsonPropertyName("donate-level")]
        public int DonateLevel { get; set; } = 1;
    }

    public class CpuConfig
    {
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; } = true;
        
        [JsonPropertyName("threads")]
        public CpuThread[] Threads { get; set; } = new CpuThread[0];
        
        [JsonPropertyName("max-cpu-usage")]
        public int MaxCpuUsage { get; set; } = 75;
        
        [JsonPropertyName("priority")]
        public int Priority { get; set; } = 1;
        
        [JsonPropertyName("huge-pages")]
        public bool HugePages { get; set; } = true;
        
        [JsonPropertyName("hw-aes")]
        public bool HardwareAes { get; set; } = true;
    }

    public class CpuThread
    {
        [JsonPropertyName("low_power_mode")]
        public bool LowPowerMode { get; set; } = false;
        
        [JsonPropertyName("no_prefetch")]
        public bool NoPrefetch { get; set; } = true;
        
        [JsonPropertyName("affinity")]
        public long Affinity { get; set; } = 0;
    }

    public class Pool
    {
        [JsonPropertyName("url")]
        public string Url { get; set; } = "gulf.moneroocean.stream:10128";
        
        [JsonPropertyName("user")]
        public string User { get; set; } = "your-wallet-address-here";
        
        [JsonPropertyName("pass")]
        public string Pass { get; set; } = "x";
        
        [JsonPropertyName("rig-id")]
        public string RigId { get; set; } = "worker1";
        
        [JsonPropertyName("keepalive")]
        public bool Keepalive { get; set; } = true;
        
        [JsonPropertyName("tls")]
        public bool Tls { get; set; } = false;
    }

    public class ApiConfig
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = "launcher";
        
        [JsonPropertyName("port")]
        public int Port { get; set; } = 4201;
        
        [JsonPropertyName("restricted")]
        public bool Restricted { get; set; } = true;
    }

    public class SystemInfo
    {
        public string CpuName { get; set; } = "Unknown CPU";
        public int CoreCount { get; set; } = 4;
        public int ThreadCount { get; set; } = 4;
        public float TotalRAM { get; set; } = 8;
        public bool HasAES { get; set; } = false;
    }
}