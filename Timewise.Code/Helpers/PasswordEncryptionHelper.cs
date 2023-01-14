namespace Timewise.Code.Helpers;

using System.Security.Cryptography;

/// <summary>
/// Klasa pomocnicza zajmująca się szyfrowaniem haseł użytkownika.
/// Jest to klasa statyczna, a więc nie można jej instancjonować.
/// </summary>
public static class PasswordEncryptionHelper
{
	/// <summary>
	/// Metoda szyfrująca podane hasło i generująca hash.
	/// Tak wygenerowany hash jest następnie przechowywany w bazie danych w miejscu hasła.
	/// Metoda używa algorytmu szyfrowania SHA256.
	/// </summary>
	/// <param name="password">Hasło wpisane przez użytkownika.</param>
	/// <returns>Hash wygenerowany algorytmem SHA256.</returns>
	public static string GenerateHash(string password)
	{
		byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);

		var hashAlgorithm = SHA256.Create();
		
		byte[] hashBytes = hashAlgorithm.ComputeHash(passwordBytes);
		
		return Convert.ToBase64String(hashBytes);
	}

	/// <summary>
	/// Metoda wykorzystywana do porównania dwóch hashów.
	/// </summary>
	/// <param name="hash1">Pierwszy z przekazanych hashów.</param>
	/// <param name="hash2">Drugi z przekazanych hashów.</param>
	/// <returns>True, jeżeli zaszyfrowane hasła są identyczne. W przeciwnym wypadku false.</returns>
	public static bool CompareHashes(string hash1, string hash2)
	{
		return hash1.Equals(hash2);
	}
}