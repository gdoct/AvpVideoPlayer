using System.Diagnostics;
using System.IO;
using System.Text;

namespace AvpVideoPlayer.Utility;

public static class Executable
{
    // run an executable and return the output
    // https://stackoverflow.com/a/9730455
    public static (int, string?) Run(string executableName, string arguments)
    {
        int exitcode = 0;
        if (string.IsNullOrWhiteSpace(executableName))
        {
            throw new ArgumentException($"'{nameof(executableName)}' cannot be null or whitespace.", nameof(executableName));
        }

        string executable, workingfolder;
        if (!Path.IsPathFullyQualified(executableName))
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            if (assembly == null) throw new NullReferenceException(nameof(assembly));
            workingfolder = new FileInfo(assembly.Location).DirectoryName ?? string.Empty;
            executable = Path.Combine(workingfolder, executableName);
        }
        else
        {
            workingfolder = new FileInfo(executableName).DirectoryName ?? string.Empty;
            executable = executableName;
        }
        if (!File.Exists(executable)) throw new FileNotFoundException(nameof(executable));
        Process? process = null;
        var outputStringBuilder = new StringBuilder();

        try
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = executable,
                WorkingDirectory = workingfolder,
                Arguments = arguments,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = false
            };
            process = new Process
            {
                StartInfo = processStartInfo,
                EnableRaisingEvents = false
            };
            process.OutputDataReceived += (sender, eventArgs) => outputStringBuilder.AppendLine(eventArgs.Data);
            process.ErrorDataReceived += (sender, eventArgs) => outputStringBuilder.AppendLine(eventArgs.Data);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            var processExited = process.WaitForExit(15000);
            if (processExited)
                exitcode = process.ExitCode;
            else // we timed out...
            {
                process.Kill();
                throw new Exception("ERROR: Process took too long to finish");
            }
        }
        finally
        {
            process?.Close();
        }
        return (exitcode, outputStringBuilder.ToString());
    }
}
