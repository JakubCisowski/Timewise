namespace Timewise.Code.Helpers;

/// <summary>
/// Klasa pomocnicza dokonująca konwersji czasu.
/// Jest to klasa statyczna, a więc nie można jej instancjonować.
/// </summary>
public static class TimeConverter
{
	/// <summary>
	/// Metoda zwracająca czas jako ilość sekund, które upłynęły od 1 stycznia 1970 roku (tzw. czas UNIXowy).
	/// </summary>
	/// <param name="dateTime">Przekazana data jako System.DateTime.</param>
	/// <returns>Czas UNIXowy, a więc ilość sekund, jakie upłynęły od 1 styczna 1970 roku.</returns>
	public static long ToUnixTime(this DateTime dateTime)
	{
		return (long)(dateTime - new DateTime(1970, 1, 1)).TotalSeconds;
	}

	/// <summary>
	/// Metoda zamieniająca czas w ilości sekund na czas jako łączną ilość godzin, minut i sekund.
	/// </summary>
	/// <param name="seconds">Ilość sekund do zamiany.</param>
	/// <returns>Czas w formacie z łączną ilością godzin, minut [0-59] i sekund [0-59].</returns>
	public static (int Hour, int Minute, int Second) SecondsToTime(int seconds)
	{
		int hour = seconds / 3600;
		seconds %= 3600;
		int minute = seconds / 60;
		seconds %= 60;
		int second = seconds;

		return (hour, minute, second);
	}
}