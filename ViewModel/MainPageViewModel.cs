using CommunityToolkit.Mvvm.Messaging;

using System.Diagnostics;

namespace LiveTileControl.ViewModel {
    public partial class MainPageViewModel : ObservableObject {

        private readonly ApiService _apiService;
        private Controls.LiveTileControl _liveTileControl;
        IDispatcherTimer _timer;

        [ObservableProperty]
        private string? _data;

        public MainPageViewModel(ApiService apiService, Controls.LiveTileControl liveTileControl) {

            _apiService = apiService;
            _liveTileControl = liveTileControl;
            _timer = Application.Current?.Dispatcher?.CreateTimer() ?? throw new InvalidOperationException("Dispatcher is not available.");

            // Ensure LiveTileControl is registered to listen for updates
            WeakReferenceMessenger.Default.Register<string>(this, async (recipient, message) => {
                await liveTileControl.UpdateTileSafely(message);
            });

            InitializeAsync();

        }

        private async void InitializeAsync() {
            await StartFetchingData();
        }

        private async Task StartFetchingData() {

            var apiUrl = "https://jsonplaceholder.typicode.com/posts/1";

            while(true) {

                try {

                    var newData = await _apiService.FetchAndFormatData(apiUrl);
                    Debug.WriteLine($"Fetched new data: {newData}");

                    int i = 0;// to make the data change every time
                    _timer.Interval = TimeSpan.FromSeconds(10);
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        _timer.Tick += (s, e) => {
                            i = i + 10;
                            Data = newData + i; // This should trigger the tile flip
                        };
                    });
                    _timer.Start(); // Ensures time for animation

                } catch(Exception e) {

                    Debug.WriteLine(e.Message);
                }

                await Task.Delay(30000);
            }
        }
    }
}