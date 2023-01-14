namespace Timewise.Code.Models;

using Plugin.LocalNotification;
using System.Windows.Input;
using Database.Entities;
using Database.Repositories;
using Microsoft.Maui.Controls.Shapes;
using XCalendar.Core.Enums;
using XCalendar.Core.Models;

/// <summary>
/// Klasa reprezentująca kalendarz.
/// </summary>
public class Calendar
{
	/// <summary>
	/// Instancja kalendarza zawierająca podstawową konfigurację.
	/// </summary>
	public Calendar<CalendarDay> MyCalendar { get; set; } = new Calendar<CalendarDay>()
	{
		// Konfiguracja wybierania w kalendarzu -> tylko jeden dzień naraz
		SelectionAction = SelectionAction.Replace, 
		SelectionType = SelectionType.Single
	};

	/// <summary>
	/// Komenda wywoływana po kliknięciu w przycisk przełączenia miesiąca na kalendarzu.
	/// </summary>
	public ICommand NavigateCalendarCommand { get; set; }

	/// <summary>
	/// Komenda wywoływana po kliknięciu w przycisk dnia (wybieranie dnia).
	/// </summary>
	public ICommand ChangeDateSelectionCommand { get; set; }

	/// <summary>
	/// Lista przypomnień.
	/// </summary>
	public static List<Reminder> Reminders { get; set; } = new List<Reminder>();

	/// <summary>
	/// Wybrana przez użytkownika data.
	/// </summary>
	public static DateTime SelectedDate { get; set; }

	/// <summary>
	/// Etykieta na interfejsie wyświetlająca wybraną datę.
	/// </summary>
	public Label SelectedDateLabel { get; set; }

	/// <summary>
	/// Kontener interfejsu przechowujący listę powiadomień dla danego dnia.
	/// </summary>
	public VerticalStackLayout RemindersStackLayout { get; set; }

	/// <summary>
	/// Konstruktor inicjalizujący kalendarz.
	/// </summary>
	/// <param name="selectedDateLabel">Etykieta na interfejsie wyświetlająca wybraną datę.</param>
	/// <param name="remindersStackLayout">Kontener interfejsu przechowujący listę powiadomień dla danego dnia.</param>
	public Calendar(Label selectedDateLabel, VerticalStackLayout remindersStackLayout)
	{
		NavigateCalendarCommand = new Command<int>(NavigateCalendar);
		ChangeDateSelectionCommand = new Command<DateTime>(ChangeDateSelection);

		SelectedDateLabel = selectedDateLabel;
		RemindersStackLayout = remindersStackLayout;
	}

	/// <summary>
	/// Metoda przełączająca miesiąc na kalendarzu.
	/// </summary>
	/// <param name="amount">Ilość miesięcy</param>
	public void NavigateCalendar(int amount)
	{
		MyCalendar?.NavigateCalendar(amount);
	}

	/// <summary>
	/// Metoda dodajaca przypomnienie.
	/// </summary>
	/// <param name="reminder">Dodawane przypomnienie</param>
	public void AddReminder(Reminder reminder)
	{
		Reminders.Add(reminder);
		
		// Generate StackLayout entry
		GenerateReminderUI(reminder);
	}

	/// <summary>
	/// Metoda generująca interfejs przypomnienia.
	/// </summary>
	/// <param name="reminder">Dodawane przypomnienie</param>
	private void GenerateReminderUI(Reminder reminder)
	{
		var border = GenerateReminderBorder(reminder);
		
		RemindersStackLayout.Children.Add(border);
	}

	/// <summary>
	/// Metoda usuwająca przypomnienie.
	/// </summary>
	/// <param name="reminder">Usuwane przypomnienie</param>
	public void RemoveReminder(Reminder reminder)
	{
		Reminders.Remove(reminder);

		GenerateAllRemindersUI(SelectedDate);
	}

