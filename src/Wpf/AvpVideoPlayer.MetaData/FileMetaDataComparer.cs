using System.Diagnostics.CodeAnalysis;

namespace AvpVideoPlayer.MetaData;

internal class FileMetaDataComparer : IEqualityComparer<FileMetaData?>
{
    public bool Equals(FileMetaData? left, FileMetaData? right)
    {
        if (left is null || right is null)
        {
            return left is null && right is null;
        }

        return string.Compare(left.Name, right.Name, StringComparison.OrdinalIgnoreCase) == 0
            && left.Length.Equals(right.Length)
            && left.Rating.Equals(right.Rating)
            && string.Compare(left.Path, right.Path, StringComparison.OrdinalIgnoreCase) == 0
            && left.Tags.ListEquals(right.Tags);
    }

    public int GetHashCode([DisallowNull] FileMetaData obj) => (obj.Path, obj.Length, obj.Tags).GetHashCode();
}
