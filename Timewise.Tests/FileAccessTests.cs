namespace Timewise.Tests;

using Code.Database.Helpers;
using Code.Helpers;

[TestFixture]
public class FileAccessTests
{
	[Test]
	public void DatabaseConnectionString_IsLoaded()
	{
		try
		{
			var databaseConnectionString = ConnectionStringHelper.DbConnectionString;
		
			Assert.That(databaseConnectionString, Is.Not.Empty);
			Assert.That(databaseConnectionString, Is.Not.Null);
		}
		catch (Exception)
		{
			Assert.Fail();
		}
	}

	[Test]
	public void NameDayNames_IsLoaded()
	{
		try
		{
			var nameDayNames = FileHelper.GetNameDayNames();
			
			Assert.That(nameDayNames, Is.Not.Empty);
			Assert.That(nameDayNames, Is.Not.Null);
		}
		catch (Exception)
		{
			Assert.Fail();
		}
	}

	[Test]
	public void Holidays_IsLoaded()
	{
		try
		{
			var holidays = FileHelper.GetHolidays();
			
			Assert.That(holidays, Is.Not.Null);
		}
		catch (Exception)
		{
			Assert.Fail();
		}
	}
}