	/// <summary>
	/// Metoda generująca interfejs wszystkich przypomnień.
	/// </summary>
	private void GenerateAllRemindersUI(DateTime todayDate)
	{
		RemindersStackLayout.Children.Clear();
		
		var remindersToday = Reminders.Where(r => r.DateTime.Date == todayDate.Date).ToList();
		
		foreach (var reminder in remindersToday)
		{
			var border = GenerateReminderBorder(reminder);

			RemindersStackLayout.Children.Add(border);
		}
	}

	/// <summary>
	/// Metoda generująca interfejs przypomnienia.
	/// </summary>
	/// <param name="reminder">Przypomnienie do wygenerowania na interfejsie.</param>
	/// <returns>Interfejs przypomnienia.</returns>
	private Border GenerateReminderBorder(Reminder reminder)
	{
		var guid = Guid.NewGuid().ToString("N");
		
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
			HeightRequest = 60,
			VerticalOptions = LayoutOptions.Center
		};

		var label = new Label()
		{
			StyleId = "Label" + guid,
			HorizontalOptions = LayoutOptions.CenterAndExpand,
			FontSize = 18,
			TextColor = Color.FromArgb("#FF2B0B98"),
			Text = $"{reminder.DateTime:HH:mm} - {reminder.Description}",
			VerticalTextAlignment = TextAlignment.Center
		};

		var button = new Button()
		{
			StyleId = "Button" + guid,
			Text = "Usuń",
			WidthRequest = 70,
			HeightRequest = 40,
			BindingContext = reminder,
			Margin = new Thickness(5, 0, 10, 0)
		};
		button.Clicked += ButtonOnClicked;

		var horizontalStackLayout = new HorizontalStackLayout()
		{
			VerticalOptions = LayoutOptions.Center
		};

		horizontalStackLayout.Children.Add(button);
		horizontalStackLayout.Children.Add(label);

		border.Content = horizontalStackLayout;

		return border;
	}

	/// <summary>
	/// Metoda usuwająca przypomnienie.
	/// </summary>
	/// <param name="sender">Sender zdarzenia (przycisk przy konkretntym przypomnieniu).</param>
	/// <param name="e">Argumenty zdarzenia.</param>
	private async void ButtonOnClicked(object sender, EventArgs e)
	{
		var reminder = (sender as Button).BindingContext as Reminder;
		
		RemoveReminder(reminder);

		using (var repo = new EntityRepository())
		{
			var reminders = await repo.GetAll<Reminder>();
			var reminderFromDb = reminders.FirstOrDefault(x => x.DateTime == reminder.DateTime && x.Description == reminder.Description && x.UserId == User.CurrentUser.Id);

			if (reminderFromDb != null)
			{
				await repo.Delete<Reminder>(reminderFromDb.Id);
			}
		}
	}

	/// <summary>
	/// Metoda ustawiająca wybraną datę.
	/// </summary>
	/// <param name="dateTime">Wybrana data.</param>
	public void ChangeDateSelection(DateTime dateTime)
	{
		SelectedDate = dateTime;
		
		MyCalendar?.ChangeDateSelection(dateTime);

		SelectedDateLabel.Text = $"Wybrana data: {dateTime.ToLongDateString()}";
		
		GenerateAllRemindersUI(dateTime);

#if ANDROID
		CreateNotificationAndroid(dateTime);
#endif
	}

	/// <summary>
	/// Metoda asynchroniczna tworząca lokalne powiadomienie na systemie Android.
	/// </summary>
	/// <param name="dateTime">Czas powiadomienia.</param>
	public async void CreateNotificationAndroid(DateTime dateTime)
	{
		var notification = new NotificationRequest
		{
			NotificationId = 100,
			Title = "Test",
			Description = "Test Description",
			ReturningData = "Dummy data", // Returning data when tapped on notification.
			Schedule =
			{
				NotifyTime = DateTime.Now.AddSeconds(1) // Used for Scheduling local notification, if not specified notification will show immediately.
			}
		};
		await LocalNotificationCenter.Current.Show(notification);
	}
}