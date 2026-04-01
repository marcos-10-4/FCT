using CTG_App.Modelos;
using System.Net.Http.Json;

namespace CTG_App.Vistas;

public partial class MenuPrincipal : ContentPage
{
    private HttpClient client = new HttpClient
    {
        BaseAddress = new Uri("http://10.0.2.2:5085/") // Cambia a tu URL de API
    };

    private Usuario usuarioLogueado;

    public MenuPrincipal(Usuario usuario)
    {
        InitializeComponent();
        usuarioLogueado = usuario; // Guardamos el usuario que hizo login
        CargarUsuarios();
    }

    // Cargar todos los usuarios excepto el que inició sesión
    private async void CargarUsuarios()
    {
        try
        {
            var usuarios = await client.GetFromJsonAsync<List<Usuario>>("api/UsuariosControlador");

            // Excluir al usuario actual
            usuarios.RemoveAll(u => u.Id == usuarioLogueado.Id);

            UsuariosList.ItemsSource = usuarios;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se pudieron cargar los usuarios: " + ex.Message, "OK");
        }
    }

    // Evento de selección de usuario para abrir chat
    private async void OnUsuarioSeleccionado(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0)
            return;

        var usuarioSeleccionado = e.CurrentSelection[0] as Usuario;
        if (usuarioSeleccionado == null) return;

        // Abrir pantalla de chat pasando IDs correctos
        await Navigation.PushAsync(new Chat(usuarioLogueado.Id, usuarioSeleccionado.Id));

        // Deseleccionar para poder elegir otro usuario después
        ((CollectionView)sender).SelectedItem = null;
    }

    // Botones del menú
    private async void OnPerfilClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Perfil());
    }

    private async void OnPartidoClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegistrarPartido());
    }

    private async void OnRankingClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Ranking());
    }

    // Maneja el tap en el Frame de cada usuario
    private async void OnUsuarioTapped(object sender, EventArgs e)
    {
        var frame = sender as Frame;
        var usuarioSeleccionado = frame?.BindingContext as Usuario;
        if (usuarioSeleccionado == null) return;

        await Navigation.PushAsync(new Chat(usuarioLogueado.Id, usuarioSeleccionado.Id));

        // Deseleccionar cualquier item para permitir futuros taps
        UsuariosList.SelectedItem = null;
    }
    private async void OnNoticiasClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Noticias());
    }
    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}