using CTG_App.Modelos;
using CTG_App.Servicios;
using System.Net.Http.Json;

namespace CTG_App.Vistas;

public partial class Perfil : ContentPage
{
    HttpClient client = new HttpClient
    {
        BaseAddress = new Uri("http://10.0.2.2:5085/")
    };

    public Perfil()
    {
        InitializeComponent();
        CargarPerfil();
    }

    private async void CargarPerfil()
    {
        try
        {
            var usuario = await client.GetFromJsonAsync<Usuario>($"api/UsuariosControlador/{Sesion.UsuarioId}");

            NombreLabel.Text = $"Nombre: {usuario.Nombre}";
            EmailLabel.Text = $"Email: {usuario.Email}";
            PuntosLabel.Text = $"Puntos: {usuario.Puntos}";
            PartidosLabel.Text = $"Partidos: {usuario.PartidosJugados}";
            VictoriasLabel.Text = $"Victorias: {usuario.Victorias}";
            DerrotasLabel.Text = $"Derrotas: {usuario.Derrotas}";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}