namespace Timewise.Code.Database.Helpers;

using Microsoft.Data.SqlClient;
using Timewise.Code.Helpers;

/// <summary>
/// Klasa pomocnicza, służąca do sprawdzenia połączenia z bazą danych.
/// </summary>
public static class DatabaseConnectionHelper
{
	/// <summary>
	/// Metoda otwierająca i zamykająca połączenie z bazą danych w celu sprawdzenia, czy udało się je nawiązać.
	/// </summary>
	public static void TestDatabaseConnection()
	{
		using (var conn = new SqlConnection(SecretKeysHelper.DbConnectionString))
		{
			conn.Open();
			conn.Close();
		}
	}
}