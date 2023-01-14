namespace Timewise.Code.Models;

/// <summary>
/// Klasa reprezentująca zdarzenie, do którego jest odliczany czas.
/// Posiada swoje unikatowe Id, nazwę zdarzenia oraz powiązany z nim zegar (TimeCounterDown).
/// </summary>
public class TimeCounterDownEvent
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
	/// Zegar odmierzający czas w dół, powiązany ze zdarzeniem.
	/// </summary>
	public TimeCounterDown TimeCounterDown { get; init; }

	/// <summary>
	/// Domyślny konstruktor, generujący nowe Id, ustawiający nazwę oraz zegar.
	/// </summary>
	/// <param name="name">Nazwa zdarenia, może być pusta lub null.</param>
	/// <param name="timeCounterDown">Stworzony wcześniej obiekt zegara.</param>
	public TimeCounterDownEvent(string name, TimeCounterDown timeCounterDown)
	{
		Id = Guid.NewGuid();
		Name = name;
		TimeCounterDown = timeCounterDown;
	}
}