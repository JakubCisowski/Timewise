namespace Timewise.App.Pages;

using Code.Database.Repositories;
using Code.Exceptions;
using Code.Helpers;
using Code.Models;
using Microsoft.Maui.Controls.Shapes;
using Timewise.Code.Database.Entities;

public partial class TimeCounterUpPage : ContentPage
{
	private TimeCounterUp _timeCounter;
	public static List<Code.Models.TimeCounterUpEvent> TimeCounterUpEvents;

	public TimeCounterUpPage()
	{
		InitializeComponent();

		TimeCounterUpEvents = TimeCounterUpEvents ?? new();

		if (TimeCounterUpEvents.Count > 0)
		{
			foreach (var ev in TimeCounterUpEvents)
			{
				var grid = GenerateTimeCounterGrid(ev);
				TimeCounterGridsPanel.Add(grid);
			}
		}

		EventTimeZonePicker.ItemsSource = TimeZoneInfo.GetSystemTimeZones();
	}

	private Grid GenerateTimeCounterGrid(Code.Models.TimeCounterUpEvent ev)
	{
		
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
		timeCounterBinding.Bindings.Add(new Binding("TimeCounterUp.TimeSince.Years")); //https://cdn.discordapp.com/attachments/589530621244604417/1057268412105113671/image.png
		timeCounterBinding.Bindings.Add(new Binding("TimeCounterUp.TimeSince.Months"));
		timeCounterBinding.Bindings.Add(new Binding("TimeCounterUp.TimeSince.Days"));
		timeCounterBinding.Bindings.Add(new Binding("TimeCounterUp.TimeSince.Hours"));
		timeCounterBinding.Bindings.Add(new Binding("TimeCounterUp.TimeSince.Minutes"));
		timeCounterBinding.Bindings.Add(new Binding("TimeCounterUp.TimeSince.Seconds"));

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

		// zmienić format wyświetlania DatePicker i TimePicker
		var timeThen = (Time) eventDateTime;

		// Jeżeli równe to jest okej - zaczynamy licznik od ~0
		if (timeThen > Time.Now)
		{
			await DisplayAlert("Błąd", "Należy podać datę w przeszłości.", "OK");
			return;
		}

		var timeDifference = DateHelper.TimeSince(timeThen);

		_timeCounter = new TimeCounterUp(timeDifference, timeThen);
		_timeCounter.Start();

		var timeEvent = new Code.Models.TimeCounterUpEvent(eventName, _timeCounter);
		TimeCounterUpEvents.Add(timeEvent);

		var grid = GenerateTimeCounterGrid(timeEvent);
		TimeCounterGridsPanel.Add(grid);

		if (User.CurrentUser != null)
		{
			using (var repo = new EntityRepository())
			{
				await repo.Add((Code.Database.Entities.TimeCounterUpEvent) timeEvent);
			}
		}
	}

	private void EventTimeCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
	{
		EventTimePicker.IsEnabled = e.Value;
	}

	private async void BackButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///MainPage", true);
	}

	private async void DeleteTimeCounterButton_Clicked(object sender, EventArgs e)
	{
		var button = (sender as Button)!;
		var parent = (button.Parent as Grid)!;
		var parentParent = (parent.Parent as VerticalStackLayout)!;

		var timeEvent = button.BindingContext as Code.Models.TimeCounterUpEvent;
		timeEvent?.TimeCounterUp.Stop();
		TimeCounterUpEvents.Remove(TimeCounterUpEvents.FirstOrDefault(x => x.Id == timeEvent?.Id));
		
		if (timeEvent != null)
		{
			try
			{
				using (var repo = new EntityRepository())
				{
					var entities = await repo.GetAll<Code.Database.Entities.TimeCounterUpEvent>();
					
					var timeCounterUpFromDb = entities.FirstOrDefault(x => x.Name == timeEvent.Name && x.StartTime == timeEvent.TimeCounterUp.StartTime);

					if (timeCounterUpFromDb != null)
					{
						await repo.Delete<Code.Database.Entities.TimeCounterUpEvent>(timeCounterUpFromDb.Id);
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