namespace Timewise.Code.Database.Entities;

using Interfaces;
using Models;

/// <summary>
/// Encja reprezentująca zdarzenie odmierzania czasu w górę w aplikacji.
/// </summary>
public class TimeCounterUpEvent : IEntity
{
	/// <summary>
	/// Identyfikator zdarzenia, generowany z poziomu bazy danych.
	/// </summary>
	public int Id { get; set; }
	
	/// <summary>
	/// Opcjonalna nazwa zdarzenia.
	/// </summary>
	public string Name { get; set; }
	
	/// <summary>
	/// Czas początkowy zdarzenia.
	/// </summary>
	public DateTime StartTime { get; set; }
	
	/// <summary>
	/// Id użytkownika, do którego przypisane jest zdarzenie.
	/// </summary>
	public int UserId { get; set; }
	
	/// <summary>
	/// Operator konwersji niejawnej z typu encji na typ modelu.
	/// Wywoływany automatycznie w sytuacji, gdy oczekiwany jest obiekt typu TimeCounterUpEvent.
	/// Generuje nowy obiekt modelu na podstawie obiektu encji.
	/// </summary>
	/// <param name="ev">Obiekt wczytany z bazy danych.</param>
	/// <returns>Obiekt modelu typu TimeCounterUpEvent.</returns>
	public static implicit operator Models.TimeCounterUpEvent(TimeCounterUpEvent ev)
	{
		return new Models.TimeCounterUpEvent(ev.Name, new TimeCounterUp(new Time(ev.StartTime.Hour, ev.StartTime.Minute, ev.StartTime.Second,ev.StartTime.Millisecond, new Date(ev.StartTime.Day, ev.StartTime.Month, ev.StartTime.Year))));
	}

	/// <summary>
	/// Operator konwersji jawnej z typu modelu na typ encji.
	/// Generuje nowy obiekt encji na podstawie obiektu modelu.
	/// </summary>
	/// <param name="timeCounterUpEvent">Obiekt modelu, np. wygenerowany przy pomocy interfejsu użytkownika.</param>
	/// <returns>Obiekt encji, gotowy do zapisania do bazy danych.</returns>
	public static explicit operator TimeCounterUpEvent(Models.TimeCounterUpEvent timeCounterUpEvent)
	{
		return new TimeCounterUpEvent()
		{
			Name = timeCounterUpEvent.Name,
			StartTime = timeCounterUpEvent.TimeCounterUp.StartTime,
			UserId = User.CurrentUser != null ? User.CurrentUser.Id : 0
		};
	}
}