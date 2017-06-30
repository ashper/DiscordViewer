using Discord;
using Discord.WebSocket;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DiscordViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainAsync();
            messages = new ObservableCollection<DiscordMessage>();
            lvMain.ItemsSource = messages;
        }

        private DiscordSocketClient _client;
        public ObservableCollection<DiscordMessage> messages;

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
                MessageCacheSize = 2000,
            });

            await _client.LoginAsync(TokenType.Bot, "MzI3ODgyNDU5MjEzNzI1Njk4.DC73PA.ZWTiwIESvrmqQgastOKvvgcQkBw");
            await _client.StartAsync();
            _client.MessageReceived += MessageRecieved;

            await Task.Delay(-1);
        }

        private Task MessageRecieved(SocketMessage message)
        {
            if (message.Channel.Name == "ffxiv-general")
            {
                var newMessage = new DiscordMessage()
                {
                    Id = message.Id,
                    User = message.Author.Username,
                    Content = message.Content,
                    AvatarUrl = message.Author.GetAvatarUrl(),
                    MessageOrder = CheckIfNewMessageSet(message)
                };
                Dispatcher.Invoke(() =>
                {
                    messages.Add(newMessage);
                    lvMain.ScrollIntoView(messages.Last());
                });
            }

            return Task.CompletedTask;
        }

        private MessageOrder CheckIfNewMessageSet(SocketMessage message)
        {
            if (messages.Count <= 0)
            {
                return MessageOrder.First;
            }
            else if (messages.Last().User == message.Author.Username)
            {
                return MessageOrder.FollowOn;
            }
            else
            {
                return MessageOrder.New;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (messages.Count > 0)
            {
                lvMain.ScrollIntoView(messages.Last());
            }
        }
    }

    public class ChatTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement elemnt = container as FrameworkElement;
            DiscordMessage chatMessage = (DiscordMessage)item;

            switch (chatMessage.MessageOrder)
            {
                case MessageOrder.First:
                    return elemnt.FindResource("FirstChatTemplate") as DataTemplate;

                case MessageOrder.FollowOn:
                    return elemnt.FindResource("SlimChatTemplate") as DataTemplate;

                case MessageOrder.New:

                default:
                    return elemnt.FindResource("FullChatTemplate") as DataTemplate;
            }
        }
    }
}