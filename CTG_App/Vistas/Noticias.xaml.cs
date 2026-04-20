
using CTG_App.Modelos;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace CTG_App.Vistas;

public partial class Noticias : ContentPage
{
    private HttpClient client = new HttpClient
    {
        BaseAddress = new Uri("http://10.0.2.2:5085/")
    };
    public Noticia NoticiaDestacada { get; set; }

    public ObservableCollection<Noticia> OtrasNoticias { get; set; }
    private Usuario usuarioActual;
    public bool EsAdmin => usuarioActual?.Rol == "Admin";
    public Noticias(Usuario usuario)
    {
        InitializeComponent();

        usuarioActual = usuario;

        if (usuarioActual.Rol == "Admin")
        {
            BtnPublicarNoticia.IsVisible = true;
        }
        CargarNoticias();
        

        BindingContext = this;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarNoticias();
    }
    private async void OnNoticiaTapped(object sender, EventArgs e)
    {
        var frame = sender as Frame;
        var noticia = frame.BindingContext as Noticia;

        if (noticia != null)
        {
            await Navigation.PushAsync(new NoticiaDetalle(noticia));
        }
    }
    private async void OnPublicarNoticiaClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PublicarNoticia());
    }
    private async void CargarNoticias()
    {
        try
        {
            var lista = await client.GetFromJsonAsync<List<Noticia>>("api/NoticiasControlador");

            if (lista == null) return;

            NoticiaDestacada = lista.FirstOrDefault();

            OtrasNoticias = new ObservableCollection<Noticia>(lista.Skip(1));

            BindingContext = null;
            BindingContext = this;
        }
        catch
        {
            await DisplayAlert("Error", "No se pudieron cargar las noticias", "OK");
        }
    }
    private async void OnEliminarNoticia(object sender, EventArgs e)
    {
        var button = sender as Button;
        var noticia = button?.BindingContext as Noticia;

        if (noticia == null)
            return;

        bool confirmar = await DisplayAlert("Eliminar",
            "¿Seguro que quieres eliminar esta noticia?",
            "Sí", "No");

        if (!confirmar)
            return;

        var response = await client.DeleteAsync($"api/NoticiasControlador/{noticia.Id}");

        if (response.IsSuccessStatusCode)
        {
            CargarNoticias();
        }
        else
        {
            await DisplayAlert("Error", "No se pudo eliminar la noticia", "OK");
        }
    }
}