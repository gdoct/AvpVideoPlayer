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

        public List<ChannelInfo> ParsePlaylist()
        {
            var result = new List<ChannelInfo>();
            if (string.IsNullOrWhiteSpace(_path) || !File.Exists(_path)) return result;
            var data = File.ReadAllLines(_path);

            string id = string.Empty;
            string name = string.Empty;
            string logo = string.Empty;
            string group = string.Empty;

            foreach (var line in data)
            {
                if (TryParseGroup(line, out string groupname))
                {
                    group = groupname;
                }
                if (line.StartsWith("#EXTINF:", StringComparison.OrdinalIgnoreCase))
                {
                    id = ExtractAttributeValue(line, "tvg-ID");
                    name = ExtractAttributeValue(line, "tvg-name");
                    logo = ExtractAttributeValue(line, "tvg-logo");
                    var grpname = ExtractAttributeValue(line, "group-title");
                    if (!string.IsNullOrWhiteSpace(grpname))
                    {
                        group = grpname;
                    }
                }
                else if (!string.IsNullOrWhiteSpace(id) &&
                    Uri.IsWellFormedUriString(line, UriKind.Absolute))
                {
                    var channel = new ChannelInfo(id, name, logo, new Uri(line), group);
                    result.Add(channel);
                    id = string.Empty;
                }
            }

            return result;
        }

        private static bool TryParseGroup(string line, out string name)
        {
            const string pattern = "##### .* #####";
            var match = Regex.Match(line, pattern);
            if (!match.Success)
            {
                name = string.Empty;
                return false;
            };
            var group = match.Groups[0].Value;
            name = group[6..^6];
            return true;
        }

        static string ExtractAttributeValue(string line, string attribute)
        {
            // Create a regex pattern that matches the attribute value
            // The pattern uses positive lookbehind to find the position where the attribute value starts
            // It then matches everything up to the ending double quote
            string pattern = @"(?<=\b" + attribute + @"="")[^""]*";

            // Create a regex object with the pattern
            Regex regex = new Regex(pattern);

            // Find the first match in the XML string
            Match match = regex.Match(line);

            // If there is a match, return its value
            if (match.Success)
            {
                return match.Value;
            }

            // Otherwise, return null
            else
            {
                return ParseChannelAttribute(line, attribute, true);
            }
        }

        private static string ParseChannelAttribute(string line, string attribute, bool useLineEndWhenEmpty = false)
        {
            string pattern = attribute + "=\"([^\"]+)\"";
            if (Regex.IsMatch(line, pattern))
            {
                var match = Regex.Match(line, pattern);
                return match.Value[10..^1];
            }
            if (useLineEndWhenEmpty && line.IndexOf(',') > 0)
            {
                var idx = line.LastIndexOf(',') + 1;
                if (idx < line.Length)
                {
                    var sub = line[idx..];
                    if (sub.IndexOf(" #") > 0)
                    {
                        sub = sub[..sub.IndexOf(" #")];
                    }
                    return sub;
                }
            }

            return string.Empty;
        }

        public class ChannelInfo
        {
            public ChannelInfo(string id, string name, string logo, Uri uri, string group)
            {
                Id = id;
                Name = name;
                Logo = logo;
                Uri = uri;
                Group = group;
            }

            public string Id { get; set; }
            public string Name { get; set; }
            public string Logo { get; set; }
            public Uri Uri { get; set; }
            public string Group { get; set; }
        }
    }
}