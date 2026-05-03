using CTG_App.Modelos;
using CTG_App.Servicios;
using System.Net.Http.Json;

namespace CTG_App.Vistas;

public partial class MenuPrincipal : ContentPage
{
    private HttpClient client = new HttpClient
    {
        BaseAddress = new Uri("http://10.0.2.2:5085/") // Cambia a tu URL de API
    };

    private Usuario usuarioLogueado;

    // La visibilidad del botón Eliminar se controla por elemento (Usuario.MostrarEliminar)

    public MenuPrincipal(Usuario usuario)
    {
        InitializeComponent();
        usuarioLogueado = usuario; // Asignar antes de usar
        client.DefaultRequestHeaders.Authorization =
        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Sesion.Token);
        if (usuarioLogueado.Rol != "Entrenador")
        {
            BtnRegistrarPartido.IsVisible = false;
        }
        CargarUsuarios();
    }

    // cache local de usuarios para búsquedas
    private List<Usuario> _usuariosCache = new List<Usuario>();

    private void ApplyFilter(string filtro)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filtro))
            {
                UsuariosList.ItemsSource = _usuariosCache;
                return;
            }

            var f = filtro.Trim().ToLower();
            var filtrados = _usuariosCache.Where(u => (u.Nombre ?? string.Empty).ToLower().Contains(f)
                                                   || (u.Email ?? string.Empty).ToLower().Contains(f)).ToList();
            UsuariosList.ItemsSource = filtrados;
        }
        catch
        {
            // ignorar errores de filtrado
        }
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        ApplyFilter(e.NewTextValue);
    }

    private void OnSearchButtonPressed(object sender, EventArgs e)
    {
        ApplyFilter(UsuariosSearchBar.Text);
    }

    // Cargar todos los usuarios excepto el que inició sesión
    private async void CargarUsuarios()
    {
        try
        {
            var usuarios = await client.GetFromJsonAsync<List<Usuario>>("api/UsuariosControlador");

            // Excluir al usuario actual
            usuarios.RemoveAll(u => u.Id == usuarioLogueado.Id);

            // Marcar si se muestra el botón eliminar por elemento según el rol del usuario logueado
            bool mostrarEliminarParaTodos = usuarioLogueado.Rol?.ToLower() == "admin";
            foreach (var u in usuarios)
            {
                u.MostrarEliminar = mostrarEliminarParaTodos;
            }

            // cachear y mostrar
            _usuariosCache = usuarios;
            UsuariosList.ItemsSource = _usuariosCache;
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
        await Navigation.PushAsync(new RegistrarPartido(usuarioLogueado));
    }

    private async void OnRankingClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Ranking());
    }

    // Maneja el tap en el Frame de cada usuario
    private async void OnUsuarioTapped(object sender, EventArgs e)
    {
        Usuario usuarioSeleccionado = null;

        if (sender is BindableObject bo)
        {
            usuarioSeleccionado = bo.BindingContext as Usuario;
        }

        if (usuarioSeleccionado == null)
        {
            return;
        }

        await Navigation.PushAsync(new Chat(usuarioLogueado.Id, usuarioSeleccionado.Id));

        // Deseleccionar cualquier item para permitir futuros taps
        UsuariosList.SelectedItem = null;
    }
    private async void OnNoticiasClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Noticias(usuarioLogueado));
    }
    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
    private async void OnEliminarUsuarioClicked(object sender, EventArgs e)
    {
        var boton = sender as Button;
        var usuario = boton?.BindingContext as Usuario;
        if (usuarioLogueado.Rol?.ToLower() != "admin")
        {
            boton.IsVisible = false;
        }
        if (usuario == null) return;

        bool confirmar = await DisplayAlert("Confirmar", "¿Eliminar usuario?", "Sí", "No");

        if (!confirmar) return;

        var response = await client.DeleteAsync($"api/UsuariosControlador/{usuario.Id}");

        if (response.IsSuccessStatusCode)
        {
            await DisplayAlert("OK", "Usuario eliminado", "OK");
            CargarUsuarios(); // refrescar lista
        }
        else
        {
            await DisplayAlert("Error", "No tienes permisos o fallo en API", "OK");
        }
    }
}