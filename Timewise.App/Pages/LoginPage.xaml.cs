namespace Timewise.App.Pages;

using Code.Database.Repositories;
using Code.Helpers;
using System.Diagnostics;
using Code.Exceptions;
using Timewise.Code.Database.Entities;
using Timewise.Code.Database.Helpers;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();

		if (Debugger.IsAttached)
		{
			UsernameEntry.Text = "admin";
			PasswordEntry.Text = "admin";
		}
	}

	private async void LoginButton_Clicked(object sender, EventArgs e)
	{
		var username = UsernameEntry.Text;
		var password = PasswordEntry.Text;

		if (username == null | password == null)
		{
			await DisplayAlert("B³¹d", "Nazwa u¿ytkownika ani has³o nie mo¿e byæ puste.", "OK");
			return;
		}

		var isUsernameValid = username.Length >= 3 && username.Length <= 12;
		var isPasswordValid = password.Length >= 3 && password.Length <= 64;

		if (isUsernameValid && isPasswordValid)
		{
			try
			{
				var isUserValid = await UserAccountHelper.IsUserValidAsync(username, PasswordEncryptionHelper.GenerateHash(password));

				if (isUserValid.Exists)
				{
					User.CurrentUser = isUserValid.OutUser;
					await Shell.Current.GoToAsync("///MainPage", true);
				}
				else
				{
					await DisplayAlert("B³¹d", "Nieprawid³owa nazwa u¿ytkownika lub has³o", "OK");
				}
			}
			catch (DatabaseException ex)
			{
				await DisplayAlert("B³¹d przy wczytaniu z bazy danych", ex.Message, "OK");
			}
		}
		else
		{
			await DisplayAlert("B³¹d", "Nazwa u¿ytkownika musi byæ d³ugoœci od 3 do 12 znaków, a has³o - od 3 do 64.", "OK");
		}
	}

	private async void RegisterButton_Clicked(object sender, EventArgs e)
	{
		RegisterButton.IsEnabled = false;
		
		var username = UsernameEntry.Text;
		var password = PasswordEntry.Text;

		if (username == null || password == null)
		{
			await DisplayAlert("B³¹d", "Nazwa u¿ytkownika ani has³o nie mo¿e byæ puste.", "OK");
			RegisterButton.IsEnabled = true;
			return;
		}

		var isUsernameValid = username.Length >= 3 && username.Length <= 12;
		var isPasswordValid = password.Length >= 3 && password.Length <= 64;

		if (isUsernameValid && isPasswordValid)
		{
			try
			{
				var isUsernameTaken = await UserAccountHelper.IsUsernameTaken(username);

				if (isUsernameTaken)
				{
					await DisplayAlert("B³¹d", "Wybrana nazwa u¿ytkownika ju¿ istnieje", "OK");
				}
				else
				{
					var user = await UserAccountHelper.CreateUserAccount(username, PasswordEncryptionHelper.GenerateHash(password));
					User.CurrentUser = user;
					await Shell.Current.GoToAsync("///MainPage", true);
				}
			}
			catch (DatabaseException ex)
			{
				await DisplayAlert("B³¹d przy wczytaniu z bazy danych", ex.Message, "OK");
			}
		}

		RegisterButton.IsEnabled = true;
	}

	private async void EnterWithoutLoggingInButton_Clicked(object sender, EventArgs e)
	{
		bool result = await DisplayAlert("Jesteœ pewien?", "Wejœcie do aplikacji bez logownia sprawia, ¿e ¿adne dane nie zostan¹ zapisane - po wyjœciu z aplikacji, wszystkie zegary zostan¹ utracone. Kontynuowaæ?", "Tak", "Nie");

		if (result)
		{
			await Shell.Current.GoToAsync("///MainPage", true);
		}
	}
}