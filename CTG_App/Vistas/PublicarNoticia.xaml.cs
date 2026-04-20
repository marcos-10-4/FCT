using CTG_App.Modelos;
using System.Net.Http.Json;

namespace CTG_App.Vistas;

public partial class PublicarNoticia : ContentPage
{
	public PublicarNoticia()
	{
		InitializeComponent();
	}
    private HttpClient client = new HttpClient
    {
        BaseAddress = new Uri("http://10.0.2.2:5085/")
    };

    private async void OnPublicarClicked(object sender, EventArgs e)
    {
        var noticia = new Noticia
        {
            Titulo = TituloEntry.Text,
            Contenido = ContenidoEditor.Text,
            Imagen = "tenis.jpg"
        };

        var response = await client.PostAsJsonAsync("api/NoticiasControlador", noticia);

        if (response.IsSuccessStatusCode)
        {
            await DisplayAlert("OK", "Noticia publicada", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            await DisplayAlert("Error", error, "OK");
        }
    }
}