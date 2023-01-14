namespace Timewise.App.Pages;

using Code.Helpers;

public partial class TimeZoneConverterPage : ContentPage
{
	public TimeZoneConverterPage()
	{
		InitializeComponent();

		FirstTimeZonePicker.ItemsSource = TimeZoneInfo.GetSystemTimeZones();
		SecondTimeZonePicker.ItemsSource = TimeZoneInfo.GetSystemTimeZones();
	}

	private async void BackButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///converters");
	}

	private void CheckTimeInOtherTimeZoneButton_Clicked(object sender, EventArgs e)
	{
		var eventTimeSpan = EventTimePicker.Time;
		var eventDateTime = new DateTime(1, 1, 1, eventTimeSpan.Hours, eventTimeSpan.Minutes, eventTimeSpan.Seconds);

		var firstTimeZone = FirstTimeZonePicker.SelectedItem as TimeZoneInfo;
		var secondTimeZone = SecondTimeZonePicker.SelectedItem as TimeZoneInfo;

		if (firstTimeZone == null || secondTimeZone == null)
		{
			return;
		}

		var converted = TimeZoneConverter.ConvertDateTimeToTimeZone(eventDateTime, firstTimeZone, secondTimeZone);

		SecondTimeZoneTimeLabel.Text = converted.ToString("T");
		SecondTimeZoneBorder.IsVisible = true;
	}
}