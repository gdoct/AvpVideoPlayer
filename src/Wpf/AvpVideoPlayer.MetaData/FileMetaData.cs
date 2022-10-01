namespace AvpVideoPlayer.MetaData;

public class FileMetaData
{
    public string? Path { get; init; }

    public string? Name { get; init; }

    public string FullName => System.IO.Path.Combine(Path ?? "", Name ?? "");

    public long Length { get; init; } 

    public int Rating { get; set; } 

    public List<string> Tags { get; init; } = new List<string>();

    public static FileMetaData Create(FileInfo file) =>
        new ()
        {
            Path = file.DirectoryName ?? string.Empty,
            Name = file.Name,
            Length = file.Exists ? file.Length : 0,
        };

    public static FileMetaData Empty =>
        new () { Path="", Name="", Length=0 };
}