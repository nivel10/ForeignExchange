namespace ForeignExchange.Services
{
    using System.Threading.Tasks;

    public class DialogService
    {
        public async Task ShowMessage(string title, string message, string button)
        {
            await App.Current.MainPage.DisplayAlert(title, message, button);
        }
    }
}
