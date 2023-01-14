namespace Timewise.App.Pages;

using System.Globalization;
using Code.Exceptions;
using Code.Helpers;

public partial class LocationInfoPage : ContentPage
{
	public LocationInfoPage()
	{
		InitializeComponent();
	}

	private async void BackButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///MainPage");
	}

	private async void CheckButton_Clicked(object sender, EventArgs e)
	{
		var location = LocationEntry.Text;

		try
		{
			var locationInfo = await ApiHelper.GetWeather(location);

			var nameDayInfo = FileHelper.GetNameDayNames();
			nameDayInfo = nameDayInfo.Replace(",", ", ");

			var holidayInfo = FileHelper.GetHolidays();
			
			LocalDateTimeEntry.Text = locationInfo.LocalDateTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.CurrentUICulture);
			TemperatureEntry.Text = locationInfo.Temperature.ToString(CultureInfo.CurrentUICulture);
			FeelsLikeEntry.Text = locationInfo.FeelsLike.ToString(CultureInfo.CurrentUICulture);
			HumidityEntry.Text = locationInfo.Humidity.ToString(CultureInfo.CurrentUICulture);
			PressureEntry.Text = locationInfo.Pressure.ToString(CultureInfo.CurrentUICulture);
			PrecipitationEntry.Text = locationInfo.Precipitation.ToString(CultureInfo.CurrentUICulture);
			SnowEntry.Text = locationInfo.Snow.ToString(CultureInfo.CurrentUICulture);
			WindSpeedEntry.Text = locationInfo.WindSpeed.ToString(CultureInfo.CurrentUICulture);
			SunriseEntry.Text = locationInfo.Sunrise.ToString("HH:mm:ss", CultureInfo.CurrentUICulture);
			SunsetEntry.Text = locationInfo.Sunset.ToString("HH:mm:ss", CultureInfo.CurrentUICulture);
			MoonphaseEntry.Text = locationInfo.Moonphase.ToString(CultureInfo.CurrentUICulture);

			foreach (var layout in InfoGrid.Children.OfType<VerticalStackLayout>())
			{
				layout.IsVisible = true;
			}
			
			DescriptionLabel.Text = locationInfo.Description;
			DescriptionBorder.IsVisible = true;

			if (!string.IsNullOrEmpty(holidayInfo))
			{
				HolidaysLabel.Text = holidayInfo;
				HolidaysBorder.IsVisible = true;
			}
			
			if (!string.IsNullOrEmpty(nameDayInfo))
			{
				NameDayLabel.Text = nameDayInfo;
				NameDayBorder.IsVisible = true;
			}
		}
		catch (ApiException ex)
		{
			await DisplayAlert("B³¹d przy uzyskiwaniu danych z API", ex.Message, "OK");
		}
		catch (Exception ex)
		{
			await DisplayAlert("B³¹d", ex.Message, "OK");
		}
	}
}