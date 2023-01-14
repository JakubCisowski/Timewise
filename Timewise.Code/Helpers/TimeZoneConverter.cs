namespace Timewise.Code.Helpers;

/// <summary>
/// Klasa pomocnicza dokonująca konwersji między strefami czasowymi.
/// Jest to klasa statyczna, a więc nie można jej instancjonować.
/// </summary>
public static class TimeZoneConverter
{
	/// <summary>
	/// Metoda konwertująca czas z jednej strefy czasowej do drugiej.
	/// </summary>
	/// <param name="dateTime">Czas przekazany w strefie czasowej <param name="from" /></param>
	/// <param name="from"></param>
	/// <param name="to"></param>
	/// <returns></returns>
	public static DateTime ConvertDateTimeToTimeZone(DateTime dateTime, TimeZoneInfo from, TimeZoneInfo to)
	{
		return from.Equals(to) ? dateTime : TimeZoneInfo.ConvertTime(dateTime, from, to);
	}
}