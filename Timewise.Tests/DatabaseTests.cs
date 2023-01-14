namespace Timewise.Tests;

using Code.Database.Helpers;

[TestFixture]
public class DatabaseTests
{
	[Test]
	public void DatabaseTest_ConnectsWithNoException()
	{
		Assert.DoesNotThrow(() => DatabaseConnectionHelper.TestDatabaseConnection());
	}
}