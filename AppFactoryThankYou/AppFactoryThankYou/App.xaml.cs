using Xamarin.Forms;

namespace AppFactoryThankYou
{
	public partial class App : Application
	{
		public const string SleepMessage = "Sleep";

		public App()
		{
			InitializeComponent();

			MainPage = new NavigationPage(new MainPage());
		}

		protected override void OnSleep()
		{
			base.OnSleep();

			MessagingCenter.Send(this, SleepMessage);
		}
	}
}
