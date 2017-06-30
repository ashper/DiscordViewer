using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace DiscordViewer
{
    public class DiscordMessage : INotifyPropertyChanged
    {
        public ulong Id { get; set; }
        public string User { get; set; }
        public string AvatarUrl { get; set; }
        public string Content { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}