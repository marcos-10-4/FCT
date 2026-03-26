namespace CTG_App.Vistas;

public partial class MenuPrincipal : ContentPage
{
    public MenuPrincipal()
    {
        InitializeComponent();
    }

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
    private async void OnChatClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Chat());
    }
    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}