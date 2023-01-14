namespace Timewise.Code.Exceptions;

/// <summary>
/// Klasa wyjątku rzucanego w przypadku problemów z dostępem do bazy danych.
/// </summary>
public class DatabaseException : Exception
{
	/// <summary>
	/// Domyślny konstruktor, wywołujący konstruktor klasy bazowej Exception.
	/// </summary>
	public DatabaseException() : base() { }

	/// <summary>
	/// Konstruktor z wiadomością, wywołujący konstruktor klasy bazowej Exception.
	/// </summary>
	/// <param name="message">Wiadomość o wyjątku, np. treść błędu.</param>
	public DatabaseException(string message) : base(message) { }
}