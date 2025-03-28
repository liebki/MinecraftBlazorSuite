using System.Diagnostics;

namespace MinecraftBlazorSuite.Services;

public class ProcessManager : IDisposable
{
    private const string JavaArgsPrefix = "-jar -Djline.terminal=jline.UnsupportedTerminal";
    private const string JavaArgsSuffix = "nogui";
    private readonly string _jarPath;
    private readonly string _javaArguments;
    private readonly object _processLock = new();
    private readonly string _workingDirectory;
    private Process? _minecraftServerProcess;

    public ProcessManager(string jarPath, string javaArguments)
    {
        _jarPath = jarPath;
        _javaArguments = javaArguments;
        _workingDirectory = Path.GetDirectoryName(jarPath) ?? throw new ArgumentException("Invalid jar path");
    }

    public bool IsRunning => _minecraftServerProcess is { HasExited: false };

    public int? ProcessId => _minecraftServerProcess?.Id;

    public TimeSpan CurrentCpuTime => _minecraftServerProcess?.TotalProcessorTime ?? TimeSpan.Zero;

    public void Dispose()
    {
        Stop();
        _minecraftServerProcess?.Dispose();
    }

    public event Action<string>? OnOutputReceived;
    public event Action? OnProcessExited;

    public bool Start()
    {
        if (_minecraftServerProcess is { HasExited: false })
            return false;

        _minecraftServerProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "java",
                Arguments = $"{JavaArgsPrefix} {_javaArguments} \"{_jarPath}\" {JavaArgsSuffix}",
                WorkingDirectory = _workingDirectory,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                CreateNoWindow = true
            }
        };

        _minecraftServerProcess.OutputDataReceived += OnOutputDataReceived;
        _minecraftServerProcess.Exited += (_, _) => OnProcessExited?.Invoke();
        _minecraftServerProcess.Start();
        _minecraftServerProcess.BeginOutputReadLine();

        return true;
    }

    public void Stop()
    {
        lock (_processLock)
        {
            if (_minecraftServerProcess is not { HasExited: false })
                return;

            _minecraftServerProcess.StandardInput.WriteLineAsync("stop");
            _minecraftServerProcess.WaitForExitAsync();
        }
    }

    public void SendCommand(string command)
    {
        lock (_processLock)
        {
            if (_minecraftServerProcess is { HasExited: false })
                _minecraftServerProcess.StandardInput.WriteLineAsync(command);
        }
    }

    public long MemoryUsage()
    {
        if (_minecraftServerProcess is not { HasExited: false })
            return 0;

        return _minecraftServerProcess.WorkingSet64 / 1024 / 1024; // Convert to MB
    }

    private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Data)) OnOutputReceived?.Invoke(e.Data);
    }
}