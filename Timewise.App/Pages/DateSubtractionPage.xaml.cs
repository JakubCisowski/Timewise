namespace Timewise.App.Pages;

using Code.Models;

public partial class DateSubtractionPage : ContentPage
{
	public DateSubtractionPage()
	{
		InitializeComponent();
	}

	private async void BackButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///calculations", true);
	}

	private void SubtractDateButton_Clicked(object sender, EventArgs e)
	{
		var event1Date = FirstEventDatePicker.Date;
		var event2Date = SecondEventDatePicker.Date;

		TimeSpan result;

		if (event1Date > event2Date)
		{
			result = event1Date - event2Date;
		}
		else
		{
			result = event2Date - event1Date;
		}

		var resultDate = new Date(result);

		DateSubtractionResult.Text = $"£¹czna iloœæ dni: {result.Days}.\nDni: {resultDate.Day}, Miesi¹ce: {resultDate.Month}, Lata: {resultDate.Year}";
		DateSubtractionBorder.IsVisible = true;
	}
}