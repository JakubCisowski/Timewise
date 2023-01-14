namespace Timewise.Code.Database.Interfaces;

/// <summary>
/// Generyczny interfejs repozytorium, zawierająca podstawowe metody do obsługi bazy danych.
/// W przypadku dostępu do bazy danych gdziekolwiek w aplikacji, dostęp ten odbywa się poprzez konkretne implementacje tego interfejsu.
/// </summary>
/// <typeparam name="T">Typ encji, implementujący interfejs IEntity.</typeparam>
public interface IGenericRepository
{
	/// <summary>
	/// Metoda obsługująca zapytanie SELECT do bazy danych.
	/// </summary>
	/// <param name="id">Identyfikator obiektu.</param>
	/// <returns>Obiekt z bazy danych.</returns>
	public Task<T> Get<T>(int id) where T:IEntity;
	
	/// <summary>
	/// Metoda obsługująca zapytanie SELECT ALL do bazy danych.
	/// </summary>
	/// <returns>Kolekcja obiektów danego typu z bazy danych.</returns>
	public Task<IEnumerable<T>> GetAll<T>() where T:IEntity;
	
	/// <summary>
	/// Metoda obsługująca zapytanie INSERT do bazy danych.
	/// </summary>
	/// <param name="entity">Obiekt do dodania.</param>
	public Task Add<T>(T entity) where T:IEntity;
	
	/// <summary>
	/// Metoda obsługująca zapytanie UPDATE do bazy danych.
	/// </summary>
	/// <param name="entity">Obiekt do zaktualizowania.</param>
	/// <remarks>W przypadku aktualizacji obiektu, istotne jest jego Id.</remarks>
	public Task Update<T>(T entity) where T:IEntity;
	
	/// <summary>
	/// Metoda obsługująca zapytanie DELETE do bazy danych.
	/// </summary>
	/// <param name="id">Identyfikator obiektu do usunięcia.</param>
	public Task Delete<T>(int id) where T:IEntity;

	/// <summary>
	/// Metoda obsługująca zapytanie DELETE ALL do bazy danych.
	/// Usuwa wszystkie obiekty powiązane z użytkownikiem o danym Id.
	/// </summary>
	/// <param name="id">Identyfikator użytkownika do usunięcia.</param>
	public Task Clear<T>(int id) where T:IEntity;
}