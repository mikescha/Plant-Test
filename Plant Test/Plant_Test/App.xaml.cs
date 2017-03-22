using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite.Net.Interop;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Plant_Test
{
	public partial class App : Application
	{
        public static PlantRepository PlantData { get; set; }

		public App (string dbPath, ISQLitePlatform sqlitePlatform)
		{
			InitializeComponent();

            PlantData = new PlantRepository(sqlitePlatform, dbPath);
            MainPage = new Plant_Test.MainPage();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
