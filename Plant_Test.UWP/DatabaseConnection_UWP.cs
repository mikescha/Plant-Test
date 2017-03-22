using SQLite;
using Xamarin.Forms;
using Plant_Test.UWP;
using Windows.Storage;
using System.IO;

[assembly: Dependency(typeof(DatabaseConnection_UWP))]
namespace Plant_Test.UWP
{
    public class DatabaseConnection_UWP : IDatabaseConnection
    {
        public SQLiteConnection DbConnection()
        {
            var dbName = "miniplant.db3";
            var path = Path.Combine(ApplicationData.
                Current.LocalFolder.Path, dbName);
            return new SQLiteConnection(path);
        }
    }
}
