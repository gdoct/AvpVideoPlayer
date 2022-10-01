namespace AvpVideoPlayer.MetaData;

public static class Extensions
{
    public static bool ListEquals<T>(this IList<T> self, IList<T> otherlist)
    {
        if (self is null || otherlist is null)
        {
            return self is null && otherlist is null;
        }
        if (self.Count != otherlist.Count)
            return false;
        for (int i = 0; i < self.Count; i++)
        {
            var item = self[i];
            if (item != null && !item.Equals(otherlist[i]))
                return false;
        }
        return true;
    }
}
