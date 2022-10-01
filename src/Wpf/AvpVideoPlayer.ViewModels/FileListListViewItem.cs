using AvpVideoPlayer.MetaData;
using System.Linq;

namespace AvpVideoPlayer.ViewModels;


public class FileListListViewItem : BaseViewModel
{
    private bool _isSelected;
    private bool _isActivated;
    private FileMetaData? _metadata;

    public FileListListViewItem(FileViewModel file, FileMetaData metaData)
    {
        File = file;
        MetaData = metaData;
    }

    public FileViewModel File { get; }

    public FileMetaData? MetaData
    {
        get => _metadata; 
        set
        { 
            SetProperty(ref _metadata, value); 
            RaisePropertyChanged(nameof(Tags));
        }
    }

    public string Tags => string.Join(", ", MetaData?.Tags.OrderBy(x => x) ?? Enumerable.Empty<string>());

    public bool IsSelected
    {
        get
        {
            return _isSelected;
        }
        set
        {
            _isSelected = value;
            RaisePropertyChanged();
        }
    }

    public bool IsActivated
    {
        get
        {
            return _isActivated;
        }
        set
        {
            _isActivated = value;
            RaisePropertyChanged();
        }
    }
}
