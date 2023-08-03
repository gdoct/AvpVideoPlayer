using AvpVideoPlayer.Api;
using AvpVideoPlayer.ViewModels.Events;
using AvpVideoPlayer.ViewModels.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reactive.Linq;
using Xunit;

namespace AvpVideoPlayer.ViewModels.Tests;

public class ViewModelEventTests
{
    public ViewModelEventTests()
    {
    }

    public class EventTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new[] { new ActivateSubtitleEvent(new SubtitleInfo())};
            yield return new[] { new FullScreenEvent(true)};
            yield return new[] { new FullScreenEvent(false)};
            yield return new[] { new LoadSubtitlesEvent("fdssdf")};
            yield return new[] { new LoadSubtitlesEvent(string.Empty)};
            yield return new[] { new PathChangedEvent("path")};
            yield return new[] { new PlayDurationChangedEvent(2423)};
            yield return new[] { new PlaylistMoveEvent(PlayListMoveTypes.Forward)};
            yield return new[] { new PlayPositionChangedEvent(new())};
            yield return new[] { new PlayPositionChangeRequestEvent(new())};
            yield return new[] { new PlayStateChangedEvent(new())};
            yield return new[] { new PlayStateChangeRequestEvent(new())};
            yield return new[] { new SearchTextChangedEvent("search")};
            yield return new[] { new SelectedFileChangedEvent(new FileViewModel(FileTypes.Folder)) };
            yield return new[] { new SelectVideoEvent(new VideoFileViewModel(new System.IO.FileInfo("test"))) };
            yield return new[] { new SetSubtitleSizeEvent(new())};
            yield return new[] { new SubtitlesLoadedEvent(new List<SubtitleInfo>())};
            yield return new[] { new SubtitlesLoadedEvent(new List<SubtitleInfo>())};
            yield return new[] { new ToggleSubtitlesEvent(true)};
            yield return new[] { new ToggleSubtitlesEvent(false)};
            yield return new[] { new VolumeChangedEvent(new())};
            yield return new[] { new VolumeChangeRequestEvent(new())};
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    [Theory]
    [ClassData(typeof(EventTestData))]
    public void CanProcessEvent(EventBase eventToProcess)
    {
        using var eventhub = new EventHub.EventHub();
        bool isHit = false;
        int hitcount = 0;
        var callback = (EventBase e) =>
        {
            isHit = true;
            hitcount++;
        };

        using (eventhub.Events.Subscribe(callback))
        {
            eventhub.Publish(eventToProcess);
            Assert.True(isHit);
        }

        Assert.Equal(1, hitcount);
    }

    [Fact]
    public void CanProcessEvents()
    {
        using var eventhub = new EventHub.EventHub();
        var events = new EventBase[] {
                     new ActivateSubtitleEvent(new SubtitleInfo()),
                     new FullScreenEvent(true),
                     new FullScreenEvent(false),
                     new LoadSubtitlesEvent("fdssdf"),
                     new LoadSubtitlesEvent(string.Empty),
                     new PathChangedEvent("path"),
                     new PlayDurationChangedEvent(2423),
                     new PlaylistMoveEvent(PlayListMoveTypes.Forward),
                     new PlayPositionChangedEvent(new()),
                     new PlayPositionChangeRequestEvent(new()),
                     new PlayStateChangedEvent(new()),
                     new PlayStateChangeRequestEvent(new()),
                     new SearchTextChangedEvent("search"),
                     new SelectVideoEvent(new VideoFileViewModel(new System.IO.FileInfo("video"))),
                     new SetSubtitleSizeEvent(new()),
                     new SubtitlesLoadedEvent(new List<SubtitleInfo>()),
                     new SubtitlesLoadedEvent(new List<SubtitleInfo>()),
                     new ToggleSubtitlesEvent(true),
                     new ToggleSubtitlesEvent(false),
                     new VolumeChangedEvent(new()),
                     new VolumeChangeRequestEvent(new()),
        };
        bool isHit = false;
        int hitcount = 0;
        var callback = (EventBase e) =>
        {
            isHit = true;
            hitcount++;
        };

        using (eventhub.Events.Subscribe(callback))
        {
            foreach (var appevent in events)
            {
                isHit = false;
                eventhub.Publish(appevent);
                Assert.True(isHit);
            }
        }

        Assert.Equal(events.Length, hitcount);
    }

    [Fact]
    public void CheckSubtitleProperties()
    {
        LoadSubtitlesEvent unitTestEvent = new("blabla");
        var callback = (LoadSubtitlesEvent e) =>
        {
            Assert.Equal(e.Filename, unitTestEvent.Data);
        };
        using (var eventHub = new EventHub.EventHub())
        using (eventHub.Events.OfType<LoadSubtitlesEvent>().Subscribe(callback))
        {
            eventHub.Publish(unitTestEvent);
        }
    }

    [Fact]
    public void CheckToggleSubtitleProperties()
    {
        ToggleSubtitlesEvent unitTestEvent = new(true)
        {
            ActiveSubtitle = new SubtitleInfo(),
            IsHandled = true
        };
        var callback = (ToggleSubtitlesEvent e) =>
        {
            Assert.Equal(e.Data, unitTestEvent.Data);
            Assert.Equal(e.ActiveSubtitle, unitTestEvent.ActiveSubtitle);
            Assert.Equal(e.IsHandled, unitTestEvent.IsHandled);
        };
        using (var eventHub = new EventHub.EventHub())
        using (eventHub.Events.OfType<ToggleSubtitlesEvent>().Subscribe(callback))
        {
            eventHub.Publish(unitTestEvent);
        }
    }
}
