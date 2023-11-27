using System.Text.Json;

namespace Aggregator.Extensions;

public static class HttpClientExtensions
{
    public static async Task<T> ReadContentAs<T>(this HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            // Log the error details
            Console.WriteLine($"API Error: {response.StatusCode} - {response.ReasonPhrase}");
                
            // Capture additional error details
            var errorContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Console.WriteLine($"Error Content: {errorContent}");

            // Throw a more specific exception or return an error response
            throw new ApplicationException($"Error calling the API: {response.ReasonPhrase}");
        }

        var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        return JsonSerializer.Deserialize<T>(dataAsString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}