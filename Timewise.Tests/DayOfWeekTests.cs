namespace Timewise.Tests;

using Code.DataTypes;
using Code.Exceptions;
using Code.Helpers;

[TestFixture]
public class DayOfWeekTests
{
	public static IEnumerable<TestCaseData> DateGenerator_CorrectValues
	{
		get
		{
			yield return new TestCaseData(new Date(6,11,2022), DayOfWeek.Sunday);
			yield return new TestCaseData(new Date(31,10,2022), DayOfWeek.Monday);
			yield return new TestCaseData(new Date(12,12,2012), DayOfWeek.Wednesday);
			yield return new TestCaseData(new Date(1,1,2000), DayOfWeek.Saturday);
			yield return new TestCaseData(new Date(31,12,2030), DayOfWeek.Tuesday);
			yield return new TestCaseData(new Date(5,7,2109), DayOfWeek.Friday);
			yield return new TestCaseData(new Date(19,4,1969), DayOfWeek.Saturday);
		}
	}
	
	public static IEnumerable<TestCaseData> DateGenerator_IncorrectValues
	{
		get
		{
			yield return new TestCaseData(new Date(6,11,-2022));
			yield return new TestCaseData(new Date(6,11,0));
			yield return new TestCaseData(new Date(29,2,2013));
		}
	}
	
	[Test]
	[TestCaseSource(nameof(DateGenerator_CorrectValues))]
	public void DayOfWeek_WorksCorrectly_ValidDate(Date date, DayOfWeek dayOfWeek)
	{
		Assert.That(DateHelper.GetDayOfWeek(date), Is.EqualTo(dayOfWeek));
	}
	
	[Test]
	[TestCaseSource(nameof(DateGenerator_IncorrectValues))]
	public void DayOfWeek_Throws_InvaliDate(Date date)
	{
		Assert.Throws<TimeException>(()=>DateHelper.GetDayOfWeek(date));
	}
}