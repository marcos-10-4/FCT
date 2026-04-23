using System.Net.Http.Json;
using CTG_App.Modelos;

namespace CTG_App.Vistas;
public partial class RegistrarPartido : ContentPage
{
    HttpClient client = new HttpClient
    {
        BaseAddress = new Uri("http://10.0.2.2:5085/")
    };
    private Usuario usuarioActual;
    List<Usuario> usuarios;
    public RegistrarPartido(Usuario usuario)
    {
        InitializeComponent();

        usuarioActual = usuario;

        if (usuarioActual.Rol != "Entrenador")
        {
            DisplayAlert("Acceso denegado",
                "Solo los entrenadores pueden registrar partidos",
                "OK");

            // No se puede await en el constructor; despachar navegación sin await
            Navigation.PopAsync();
        }
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
        
        if (jugador1 == null || jugador2 == null || ganador == null)
        {
            await DisplayAlert("Error", "Selecciona los jugadores y el ganador", "OK");
            return;
        }
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
    private void OnJugadoresSeleccionados(object sender, EventArgs e)
    {
        if (Jugador1Picker.SelectedItem == null || Jugador2Picker.SelectedItem == null)
        {
            return;
        }

        var jugador1 = Jugador1Picker.SelectedItem as Usuario;
        var jugador2 = Jugador2Picker.SelectedItem as Usuario;

        if (jugador1.Id == jugador2.Id)
        {
            DisplayAlert("Error", "No puedes seleccionar el mismo jugador", "OK");
            Jugador2Picker.SelectedItem = null;
            return;
        }

        GanadorPicker.ItemsSource = new List<Usuario>
        {
            jugador1,
            jugador2
        };

        GanadorPicker.ItemDisplayBinding = new Binding("Nombre");
    }
}