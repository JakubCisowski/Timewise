namespace Timewise.Code.Database.QueryBuilders;

using Entities;
using Exceptions;
using Interfaces;

/// <summary>
/// Pomocnicza, generyczna klasa tworząca zapytania SQL dla każdej z tabeli.
/// </summary>
public static class QueryBuilder
{
	/// <summary>
	/// Metoda tworząca zapytanie SELECT ALL.
	/// </summary>
	public static string BuildSelectAllQuery<T>() where T : IEntity
	{
		if (typeof(T) == typeof(User))
		{
			return "SELECT * FROM dbo.[User];";
		}
		
		if (typeof(T) == typeof(TimeCounterDownEvent))
		{
			return "SELECT * FROM dbo.[TimeCounterDownEvent];";
		}
		
		if (typeof(T) == typeof(TimeCounterUpEvent))
		{
			return "SELECT * FROM dbo.[TimeCounterUpEvent];";
		}
		
		if (typeof(T) == typeof(Reminder))
		{
			return "SELECT * FROM dbo.[Reminder];";
		}

		throw new DatabaseException("No query for type " + typeof(T).Name);
	}
	
	/// <summary>
	/// Metoda tworząca zapytanie SELECT.
	/// </summary>
	public static string BuildSelectQuery<T>(int id) where T : IEntity
	{
		if (typeof(T) == typeof(User))
		{
			return $"SELECT * FROM dbo.[User] WHERE Id = {id};";
		}
		
		if (typeof(T) == typeof(TimeCounterDownEvent))
		{
			return $"SELECT * FROM dbo.[TimeCounterDownEvent] WHERE Id = {id};";
		}
		
		if (typeof(T) == typeof(TimeCounterUpEvent))
		{
			return $"SELECT * FROM dbo.[TimeCounterUpEvent] WHERE Id = {id};";
		}
		
		if (typeof(T) == typeof(Reminder))
		{
			return $"SELECT * FROM dbo.[Reminder] WHERE Id = {id};";
		}

		throw new DatabaseException("No query for type " + typeof(T).Name);
	}

	/// <summary>
	/// Metoda tworząca zapytanie INSERT.
	/// </summary>
	public static string BuildInsertQuery<T>(T entity) where T : IEntity
	{
		if (typeof(T) == typeof(User))
		{
			if (entity is not User user)
			{
				throw new DatabaseException("Passed entity is not of type User.");
			}
			
			return $"INSERT INTO dbo.[User] (Username, Password) VALUES ('{user.Username}', '{user.Password}');";
		}
		
		if (typeof(T) == typeof(TimeCounterDownEvent))
		{
			if (entity is not TimeCounterDownEvent timeCounterDownEvent)
			{
				throw new DatabaseException("Passed entity is not of type TimeCounterDownEvent.");
			}
			
			return $"INSERT INTO dbo.[TimeCounterDownEvent] (Name, EndTime, UserId) VALUES ('{timeCounterDownEvent.Name}'," +
			       $"'{timeCounterDownEvent.EndTime:yyyy-MM-dd HH:mm:ss.fff}', '{timeCounterDownEvent.UserId}');";
		}
		
		if (typeof(T) == typeof(TimeCounterUpEvent))
		{
			if (entity is not TimeCounterUpEvent timeCounterUpEvent)
			{
				throw new DatabaseException("Passed entity is not of type TimeCounterUpEvent.");
			}
			
			return $"INSERT INTO dbo.[TimeCounterUpEvent] (Name, StartTime, UserId) VALUES ('{timeCounterUpEvent.Name}'," +
			       $" '{timeCounterUpEvent.StartTime:yyyy-MM-dd HH:mm:ss.fff}', '{timeCounterUpEvent.UserId}');";
		}
		
		if (typeof(T) == typeof(Reminder))
		{
			if (entity is not Reminder reminder)
			{
				throw new DatabaseException("Passed entity is not of type Reminder.");
			}
			
			return $"INSERT INTO dbo.[Reminder] (Description, DateTime, UserId) VALUES ('{reminder.Description}'," +
			       $" '{reminder.DateTime:yyyy-MM-dd HH:mm:ss.fff}', '{reminder.UserId}');";
		}

		throw new DatabaseException("No query for type " + typeof(T).Name);
	}

