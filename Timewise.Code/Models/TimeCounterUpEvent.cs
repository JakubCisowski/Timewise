namespace Timewise.Code.Models;

/// <summary>
/// Klasa reprezentująca zdarzenie, od którego jest odmierzany czas.
/// Posiada swoje unikatowe Id, nazwę zdarzenia oraz powiązany z nim zegar (TimeCounterUp).
/// </summary>
public class TimeCounterUpEvent
{
	/// <summary>
	/// Unikatowy identyfikator zdarzenia.
	/// Pozwala zidentyfikować go na interfejsie użytkownika. Kontrolki powiązane z tym zdarzeniem posiadają w nazwie ten identyfikator.
	/// </summary>
	public Guid Id { get; }
	
	/// <summary>
	/// Nazwa zdarzenia, wybrana przez użytkownika. Może być pusta - zdarzenie nie musi mieć nazwy.
	/// </summary>
	public string Name { get; init; }
	
	/// <summary>
	/// Zegar odmierzający czas w górę, powiązany ze zdarzeniem.
	/// </summary>
	public TimeCounterUp TimeCounterUp { get; init; }

	/// <summary>
	/// Domyślny konstruktor, generujący nowe Id, ustawiający nazwę oraz zegar.
	/// </summary>
	/// <param name="name">Nazwa zdarenia, może być pusta lub null.</param>
	/// <param name="timeCounterUp">Stworzony wcześniej obiekt zegara.</param>
	public TimeCounterUpEvent(string name, TimeCounterUp timeCounterUp)
	{
		Id = Guid.NewGuid();
		Name = name;
		TimeCounterUp = timeCounterUp;
	}
}