namespace Timewise.App.Pages;

using Code.Helpers;

public partial class UnixTimePage : ContentPage
{
	public UnixTimePage()
	{
		InitializeComponent();
	}

	private void CheckUnixTimeButton_Clicked(object sender, EventArgs e)
	{
		var eventDate = EventDatePicker.Date;
		var eventTime = EventTimePicker.IsEnabled ? EventTimePicker.Time : System.TimeSpan.Zero;

		var date = new DateTime(eventDate.Year, eventDate.Month, eventDate.Day, eventTime.Hours, eventTime.Minutes, eventTime.Seconds, DateTimeKind.Utc);

		var unixTime = date.ToUnixTime();

		UnixTimeLabel.Text = $"{unixTime + " s"}";
		UnixTimeBorder.IsVisible = true;
	}

	private async void BackButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///converters");
	}

	private void EventTimeCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
	{
		EventTimePicker.IsEnabled = e.Value;
	}
}