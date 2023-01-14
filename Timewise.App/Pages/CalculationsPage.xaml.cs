namespace Timewise.App.Pages;

public partial class CalculationsPage : ContentPage
{
	public CalculationsPage()
	{
		InitializeComponent();
	}

	private async void BackButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///MainPage");
	}

	private async void DateSubtractionPageButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///dateSubtraction");
	}

	private async void DayOfWeekPageButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///dayOfWeek");
	}

	private async void LeapYearPageButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///leapYear");
	}

	private async void WeekNumberPageButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///weekNumber");
	}
}