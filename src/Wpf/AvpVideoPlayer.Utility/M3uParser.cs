using System.IO;
using System.Text.RegularExpressions;

namespace AvpVideoPlayer.Utility
{
    public class M3uParser
    {
        private string _path;

        public M3uParser(string path)
        {
            _path = path;
        }

        public Dictionary<string, Uri> ParsePlaylist()
        {
            Dictionary<string, Uri> result = new Dictionary<string, Uri>();
            if(string.IsNullOrWhiteSpace(_path) || !File.Exists(_path)) return result;
            var data = File.ReadAllLines(_path);

            string channelname = string.Empty;

            foreach(var line in data)
            {
                if (line.StartsWith("#EXTINF:", StringComparison.OrdinalIgnoreCase))
                {
                    channelname = ParseChannelName(line);
                }
                else if (!string.IsNullOrWhiteSpace(channelname) &&
                    Uri.IsWellFormedUriString(line, UriKind.Absolute))
                {
                    var channeluri = new Uri(line);
                    if (!result.ContainsKey(channelname))
                        result.Add(channelname, channeluri);
                    channelname = string.Empty;
                }
            }

            return result;
        }

        private static string ParseChannelName(string line)
        {
            const string pattern = "tvg-name=\"([^\"]+)\"";
            if (Regex.IsMatch(line, pattern))
            {
                var match = Regex.Match(line, pattern);
                return match.Value[10..^1];
            }
            if (line.IndexOf(',') > 0)
            {
                var idx = line.LastIndexOf(',') + 1;
                if (idx < line.Length)
                    return line[idx..];
            }

            return string.Empty;
        }
    }
}