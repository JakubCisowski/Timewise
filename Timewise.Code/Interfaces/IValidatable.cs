namespace Timewise.Code.Interfaces;

/// <summary>
/// Interfejs implementowany przez klasy powiązane z czasem.
/// Służy do określenia, czy dany czas jest poprawny.
/// </summary>
public interface IValidatable
{
	/// <summary>
	/// Czy dany czas, data lub odstęp czasowy jest poprawny, to znaczy, czy reprezentuje istniejącą datę lub nieujemny odstęp czasowy.
	/// </summary>
	public bool IsValid { get; }
}