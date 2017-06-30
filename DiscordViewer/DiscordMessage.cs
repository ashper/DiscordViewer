using System.ComponentModel;

namespace DiscordViewer
{
    public class DiscordMessage : INotifyPropertyChanged
    {
        public ulong Id
        {
            get; set;
        }

        public string User
        {
            get; set;
        }

        public string AvatarUrl
        {
            get; set;
        }

        public string Content
        {
            get; set;
        }

        public MessageOrder MessageOrder
        {
            get; set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public enum MessageOrder
    {
        First, New, FollowOn
    }
}