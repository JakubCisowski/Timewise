namespace Timewise.Code.Database.Helpers;

using Timewise.Code.Database.Entities;
using Timewise.Code.Database.Repositories;

/// <summary>
/// Klasa pomocnicza służąca do obsługi kont użytkownika.
/// </summary>
public static class UserAccountHelper
{
	/// <summary>
	/// Metoda sprawdzająca, czy użytkownik o podanych danych istnieje w bazie danych.
	/// Metoda wyszukuje użytkownika po nazwie, a następnie sprawdza, czy podane hasło jest poprawne.
	/// </summary>
	/// <param name="username">Nazwa użytkownika podana w aplikacji.</param>
	/// <param name="encryptedPassword">Hasło podane w aplikacji po zaszyfrowaniu.</param>
	/// <returns>Krotka dwuelementowa, zawierająca informację, czy użytkownik został odnaleziony, oraz - jeżeli tak - zwracająca obiekt użytkownika pobrany z bazy danych.</returns>
	public static async Task<(bool Exists, User OutUser)> IsUserValidAsync(string username, string encryptedPassword)
	{
		using (var repo = new EntityRepository())
		{
			var users = await repo.GetAll<User>();

			var list = users.ToList();
			
			var isUserValid = list.Any(u => u.Username == username && u.Password == encryptedPassword);
			var user = list.FirstOrDefault(u => u.Username == username && u.Password == encryptedPassword);

			return (isUserValid, user);
		}
	}

	/// <summary>
	/// Metoda sprawdzająca, czy dana nazwa użytkownika istnieje już w bazie danych ("jest zajęta").
	/// </summary>
	/// <param name="username">Nazwa użytkownika do sprawdzenia.</param>
	/// <returns>True, jeżeli nazwa została odnaleziona w bazie danych; w przeciwnym wypadku false.</returns>
	public static async Task<bool> IsUsernameTaken(string username)
	{
		using (var repo = new EntityRepository())
		{
			var users = await repo.GetAll<User>();
				
			var isUserValid = users.Any(u => u.Username == username);

			return isUserValid;
		}
	}

	/// <summary>
	/// Metoda tworząca nowe konto użytkownika.
	/// </summary>
	/// <param name="username">Nazwa użytkownika podana przy rejestracji.</param>
	/// <param name="encryptedPassword">Hasło podane przy rejestracji, po zaszyfrowaniu.</param>
	/// <returns>Nowy obiekt użytkownika po dodaniu go do bazy danych.</returns>
	public static async Task<User> CreateUserAccount(string username, string encryptedPassword)
	{
		using (var repo = new EntityRepository())
		{
			var user = new User(username, encryptedPassword);
			await repo.Add(user);

			return user;
		}
	}
}