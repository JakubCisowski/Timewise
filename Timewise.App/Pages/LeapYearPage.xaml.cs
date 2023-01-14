namespace Timewise.App.Pages;

using Code.Helpers;

public partial class LeapYearPage : ContentPage
{
	public LeapYearPage()
	{
		InitializeComponent();
	}

	private async void BackButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///calculations");
	}

	private async void CalculateLeapYearButton_Clicked(object sender, EventArgs e)
	{
		string yearInput = YearEntry.Text?.Trim();
		bool isNotEmpty = !string.IsNullOrEmpty(yearInput);
		bool isNumber = int.TryParse(yearInput, out int year);
		bool isPositive = year > 0;

		if (isNotEmpty && isNumber && isPositive)
		{
			LeapYearLabel.Text = $"{year} to rok {(DateHelper.IsLeapYear(year) ? "przestêpny" : "nieprzestêpny")}";
			LeapYearBorder.IsVisible = true;
			LeapYearLabel.IsVisible = true;
		}
		else
		{
			await DisplayAlert("B³¹d", "Nale¿y podaæ poprawny rok.", "OK");
		}
	}
}