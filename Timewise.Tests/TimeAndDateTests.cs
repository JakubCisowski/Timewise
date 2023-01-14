using Timewise.Code.Models;

namespace Timewise.Tests;

[TestFixture]
public class TimeAndDateTests
{
	public static IEnumerable<TestCaseData> TimeCorrectTestCaseGenerator
	{
		get
		{
			yield return new TestCaseData(new Time());
			yield return new TestCaseData(new Time(12, 12, 12, 0, new Date(12, 12, 2012)));
			yield return new TestCaseData(new Time(5, 0, 5, 0, new Date(1, 1, 1)));
			yield return new TestCaseData(new Time(23, 59, 59, 999, new Date(31, 12, 2022)));
			yield return new TestCaseData(new Time(23, 59, 59, 999, new Date()));
		}
	}
	
	public static IEnumerable<TestCaseData> TimeIncorrectTestCaseGenerator
	{
		get
		{
			yield return new TestCaseData(new Time(-1,-1,-1,-1, new Date()));
			yield return new TestCaseData(new Time(1, 1, 1, 1, new Date(-1, -1, -1)));
			yield return new TestCaseData(new Time(1, 1, 1, 1, new Date(30,2,2012)));
			yield return new TestCaseData(new Time(1, 1, 1, 1, new Date(31,6,2017)));
			yield return new TestCaseData(new Time(1, 1, 1, 1, new Date(0, 0, 0)));
		}
	}

	[Test]
	[TestCaseSource(nameof(TimeCorrectTestCaseGenerator))]
	public void Time_IsValid_ValuesInBounds(Time time)
	{
		Assert.That(time.IsValid, Is.True);
	}

	[Test]
	[TestCaseSource(nameof(TimeIncorrectTestCaseGenerator))]
	public void Time_IsInvalid_ValuesOutOfBounds(Time time)
	{
		Assert.That(time.IsValid, Is.False);
	}

	[Test]
	public void Time_Now_HasValue()
	{
		var now = Time.Now;
		
		Assert.That(now, Is.Not.EqualTo(null));
		Assert.That(now.IsValid, Is.True);
		Assert.That(now, Is.Not.EqualTo(new Time(0, 0, 0, 0, new Date(1, 1, 1))));
	}

	[Test]
	public void Date_Empty_IsAllZeroes()
	{
		var empty = Date.Empty;
		
		Assert.That(empty, Is.Not.EqualTo(null));
		Assert.That(empty, Is.Not.EqualTo(new Date()));
		Assert.Multiple(() =>
		{
			empty.Day = 0;
			empty.Month = 0;
			empty.Year = 0;
		});
	}

	[Test]
	public void Date_Equals_IsTrue()
	{
		var date1 = new Date(12, 12, 2012);
		var date2 = new Date(12, 12, 2012);
		
		Assert.That(date1, Is.EqualTo(date2));
	}

	[Test]
	public void Time_Equals_IsTrue()
	{
		var time1 = new Time(12, 12, 12, 0, new Date(12, 12, 2012));
		var time2 = new Time(12, 12, 12, 0, new Date(12, 12, 2012));
		
		Assert.That(time1, Is.EqualTo(time2));
	}

	[Test]
	public void Time_GreaterThanLessThanOperators_WorkCorrectly()
	{
		var time1 = new Time(0,0,0,0, new Date(1,1,1));
		var time2 = new Time(12, 12, 12, 0, new Date(12, 12, 2012));
		var time3 = new Time(12, 12, 12, 0, new Date(13, 12, 2012));
		var time4 = new Time(11, 11, 11, 0, new Date(12, 12, 2012));
		
		Assert.Multiple(() =>
		{
			Assert.That(time1 < time2, Is.True);
			Assert.That(time1 < time3, Is.True);
			Assert.That(time1 < time4, Is.True);
		});
		
		Assert.Multiple(() =>
		{
			Assert.That(time2 > time1, Is.True);
			Assert.That(time2 < time1, Is.False);
		});
		
		Assert.Multiple(() =>
		{
			Assert.That(time3 > time2, Is.True);
			Assert.That(time4 < time2, Is.True);
			Assert.That(time4 < time3, Is.True);
		});
	}

	[Test]
	public void Time_Subtraction_WorksCorrectly()
	{
		var time1 = new Time(12, 12, 12, 0, new Date(12, 12, 2012));
		var time2 = new Time(12, 12, 12, 0, new Date(12, 12, 2012));
		
		var timeDifference = time1 - time2;
		
		Assert.Multiple(() =>
		{
			timeDifference.Years = 0;
			timeDifference.Months = 0;
			timeDifference.Days = 0;
			timeDifference.Hours = 0;
			timeDifference.Minutes = 0;
			timeDifference.Seconds = 0;
		});

		time2 = new Time(13, 13, 12, 0, new Date(12, 12, 2013));
		
		timeDifference = time2 - time1;
		
		Assert.Multiple(() =>
		{
			timeDifference.Years = 1;
			timeDifference.Months = 0;
			timeDifference.Days = 0;
			timeDifference.Hours = 1;
			timeDifference.Minutes = 1;
			timeDifference.Seconds = 0;
		});
	}

	[Test]
	public void Time_Subtraction_ReturnsTimeSpan()
	{
		var time1 = new Time(12, 12, 12, 0, new Date(12, 12, 2012));
		var time2 = new Time(13, 13, 13, 0, new Date(13, 12, 2013));

		var actual = time2 - time1;
		var expected = new Code.Models.Duration(1,0,1,1,1,1,0);

		Assert.That(actual, Is.EqualTo(expected));
	}

	[Test]
	public void Time_CheckCarryAddition_WorksCorrectly()
	{
		var time = new Time(23, 59, 0, 0, new Date(12, 12, 2022));
		var timeSpan = new System.TimeSpan(0, 0, 60);

		var timeAdded = time.Add(timeSpan);
	}

	[Test]
	public void Time_ToString_CorrectFormat()
	{
		var time = new Time(12, 12, 12, 0, new Date(12, 12, 2012));
		
		Assert.That(time.ToString(), Is.EqualTo("12/12/2012 12:12:12"));

		time = new Time(0, 0, 0, 0, new Date(1, 1, 1));
		Assert.That(time.ToString(), Is.EqualTo("1/01/1 00:00:00"));
	}
}