using System.Text;
using System.Text.Json;

namespace CTG_App.Vistas;

public partial class Registro : ContentPage
{
    public Registro()
    {
        InitializeComponent();
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        var usuario = new
        {
            nombre = NombreEntry.Text,
            email = EmailEntry.Text,
            passwordHash = PasswordEntry.Text,
            rol = "Socio",
            puntos = 0,
            partidosJugados = 0,
            victorias = 0,
            derrotas = 0
        };

        var json = JsonSerializer.Serialize(usuario);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpClient client = new HttpClient();

        var response = await client.PostAsync("http://10.0.2.2:5085/api/UsuariosControlador/registro", content);

        if (response.IsSuccessStatusCode)
        {
            await DisplayAlert("Éxito", "Usuario registrado correctamente", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Error", "No se pudo registrar", "OK");
        }
    }
}