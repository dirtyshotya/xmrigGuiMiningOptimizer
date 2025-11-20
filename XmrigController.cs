using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace XmrigLauncher
{
    public class XmrigController : IDisposable
    {
        private Process? xmrigProcess;
        private readonly HttpClient httpClient;
        private readonly string apiBase = "http://127.0.0.1:420"; // Use your port 420
        private bool disposedValue;

        public XmrigController()
        {
            httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(3);
        }

        public void Start(string xmrigPath, string arguments = "")
        {
            if (string.IsNullOrWhiteSpace(xmrigPath))
                throw new ArgumentException("xmrigPath is required.", nameof(xmrigPath));

            if (!File.Exists(xmrigPath))
                throw new FileNotFoundException("xmrig.exe not found.", xmrigPath);

            if (IsRunning())
                return;

            xmrigProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = xmrigPath,
                    Arguments = arguments,
                    WorkingDirectory = Path.GetDirectoryName(xmrigPath) ?? AppDomain.CurrentDomain.BaseDirectory,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                },
                EnableRaisingEvents = true
            };

            xmrigProcess.OutputDataReceived += (s, e) => { 
                if (!string.IsNullOrEmpty(e.Data)) 
                    Debug.WriteLine($"[XMRig] {e.Data}"); 
            };
            xmrigProcess.ErrorDataReceived += (s, e) => { 
                if (!string.IsNullOrEmpty(e.Data)) 
                    Debug.WriteLine($"[XMRig ERROR] {e.Data}"); 
            };

            xmrigProcess.Start();
            xmrigProcess.BeginOutputReadLine();
            xmrigProcess.BeginErrorReadLine();

            // Wait for XMRig to start
            Thread.Sleep(2000);
        }

        public void Stop()
        {
            try
            {
                if (xmrigProcess != null && !xmrigProcess.HasExited)
                {
                    xmrigProcess.Kill(entireProcessTree: true);
                    xmrigProcess.WaitForExit(3000);
                }
            }
            catch
            {
                // ignore
            }
            finally
            {
                xmrigProcess?.Dispose();
                xmrigProcess = null;
            }
        }

        public bool IsRunning()
        {
            try
            {
                return xmrigProcess != null && !xmrigProcess.HasExited;
            }
            catch
            {
                return false;
            }
        }

        public async Task<double> GetHashrateAsync(int maxRetries = 5, int delayMs = 1000)
        {
            if (!IsRunning())
                return -1;

            string url = $"{apiBase}/api.json"; // Try the main API endpoint

            for (int attempt = 0; attempt < maxRetries; attempt++)
            {
                try
                {
                    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
                    HttpResponseMessage resp = await httpClient.GetAsync(url, cts.Token);
                    
                    if (resp.IsSuccessStatusCode)
                    {
                        string json = await resp.Content.ReadAsStringAsync(cts.Token);
                        return ParseHashrateFromJson(json);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Hashrate query attempt {attempt + 1} failed: {ex.Message}");
                }

                if (attempt < maxRetries - 1)
                    await Task.Delay(delayMs);
            }

            return -1;
        }

        private double ParseHashrateFromJson(string json)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                // Try to find hashrate in various possible locations
                if (root.TryGetProperty("hashrate", out var hashrate))
                {
                    // Check if hashrate is an object with total property
                    if (hashrate.ValueKind == JsonValueKind.Object &&
                        hashrate.TryGetProperty("total", out var total) &&
                        total.ValueKind == JsonValueKind.Array &&
                        total.GetArrayLength() > 0)
                    {
                        return total[0].GetDouble();
                    }
                    // Check if hashrate is a direct number
                    else if (hashrate.ValueKind == JsonValueKind.Number)
                    {
                        return hashrate.GetDouble();
                    }
                }

                // Try results section
                if (root.TryGetProperty("results", out var results) &&
                    results.TryGetProperty("hashrate_total", out var hrTotal) &&
                    hrTotal.ValueKind == JsonValueKind.Array &&
                    hrTotal.GetArrayLength() > 0)
                {
                    return hrTotal[0].GetDouble();
                }

                // Try connection section
                if (root.TryGetProperty("connection", out var connection) &&
                    connection.TryGetProperty("pool", out var pool) &&
                    pool.TryGetProperty("diff", out var diff))
                {
                    // Sometimes we can get hashrate from difficulty and shares
                    if (pool.TryGetProperty("accepted", out var accepted) &&
                        accepted.GetInt64() > 0)
                    {
                        // This is a rough estimate - real hashrate comes from the hashrate field
                        return 0; // Return 0 to indicate we found the API but no hashrate data
                    }
                }
            }
            catch (JsonException ex)
            {
                Debug.WriteLine($"JSON parsing error: {ex.Message}");
            }

            return -1;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    httpClient?.Dispose();
                    xmrigProcess?.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}