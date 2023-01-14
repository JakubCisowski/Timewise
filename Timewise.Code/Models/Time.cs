namespace Timewise.Code.Models;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Exceptions;
using Interfaces;

/// <summary>
/// Klasa reprezentująca punkt w czasie.
/// </summary>
public class Time : IValidatable, INotifyPropertyChanged
{
	private int _hour;
	private int _minute;
	private int _second;
	private int _millisecond;

	/// <summary>
	/// Zdarzenie niezbędne do poprawnego, automatycznego aktualizowania interfejsu użytkownika. 
	/// </summary>
	public event PropertyChangedEventHandler PropertyChanged;

	/// <summary>
	/// Godzina w czasie.
	/// </summary>
	public int Hour { get => _hour;
		set
		{
			_hour = value;
			OnPropertyChanged();
		}
	}
	
	/// <summary>
	/// Minuta w czasie.
	/// </summary>
	public int Minute
	{
		get => _minute; set
		{
			_minute = value;
			OnPropertyChanged();
		}
	}
	
	/// <summary>
	/// Sekunda w czasie.
	/// </summary>
	public int Second
	{
		get => _second; set
		{
			_second = value;
			OnPropertyChanged();
		}
	}
	
	/// <summary>
	/// Milisekunda w czasie.
	/// </summary>
	public int Millisecond
	{
		get => _millisecond; set
		{
			_millisecond = value;
			OnPropertyChanged();
		}
	}

	/// <summary>
	/// Data powiązana z obiektem czasu.
	/// Każdy obiekt czasu ma jakąś konkretną datę.
	/// </summary>
	public Date Date { get; set; }
	
	/// <summary>
	/// Strefa czasowa powiązana z obiektem czasu.
	/// Reprezentuje, w jakiej strefie czasowej jest dana godzina i data.
	/// <example>1/01/2023 12:00:00.000 UTC+1</example>
	/// </summary>
	public TimeZoneInfo TimeZone { get; set; }

	/// <summary>
	/// Właściwość zwracająca czas w tym momencie.
	/// Uwzględnia lokalną strefę czasową.
	/// </summary>
	public static Time Now
	{
		get
		{
			var now = DateTime.Now;

			var date = new Date(now.Day, now.Month, now.Year);
			var time = new Time(now.Hour, now.Minute, now.Second, now.Millisecond, date);
			time.TimeZone = TimeZoneInfo.Local;

			return time;
		}
	}

	/// <summary>
	/// Implementacja metody udostępnianej przez interfejs IValidatable.
	/// Zwraca true, jeżeli czas jest poprawny, w przeciwnym wypadku false.
	/// </summary>
	public bool IsValid
	{
		get
		{
			bool isMillisecondValid = Millisecond >= 0 && Millisecond < 1000;
			bool isSecondValid = Second >= 0 && Second < 60;
			bool isMinuteValid = Minute >= 0 && Minute < 60;
			bool isHourValid = Hour >= 0 && Hour < 24;

			bool isDateValid = Date.IsValid;

			return isMillisecondValid && isSecondValid && isMinuteValid && isHourValid && isDateValid;
		}
	}

	/// <summary>
	/// Domyślny konstruktor, inicjalizujący czas na obecny moment.
	/// </summary>
	public Time()
	{
		var now = Now;
		
		Hour = now.Hour;
		Minute = now.Minute;
		Second = now.Second;
		Millisecond = now.Millisecond;
		Date = new Date(now.Date.Day, now.Date.Month, now.Date.Year);
		
		TimeZone = TimeZoneInfo.Local;
	}

	/// <summary>
	/// Konstruktor inicjalizujący na wybrany moment (czas i datę).
	/// </summary>
	/// <param name="hour">Wybrana godzina.</param>
	/// <param name="minute">Wybrana minuta.</param>
	/// <param name="second">Wybrana sekunda.</param>
	/// <param name="millisecond">Wybrana milisekunda.</param>
	/// <param name="date">Obiekt reprezentujący datę.</param>
	public Time(int hour, int minute, int second, int millisecond, Date date)
	{
		Hour = hour;
		Minute = minute;
		Second = second;
		Millisecond = millisecond;
		Date = date;
	}

