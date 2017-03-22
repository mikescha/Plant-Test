using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using SQLite.Net.Platform.XamarinAndroid;

namespace Plant_Test.Droid
{
	[Activity (Label = "Plant_Test", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
            //System.Diagnostics.Debug.WriteLine("Starting!");
            base.OnCreate (bundle);
            
            global::Xamarin.Forms.Forms.Init (this, bundle);
            string dbPath = FileAccessHelper.GetLocalFilePath("miniplant.db3");

            LoadApplication (new Plant_Test.App (dbPath, new SQLitePlatformAndroid ()));
		}
	}
}

