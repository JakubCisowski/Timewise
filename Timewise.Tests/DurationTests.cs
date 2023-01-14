namespace Timewise.Tests;

using Code.Exceptions;

[TestFixture]
public class DurationTests
{
	[Test]
	public void Duration_DefaultConstructor_Zero()
	{
		var timeSpan = new Duration();
		
		Assert.Multiple(() =>
		{
			Assert.That(timeSpan.Years, Is.EqualTo(0));
			Assert.That(timeSpan.Months, Is.EqualTo(0));
			Assert.That(timeSpan.Days, Is.EqualTo(0));
			Assert.That(timeSpan.Hours, Is.EqualTo(0));
			Assert.That(timeSpan.Minutes, Is.EqualTo(0));
			Assert.That(timeSpan.Seconds, Is.EqualTo(0));
			Assert.That(timeSpan.Milliseconds, Is.EqualTo(0));
		});
	}

	[Test]
	public void Duration_Addition_CorrectResult()
	{
		var timeSpan1 = new Duration(12, 12, 12, 12, 12, 12, 0);
		var timeSpan2 = new Duration(10, 10, 10, 10, 10, 10, 0);

		var actual = timeSpan1 + timeSpan2;
		var expected = new Duration(22, 10, 23, 22, 22, 22, 0);
		
		Assert.That(actual, Is.EqualTo(expected));

		timeSpan2 = new Duration();
		
		actual = timeSpan1 + timeSpan2;
		expected = timeSpan1;
		
		Assert.That(actual, Is.EqualTo(expected));
	}

	[Test]
	public void Duration_Addition_IsCommutative()
	{
		var timeSpan1 = new Duration(12, 12, 12, 12, 12, 12, 0);
		var timeSpan2 = new Duration(10, 10, 10, 10, 10, 10, 0);
		
		var result1 = timeSpan1 + timeSpan2;
		var result2 = timeSpan2 + timeSpan1;
		
		Assert.That(result1, Is.EqualTo(result2));
	}

	[Test]
	public void Duration_Subtraction_CorrectResult()
	{
		var timeSpan1 = new Duration(12, 12, 12, 12, 12, 12, 0);
		var timeSpan2 = new Duration(10, 10, 10, 10, 10, 10, 0);

		var actual = timeSpan1 - timeSpan2;
		var expected = new Duration(2, 2, 2, 2, 2, 2, 0);
		
		Assert.That(actual, Is.EqualTo(expected));

		timeSpan2 = new Duration();
		
		actual = timeSpan1 - timeSpan2;
		expected = timeSpan1;
		
		Assert.That(actual, Is.EqualTo(expected));
	}

	[Test]
	public void Duration_Subtraction_ThrowsForInvalidArguments()
	{
		var timeSpan1 = new Duration(10, 10, 10, 10, 10, 10, 0);
		var timeSpan2 = new Duration(12, 12, 12, 12, 12, 12, 0);

		Duration result = null;
		
		Assert.Throws<TimeException>(() => result = timeSpan1 - timeSpan2);
		
		timeSpan2 = new Duration(10, 10, 10, 10, 10, 10, 1);
		
		Assert.Throws<TimeException>(() => result = timeSpan1 - timeSpan2);
	}

	[Test]
	public void Duration_GreaterThanLessThanOperators_WorkCorrectly()
	{
		var timeSpan1 = new Duration();
		var timeSpan2 = new Duration(12, 12, 12, 12, 12, 12, 0);
		var timeSpan3 = new Duration(12, 12, 12, 12, 12, 12, 12);
		var timeSpan4 = new Duration(5, 5, 300, 0, 0, 0, 0);
		
		Assert.Multiple(() =>
		{
			Assert.That(timeSpan1 < timeSpan2, Is.True);
			Assert.That(timeSpan1 < timeSpan3, Is.True);
			Assert.That(timeSpan1 < timeSpan4, Is.True);
			
			Assert.That(timeSpan1 > timeSpan2, Is.False);
			Assert.That(timeSpan1 > timeSpan3, Is.False);
			Assert.That(timeSpan1 > timeSpan4, Is.False);
		});
		
		Assert.Multiple(() =>
		{
			Assert.That(timeSpan2 < timeSpan3, Is.True);
			Assert.That(timeSpan3 < timeSpan4, Is.True);
			Assert.That(timeSpan4 > timeSpan2, Is.True);
		});
	}

	[Test]
	public void Duration_CheckCarrySubtraction_WorksCorrectly()
	{
		var time1 = new Time(0, 0, 0, 0, new Date(25, 04, 2005));
		var time2 = new Time(0, 0, 0, 0, new Date(27, 02, 2005));

		var duration = time1 - time2;

		Assert.Pass();
	}
	
	[Test]
	public void Duration_ToString_CorrectFormat()
	{
		var timeSpan = new Duration(10, 5, 2, 3, 3, 3, 0);
		
		Assert.That(timeSpan.ToString(), Is.EqualTo("2 lat, 5 miesięcy, 10 dni, 3 godzin, 3 minut, 3 sekund, 0 milisekund"));

		timeSpan = new Duration();
		
		Assert.That(timeSpan.ToString(), Is.EqualTo("0 lat, 0 miesięcy, 0 dni, 0 godzin, 0 minut, 0 sekund, 0 milisekund"));
	}
}