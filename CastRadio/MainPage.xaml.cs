using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using GoogleCast;
using GoogleCast.Channels;
using GoogleCast.Models.Media;
using Xamarin.Forms;

namespace CastRadio
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public string CastUrl { get; set; } = "https://icecast-qmusicnl-cdp.triple-it.nl/Qmusic_nl_live_32.aac";

        public ObservableCollection<IReceiver> CastDevices { get; set; }
        public IReceiver SelectedCastDevice { get; set; }

        public Command StartCastCommand { get; set; }

        public MainPage()
        {
            InitializeComponent();

            Init();

            StartCastCommand = new Command(StartCasting);
        }

        async void StartCasting()
        {
            var sender = new Sender();
            // Connect to the Chromecast
            await sender.ConnectAsync(SelectedCastDevice);
            // Launch the default media receiver application
            var mediaChannel = sender.GetChannel<IMediaChannel>();
            await sender.LaunchAsync(mediaChannel);
            // Load and play
            var mediaStatus = await mediaChannel.LoadAsync(
                new MediaInformation() { ContentId = CastUrl });
        }

        async Task Init()
        {
            // Use the DeviceLocator to find all Chromecasts on our network
            var receivers = await new DeviceLocator().FindReceiversAsync();

            CastDevices = new ObservableCollection<IReceiver>();

            foreach (var r in receivers)
                CastDevices.Add(r);

            BindingContext = this;
        }
    }
}