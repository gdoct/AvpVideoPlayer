using System;
using System.IO;

namespace AvpVideoPlayer.Uwp.Api 
{

    /// <summary>
    /// extensions to check if a file is a video file, a subtitle or else
    /// </summary>
    public static class FileExtensions
    {
        public static bool IsValidFile(FileInfo file)
        {
            return IsVideoFile(file) || IsSubtitleFile(file);
        }

        public static bool IsValidFile(string file)
        {
            var fi = new FileInfo(file);
            return IsValidFile(fi);
        }

        public static bool IsVideoFile(FileInfo file)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            return file.Extension.ToLowerInvariant() switch
            {
                ".mkv" or ".mpg" or ".mp4" or ".mpv" or ".webm" or ".264" or ".mpeg" => true,
                _ => false,
            };
        }

        public static bool IsSubtitleFile(FileInfo file)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            return file.Extension.ToLowerInvariant() switch
            {
                ".srt" or ".ssa" or ".vtt" or ".tt" or ".sv" or ".mdvd" => true,
                _ => false,
            };
        }
    }
}