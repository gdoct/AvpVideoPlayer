namespace AvpVideoPlayer.ViewModels.IO
{
    public class VideoStreamCategoryViewModel : FileViewModel
    {
        public VideoStreamCategoryViewModel(string name, string path) : base(Api.FileTypes.Category)
        {
            Name = name;
            Path = path;
        }
    }
}
