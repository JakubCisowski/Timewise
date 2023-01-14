namespace Timewise.App.Pages;

using Code.Exceptions;
using Code.Models;

public partial class StopwatchPage : ContentPage
{
	private TimeCounterUp _stopwatch;
	private Duration _lastLap;
	private int _counter = 1;

	public StopwatchPage()
	{
		InitializeComponent();
	}

	private void StartStopwatchButton_Clicked(object sender, EventArgs e)
	{
		if (_stopwatch == null || _stopwatch?.IsRunning == false)
		{
			_stopwatch = new TimeCounterUp(Time.Now);
			StopwatchLabel.BindingContext = _stopwatch;
			LapsLabel.Text = "";

			_stopwatch.Start();
			StartStopwatchButton.Text = "Stop";
			
			LapsInfoLabel.IsVisible = false;
			LapsBorder.IsVisible = false;
			LapsLabel.Text = "";
			_counter = 1;
			_lastLap = default;

			StopwatchBorder.IsVisible = true;
		}
		else
		{
			_stopwatch?.Stop();
			StartStopwatchButton.Text = "Start";
		}
	}

	private void ResetStopwatchButton_Clicked(object sender, EventArgs e)
	{
		_stopwatch?.Stop();
		_stopwatch = null;
		StopwatchBorder.IsVisible = false;
		LapsInfoLabel.IsVisible = false;
		LapsBorder.IsVisible = false;
		StartStopwatchButton.Text = "Start";
		LapsLabel.Text = "";

		_counter = 1;
		_lastLap = default;
	}

	private void StopwatchLapButton_Clicked(object sender, EventArgs e)
	{
		if (_stopwatch == null)
		{
			return;
		}

		if (_stopwatch.IsRunning)
		{
			var timeElapsed = _stopwatch.CreateTimestamp();
			var timeSinceLastLap = timeElapsed;

			if (_lastLap != default)
			{
				try
				{
					timeSinceLastLap -= _lastLap;
				}
				catch (TimeException)
				{
					return;
				}
			}

			LapsLabel.Text += $"{_counter++}.\t{timeSinceLastLap.Hours:00}:{timeSinceLastLap.Minutes:00}:{timeSinceLastLap.Seconds:00}:{timeSinceLastLap.Milliseconds:000}{Environment.NewLine}";

			_lastLap = timeElapsed;

			LapsBorder.IsVisible = true;
			LapsInfoLabel.IsVisible = true;
		}
	}

	private async void BackButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///MainPage", true);
	}
}