using CTG_App.Modelos;
using System.Collections.ObjectModel;

namespace CTG_App.Vistas;

public partial class Noticias : ContentPage
{
    public Noticia NoticiaDestacada { get; set; }

    public ObservableCollection<Noticia> OtrasNoticias { get; set; }
    public Noticias()
    {
        InitializeComponent();

        NoticiaDestacada = new Noticia
        {
            Titulo = "Gran torneo del club",
            Fecha = "15/06/2026",
            Contenido = "Este mes se celebrará el gran torneo anual del club con premios para los mejores jugadores.",
            Imagen = "tenis.jpg"
        };

        OtrasNoticias = new ObservableCollection<Noticia>
        {
            new Noticia
            {
                Titulo = "Nueva pista disponible",
                Fecha = "10/06/2026",
                Contenido = "El club ha inaugurado una nueva pista para los socios.",
                Imagen = "pista.jpg"
            },
            new Noticia
            {
                Titulo = "Clases para principiantes",
                Fecha = "05/06/2026",
                Contenido = "Se abrirán nuevas clases para jugadores que empiezan.",
                Imagen = "clases.jpg"
            }
        };

        BindingContext = this;
    }

}