namespace Timewise.App.Pages;

using Code.Helpers;
using Code.Models;

public partial class DayOfWeekPage : ContentPage
{
	public DayOfWeekPage()
	{
		InitializeComponent();
	}

	private async void BackButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///calculations", true);
	}

	private void CheckDayOfWeekButton_Clicked(object sender, EventArgs e)
	{
		var dateTime = EventDatePicker.Date;
		var date = new Date(dateTime.Day, dateTime.Month, dateTime.Year);

		var dayOfWeek = DateHelper.GetDayOfWeek(date);

		DayOfWeekResult.Text = DayOfWeekFormatter.DayOfWeekToString(dayOfWeek);
		DayOfWeekBorder.IsVisible = true;
	}
}