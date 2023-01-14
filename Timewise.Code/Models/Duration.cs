namespace Timewise.Code.Models;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Exceptions;
using Helpers;

/// <summary>
/// Klasa reprezentująca odstęp czasu.
/// Nie jest to punkt w czasie, a ilość czasu pomiędzy dwoma punktami, na przykład dwoma zdarzeniami.
/// </summary>
public class Duration : INotifyPropertyChanged
{
	private int _days;
	private int _months;
	private int _years;
	private int _hours;
	private int _minutes;
	private int _seconds;
	private int _milliseconds;
	
	/// <summary>
	/// Zdarzenie niezbędne do poprawnego, automatycznego aktualizowania interfejsu użytkownika. 
	/// </summary>
	public event PropertyChangedEventHandler PropertyChanged;

	/// <summary>
	/// Ilość dni.
	/// </summary>
	public int Days
	{
		get => _days;
		set
		{
			_days = value;
			OnPropertyChanged();
		}
	}

	/// <summary>
	/// Ilość miesięcy.
	/// </summary>
	public int Months
	{
		get => _months;
		set
		{
			_months = value;
			OnPropertyChanged();
		}
	}

	/// <summary>
	/// Ilość lat.
	/// </summary>
	public int Years
	{
		get => _years;
		set
		{
			_years = value;
			OnPropertyChanged();
		}
	}

	/// <summary>
	/// Ilość godzin.
	/// </summary>
	public int Hours
	{
		get => _hours;
		set
		{
			_hours = value;
			OnPropertyChanged();
		}
	}

	/// <summary>
	/// Ilość minut.
	/// </summary>
	public int Minutes
	{
		get => _minutes;
		set
		{
			_minutes = value;
			OnPropertyChanged();
		}
	}

	/// <summary>
	/// Ilość sekund.
	/// </summary>
	public int Seconds
	{
		get => _seconds;
		set
		{
			_seconds = value;
			OnPropertyChanged();
		}
	}

	/// <summary>
	/// Ilość milisekund.
	/// </summary>
	public int Milliseconds
	{
		get => _milliseconds;
		set
		{
			_milliseconds = value;
			OnPropertyChanged();
		}
	}

	/// <summary>
	/// Domyślny, pusty konstruktor, tworzący odstęp czasu o zerowej długości.
	/// </summary>
	public Duration()
	{
	}

	/// <summary>
	/// Konstruktor inicjalizujący odstęp czasu na podstawie podanych wartości.
	/// </summary>
	/// <param name="days">Ilość dni.</param>
	/// <param name="months">Ilość miesięcy.</param>
	/// <param name="years">Ilość lat.</param>
	/// <param name="hours">Ilość godzin.</param>
	/// <param name="minutes">Ilość minut.</param>
	/// <param name="seconds">Ilość sekund.</param>
	/// <param name="milliseconds">Ilość milisekund.</param>
	public Duration(int days, int months, int years, int hours, int minutes, int seconds, int milliseconds)
	{
		Days = days;
		Months = months;
		Years = years;
		Hours = hours;
		Minutes = minutes;
		Seconds = seconds;
		Milliseconds = milliseconds;
	}

	/// <summary>
	/// Funkcja sprawdzająca przeniesienie przy odejmowaniu czasów.
	/// Wywoływana jest przy odejmowaniu dwóch obiektów typu Time, jako że operacja odejmowania zwraca Duration (odstęp czasowy).
	/// <example>Jeżeli po odjęciu dwóch czasów, ilość dni w odstępie czasowym wynosi -2, należy zmniejszyć miesiąc.</example>
	/// </summary>
	public void CheckCarrySubtraction(int odejmowanyMonth, int odejmowanyYear)
	{
		do
		{
			if (Years < 0)
			{
				throw new TimeException("Odejmowanie obiektów TimeSpan nie powiodło się - odjemnik jest wcześniejszym czasem niż odjemna.");
			}

			if (Months < 0)
			{
				Years--;
				Months += 12;
			}

			if (Days < 0)
			{
				int daysInMonth = odejmowanyMonth == 0 || odejmowanyYear == 0 ? 30 : DateHelper.GetAmountOfDaysInMonth(odejmowanyMonth, odejmowanyYear);
				Months--;
				Days += daysInMonth;
			}

			if (Hours < 0)
			{
				Days--;
				Hours += 24;
			}

			if (Minutes < 0)
			{
				Hours--;
				Minutes += 60;
			}

			if (Seconds < 0)
			{
				Minutes--;
				Seconds += 60;
			}

			if (Milliseconds < 0)
			{
				Seconds--;
				Milliseconds += 1000;
			}
		}
		while (Months < 0 || Days < 0 || Hours < 0 || Minutes < 0 || Seconds < 0 || Milliseconds < 0);
	}

	/// <summary>
	/// Funkcja sprawdzająca przeniesienie przy dodawaniu odstępów czasowych.
	/// <example>Jeżeli po dodaniu dwóch czasów, ilość godzin wynosi 25, to należy zwiększyć ilość dni o 1.</example>
	/// </summary>
	public void CheckCarryAddition()
	{
		if (Milliseconds >= 1000)
		{
			Seconds += Milliseconds / 1000;
			Milliseconds %= 1000;
		}

		if (Seconds >= 60)
		{
			Minutes += Seconds / 60;
			Seconds %= 60;
		}

		if (Minutes >= 60)
		{
			Hours += Minutes / 60;
			Minutes %= 60;
		}

		if (Hours >= 24)
		{
			Days += Hours / 24;
			Hours %= 24;
		}

		if (Days >= 30)
		{
			Months += Days / 30;
			Days %= 30;
		}

		if (Months > 12)
		{
			Years += Months / 12;
			Months %= 12;
		}
	}

