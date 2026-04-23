using CTG_App.Modelos;
using CTG_App.Servicios;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

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
            // Leer la respuesta como LoginRespuesta y usar el usuario incluido
            var datos = await response.Content.ReadFromJsonAsync<LoginRespuesta>();
            if (datos != null)
            {
                var token = datos.Token;
                Sesion.Token = token;
                // establecer token en el cliente actual (si se usa más tarde)
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var usuario = datos.Usuario;
                if (usuario != null)
                {
                    Sesion.UsuarioId = usuario.Id;
                    await Navigation.PushAsync(new MenuPrincipal(usuario));
                    EmailEntry.Text = string.Empty;
                    PasswordEntry.Text = string.Empty;
                    return;
                }
            }

            await DisplayAlert("Error", "Respuesta inválida del servidor", "OK");
            return;
        }
        else
        {
            await DisplayAlert("Error", "Email o contraseña incorrectos", "OK");
        }
        EmailEntry.Text = string.Empty;
        PasswordEntry.Text = string.Empty;
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Registro());
    }

}