	/// <summary>
	/// Metoda tworząca zapytanie UPDATE.
	/// </summary>
	public static string BuildUpdateQuery<T>(T entity) where T : IEntity
	{
		if (typeof(T) == typeof(User))
		{
			if (entity is not User user)
			{
				throw new DatabaseException("Passed entity is not of type User.");
			}
			
			return $"UPDATE dbo.[User] SET Username = '{user.Username}', Password = '{user.Password}' WHERE Id = {user.Id};";
		}
		
		if (typeof(T) == typeof(TimeCounterDownEvent))
		{
			if (entity is not TimeCounterDownEvent timeCounterDownEvent)
			{
				throw new DatabaseException("Passed entity is not of type TimeCounterDownEvent.");
			}
			
			return $"UPDATE dbo.[TimeCounterDownEvent] SET Name = '{timeCounterDownEvent.Name}', EndTime = '{timeCounterDownEvent.EndTime}'," +
			       $" UserId = '{timeCounterDownEvent.UserId}' WHERE Id = {timeCounterDownEvent.Id};";
		}
		
		if (typeof(T) == typeof(TimeCounterUpEvent))
		{
			if (entity is not TimeCounterUpEvent timeCounterUpEvent)
			{
				throw new DatabaseException("Passed entity is not of type TimeCounterUpEvent.");
			}
			
			return $"UPDATE dbo.[TimeCounterUpEvent] SET Name = '{timeCounterUpEvent.Name}', StartTime = '{timeCounterUpEvent.StartTime}'," +
			       $" UserId = '{timeCounterUpEvent.UserId}' WHERE Id = {timeCounterUpEvent.Id};";
		}
		
		if (typeof(T) == typeof(Reminder))
		{
			if (entity is not Reminder reminder)
			{
				throw new DatabaseException("Passed entity is not of type Reminder.");
			}
			
			return $"UPDATE dbo.[Reminder] SET Description = '{reminder.Description}', DateTime = '{reminder.DateTime}'," +
			       $" UserId = '{reminder.UserId}' WHERE Id = {reminder.Id};";
		}
		
		throw new DatabaseException("No query for type " + typeof(T).Name);
	}

	/// <summary>
	/// Metoda tworząca zapytanie DELETE.
	/// </summary>
	public static string BuildDeleteQuery<T>(int id) where T : IEntity
	{
		if (typeof(T) == typeof(User))
		{
			return $"DELETE FROM dbo.[User] WHERE Id = {id};";
		}
		
		if (typeof(T) == typeof(TimeCounterDownEvent))
		{
			return $"DELETE FROM dbo.[TimeCounterDownEvent] WHERE Id = {id};";
		}
		
		if (typeof(T) == typeof(TimeCounterUpEvent))
		{
			return $"DELETE FROM dbo.[TimeCounterUpEvent] WHERE Id = {id};";
		}
		
		if (typeof(T) == typeof(Reminder))
		{
			return $"DELETE FROM dbo.[Reminder] WHERE Id = {id};";
		}
		
		throw new DatabaseException("No query for type " + typeof(T).Name);
	}
	
	/// <summary>
	/// Metoda tworząca zapytanie DELETE ALL.
	/// </summary>
	public static string BuildClearQuery<T>(int id) where T : IEntity
	{
		if (typeof(T) == typeof(User))
		{
			return $"DELETE FROM dbo.[User] WHERE Id = {id};";
		}
		
		if (typeof(T) == typeof(TimeCounterDownEvent))
		{
			return $"DELETE FROM dbo.[TimeCounterDownEvent] WHERE UserId = {id};";
		}
		
		if (typeof(T) == typeof(TimeCounterUpEvent))
		{
			return $"DELETE FROM dbo.[TimeCounterUpEvent] WHERE UserId = {id};";
		}
		
		if (typeof(T) == typeof(Reminder))
		{
			return $"DELETE FROM dbo.[Reminder] WHERE UserId = {id};";
		}
		
		throw new DatabaseException("No query for type " + typeof(T).Name);
	}
}