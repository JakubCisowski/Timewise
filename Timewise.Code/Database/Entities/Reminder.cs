namespace Timewise.Code.Database.Entities;

using Interfaces;

/// <summary>
/// Encja reprezentująca przypomnienie ustawione przez użytkownika w kalendarzu.
/// </summary>
public class Reminder : IEntity
{
	/// <summary>
	/// Identyfikator przypomnienia, generowany z poziomu bazy danych.
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Etykieta przypomnienia.
	/// </summary>
	public string Description { get; set; }

	/// <summary>
	/// Ustawiony przez użytkownika czas wywołania przypomnienia.
	/// </summary>
	public DateTime DateTime { get; set; }

	/// <summary>
	/// Id użytkownika, do którego przypisane jest przypomnienie.
	/// </summary>
	public int UserId { get; set; }
}