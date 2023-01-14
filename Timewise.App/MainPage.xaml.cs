namespace Timewise.App;

using Code.Database.Entities;
using Code.Database.Repositories;
using Code.Exceptions;
using Code.Models;
using Pages;
using TimeCounterDownEvent = Code.Database.Entities.TimeCounterDownEvent;
using TimeCounterUpEvent = Code.Database.Entities.TimeCounterUpEvent;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		
		// Wczytywanie z bazy danych

		if (User.CurrentUser == null)
		{
			return;
		}

		LoadFromDatabase();
	}

	private async void LoadFromDatabase()
	{
		try
		{
			using (var repo = new EntityRepository())
			{
				// Usunięcie przeterminowanych obiektów.
				var timeCounterDownEvents = await repo.GetAll<TimeCounterDownEvent>();

				timeCounterDownEvents = timeCounterDownEvents.Where(x => x.UserId == User.CurrentUser.Id);
				
				foreach (var timeCounterDownEvent in timeCounterDownEvents)
				{
					if (timeCounterDownEvent.EndTime < DateTime.Now)
					{
						await repo.Delete<TimeCounterDownEvent>(timeCounterDownEvent.Id);
					}
				}

				timeCounterDownEvents = await repo.GetAll<TimeCounterDownEvent>();
				
				timeCounterDownEvents = timeCounterDownEvents.Where(x => x.UserId == User.CurrentUser.Id);

				var timeCounterDownEventsList = new List<Code.Models.TimeCounterDownEvent>();

				foreach (var ev in timeCounterDownEvents)
				{
					timeCounterDownEventsList.Add(ev);
				}

				foreach (var timeCounterEv in timeCounterDownEventsList)
				{
					timeCounterEv.TimeCounterDown.Start();
				}

				TimeCounterDownPage.TimeCounterDownEvents = timeCounterDownEventsList;
				
				var timeCounterUpEvents = await repo.GetAll<TimeCounterUpEvent>();
				timeCounterUpEvents = timeCounterUpEvents.Where(x => x.UserId == User.CurrentUser.Id);
				
				var timeCounterUpEventsList = new List<Code.Models.TimeCounterUpEvent>();

				foreach (var ev in timeCounterUpEvents)
				{
					timeCounterUpEventsList.Add(ev);
				}

				foreach (var timeCounterEv in timeCounterUpEventsList)
				{
					timeCounterEv.TimeCounterUp.Start();
				}

				TimeCounterUpPage.TimeCounterUpEvents = timeCounterUpEventsList;
				
				var reminders = await repo.GetAll<Reminder>();

				Calendar.Reminders = reminders.Where(x=>x.UserId == User.CurrentUser.Id).ToList();
			}
		}
		catch (DatabaseException ex)
		{
			await DisplayAlert("Błąd przy wczytywaniu z bazy danych", ex.Message, "OK");
		}
	}

	private async void TimerButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///timer", true);
	}

	private async void StopwatchButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///stopwatch", true);
	}

	private async void ElapsingTimeButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///timeCounterUp", true);
	}

	private async void CountdownButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///timeCounterDown", true);
	}

	private async void CalculationsButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///calculations", true);
	}

	private async void ConvertersButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///converters", true);
	}

	private async void LocationInfoButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///locationInfo", true);
	}

	private async void CalendarButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///calendar", true);
	}
}