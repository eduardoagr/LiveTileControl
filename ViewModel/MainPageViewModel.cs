using CommunityToolkit.Mvvm.Messaging;

using System.Diagnostics;

namespace LiveTileControl.ViewModel {
    public partial class MainPageViewModel : ObservableObject {

        private readonly ApiService _apiService;
        private Controls.LiveTileControl _liveTileControl;

        [ObservableProperty]
        private string _data;

        public MainPageViewModel(ApiService apiService, Controls.LiveTileControl liveTileControl) {

            _apiService = apiService;
            _liveTileControl = liveTileControl;

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

                    WeakReferenceMessenger.Default.Send(newData);

                    _liveTileControl.SetDataAvailable(true);

                    await Task.Delay(10000); // Ensures time for animation

                } catch(Exception e) {

                    Debug.WriteLine(e.Message);
                }
            }
        }
    }
}