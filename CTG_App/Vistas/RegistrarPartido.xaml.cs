using System.Net.Http.Json;
using CTG_App.Modelos;

namespace CTG_App.Vistas;
public partial class RegistrarPartido : ContentPage
{
    HttpClient client = new HttpClient
    {
        BaseAddress = new Uri("http://10.0.2.2:5085/")
    };

    List<Usuario> usuarios;

    public RegistrarPartido()
    {
        InitializeComponent();
        CargarUsuarios();
    }

    private async void CargarUsuarios()
    {
        try
        {
            usuarios = await client.GetFromJsonAsync<List<Usuario>>("api/UsuariosControlador");

            if (usuarios == null)
            {
                await DisplayAlert("Error", "No se cargaron usuarios", "OK");
                return;
            }

            Jugador1Picker.ItemsSource = usuarios;
            Jugador2Picker.ItemsSource = usuarios;
            GanadorPicker.ItemsSource = usuarios;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error API", ex.Message, "OK");
        }
    }
    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        var jugador1 = (Usuario)Jugador1Picker.SelectedItem;
        var jugador2 = (Usuario)Jugador2Picker.SelectedItem;
        var ganador = (Usuario)GanadorPicker.SelectedItem;

        var partido = new
        {
            jugador1Id = jugador1.Id,
            jugador2Id = jugador2.Id,
            ganadorId = ganador.Id
        };

        var response = await client.PostAsJsonAsync("api/PartidosControlador/partido", partido);

        if (response.IsSuccessStatusCode)
        {
            await DisplayAlert("OK", "Partido registrado", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Error", "No se pudo guardar", "OK");
        }
    }
}