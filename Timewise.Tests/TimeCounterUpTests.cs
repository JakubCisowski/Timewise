namespace Timewise.Tests;

[TestFixture]
public class TimeCounterUpTests
{
	[Test]
	public void TimeCounterUp_IsRunning_AfterStarting()
	{
		var timeCounter = new TimeCounterUp(new Duration(), new Time());
		timeCounter.Start();

		Assert.That(timeCounter.IsRunning, Is.True);
	}
	
	[Test]
	public void TimeCounterUp_IsNotRunning_AfterStopping()
	{
		var timeCounter = new TimeCounterUp(new Duration(), new Time());
		timeCounter.Start();
		
		timeCounter.Stop();

		Assert.That(timeCounter.IsRunning, Is.False);
	}

	[Test]
	public void TimeCounterUp_Laps_AreAddedToList()
	{
		var timeCounter = new TimeCounterUp(new Duration(), new Time());
		timeCounter.Start();

		var time1 = timeCounter.CreateTimestamp();
		var time2 = timeCounter.CreateTimestamp();
		var time3 = timeCounter.CreateTimestamp();

		Assert.That(timeCounter.Timestamps, Has.Count.EqualTo(3));

		Assert.That(timeCounter.Timestamps[0], Is.EqualTo(time1));
		Assert.That(timeCounter.Timestamps[1], Is.EqualTo(time2));
		Assert.That(timeCounter.Timestamps[2], Is.EqualTo(time3));
	}

	[Test]
	public void TimeCounterUp_ToString_CorrectFormat()
	{
		var timeCounter = new TimeCounterUp(new Duration(), new Time());

		Assert.That(timeCounter.ToString(), Is.EqualTo("00:00:00:000"));

		timeCounter.TimeSince.Hours = 1;
		timeCounter.TimeSince.Minutes = 2;
		timeCounter.TimeSince.Seconds = 3;
		timeCounter.TimeSince.Milliseconds = 456;

		Assert.That(timeCounter.ToString(), Is.EqualTo("01:02:03:456"));
	}
}