	/// <summary>
	/// Konstruktor inicjalizujący na wybrany moment za pomocą obiektów systemowych DateTime i TimeSpan.
	/// </summary>
	/// <param name="date">Data jako System.DateTime.</param>
	/// <param name="time">Czas jako System.TimeSpan.</param>
	public Time(DateTime date, TimeSpan time)
	{
		Hour = time.Hours;
		Minute = time.Minutes;
		Second = time.Seconds;
		Millisecond = time.Milliseconds;

		Date = new Date(date.Day, date.Month, date.Year);
	}

	/// <summary>
	/// Obsolete?
	/// </summary>
	/// <param name="timeSpan"></param>
	/// <returns></returns>
	public Time Add(System.TimeSpan timeSpan)
	{
		var time = new Time(this.Hour + timeSpan.Hours, this.Minute + timeSpan.Minutes, this.Second + timeSpan.Seconds, this.Millisecond + timeSpan.Milliseconds,
			new Date(this.Date.Day, this.Date.Month, this.Date.Year));

		time.CheckCarryAddition();

		return time;
	}

	/// <summary>
	/// Funkcja sprawdzająca przeniesienie przy dodawaniu czasów.
	/// <example>Jeżeli po dodaniu dwóch czasów, ilość godzin wynosi 25, to należy zwiększyć ilość dni o 1.</example>
	/// </summary>
	public void CheckCarryAddition()
	{
		if (Millisecond >= 1000)
		{
			Second += Millisecond / 1000;
			Millisecond %= 1000;
		}

		if (Second >= 60)
		{
			Minute += Second / 60;
			Second %= 60;
		}

		if (Minute >= 60)
		{
			Hour += Minute / 60;
			Minute %= 60;
		}

		if (Hour >= 24)
		{
			Date.Day += Hour / 24;
			Hour %= 24;
		}

		if (Date.Day >= 30)
		{
			Date.Month += Date.Day / 30;
			Date.Day %= 30;
		}

		if (Date.Month > 12)
		{
			Date.Year += Date.Month / 12;
			Date.Month %= 12;
		}
	}

	/// <summary>
	/// Przeciążony operator odejmowania, służący do odejmowania dwóch obiektów typu Time.
	/// </summary>
	/// <param name="left">Odjemna w operacji odejmowania.</param>
	/// <param name="right">Odjemnik w operacji odejmowania.</param>
	/// <returns>Wynik odejmowania jako odstęp czasowy (Duration).</returns>
	/// <remarks>Jeżeli odjemna jest w czasie przed odjemnikiem, zwrócony zostanie wyjątek.</remarks>
	public static Duration operator -(Time left, Time right)
	{
		if (left < right)
		{
			throw new TimeException("Odjemna jest w czasie przed odjemnikiem.");
		}
		
		int resultHour = left.Hour - right.Hour;
		int resultMinute = left.Minute - right.Minute;
		int resultSecond = left.Second - right.Second;
		int resultMillisecond = left.Millisecond - right.Millisecond;

		int resultYear = left.Date.Year - right.Date.Year;
		int resultMonth = left.Date.Month - right.Date.Month;
		int resultDay = left.Date.Day - right.Date.Day;

		var result = new Duration(resultDay, resultMonth, resultYear, resultHour, resultMinute, resultSecond, resultMillisecond);
		result.CheckCarrySubtraction(right.Date.Month, right.Date.Year);

		return result;
	}

