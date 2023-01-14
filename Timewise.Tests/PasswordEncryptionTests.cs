namespace Timewise.Tests;

using Code.Helpers;

[TestFixture]
public class PasswordEncryptionTests
{
	[Test]
	public void PasswordEncryption_EncryptsCorrectly()
	{
		string password1 = "PASSWORD";
		string password2 = "ASDF1234";

		string hash1 = PasswordEncryptionHelper.GenerateHash(password1);
		string hash2 = PasswordEncryptionHelper.GenerateHash(password2);
		
		Assert.That(hash1, Is.Not.Empty);
		Assert.That(hash2, Is.Not.Empty);
		
		Assert.That(hash1, Is.Not.EqualTo(hash2));
	}
	
	[Test]
	public void PasswordEncryption_ComparesCorrectly()
	{
		string password1 = "PASSWORD";
		string password2 = "PASSWORD";

		string hash1 = PasswordEncryptionHelper.GenerateHash(password1);
		string hash2 = PasswordEncryptionHelper.GenerateHash(password2);
		
		Assert.That(PasswordEncryptionHelper.CompareHashes(hash1, hash2), Is.True);
	}

	[Test]
	public void PasswordEncryption_EmojiEncryption()
	{
		string password = "123😂😂😂";

		string hash = PasswordEncryptionHelper.GenerateHash(password);

		Assert.That(hash, Is.Not.Empty);
	}
}