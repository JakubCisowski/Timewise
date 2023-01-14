namespace Timewise.App.Pages;

using System.Globalization;

public partial class WeekNumberPage : ContentPage
{
	public WeekNumberPage()
	{
		InitializeComponent();
	}

	private async void BackButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///calculations", true);
	}

	private void CheckWeekNumberButton_Clicked(object sender, EventArgs e)
	{
		var dateTime = EventDatePicker.Date;

		var cultureInfo = CultureInfo.CurrentCulture;

		var weekOfYear = cultureInfo.Calendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

		WeekNumberResult.Text = $"Numer tygodnia: {weekOfYear}";
		WeekNumberBorder.IsVisible = true;
	}
}