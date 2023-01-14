namespace Timewise.Code.Models;

using System.Timers;
using Interfaces;

/// <summary>
/// Klasa reprezentująca zegar odmierzający czas w dół.
/// Używana na dwa sposoby:
/// - Jako czas do pewnego zdarzenia w przyszłości
/// - Jako timer, ustawiany na pewien czas (np. 20 minut) i odmierzający go w dół
/// </summary>
public class TimeCounterDown : IValidatable
{
	private Timer _timer;
	
	/// <summary>
	/// Czas pozostały do sprecyzowanego momentu końcowego.
	/// Wartość ta jest modyfikowana poprzez Timer.
	/// </summary>
	public Duration TimeLeft { get; set; }
	
	/// <summary>
	/// Niezmienny czas, punkt w przyszłości, sygnalizujący koniec odmierzania czasu.
	/// Reprezentuje moment zdarzenia ustawionego przez użytkownika.
	/// </summary>
	public Time EndTime { get; set; }

	/// <summary>
	/// Czy timer jest włączony.
	/// </summary>
	public bool IsRunning => _timer != null && _timer.Enabled;
	
	/// <summary>
	/// Czy obiekt jest poprawny - czy reprezentuje możliwy do zapisania czas.
	/// Implementacja z interfejsu IValidatable.
	/// </summary>
	public bool IsValid
	{
		get
		{
			bool isSecondsValid = TimeLeft.Seconds >= 0 && TimeLeft.Seconds < 60;
			bool isMinutesValid = TimeLeft.Minutes >= 0 && TimeLeft.Minutes < 60;
			bool isHoursValid = TimeLeft.Hours >= 0 && TimeLeft.Hours < 24;

			return isSecondsValid && isMinutesValid && isHoursValid;
		}
	}

	/// <summary>
	/// Konstruktor inicjalizujący obiekt z przekazanym czasem końcowym zdarzenia oraz już częściowo odmierzonym czasem.
	/// </summary>
	/// <param name="duration">Częściowo odmierzony czas do zdarzenia.</param>
	/// <param name="endTime">Czas końcowy, do którego odliczamy.</param>
	public TimeCounterDown(Duration duration, Time endTime)
	{
		TimeLeft = new Duration(duration.Days, duration.Months, duration.Years, duration.Hours,duration.Minutes,
			duration.Seconds, duration.Milliseconds);

		Initialize(endTime);
	}

	/// <summary>
	/// Metoda inicjalizująca timer oraz pozostałe zmienne.
	/// </summary>
	/// <param name="endTime">Czas końcowy, do którego odliczamy czas, przekazany z konstruktora.</param>
	private void Initialize(Time endTime)
	{
		_timer = new Timer()
		{
			Interval = 1000,
			AutoReset = true
		};
		_timer.Elapsed += _timer_Elapsed;

		EndTime = endTime;
	}

	/// <summary>
	/// Metoda rozpoczynająca działanie zegara.
	/// </summary>
	public void Start()
	{
		_timer.Start();
	}
	
	/// <summary>
	/// Metoda pauzująca działanie zegara.
	/// </summary>
	public void Stop()
	{
		_timer.Stop();
	}

	/// <summary>
	/// Zdarzenie wywoływane co 1s przez Timer. Powoduje aktualizację odmierzanego czasu.
	/// </summary>
	/// <param name="sender">Obiekt wywołujący zdarzenie, w tym przypadku timer.</param>
	/// <param name="e">Parametr zdarzenia.</param>
	private void _timer_Elapsed(object sender, ElapsedEventArgs e)
	{
		if (TimeLeft.Seconds == 0 && TimeLeft.Minutes == 0 && TimeLeft.Hours == 0)
		{
			Stop();
		}

		if (TimeLeft.Seconds > 0)
		{
			TimeLeft.Seconds--;
		}
		else
		{
			if (TimeLeft.Minutes > 0)
			{
				TimeLeft.Seconds = 59;
				TimeLeft.Minutes--;
			}
			else
			{
				TimeLeft.Seconds = 59;
				TimeLeft.Minutes = 59;
				TimeLeft.Hours--;
			}
		}
	}

	public override string ToString()
	{
		return $"{TimeLeft.Hours}h {TimeLeft.Minutes}m {TimeLeft.Seconds}s";
	}
}