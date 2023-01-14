namespace Timewise.Tests;

[TestFixture]
public class TimeCounterDownTests
{
	public static IEnumerable<TestCaseData> CountdownCorrectTestCaseGenerator
	{
		get
		{
			yield return new TestCaseData(new TimeCounterDown(new Duration(0,0,0,0,0,0,0), new Time(0,0,0,0, new Date(0,0,0))));
			yield return new TestCaseData(new TimeCounterDown(new Duration(5,0,0,5,5,5,0), new Time(0,0,0,0, new Date(0,0,0))));
			yield return new TestCaseData(new TimeCounterDown(new Duration(13,0,0,0,0,0,0), new Time(0,0,0,0, new Date(11,12,2025))));
			yield return new TestCaseData(new TimeCounterDown(new Duration(2,2,2,2,2,2,2), new Time(2,2,2,2, new Date(1,12,2023))));
		}
	}
	
	public static IEnumerable<TestCaseData> CountdownIncorrectTestCaseGenerator
	{
		get
		{
			yield return new TestCaseData(new TimeCounterDown(new Duration(0,0,0,0,0,-1,0), new Time(0,0,0,0, new Date(0,0,0))));
			yield return new TestCaseData(new TimeCounterDown(new Duration(0,0,0,23,59,60,0), new Time(0,0,0,0, new Date(0,0,0))));
			yield return new TestCaseData(new TimeCounterDown(new Duration(0,0,0,24,0,0,0), new Time(0,0,0,0, new Date(32,13,1))));
		}
	}

	[Test]
	public void TimeCounterDown_IsRunning_AfterStarting()
	{
		var countdown = new TimeCounterDown(new Duration(5,5,5,5,5,5,5), new Time(12,0,0,0,new Date(1,1,2024)));
		countdown.Start();

		Assert.That(countdown.IsRunning, Is.True);
	}

	[Test]
	[TestCaseSource(nameof(CountdownCorrectTestCaseGenerator))]
	public void TimeCounterDown_IsValid_ValuesInBounds(TimeCounterDown time)
	{
		Assert.That(time.IsValid, Is.True);
	}

	[Test]
	[TestCaseSource(nameof(CountdownIncorrectTestCaseGenerator))]
	public void TimeCounterDown_IsInvalid_ValuesOutOfBounds(TimeCounterDown time)
	{
		Assert.That(time.IsValid, Is.False);
	}

	[Test]
	public void TimeCounterDown_ToString_CorrectFormat()
	{
		var countdown = new TimeCounterDown(new Duration(5,5,5,5,5,5,5), new Time(12,0,0,0,new Date(1,1,2024)));
		Assert.That(countdown.ToString(), Is.EqualTo("5h 5m 5s"));
		
		countdown = new TimeCounterDown(new Duration(), new Time());
		Assert.That(countdown.ToString(), Is.EqualTo("0h 0m 0s"));
	}
}