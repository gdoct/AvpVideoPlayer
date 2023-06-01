using System.Collections;
using System.Linq;

namespace AvpVideoPlayer.Api;

/// <summary>
/// Base class for events for use in the EventHub
/// </summary>
public abstract class EventBase {
    public override string ToString()
    {
        return $"Event: {GetType().Name}";
    }

    public object Clone() => this.MemberwiseClone();

    public Guid Id { get; } = Guid.NewGuid();
}

/// <summary>
/// Generic base class for events for use in the EventHub
/// </summary>
public abstract class EventBase<T> : EventBase
{
    protected EventBase(T eventdata)
    {
        if (eventdata is null) throw new ArgumentNullException(nameof(eventdata));
        Data = eventdata;
    }

    /// <summary>
    /// The data for this event
    /// </summary>
    public T Data { get; }

    public override string ToString()
    {
        return $"{GetType().Name} => {FormatData()}";
    }

    private string FormatData()
    {
        var d = Data;
        if (d is string s) // string is Enumerable[char]
        {
            if (s.Length > 20) return s[0..20];
            return s;
        }
        if (d is IEnumerable<object> enumerable) return $"{enumerable.Count()} items";
        return d?.ToString() ?? "<empty>";
    }
}
