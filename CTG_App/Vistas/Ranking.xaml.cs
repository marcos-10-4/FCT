using CTG_App.Servicios;

namespace CTG_App.Vistas;

public partial class Ranking : ContentPage
{
    ApiServicio servicio = new ApiServicio();
    public Ranking()
	{
		InitializeComponent();
        CargarRanking();
    }
    private async void CargarRanking()
    {
        try
        {
            var ranking = await servicio.GetRankingAsync();
            RankingList.ItemsSource = ranking;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se pudo cargar el ranking: " + ex.Message, "OK");
        }
    }

}