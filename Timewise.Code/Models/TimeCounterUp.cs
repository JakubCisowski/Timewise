namespace Timewise.Code.Models;

using System.Timers;
using Interfaces;

/// <summary>
/// Klasa reprezentująca zegar odmierzający czas w górę.
/// Używana na dwa sposoby:
/// - Jako czas od pewnego zdarzenia w przeszłości
/// - Jako stoper, odmierzający czas w górę z dużą precyzją
/// </summary>
public class TimeCounterUp : IValidatable
{
	private System.Diagnostics.Stopwatch _stopwatch;
	private Timer _timer;

	/// <summary>
	/// Czas naliczany od sprecyzowanego momentu startowego.
	/// Wartość ta jest modyfikowana poprzez Timer.
	/// </summary>
	public Duration TimeSince { get; set; }
	
	/// <summary>
	/// Niezmienny czas, punkt w przeszłości, sygnalizujący początek odmierzania czasu.
	/// Reprezentuje moment zdarzenia ustawionego przez użytkownika.
	/// </summary>
	public Time StartTime { get; set; }

	/// <summary>
	/// Lista stempli czasowych, które są zapisywane na życzenie użytkownika podczas działania stopera.
	/// </summary>
	public List<Duration> Timestamps { get; set; }

	/// <summary>
	/// Czy stoper jest włączony.
	/// </summary>
	public bool IsRunning => _stopwatch != null && _stopwatch.IsRunning;
	
	/// <summary>
	/// Czy obiekt jest poprawny - czy reprezentuje możliwy do zapisania czas.
	/// Implementacja z interfejsu IValidatable.
	/// </summary>
	public bool IsValid
	{
		get
		{
			bool isSecondsValid = TimeSince.Seconds >= 0 && TimeSince.Seconds < 60;
			bool isMinutesValid = TimeSince.Minutes >= 0 && TimeSince.Minutes < 60;
			bool isHoursValid = TimeSince.Hours >= 0 && TimeSince.Hours < 24;

			return isSecondsValid && isMinutesValid && isHoursValid;
		}
	}
	
	/// <summary>
	/// Konstruktor inicjalizujący obiekt z przekazanym czasem początkowym zdarzenia.
	/// </summary>
	/// <param name="startTime">Czas początkowy, od którego naliczamy.</param>
	public TimeCounterUp(Time startTime)
	{
		TimeSince = new Duration();

		Initialize(startTime);
	}

	/// <summary>
	/// Konstruktor inicjalizujący obiekt z przekazanym czasem początkowym zdarzenia oraz już częściowo odmierzonym czasem.
	/// </summary>
	/// <param name="duration">Częściowo odmierzony czas od zdarzenia.</param>
	/// <param name="startTime">Czas początkowy, od którego naliczamy.</param>
	public TimeCounterUp(Duration duration, Time startTime)
	{
		TimeSince = new Duration(duration.Days, duration.Months, duration.Years, duration.Hours, duration.Minutes,
			duration.Seconds, duration.Milliseconds);
		
		Initialize(startTime);
	}

	/// <summary>
	/// Metoda inicjalizująca stoper oraz pozostałe zmienne.
	/// </summary>
	/// <param name="startTime">Czas początkowy, od którego odmierzamy czas, przekazany z konstruktora.</param>
	private void Initialize(Time startTime)
	{
		Timestamps = new List<Duration>();

		_stopwatch = new System.Diagnostics.Stopwatch();
		_timer = new Timer()
		{
			Interval = 10,
			AutoReset = true
		};
		_timer.Elapsed += _timer_Elapsed;

		StartTime = startTime;
	}

	/// <summary>
	/// Zdarzenie wywoływane co 10ms przez Timer. Powoduje aktualizację odmierzanego czasu.
	/// </summary>
	/// <param name="sender">Obiekt wywołujący zdarzenie, w tym przypadku timer.</param>
	/// <param name="e">Parametr zdarzenia.</param>
	private void _timer_Elapsed(object sender, ElapsedEventArgs e)
	{
		var timeSince = Time.Now - StartTime;

		TimeSince.Hours = timeSince.Hours;
		TimeSince.Minutes = timeSince.Minutes;
		TimeSince.Seconds = timeSince.Seconds;
		TimeSince.Milliseconds = timeSince.Milliseconds;
		TimeSince.Days = timeSince.Days;
		TimeSince.Months = timeSince.Months;
		TimeSince.Years = timeSince.Years;
	}

	/// <summary>
	/// Metoda rozpoczynająca działanie zegara.
	/// </summary>
	public void Start()
	{
		_stopwatch.Start();
		_timer.Start();
	}

	/// <summary>
	/// Metoda pauzująca działanie zegara.
	/// </summary>
	public void Stop()
	{
		_stopwatch.Stop();
		_timer.Stop();
	}

	/// <summary>
	/// Metoda tworząca stempel czasowy.
	/// Oblicza czas, jaki upłynął od stworzenia ostatniego stempla czasowego, dodaje go do listy stempli czasowych, i zwraca go.
	/// </summary>
	/// <returns>Czas, jaki upłynął od stworzenia ostatniego stempla czasowego.</returns>
	/// <remarks>Jeżeli metoda jest wywoływana po raz pierwszy, wówczas jest to czas, jaki upłynął od począku timera.</remarks>
	public Duration CreateTimestamp()
	{
		var timeSince = Time.Now - StartTime;
		
		Timestamps.Add(timeSince);

		return timeSince;
	}

	public override string ToString()
	{
		return $"{TimeSince.Hours:00}:{TimeSince.Minutes:00}:{TimeSince.Seconds:00}:{TimeSince.Milliseconds:000}";
	}
}