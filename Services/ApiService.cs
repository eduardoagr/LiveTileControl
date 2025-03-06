namespace LiveTileControl.Services;
public class ApiService {

    public async Task<string> FetchAndFormatData(string apiUrl) {
        if(string.IsNullOrEmpty(apiUrl)) {
            return "No API URL provided";
        }

        using var client = new HttpClient();
        var response = await client.GetStringAsync(apiUrl);

        var post = JsonSerializer.Deserialize<Post>(response);

        return $"userId : {post?.UserId},\n" +
               $"id : {post?.Id},\n" +
               $"title : {post?.Title},\n" +
               $"body : {post?.Body}";
    }
}
