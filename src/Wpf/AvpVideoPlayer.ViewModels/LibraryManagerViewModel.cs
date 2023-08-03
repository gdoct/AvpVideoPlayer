using AvpVideoPlayer.MetaData;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace AvpVideoPlayer.ViewModels;

public class LibraryManagerViewModel : BaseViewModel
{
    public LibraryManagerViewModel(IMetaDataContext context)
    {
        _context = context;
        PurgeCommand = new ActionCommand(Purge);
        BrowseExportPathCommand = new ActionCommand(() => { });
        ExportPathCommand = new ActionCommand(ExportByTag);
        Tags = new(new TaggingService(_context).GetTags());
    }

    private readonly IMetaDataContext _context;
    private string _exportPath = "";
    private bool _canMove = false;
    private string _selectedExportTag = String.Empty;
    private string _statusMessage = String.Empty;

    public ObservableCollection<string> Tags { get; }

    public ICommand PurgeCommand { get; }
    public ICommand BrowseExportPathCommand { get; }
    public ICommand ExportPathCommand { get; }

    private void Purge()
    {
        var candidates = _context.GetMetadata().Where(x => x.Tags.Contains("Delete")).ToList();
        if (candidates.Any())
        {
            foreach (var item in candidates)
            {
                File.Delete(item.FullName);
                _context.DeleteMetadata(item);
            }
        }
    }

    private void ExportByTag()
    {
        var candidates = _context.GetMetadata().Where(x => x.Tags.Contains(SelectedExportTag)).ToList();
        if (candidates == null || !candidates.Any()) return;
        int duplicates = 0;
        int processed = 0;
        int count = candidates.Count;
        foreach (var item in candidates)
        {
            if (string.IsNullOrWhiteSpace(item.Name)) continue;
            var newname = Path.Combine(ExportPath, item.Name);
            if (File.Exists(newname))
            {
                duplicates++;
                continue;
            }
            processed++;
            StatusMessage = $"{processed}/{count} Moving: {item.FullName}";
            File.Move(item.FullName, newname);
            var newfileinfo = new FileInfo(newname);
            var newmeta = FileMetaData.Create(newfileinfo);
            newmeta.Tags.AddRange(item.Tags);
            newmeta.Rating = item.Rating;
            _context.DeleteMetadata(item);
            _context.SaveMetadata(newmeta);
        }

        StatusMessage = $"Processed: {processed}/{count}, duplicates: {duplicates}";
    }


    public string ExportPath
    {
        get => _exportPath;
        set
        {
            SetProperty(ref _exportPath, value);
            CanMove = Directory.Exists(ExportPath);
        }
    }

    public bool CanMove { get => _canMove; set => SetProperty(ref _canMove, value); }

    public string SelectedExportTag { get => _selectedExportTag; set => SetProperty(ref _selectedExportTag, value); }

    public string StatusMessage { get => _statusMessage; set => SetProperty(ref _statusMessage, value); }
}
