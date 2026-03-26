using CTG_App.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CTG_App.Servicios
{
    internal class ApiServicio
    {
        private readonly HttpClient _client;

        public ApiServicio()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://10.0.2.2:5085/") 
            };
        }
        public async Task<List<Ranking>> GetRankingAsync()
        {
            return await _client.GetFromJsonAsync<List<Ranking>>("api/UsuariosControlador/ranking");
        }
    }
}
