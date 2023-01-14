namespace Timewise.Code.Helpers;

using DataTypes;
using Exceptions;
using Models;

/// <summary>
/// Klasa pomocnicza, wykonująca pewne obliczenia i operacje na dacie.
/// Jest to klasa statyczna, a więc nie można jej instancjonować.
/// </summary>
public static class DateHelper
{
	/// <summary>
	/// Metoda sprawadzająca, czy dany rok jest przestępny.
	/// </summary>
	/// <param name="year">Rok do sprawdzenia.</param>
	/// <returns>True, jeżeli rok jest przestępny. W przeciwnym wypadku false.</returns>
	public static bool IsLeapYear(int year)
	{
		return (year % 400 == 0) || (year % 4 == 0 && year % 100 != 0);
	}

	/// <summary>
	/// Metoda zwracająca ilość dni w danym miesiącu danego roku.
	/// Uwzględnia, czy przekazany rok jest przestępny.
	/// </summary>
	/// <param name="month">Miesiąc do sprawdzenia.</param>
	/// <param name="year">Rok do sprawdzenia.</param>
	/// <returns>Ilość dni w danym miesiącu danego roku.</returns>
	/// <remarks>Jeżeli przekazany zostanie niepoprawny miesiąc, metoda zwróci wyjątek.</remarks>
	public static int GetAmountOfDaysInMonth(int month, int year)
	{
		return month switch
		{
			1 => 31,
			2 => IsLeapYear(year) ? 29 : 28,
			3 => 31,
			4 => 30,
			5 => 31,
			6 => 30,
			7 => 31,
			8 => 31,
			9 => 30,
			10 => 31,
			11 => 30,
			12 => 31,
			_ => throw new TimeException("Nieprawidłowy miesiąc"),
		};
	}

	/// <summary>
	/// Metoda zwracająca dzień tygodnia dla danej daty.
	/// </summary>
	/// <param name="date">Data, dla której sprawdzamy dzień tygodnia.</param>
	/// <returns>Dzień tygodnia jako typ wyliczeniowy, zaczynający się od poniedziałku.</returns>
	/// <remarks>Jeżeli data jest niepoprawna, metoda zwróci wyjątek.</remarks>
	public static DayOfWeek GetDayOfWeek(Date date)
	{
		if (!date.IsValid)
		{
			throw new TimeException("Podano nieprawidłową datę.");
		}
		
		int day = date.Day;
		int month = date.Month;
		int year = date.Year;

		var dateTime = new System.DateTime(year, month, day);

		int dayOfWeekInt = (int)dateTime.DayOfWeek;

		if (dayOfWeekInt == 0)
		{
			dayOfWeekInt = 6;
		}
		else
		{
			dayOfWeekInt--;
		}

		return (DayOfWeek)dayOfWeekInt;
	}

	/// <summary>
	/// Metoda wykonująca operację odejmowania przekazanej daty od daty aktualnej.
	/// </summary>
	/// <param name="pastTime">Przekazana data w przeszłości.</param>
	/// <returns>Wynik odejmowania - odstęp czasowy między czasem aktualnym, a przekazaną datą.</returns>
	public static Duration TimeSince(Time pastTime)
	{
		return Time.Now - pastTime;
	}

	/// <summary>
	/// Metoda konstruująca obiekt System.DateTime na podstawie daty, przekazanej jako DateTime, oraz czasu, przekazanego jako TimeSpan.
	/// </summary>
	/// <param name="date">Przekazana data jako DateTime.</param>
	/// <param name="time">Przekazany czas jako TimeSpan.</param>
	/// <returns>Obiekt System.DateTime o dacie jak parametr <param name="date" />, oraz czasie jak parametr <param name="time" /></returns>
	public static DateTime Construct(DateTime date, TimeSpan time)
	{
		return new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds);
	}
}