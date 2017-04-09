using System.Threading.Tasks;
using Xamarin.Forms;

namespace PlantIO
{
	public partial class App : Application
	{
        public static Task NotificationService { get; internal set; }

        public App()
		{
			InitializeComponent();

			MainPage = new PlantIOPage();
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
