namespace Timewise.Code.Database.Repositories;

using Helpers;
using Interfaces;
using Microsoft.Data.SqlClient;
using Dapper;
using Timewise.Code.Helpers;

/// <summary>
/// Generyczna klasa implementująca interfejs IGenericRepository, będąca jego konkretną implementacją.
/// Klasa implementuje również interfejs IDisposable, który pozwala na automatyczne zwolnienie zasobów po zakończeniu działania, oraz na tworzenie obiektu repozytorium wewnątrz bloku using.
/// </summary>
/// <typeparam name="T">Typ encji, implementujący interfejs IEntity.</typeparam>
public class EntityRepository : IDisposable, IGenericRepository
{
	/// <summary>
	/// Obiekt służący do łączenia się z bazą danych.
	/// </summary>
	private readonly SqlConnection _connection;
	
	/// <summary>
	/// Domyślny konstruktor, inicjalizujący i otwierający połączenie z bazą danych.
	/// </summary>
	public EntityRepository()
	{
		_connection = new SqlConnection(SecretKeysHelper.DbConnectionString);
		_connection.Open();
	}
	
	/// <summary>
	/// Metoda zwracająca obiekt encji o podanym Id.
	/// </summary>
	/// <param name="id">Id obiektu.</param>
	/// <returns>Obiekt danego typu o danym Id, jeżeli istnieje; w przeciwnym wypadku null.</returns>
	public async Task<T> Get<T>(int id) where T:IEntity
	{
		T endResult;

		var transaction = _connection.BeginTransaction();
		
		var reader = await _connection.ExecuteReaderAsync(QueryBuilders.QueryBuilder.BuildSelectQuery<T>(id), transaction: transaction);
		var resultParsed = reader.Parse(typeof(T));
		endResult = (T)resultParsed.FirstOrDefault();

		reader.Close();

		await transaction.CommitAsync();
		
		return endResult;
	}

	/// <summary>
	/// Metoda zwracająca wszystkie obiekty z bazy danych danego typu.
	/// </summary>
	/// <returns>Kolekcja wszystkich obiektów danego typu z bazy danych.</returns>
	public async Task<IEnumerable<T>> GetAll<T>() where T:IEntity
	{
		IEnumerable<T> endResult;
		
		var transaction = _connection.BeginTransaction();
		
		var reader = await _connection.ExecuteReaderAsync(QueryBuilders.QueryBuilder.BuildSelectAllQuery<T>(), transaction: transaction);
		var resultParsed = reader.Parse(typeof(T));
		endResult = resultParsed.Cast<T>().ToList();

		reader.Close();

		await transaction.CommitAsync();

		return endResult;
	}

	/// <summary>
	/// Metoda dodająca nowy obiekt do bazy danych.
	/// </summary>
	/// <param name="entity">Obiekt do dodania.</param>
	public async Task Add<T>(T entity) where T:IEntity
	{
		var transaction = _connection.BeginTransaction();
		
		await _connection.ExecuteAsync(QueryBuilders.QueryBuilder.BuildInsertQuery(entity), transaction: transaction);
		
		await transaction.CommitAsync();
	}

	/// <summary>
	/// Metoda aktualizująca obiekt w bazie danych.
	/// </summary>
	/// <param name="entity">Obiekt do zaktualizowania.</param>
	public async Task Update<T>(T entity) where T:IEntity
	{
		var transaction = _connection.BeginTransaction();
		
		await _connection.ExecuteAsync(QueryBuilders.QueryBuilder.BuildUpdateQuery(entity), transaction: transaction);
		
		await transaction.CommitAsync();
	}

	/// <summary>
	/// Metoda usuwająca z bazy danych obiekt o podanym Id.
	/// </summary>
	/// <param name="id">Id obiektu do usunięcia.</param>
	public async Task Delete<T>(int id) where T:IEntity
	{
		var transaction = _connection.BeginTransaction();
		
		await _connection.ExecuteAsync(QueryBuilders.QueryBuilder.BuildDeleteQuery<T>(id), transaction: transaction);
		
		await transaction.CommitAsync();
	}

	/// <summary>
	/// Metoda usuwająca z bazy danych wszystkie obiekty powiązane z danym Id użytkownika.
	/// </summary>
	/// <param name="id">Id użytkownika, dla którego chcemy usunąć obiekty z bazy danych.</param>
	public async Task Clear<T>(int id) where T:IEntity
	{
		var transaction = _connection.BeginTransaction();
		
		await _connection.ExecuteAsync(QueryBuilders.QueryBuilder.BuildClearQuery<T>(id), transaction: transaction);
		
		await transaction.CommitAsync();
	}

	/// <summary>
	/// Implementacja metody Dispose z interfejsu IDisposable, która pozwala na automatyczne zwolnienie zasobów i zamknięcie połączenia z bazą danych.
	/// </summary>
	public void Dispose()
	{
		_connection?.Close();
		_connection?.Dispose();
		
		GC.SuppressFinalize(this);
	}
}