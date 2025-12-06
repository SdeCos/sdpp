using System.Text.Json;

namespace ClienteApiRest
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://petstore.swagger.io/v2/pet/findByStatus?status=available");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            Console.Write(responseText);
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Pet>? pets = JsonSerializer.Deserialize<List<Pet>>(responseText, options);

            request = new HttpRequestMessage(HttpMethod.Post, "https://petstore.swagger.io/v2/pet");
            Pet nuevaMascota = new Pet
            {
                Id = 123456,
                Name = "Test"
            };

            var contenido = JsonSerializer.Serialize(nuevaMascota, options);
            var content = new StringContent(contenido, null, "application/json");
            request.Content = content;
            response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseTextCreacion = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseTextCreacion);
            Console.ReadLine();
        }
    }
}
