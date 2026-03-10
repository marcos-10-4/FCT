using Aplicacion_CTG.Modelos;
using System.Net.Http.Json;


namespace Aplicacion_CTG.Servicios
{
    internal class ServicioApi
    {
        private readonly HttpClient _client;

        public ServicioApi()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:5008/")
            };
        }

        public async Task<List<Ranking>> GetRankingAsync()
        {
            return await _client.GetFromJsonAsync<List<Ranking>>("api/Usuarios/ranking");
        }
    }
}