	/// <summary>
	/// Przeciążony operator mniejszości, sprawdzający, który z dwóch czasów był wcześniej.
	/// </summary>
	/// <param name="left">Pierwszy ze sprawdzanych czasów.</param>
	/// <param name="right">Drugi ze sprawdzanych czasów.</param>
	/// <returns>True, jeżeli pierwszy z czasów był przed drugim. False w preciwnym wypadku.</returns>
	public static bool operator <(Time left, Time right)
	{
		if (left.Date.Year != right.Date.Year)
		{
			return left.Date.Year < right.Date.Year;
		}

		if (left.Date.Month != right.Date.Month)
		{
			return left.Date.Month < right.Date.Month;
		}

		if (left.Date.Day != right.Date.Day)
		{
			return left.Date.Day < right.Date.Day;
		}

		if (left.Hour != right.Hour)
		{
			return left.Hour < right.Hour;
		}

		if (left.Minute != right.Minute)
		{
			return left.Minute < right.Minute;
		}

		if (left.Second != right.Second)
		{
			return left.Second < right.Second;
		}

		if (left.Millisecond != right.Millisecond)
		{
			return left.Millisecond < right.Millisecond;
		}

		return false;
	}

	/// <summary>
	/// Przeciążony operator większości, sprawdzający, który z dwóch czasów był później.
	/// </summary>
	/// <param name="left">Pierwszy ze sprawdzanych czasów.</param>
	/// <param name="right">Drugi ze sprawdzanych czasów.</param>
	/// <returns>True, jeżeli pierwszy z czasów jest po drugim. False w preciwnym wypadku.</returns>
	public static bool operator>(Time left, Time right)
	{
		if (left.Date.Year != right.Date.Year)
		{
			return left.Date.Year > right.Date.Year;
		}

		if (left.Date.Month != right.Date.Month)
		{
			return left.Date.Month > right.Date.Month;
		}

		if (left.Date.Day != right.Date.Day)
		{
			return left.Date.Day > right.Date.Day;
		}

		if (left.Hour != right.Hour)
		{
			return left.Hour > right.Hour;
		}

		if (left.Minute != right.Minute)
		{
			return left.Minute > right.Minute;
		}

		if (left.Second != right.Second)
		{
			return left.Second > right.Second;
		}

		if (left.Millisecond != right.Millisecond)
		{
			return left.Millisecond > right.Millisecond;
		}

		return false;
	}

	/// <summary>
	/// Operator konwersji niejawnej z typu Time na typ System.DateTime.
	/// </summary>
	/// <param name="time">Czas jako typ Time.</param>
	/// <returns>Czas reprezentowany jako systemowy DateTime.</returns>
	/// <remarks>Konwersja w tę stronę jest niejawna, więc w odpowiednim kontekście zostanie dokonana automatycznie.</remarks>
	public static implicit operator DateTime(Time time)
	{
		return new DateTime(time.Date.Year, time.Date.Month, time.Date.Day, time.Hour, time.Minute, time.Second, time.Millisecond);
	}

	/// <summary>
	/// Operator konwersji jawnej z typu System.DateTime na typ Time.
	/// </summary>
	/// <param name="dateTime">Czas jako typ System.DateTime.</param>
	/// <returns>Czas reprezentowany jako Time.</returns>
	/// <remarks>Konwersja w tę stronę jest jawna, więc wymagane jest jej sprecyzowanie.</remarks>
	public static explicit operator Time(DateTime dateTime)
	{
		return new Time(dateTime, dateTime.TimeOfDay);
	}

	/// <summary>
	/// Implementacja obsługi zdarzenia OnPropertyChanged, wymagana do poprawnej aktualizacji danych na interfejsie użytkownika.
	/// </summary>
	/// <param name="propertyName">Nazwa właściwości, która wywołała zdarzenie - domyślnie ustawia sie automatycznie.</param>
	private void OnPropertyChanged([CallerMemberName] string propertyName = "")
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	public override string ToString()
	{
		return $"{Date} {Hour:00}:{Minute:00}:{Second:00}";
	}

	public override bool Equals(object obj)
	{
		if (obj is Time time)
		{
			return time.Hour == Hour && time.Minute == Minute && time.Second == Second && time.Millisecond == Millisecond && time.Date.Equals(Date);
		}

		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}