using System.Diagnostics;

namespace LiveTileControl.ViewModel {
    public partial class MainPageViewModel : ObservableObject {

        private readonly ApiService _apiService;

        [ObservableProperty]
        private string _data;

        public MainPageViewModel(ApiService apiService) {

            _apiService = apiService;
            _data = "No data yet....";

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
                    Data = newData; // This should trigger the tile flip

                } catch(Exception e) {

                    Debug.WriteLine(e.Message);
                }

                await Task.Delay(30000);

            }
        }
    }
}