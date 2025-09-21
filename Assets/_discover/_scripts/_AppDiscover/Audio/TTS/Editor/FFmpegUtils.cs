#if UNITY_EDITOR
using System;
using System.IO;
using UnityEngine;

namespace Antura.Discover.Audio.Editor
{
    /// <summary>ffmpeg helpers used by the Voiceover tools.</summary>
    internal static class FFmpegUtils
    {
        public static string FindOnPath()
        {
            try
            {
                var envPath = System.Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
                foreach (var dir in envPath.Split(Path.PathSeparator))
                {
                    try
                    {
                        var cand = Path.Combine(dir, RuntimeFFmpegName());
                        if (File.Exists(cand))
                            return cand;
                    }
                    catch { }
                }
            }
            catch { }
            return string.Empty;
        }

        public static bool ConvertMp3ToOgg(string ffmpegPath, string srcMp3, string dstOgg, out string error)
        {
            error = null;
            try
            {
                if (string.IsNullOrEmpty(ffmpegPath))
                    ffmpegPath = RuntimeFFmpegName();

                var dir = Path.GetDirectoryName(dstOgg);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = ffmpegPath,
                    Arguments = $"-y -i \"{srcMp3}\" -c:a libvorbis -qscale:a 5 \"{dstOgg}\"",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                };
                using (var proc = System.Diagnostics.Process.Start(psi))
                {
                    _ = proc.StandardOutput.ReadToEnd();
                    string stdErr = proc.StandardError.ReadToEnd();
                    proc.WaitForExit();
                    if (proc.ExitCode != 0)
                    {
                        error = stdErr;
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        private static string RuntimeFFmpegName()
        {
            return Application.platform == RuntimePlatform.WindowsEditor ? "ffmpeg.exe" : "ffmpeg";
        }
    }
}
#endif
