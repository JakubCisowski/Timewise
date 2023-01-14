namespace Timewise.Code.Exceptions;

/// <summary>
/// Klasa wyjątku rzucanego w przypadku problemów z czasem, np. niewłaściwej operacji bądź podaniu nieistniejącej daty.
/// </summary>
public class TimeException : Exception
{
	/// <summary>
	/// Domyślny konstruktor, wywołujący konstruktor klasy bazowej Exception.
	/// </summary>
	public TimeException() : base() { }

	/// <summary>
	/// Konstruktor z wiadomością, wywołujący konstruktor klasy bazowej Exception.
	/// </summary>
	/// <param name="message">Wiadomość o wyjątku, np. treść błędu.</param>
	public TimeException(string message) : base(message) { }
}