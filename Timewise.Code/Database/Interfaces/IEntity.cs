namespace Timewise.Code.Database.Interfaces;

/// <summary>
/// Interfejs wspólny dla wszystkich obiektów bazodanowych.
/// </summary>
public interface IEntity
{
	/// <summary>
	/// Identyfikator encji, będący kluczem głównym w bazie danych.
	/// Jest to liczba całkowita generowana po stronie bazy danych.
	/// </summary>
	public int Id { get; set; }
}