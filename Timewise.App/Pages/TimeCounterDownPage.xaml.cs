#pragma warning disable CS0618 // Type or member is obsolete - używamy StackPanel zamiast Grid dla automatycznego ustawiania kontrolek
namespace Timewise.App.Pages;

using Code.Database.Entities;
using Code.Database.Repositories;
using Code.Exceptions;
using Code.Helpers;
using Code.Models;
using Microsoft.Maui.Controls.Shapes;
using TimeCounterDownEvent = Code.Models.TimeCounterDownEvent;

public partial class TimeCounterDownPage : ContentPage
{
	private TimeCounterDown _timeCounter;
	public static List<TimeCounterDownEvent> TimeCounterDownEvents;

	public TimeCounterDownPage()
	{
		InitializeComponent();

		TimeCounterDownEvents = TimeCounterDownEvents ?? new();

		if (TimeCounterDownEvents.Count > 0)
		{
			foreach (var ev in TimeCounterDownEvents)
			{
				var grid = GenerateCountdownGrid(ev);
				CountdownGridsPanel.Add(grid);
			}
		}

		EventTimeZonePicker.ItemsSource = TimeZoneInfo.GetSystemTimeZones();
	}

	private Grid GenerateCountdownGrid(TimeCounterDownEvent ev)
	{
		// Create a Grid based on the above XAML code.

		var guid = Guid.NewGuid().ToString("N");

		var grid = new Grid()
		{
			StyleId = "Grid" + guid
		};
		
		var border = new Border()
		{
			StyleId = "Border" + guid,
			StrokeThickness = 2,
			Stroke = Color.FromArgb("#FF212121"),
			Padding = 5,
			StrokeShape = new RoundRectangle()
			{
				CornerRadius = new CornerRadius(10)
			},
			HeightRequest = 65,
			VerticalOptions = LayoutOptions.Center
		};

		var timeCounterLabel = new Label()
		{
			StyleId = "Label" + guid,
			HorizontalOptions = LayoutOptions.StartAndExpand,
			FontSize = 20,
			TextColor = Color.FromArgb("#FF2B0B98"),
			BindingContext = ev,
			VerticalTextAlignment = TextAlignment.Center
		};

		var timeCounterBinding = new MultiBinding()
		{
			StringFormat = "{0}:\n{1} lat {2} miesięcy {3} dni {4:00}h {5:00}m {6:00}s",
			Mode = BindingMode.OneWay
		};
		timeCounterBinding.Bindings.Add(new Binding("Name"));
		timeCounterBinding.Bindings.Add(new Binding("TimeCounterDown.TimeLeft.Years")); //https://cdn.discordapp.com/attachments/589530621244604417/1057268412105113671/image.png
		timeCounterBinding.Bindings.Add(new Binding("TimeCounterDown.TimeLeft.Months"));
		timeCounterBinding.Bindings.Add(new Binding("TimeCounterDown.TimeLeft.Days"));
		timeCounterBinding.Bindings.Add(new Binding("TimeCounterDown.TimeLeft.Hours"));
		timeCounterBinding.Bindings.Add(new Binding("TimeCounterDown.TimeLeft.Minutes"));
		timeCounterBinding.Bindings.Add(new Binding("TimeCounterDown.TimeLeft.Seconds"));

		timeCounterLabel.SetBinding(Label.TextProperty, timeCounterBinding);

		border.Content = timeCounterLabel;
		
		grid.Add(border);

		var button = new Button()
		{
			StyleId = "Button" + guid,
			Text = "Usuń",
			WidthRequest = 80,
			HeightRequest = 40,
			BindingContext = ev,
			Margin = new Thickness(380, 0, 0, 0)
		};

		button.Clicked += DeleteTimeCounterButton_Clicked;

		grid.Add(button);

		return grid;
	}

	private async void CreateEventButton_Clicked(object sender, EventArgs e)
	{
		string eventName = EventNameEntry.Text;
		var eventDate = EventDatePicker.Date;

		// Bierzemy czas tylko jeżeli jest enabled
		var eventTime = EventTimePicker.IsEnabled ? EventTimePicker.Time : System.TimeSpan.Zero;

		var eventDateTime = DateHelper.Construct(eventDate, eventTime);

		// Uwzględniamy strefę czasową
		var eventTimeZone = EventTimeZonePicker.IsEnabled ? EventTimeZonePicker.SelectedItem as TimeZoneInfo : TimeZoneInfo.Local;

		eventDateTime = TimeZoneConverter.ConvertDateTimeToTimeZone(eventDateTime, eventTimeZone, TimeZoneInfo.Local);

		var timeInTheFuture = (Time) eventDateTime;

		// Jeżeli równe to jest okej - zaczynamy licznik od ~0
		if (timeInTheFuture < Time.Now)
		{
			await DisplayAlert("Błąd", "Należy podać datę w przeszłości", "OK");
			return;
		}

		var timeDifference = timeInTheFuture - Time.Now;

		_timeCounter = new TimeCounterDown(timeDifference, timeInTheFuture);
		_timeCounter.Start();

		var timeEvent = new TimeCounterDownEvent(eventName, _timeCounter);
		TimeCounterDownEvents.Add(timeEvent);

		var grid = GenerateCountdownGrid(timeEvent);
		CountdownGridsPanel.Add(grid);

		using (var repo = new EntityRepository())
		{
			await repo.Add((Code.Database.Entities.TimeCounterDownEvent)timeEvent);
		}
	}

	private void EventTimeCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
	{
		EventTimePicker.IsEnabled = e.Value;
	}

	private async void BackButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///MainPage");
	}

	private async void DeleteTimeCounterButton_Clicked(object sender, EventArgs e)
	{
		var button = (sender as Button)!;
		var parent = (button.Parent as Grid)!;
		var parentParent = (parent.Parent as VerticalStackLayout)!;

		var timeEvent = button.BindingContext as TimeCounterDownEvent;
		timeEvent?.TimeCounterDown.Stop();
		TimeCounterDownEvents.Remove(TimeCounterDownEvents.FirstOrDefault(x => x.Id == timeEvent?.Id));

		if (timeEvent != null)
		{
			try
			{
				using (var repo = new EntityRepository())
				{
					var entities = await repo.GetAll<Code.Database.Entities.TimeCounterDownEvent>();
					
					var timeCounterDownFromDb = entities.FirstOrDefault(x => x.Name == timeEvent.Name && x.EndTime == timeEvent.TimeCounterDown.EndTime && x.UserId == User.CurrentUser.Id);

					if (timeCounterDownFromDb != null)
					{
						await repo.Delete<Code.Database.Entities.TimeCounterDownEvent>(timeCounterDownFromDb.Id);
					}
				}
			}
			catch (DatabaseException ex)
			{
				await DisplayAlert("Błąd przy usuwaniu z bazy danych", ex.Message, "OK");
			}
		}

		parentParent.Children.Remove(parent);
	}

	private void EventTimeZoneCheckBox_OnCheckedChanged(object sender, CheckedChangedEventArgs e)
	{
		EventTimeZonePicker.IsEnabled = e.Value;

		if (e.Value)
		{
			EventTimeZonePicker.SelectedItem = TimeZoneInfo.Local;
		}
	}
}