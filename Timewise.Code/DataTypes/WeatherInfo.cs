namespace Timewise.Code.DataTypes;

using System.Text;

/// <summary>
/// Klasa reprezentująca odpowiedź otrzymaną z API.
/// Zawiera jedynie interesujące nas właściwości, a niektóre z otrzymanych w odpowiedzi ignoruje.
/// </summary>
public class WeatherInfo
{
	/// <summary>
	/// Opis pogody.
	/// </summary>
	public string Description { get; init; }
	
	/// <summary>
	/// Data i czas w lokalnej strefie czasowej.
	/// </summary>
	public DateTime LocalDateTime { get; init; }
	
	/// <summary>
	/// Temperatura powietrza.
	/// </summary>
	public float Temperature { get; init; }
	
	/// <summary>
	/// Temperatura odczuwalna.
	/// </summary>
	public float FeelsLike { get; init; }
	
	/// <summary>
	/// Wilgotność powietrza.
	/// </summary>
	public float Humidity { get; init; }
	
	/// <summary>
	/// Ilość opadu atmosferycznego.
	/// </summary>
	public float Precipitation { get; init; }
	
	/// <summary>
	/// Ilość śniegu.
	/// </summary>
	public float Snow { get; init; }
	
	/// <summary>
	/// Prędkość wiatru.
	/// </summary>
	public float WindSpeed { get; init; }
	
	/// <summary>
	/// Ciśnienie atmosferyczne.
	/// </summary>
	public float Pressure { get; init; }
	
	/// <summary>
	/// Czas wschodu Słońca.
	/// </summary>
	public DateTime Sunrise { get; init; }
	
	/// <summary>
	/// Czas zachodu Słońca.
	/// </summary>
	public DateTime Sunset { get; init; }
	
	/// <summary>
	/// Faza Księżyca.
	/// </summary>
	public float Moonphase { get; init; }

	public override string ToString()
	{
		var sb = new StringBuilder();

		sb.AppendLine("Opis: " + Description);
		sb.AppendLine("Czas pomiaru: " + LocalDateTime);
		sb.AppendLine("Temperatura: " + Temperature);
		sb.AppendLine("Odczuwalna: " + FeelsLike);
		sb.AppendLine("Humidity: " + Humidity);
		sb.AppendLine("Opady: " + Precipitation);
		sb.AppendLine("Śnieg: " + Snow);
		sb.AppendLine("Prędkość wiatru: " + WindSpeed);
		sb.AppendLine("Ciśnienie: " + Pressure);
		sb.AppendLine("Wschód Słońca: " + Sunrise);
		sb.AppendLine("Zachód Słońca: " + Sunset);
		sb.AppendLine("Faza Księżyca: " + Moonphase);

		return sb.ToString();
	}
}