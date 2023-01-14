namespace Timewise.Code.Database.QueryBuilders;

/// <summary>
/// Pomocnicza klasa zawierająca skrypty do budowania tabeli w bazie danych.
/// </summary>
public static class DatabaseCreationScriptsBuilder
{
	/// <summary>
	/// Metoda zwracająca skrypt tworzący tabelę User.
	/// </summary>
	public static string BuildUserTableScripts()
	{
		return @"CREATE TABLE dbo.[User] (
    			[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    			[Username] NVARCHAR(12) NOT NULL,
    			[Password] NVARCHAR(64) NOT NULL,
    			);";
	}

	/// <summary>
	/// Metoda zwracająca skrypt tworzący tabelę TimeCounterUpEvent.
	/// </summary>
	public static string BuildTimeCounterUpEventTableScripts()
	{
		return @"CREATE TABLE [dbo].[TimeCounterUpEvent] (
				  [Id] [INT] NOT NULL IDENTITY(1,1) PRIMARY KEY,
				  [Name] [NVARCHAR](64),
				  [StartTime] [DATETIME] NOT NULL,
				  [UserId] [INT] NOT NULL,
				  CONSTRAINT [FK_TimeCounterUpEvent_User] FOREIGN KEY ([UserId]) REFERENCES dbo.[User] ([Id])
				);";
	}

	/// <summary>
	/// Metoda zwracająca skrypt tworzący tabelę TimeCounterDownEvent.
	/// </summary>
	public static string BuildTimeCounterDownEventTableScripts()
	{
		return @"CREATE TABLE [dbo].[TimeCounterDownEvent] (
				  [Id] [INT] NOT NULL IDENTITY(1,1) PRIMARY KEY,
				  [Name] [NVARCHAR](64),
				  [EndTime] [DATETIME] NOT NULL,
				  [UserId] [INT] NOT NULL,
				  CONSTRAINT [FK_TimeCounterDownEvent_User] FOREIGN KEY ([UserId]) REFERENCES dbo.[User] ([Id])
				);";
	}
	
	/// <summary>
	/// Metoda zwracająca skrypt tworzący tabelę Reminder.
	/// </summary>
	public static string BuildReminderTableScripts()
	{
		return @"CREATE TABLE [dbo].[Reminder] (
				  [Id] [INT] NOT NULL IDENTITY(1,1) PRIMARY KEY,
				  [Description] [NVARCHAR](64) NOT NULL,
				  [DateTime] [DATETIME] NOT NULL,
				  [UserId] [INT] NOT NULL,
				  CONSTRAINT [FK_Reminder_User] FOREIGN KEY ([UserId]) REFERENCES dbo.[User] ([Id])
				);";
	}
}