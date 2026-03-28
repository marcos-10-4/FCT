using System.Net.Http.Json;
using System.Collections.ObjectModel;
using CTG_App.Modelos;

namespace CTG_App.Vistas;
public partial class Chat : ContentPage
{
    private int UsuarioActualId;
    private int ReceptorId;
    private HttpClient client;

    public ObservableCollection<Mensaje> Mensajes { get; set; }

    public Chat(int usuarioActualId, int receptorId)
    {
        InitializeComponent();

        this.UsuarioActualId = usuarioActualId;
        this.ReceptorId = receptorId;

        client = new HttpClient
        {
            BaseAddress = new Uri("http://10.0.2.2:5085/") // Cambiar según pruebas en móvil físico
        };

        Mensajes = new ObservableCollection<Mensaje>();
        BindingContext = this;

        CargarMensajes();

        // Refrescar cada 3 segundos
        Device.StartTimer(TimeSpan.FromSeconds(3), () =>
        {
            CargarMensajes();
            return true; // true para repetir
        });
    }

    private async void CargarMensajes()
    {
        try
        {
            var lista = await client.GetFromJsonAsync<List<Mensaje>>($"api/ChatControlador/{UsuarioActualId}/{ReceptorId}");

            if (lista == null) return;

            Mensajes.Clear();
            foreach (var m in lista)
            {
                m.EsMio = m.EmisorId == UsuarioActualId;
                Mensajes.Add(m);
            }

            if (Mensajes.Any())
                MensajesList.ScrollTo(Mensajes.Last(), position: ScrollToPosition.End, animate: false);
        }
        catch (Exception ex)
        {
            // Para pruebas puedes comentar esta línea
            // await DisplayAlert("Error", $"No se pudieron cargar los mensajes: {ex.Message}", "OK");
        }
    }

    private async void OnEnviarClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(MensajeEntry.Text))
            return;

        var mensaje = new Mensaje
        {
            EmisorId = UsuarioActualId,
            ReceptorId = ReceptorId,
            Texto = MensajeEntry.Text,
            Fecha = DateTime.Now
        };

        try
        {
            var response = await client.PostAsJsonAsync("api/ChatControlador", mensaje);
            if (response.IsSuccessStatusCode)
            {
                mensaje.EsMio = true;
                Mensajes.Add(mensaje);
                MensajeEntry.Text = string.Empty;
                MensajesList.ScrollTo(Mensajes.Last(), position: ScrollToPosition.End, animate: true);
            }
            else
            {
                await DisplayAlert("Error", "No se pudo enviar el mensaje", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Fallo al enviar: {ex.Message}", "OK");
        }
    }
}