	/// <summary>
	/// Przeciążony operator dodawania, służący do dodawania dwóch obiektów typu Duration.
	/// </summary>
	/// <param name="left">Pierwszy składnik w operacji odejmowania.</param>
	/// <param name="right">Drugi składnik w operacji odejmowania.</param>
	/// <returns>Wynik dodawania jako obiekt typu Duration.</returns>
	/// <remarks>Dodawanie czasów jest przemienne.</remarks>
	public static Duration operator +(Duration left, Duration right)
	{
		var duration = new Duration(left.Days + right.Days, left.Months + right.Months, left.Years + right.Years, left.Hours + right.Hours, left.Minutes + right.Minutes, left.Seconds + right.Seconds, left.Milliseconds + right.Milliseconds);

		duration.CheckCarryAddition();

		return duration;
	}

	/// <summary>
	/// Przeciążony operator odejmowania, służący do odejmowania dwóch obiektów typu Duration.
	/// </summary>
	/// <param name="left">Odjemna w operacji odejmowania.</param>
	/// <param name="right">Odjemnik w operacji odejmowania.</param>
	/// <returns>Wynik odejmowania jako obiekt typu Duration.</returns>
	/// <remarks>Jeżeli odjemna jest w czasie przed odjemnikiem, zwrócony zostanie wyjątek.</remarks>
	public static Duration operator -(Duration left, Duration right)
	{
		if (left < right)
		{
			throw new TimeException("Odejmowanie obiektów TimeSpan nie powiodło się - odjemnik jest wcześniejszym czasem niż odjemna.");
		}

		var duration = new Duration(left.Days - right.Days, left.Months - right.Months, left.Years - right.Years, left.Hours - right.Hours, left.Minutes - right.Hours, left.Seconds - right.Seconds, left.Milliseconds - right.Milliseconds);

		duration.CheckCarrySubtraction(right.Months, right.Years);

		return duration;
	}

	/// <summary>
	/// Przeciążony operator mniejszości, sprawdzający, który z dwóch odstępów czasowych jest krótszy.
	/// </summary>
	/// <param name="left">Pierwszy ze sprawdzanych odstępów czasowych.</param>
	/// <param name="right">Drugi ze sprawdzanych odstępów czasowych.</param>
	/// <returns>True, jeżeli pierwszy z czasów jest krótszy od drugiego. False w preciwnym wypadku.</returns>
	public static bool operator <(Duration left, Duration right)
	{
		if (left.Years != right.Years)
		{
			return left.Years < right.Years;
		}

		if (left.Months != right.Months)
		{
			return left.Months < right.Months;
		}

		if (left.Days != right.Days)
		{
			return left.Days < right.Days;
		}

		if (left.Hours != right.Hours)
		{
			return left.Hours < right.Hours;
		}

		if (left.Minutes != right.Minutes)
		{
			return left.Minutes < right.Minutes;
		}

		if (left.Seconds != right.Seconds)
		{
			return left.Seconds < right.Seconds;
		}

		if (left.Milliseconds != right.Milliseconds)
		{
			return left.Milliseconds < right.Milliseconds;
		}

		return false;
	}
	
	/// <summary>
	/// Przeciążony operator większości, sprawdzający, który z dwóch odstępów czasowych jest dłuższy.
	/// </summary>
	/// <param name="left">Pierwszy ze sprawdzanych odstępów czasowych.</param>
	/// <param name="right">Drugi ze sprawdzanych odstępów czasowych.</param>
	/// <returns>True, jeżeli pierwszy z czasów jest dłuższy od drugiego. False w preciwnym wypadku.</returns>
	public static bool operator >(Duration left, Duration right)
	{
		if (left.Years != right.Years)
		{
			return left.Years > right.Years;
		}

		if (left.Months != right.Months)
		{
			return left.Months > right.Months;
		}

		if (left.Days != right.Days)
		{
			return left.Days > right.Days;
		}

		if (left.Hours != right.Hours)
		{
			return left.Hours > right.Hours;
		}

		if (left.Minutes != right.Minutes)
		{
			return left.Minutes > right.Minutes;
		}

		if (left.Seconds != right.Seconds)
		{
			return left.Seconds > right.Seconds;
		}

		if (left.Milliseconds != right.Milliseconds)
		{
			return left.Milliseconds > right.Milliseconds;
		}

		return false;
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
		return $"{Years} lat, {Months} miesięcy, {Days} dni, {Hours} godzin, {Minutes} minut, {Seconds} sekund, {Milliseconds} milisekund";
	}

	public override bool Equals(object obj)
	{
		if (obj is Duration timeSpan)
		{
			return timeSpan.Days == Days && timeSpan.Months == Months && timeSpan.Years == Years && timeSpan.Hours == Hours &&
				timeSpan.Minutes == Minutes && timeSpan.Seconds == Seconds && timeSpan.Milliseconds == Milliseconds;
		}

		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}