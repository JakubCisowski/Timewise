namespace Timewise.App.Pages;

using System.Timers;
using Code.Database.Entities;
using Code.Database.Repositories;
using Code.Models;

public partial class CalendarPage : ContentPage
{
	private Calendar _calendar;
	
	public CalendarPage()
	{
		InitializeComponent();
		
		_calendar = new Calendar(SelectedDateLabel, RemindersStackLayout);

		this.BindingContext = _calendar;

		_calendar.ChangeDateSelection(DateTime.Now);
	}

	private async void BackButton_OnClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///MainPage", true);
	}

	private async void AddReminderButton_OnClicked(object sender, EventArgs e)
	{
		var description = ReminderDescriptionEntry.Text;
		var selectedTime = ReminderTimePicker.Time;

		var time = new DateTime(Calendar.SelectedDate.Year, Calendar.SelectedDate.Month, Calendar.SelectedDate.Day, selectedTime.Hours, selectedTime.Minutes, selectedTime.Seconds);

		if (time < DateTime.Now)
		{
			await DisplayAlert("B³¹d", "Nale¿y podaæ godzinê i datê w przysz³oœci!", "OK");
			return;
		}

		var reminder = new Reminder()
		{
			Description = description,
			DateTime = time,
			UserId = User.CurrentUser != null ? User.CurrentUser.Id : 0
		};

		var timer = new Timer()
		{
			Interval = (time - DateTime.Now).TotalMilliseconds,
			AutoReset = false
		};
		
		timer.Elapsed += (o, args) => TimerOnElapsed(o, args, reminder);
		timer.Start();

		_calendar.AddReminder(reminder);

		if (User.CurrentUser != null)
		{
			using (var repo = new EntityRepository())
			{
				await repo.Add(reminder);
			}
		}
	}

	private async void TimerOnElapsed(object sender, ElapsedEventArgs e, Reminder reminder)
	{
		await this.Dispatcher.DispatchAsync(async () =>
		{
			await DisplayAlert("Powiadomienie", $"{reminder.Description}", "OK");
			_calendar.RemoveReminder(reminder);
		});
	}
}