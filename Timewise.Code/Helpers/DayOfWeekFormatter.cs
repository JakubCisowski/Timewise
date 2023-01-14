namespace Timewise.Code.Helpers;

using DataTypes;

/// <summary>
/// Klasa pomocnicza, pomagająca przy formatowaniu nazw dni tygodnia.
/// Jest to klasa statyczna, a więc nie można jej instancjonować.
/// </summary>
public static class DayOfWeekFormatter
{
	/// <summary>
	/// Metoda zwracająca nazwę dnia tygodnia w formie napisu.
	/// </summary>
	/// <returns>Napis zawierający nazwę dnia tygodnia</returns>
	public static string DayOfWeekToString(DayOfWeek dayOfWeek)
	{
		switch (dayOfWeek)
		{
			case DayOfWeek.Sunday:
				return "Niedziela";
			
			case DayOfWeek.Monday:
				return "Poniedziałek";
			
			case DayOfWeek.Tuesday:
				return "Wtorek";
			
			case DayOfWeek.Wednesday:
				return "Środa";
			
			case DayOfWeek.Thursday:
				return "Czwartek";
			
			case DayOfWeek.Friday:
				return "Piątek";
			
			case DayOfWeek.Saturday:
				return "Sobota";
			
			default:
				return "Unknown";
		}
	}
}