namespace Timewise.Code.Database.Entities;

using System.ComponentModel.DataAnnotations;
using Interfaces;

/// <summary>
/// Encja reprezentująca użytkownika w bazie danych.
/// Użytkownik posiada unikalny identyfikator, nazwę użytkownika oraz hasło, jak również jest powiązany z pozostałymi encjami.
/// </summary>
public class User : IEntity
{
	/// <summary>
	/// Aktualnie zalogowany w aplikacji użytkownik, do którego przypisywane są nowoutworzone powiadomienia i zegary.
	/// </summary>
	public static User CurrentUser { get; set; }
	
	/// <summary>
	/// Identyfikator użytkownika, generowany z poziomu bazy danych.
	/// </summary>
	public int Id { get; set; }
	
	/// <summary>
	/// Nazwa użytkownika.
	/// </summary>
	[MinLength(3)]
	[MaxLength(12)]
	public string Username { get; set; }
	
	/// <summary>
	/// Hasło użytkownika w zaszyfrowanej formie.
	/// Hasło nigdy nie jest przechowywane w formie tekstowej.
	/// </summary>
	[MaxLength(64)]
	public string Password { get; set; }

	/// <summary>
	/// Domyślny, prywatny konstruktor, uniemożliwiający tworzenie pustych obiektów poza klasą.
	/// </summary>
	private User() { }

	/// <summary>
	/// Konstruktor tworzący nowego użytkownika z daną nazwą użytkownika oraz hasłem.
	/// </summary>
	/// <param name="username">Nazwa użytkownika.</param>
	/// <param name="password">Zaszyfrowane hasło.</param>
	public User(string username, string password)
	{
		Username = username;
		Password = password;
	}
}