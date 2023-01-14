namespace Timewise.Code.Models;

using Helpers;
using Interfaces;

/// <summary>
/// Klasa reprezentująca datę, powiązana z klasą Time.
/// </summary>
public class Date : IValidatable
{
	/// <summary>
	/// Dzień w czasie.
	/// </summary>
	public int Day { get; set; }
	
	/// <summary>
	/// Miesiąc w czasie.
	/// </summary>
	public int Month { get; set; }
	
	/// <summary>
	/// Rok w czasie.
	/// </summary>
	public int Year { get; set; }

	/// <summary>
	/// Właściwość zwracająca pustą datę.
	/// </summary>
	public static Date Empty => new Date(0, 0, 0);

	/// <summary>
	/// Implementacja metody udostępnianej przez interfejs IValidatable.
	/// Zwraca true, jeżeli czas jest poprawny, w przeciwnym wypadku false.
	/// </summary>
	public bool IsValid
	{
		get
		{
			bool isYearValid = Year > 0;
			bool isMonthValid = Month > 0 && Month < 13;
			bool isDayValid = Month switch
			{
				1 or 3 or 5 or 7 or 8 or 10 or 12 => Day > 0 && Day < 32,
				4 or 6 or 9 or 11 => Day > 0 && Day < 31,
				2 => Day > 0 && (DateHelper.IsLeapYear(Year) ? Day < 30 : Day < 29),
				_ => false
			};

			return isYearValid && isMonthValid && isDayValid;
		}
	}

	/// <summary>
	/// Domyślny konstruktor, inicjalizujący datę na najniższą możliwą, poprawną wartość (1/01/1).
	/// </summary>
	public Date()
	{
		Day = 1;
		Month = 1;
		Year = 1;
	}

	/// <summary>
	/// Konstruktor inicjalizujący datę na przekazaną wartość.
	/// </summary>
	/// <param name="day">Dzień daty.</param>
	/// <param name="month">Miesiąc daty.</param>
	/// <param name="year">Rok daty.</param>
	public Date(int day, int month, int year)
	{
		Day = day;
		Month = month;
		Year = year;
	}

	/// <summary>
	/// Konstruktor inicjalizujący datę na przekazaną wartość, reprezentowaną jako systemowy TimeSpan.
	/// </summary>
	/// <param name="timeSpan">Systemowy TimeSpan, na podstawie którego ustawiamy datę.</param>
	public Date(System.TimeSpan timeSpan)
	{
		int totalDays = timeSpan.Days;

		Year = totalDays / 365;
		totalDays %= 365;

		Month = totalDays / 30;
		totalDays %= 30;

		Day = totalDays;
	}

	public override string ToString()
	{
		return $"{Day}/{Month:00}/{Year}";
	}

	public override bool Equals(object obj)
	{
		if (obj is Date date)
		{
			return Day == date.Day && Month == date.Month && Year == date.Year;
		}

		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}