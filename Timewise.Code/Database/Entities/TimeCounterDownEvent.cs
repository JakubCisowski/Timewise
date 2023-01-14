namespace Timewise.Code.Database.Entities;

using Interfaces;
using Models;

/// <summary>
/// Encja reprezentująca zdarzenie odmierzania czasu w dół w aplikacji.
/// </summary>
public class TimeCounterDownEvent : IEntity
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
	/// Czas końcowy zdarzenia.
	/// </summary>
	public DateTime EndTime { get; set; }
	
	/// <summary>
	/// Id użytkownika, do którego przypisane jest zdarzenie.
	/// </summary>
	public int UserId { get; set; }
	
	/// <summary>
	/// Operator konwersji niejawnej z typu encji na typ modelu.
	/// Wywoływany automatycznie w sytuacji, gdy oczekiwany jest obiekt typu TimeCounterDownEvent.
	/// Generuje nowy obiekt modelu na podstawie obiektu encji.
	/// </summary>
	/// <param name="ev">Obiekt wczytany z bazy danych.</param>
	/// <returns>Obiekt modelu typu TimeCounterDownEvent.</returns>
	public static implicit operator Models.TimeCounterDownEvent(TimeCounterDownEvent ev)
	{
		var time = (Time)ev.EndTime;
		var duration = time - Time.Now;
		
		return new Models.TimeCounterDownEvent(ev.Name, 
			new TimeCounterDown(duration, (Time)ev.EndTime));
	}

	/// <summary>
	/// Operator konwersji jawnej z typu modelu na typ encji.
	/// Generuje nowy obiekt encji na podstawie obiektu modelu.
	/// </summary>
	/// <param name="timeCounterDownEvent">Obiekt modelu, np. wygenerowany przy pomocy interfejsu użytkownika.</param>
	/// <returns>Obiekt encji, gotowy do zapisania do bazy danych.</returns>
	public static explicit operator TimeCounterDownEvent(Models.TimeCounterDownEvent timeCounterDownEvent)
	{
		return new TimeCounterDownEvent()
		{
			Name = timeCounterDownEvent.Name,
			EndTime = timeCounterDownEvent.TimeCounterDown.EndTime,
			UserId = User.CurrentUser != null ? User.CurrentUser.Id : 0
		};
	}
}