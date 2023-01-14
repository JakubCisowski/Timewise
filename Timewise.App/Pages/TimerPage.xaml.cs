namespace Timewise.App.Pages;

using Code.Models;

public partial class TimerPage : ContentPage
{
	private TimeCounterDown _time;

	public TimerPage()
	{
		InitializeComponent();
	}

	private async void StartTimerButton_Clicked(object sender, EventArgs e)
	{
		if (_time == null)
		{
			bool hourParsed = int.TryParse(HoursEntry.Text, out int hour);
			bool minuteParsed = int.TryParse(MinutesEntry.Text, out int minute);
			bool secondParsed = int.TryParse(SecondsEntry.Text, out int second);

			if ((hourParsed & minuteParsed & secondParsed) == false)
			{
				await DisplayAlert("B³¹d", "Liczba godzin, minut i sekund musi byæ liczbami naturalnymi.", "OK");
				return;
			}

			var today = Time.Now;
			
			_time = new TimeCounterDown(new Duration(today.Date.Day, today.Date.Month, today.Date.Year, hour, minute, second, 0), default);

			if (!_time.IsValid)
			{
				await DisplayAlert("B³¹d", "Liczba godzin musi byæ w przedziale 0-23, liczba minut w przedziale 0-59, a liczba sekund w przedziale 0-59.", "OK");
				return;
			}

			TimerLabel.BindingContext = _time;
		}
		
		ToggleTimeEntry(_time.IsRunning);
	}

	private void ToggleTimeEntry(bool currentTimerState)
	{
		if (!currentTimerState)
		{
			_time?.Start();
			StartTimerButton.Text = "Pauza";

			HoursEntry.IsEnabled = false;
			MinutesEntry.IsEnabled = false;
			SecondsEntry.IsEnabled = false;

			TimerBorder.IsVisible = true;
		}
		else
		{
			_time?.Stop();
			StartTimerButton.Text = "Start";
		}
	}

	private void ResetTimerButton_Clicked(object sender, EventArgs e)
	{
		ToggleTimeEntry(true);

		_time = null;
		HoursEntry.Text = "";
		MinutesEntry.Text = "";
		SecondsEntry.Text = "";

		TimerBorder.IsVisible = false;
		
		HoursEntry.IsEnabled = true;
		MinutesEntry.IsEnabled = true;
		SecondsEntry.IsEnabled = true;
	}

	private async void BackButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///MainPage", true);
	}
}