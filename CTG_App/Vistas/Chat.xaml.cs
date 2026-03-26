using CTG_App.Modelos;
using System.Net.Http.Json;

namespace CTG_App.Vistas;

public partial class Chat : ContentPage
{
    HttpClient client = new HttpClient
    {
        BaseAddress = new Uri("http://10.0.2.2:5085/")
    };

    string usuarioActual = "Jugador";

    public Chat()
    {
        InitializeComponent();
        CargarMensajes();

        // Auto refresco cada 3 segundos
        Device.StartTimer(TimeSpan.FromSeconds(3), () =>
        {
            CargarMensajes();
            return true;
        });
    }

    private async void CargarMensajes()
    {
        try
        {
            var mensajes = await client.GetFromJsonAsync<List<Mensaje>>("api/Chat");
            MensajesList.ItemsSource = mensajes;
        }
        catch
        {
            // evitar que crashee
        }
    }

    private async void OnEnviarClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(MensajeEntry.Text))
            return;

        var mensaje = new
        {
            Emisor = usuarioActual,
            Texto = MensajeEntry.Text
        };

        await client.PostAsJsonAsync("api/Chat", mensaje);

        MensajeEntry.Text = "";

        CargarMensajes();
    }
}