namespace Timewise.App;

#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
#endif

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();

		Application.Current.UserAppTheme = AppTheme.Light;

		Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(nameof(IWindow),
			(handler, view) =>
			{
	#if WINDOWS
				var nativeWindow = handler.PlatformView;
				var windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
				var windowId = Win32Interop.GetWindowIdFromWindow(windowHandle);
				var appWindow = AppWindow.GetFromWindowId(windowId);
				var presenter = appWindow.Presenter as OverlappedPresenter;
				presenter.IsResizable = false;
				presenter.IsMaximizable = false;
#endif
			});
	}

	protected override Window CreateWindow(IActivationState activationState)
	{
		var window = base.CreateWindow(activationState);

		// Pixel 5: 1080x2340
		// iPhone 12 Mini: 1080x2340
		// Default: 540x780
		const int newWidth = 540;
		const int newHeight = 780;

		window.Width = newWidth;
		window.Height = newHeight;

		

		return window;
	}
}