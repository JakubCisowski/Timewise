namespace Timewise.Code.Helpers;

using System.Reflection;
using System.Text;

/// <summary>
/// Klasa pomocnicza, wykonująca operacje na plikach.
/// Jest to klasa statyczna, a więc nie można jej instancjonować.
/// </summary>
public static class FileHelper
{
	/// <summary>
	/// Metoda wczytująca z pliku Imieniny.csv imieniny i zwracająca dzisiejsze imieniny.
	/// </summary>
	/// <returns>Napis zawierający imiona, które obchodzą dzisiaj imieniny, oddzielone przecinkami.</returns>
	public static string GetNameDayNames()
	{
		var assembly = Assembly.GetExecutingAssembly();

		var stream = assembly.GetManifestResourceStream("Timewise.Code.Data.Imieniny.csv");

		string todayNameDays;
		
		using (var reader = new StreamReader(stream))
		{
			var lines = reader.ReadToEnd();
			
			var todayString = DateTime.Now.ToString("MM-dd");
			
			todayNameDays = lines.Split(new[] { Environment.NewLine }, StringSplitOptions.None)
				.Where(line => line.StartsWith(todayString))
				.Select(line => line.Split(';')[1])
				.LastOrDefault();
		}

		return todayNameDays;
	}

	/// <summary>
	/// Metoda wczytująca z pliku Swieta.csv święta i zwracająca dzisiejsze święta.
	/// W przypadku, gdy w danym dniu nie ma żadnych świąt, zwracany jest pusty napis.
	/// </summary>
	/// <returns>Święta obchodzone dzisiaj, lub pusty napis, gdy nie ma dziś żadnych świąt.</returns>
	public static string GetHolidays()
	{
		var assembly = Assembly.GetExecutingAssembly();

		var stream = assembly.GetManifestResourceStream("Timewise.Code.Data.Swieta.csv");

		string todayHolidays;

		using (var reader = new StreamReader(stream))
		{
			var lines = reader.ReadToEnd();

			var todayString = DateTime.Now.ToString("dd.MM.yyyy");

			var todayHolidaysLines = lines.Split(new[] { Environment.NewLine }, StringSplitOptions.None)
				.Where(line => line.Contains(todayString));
			
			var sb = new StringBuilder();
			
			foreach (var line in todayHolidaysLines)
			{
				sb.Append(line.Split(';').First());
				sb.Append(", ");
			}

			todayHolidays = sb.ToString();
		}

		return todayHolidays;
	}
}