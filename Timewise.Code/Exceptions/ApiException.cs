namespace Timewise.Code.Exceptions;

/// <summary>
/// Klasa wyjątku rzucanego w przypadku problemów z dostępem do API.
/// </summary>
public class ApiException : Exception
{
	/// <summary>
	/// Domyślny konstruktor, wywołujący konstruktor klasy bazowej Exception.
	/// </summary>
	public ApiException() : base() { }

	/// <summary>
	/// Konstruktor z wiadomością, wywołujący konstruktor klasy bazowej Exception.
	/// </summary>
	/// <param name="message">Wiadomość o wyjątku, np. treść błędu.</param>
	public ApiException(string message) : base(message) { }
}