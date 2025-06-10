using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WorkerService.Application.Dtos;

namespace WorkerService.Application.Consumers;

public class AssetApiConsumer
{
    private readonly HttpClient _httpClient;

    public AssetApiConsumer(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<AssetDto>> GetAssetsAsync()
    {
        var url = "https://b3api.vercel.app/api/Assets/";

        try
        {
            // Limite simples: aguarda 300ms para evitar sobrecarga se chamado em sequência
            await Task.Delay(300);

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();

            var assets = JsonSerializer.Deserialize<List<AssetDto>>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return assets?.Where(a => a.Ticker is not null).ToList() ?? new List<AssetDto>();
        }
        catch (HttpRequestException httpEx)
        {
            Console.WriteLine($"Erro HTTP ao buscar ativos: {httpEx.Message}");
        }
        catch (JsonException jsonEx)
        {
            Console.WriteLine($"Erro ao desserializar JSON: {jsonEx.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro inesperado: {ex.Message}");
        }

        return new List<AssetDto>(); // fallback em caso de erro
    }
}