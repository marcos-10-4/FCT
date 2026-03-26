using CTG_App.Modelos;
using CTG_App.Servicios;
using System.Net.Http.Json;

namespace CTG_App.Vistas;

public partial class InicioSesion : ContentPage
{
    HttpClient client = new HttpClient();

    public InicioSesion()
    {
        InitializeComponent();
        client.BaseAddress = new Uri("http://10.0.2.2:5085/");
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var loginData = new
        {
            email = EmailEntry.Text,
            password = PasswordEntry.Text
        };

        var response = await client.PostAsJsonAsync("api/UsuariosControlador/login", loginData);

        if (response.IsSuccessStatusCode)
        {
            var usuario = await response.Content.ReadFromJsonAsync<Usuario>();

            Sesion.UsuarioId = usuario.Id;
            await Navigation.PushAsync(new MenuPrincipal());
        }
        else
        {
            await DisplayAlert("Error", "Email o contraseña incorrectos", "OK");
        }
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Registro());
    }
}