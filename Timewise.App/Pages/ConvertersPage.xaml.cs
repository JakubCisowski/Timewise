namespace Timewise.App.Pages;

public partial class ConvertersPage : ContentPage
{
	public ConvertersPage()
	{
		InitializeComponent();
	}

	private async void UnixTimeButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///unixTime");
	}

	private async void BackButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///MainPage");
	}

	private async void TimeZoneConverterButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///timeZoneConverter");
	}
}