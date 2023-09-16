using System.Text;
using System.Text.Json;
using SolarRelog.Application.Exceptions;
using SolarRelog.Domain.Dto;

namespace SolarRelog.Application.Services;

public class SolarLogClientService
{
    public async Task<SolarLogRequest> RequestLogData(string ip, int? port = null)
    {
        var url = "http://" + ip;

        if (port != null)
            url += ":" + port;
        
        var client = new HttpClient();
        client.BaseAddress = new Uri(url);
        client.DefaultRequestHeaders.ConnectionClose = true;
        client.Timeout = new TimeSpan(0, 0, 10);

        var requestContent = "{\"801\":{\"170\":null}, \"782\":null}";
        var json = new StringContent(requestContent, Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, "/getjp"){Content = json};
        
        var response = await client.SendAsync(request);
        var respContent = await response.Content.ReadAsStringAsync();

        var logRequest = JsonSerializer.Deserialize<SolarLogRequest>(respContent);

        if (logRequest == null)
            throw new AppException($"Error deserializing SolarLog response from URL '{url}'");
        
        return logRequest;
    }
}