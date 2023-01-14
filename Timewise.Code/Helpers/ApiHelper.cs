namespace Timewise.Code.Helpers;

using System.Text.Json;
using DataTypes;
using Exceptions;

/// <summary>
/// Klasa pomocnicza, wykonująca zapytania do API i przetwarzająca otrzymane odpowiedzi.
/// Jest to klasa statyczna, a więc nie można jej instancjonować.
/// </summary>
public static class ApiHelper
{
	/// <summary>
	/// Metoda wykonująca zapytanie do API o informacje danej lokacji, i zwracająca te informacje pod postacią obiektu klasy WeatherInfo.
	/// </summary>
	/// <param name="locationName">Miejsce do sprawdzenia, domyślnie Warszawa.</param>
	/// <returns>Informacje o danej lokacji, takie jak: czas, temperatura, faza księżyca, itp.</returns>
	public static async Task<WeatherInfo> GetWeather(string locationName = "Warsaw")
	{
		using var client = new HttpClient();
		var response = await client.GetAsync($"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/{locationName}?aggregateHours=24&unitGroup=metric&key=W4Q5PKCJJEGMYWWMNMNF2W34Z");

		if (!response.IsSuccessStatusCode)
		{
			throw new ApiException("Błąd pobierania danych dla danej lokacji.");
		}

		var resultJson = await response.Content.ReadAsStringAsync();

		var resultParsed = (JsonElement)JsonSerializer.Deserialize(resultJson, typeof(JsonElement))!;

		var description = resultParsed.GetProperty("description").ToString();
		var currentConditions = resultParsed.GetProperty("currentConditions");

		var dateTime = DateTime.Parse(currentConditions.GetProperty("datetime").ToString());

		var temperature = currentConditions.GetProperty("temp").GetSingle();
		var feelsLike = currentConditions.GetProperty("feelslike").GetSingle();
		var humidity = currentConditions.GetProperty("humidity").GetSingle();
		var precipitation = currentConditions.GetProperty("precip").GetSingle();
		var snow = currentConditions.GetProperty("snow").GetSingle();
		var windSpeed = currentConditions.GetProperty("windspeed").GetSingle();
		var pressure = currentConditions.GetProperty("pressure").GetSingle();
			
		var sunriseText = currentConditions.GetProperty("sunrise").GetString();
		var sunriseDateParsed = DateTime.TryParse(sunriseText, out DateTime sunriseDate);

		var sunsetText = currentConditions.GetProperty("sunset").GetString();
		var sunsetDateParsed = DateTime.TryParse(sunsetText, out DateTime sunsetDate);

		var moonphase = currentConditions.GetProperty("moonphase").GetSingle();

		var weatherInfo = new WeatherInfo()
		{
			LocalDateTime = dateTime,
			Description = description,
			Temperature = temperature,
			FeelsLike = feelsLike,
			Humidity = humidity,
			Precipitation = precipitation,
			Snow = snow,
			WindSpeed = windSpeed,
			Pressure = pressure,
			Sunrise = sunriseDate,
			Sunset = sunsetDate,
			Moonphase = moonphase
		};

		return weatherInfo;
	}
}