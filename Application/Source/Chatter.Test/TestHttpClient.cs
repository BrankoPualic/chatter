using Chatter.Common.Extensions;
using System.Text;

public static class TestHttpClient
{
	public static async Task<string> TestAsync(HttpTest data)
	{
		var content = new StringContent(data.Payload.SerializeJsonObject(), Encoding.UTF8, "application/json");

		using var http = new HttpClient();

		var request = new HttpRequestMessage(data.Method, data.Url) { Content = content };

		var response = await http.SendAsync(request);

		return await response.Content.ReadAsStringAsync();
	}
}

public class HttpTest
{
	public string Url { get; set; }

	public HttpMethod Method { get; set; }

	public object Payload { get; set; } = null;
}