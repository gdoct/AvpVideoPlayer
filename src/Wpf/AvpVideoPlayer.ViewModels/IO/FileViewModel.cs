﻿using AvpVideoPlayer.Api;
using System;
using System.IO;

namespace AvpVideoPlayer.ViewModels.IO;

public class FileViewModel : BaseViewModel
{
    private string? _path;
    private FileTypes thumbnailType;

    public FileViewModel(FileTypes filetype)
    {
        thumbnailType = filetype;
    }

    public FileInfo? FileInfo { get; private set; }

    public DateTime? LastWriteTime
    {
        get
        {
            if (FileInfo?.Exists == true) { return FileInfo?.LastWriteTime; }
            return DateTime.Today;
        }
    }

    public string? Name { get; set; }

    public string? Path
    {
        get => _path;
        set
        {
            _path = value;
            FileInfo = value != null ? new FileInfo(value) : null;
        }
    }

    public FileTypes FileType { get => thumbnailType; set { thumbnailType = value; RaisePropertyChanged(); } }

    public long Size
    {
        get
        {
            if (FileInfo == null) return 0;
            if (FileInfo.Exists)
            {
                return FileInfo.Length;
            }
            return 0;
        }
    }
}
