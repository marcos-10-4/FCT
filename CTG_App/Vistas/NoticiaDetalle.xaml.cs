using CTG_App.Modelos;
using Microsoft.Maui.Controls;

namespace CTG_App.Vistas;

public partial class NoticiaDetalle : ContentPage
{
    public NoticiaDetalle(Noticia noticia)
    {
        InitializeComponent();
        BindingContext = noticia;
    }
}