using System;
using System.Collections.Generic;
using System.Text;
//using SQLite;

using SQLite.Net;
using SQLite.Net.Interop;
using SQLite.Net.Async;

using System.Linq;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Plant_Test
{
    public class PlantRepository
    {
        //        private SQLiteConnection database;
        //        private static object collisionLock = new object();
        private SQLiteAsyncConnection database;
        //public ObservableCollection<Plant> Plants { get; set; }

/*        public PlantDataAccess()
        {
            database = DependencyService.Get<IDatabaseConnection>().DbConnection();
            database.CreateTable<Plant>();
            Plants = new ObservableCollection<PlantData>(database.Table<Plant>());
        }
*/
        
        public PlantRepository(ISQLitePlatform sqlitePlatform, string databasePath)
        {
            if (database == null)
            {
                var connectionFunc = new Func<SQLiteConnectionWithLock>(() =>
                    new SQLiteConnectionWithLock
                    (
                        sqlitePlatform,
                        new SQLiteConnectionString(databasePath, storeDateTimeAsTicks: false)
                    ));

                database = new SQLiteAsyncConnection(connectionFunc);
                database.CreateTableAsync<Plant>();
            }

            // how do we check if the database failed?
        }
        
        public async Task<List<Plant>> GetAllPlantsAsync()
        {
            //return a list of plants saved to the Plant table in the database
            List<Plant> plants = await database.Table<Plant>().ToListAsync();

            return plants;
        }

        
        public async Task<List<Plant>> GetSomePlantsAsync()
        {
            //return a list of plants saved to the Plant table in the database
            List<Plant> plants = await database.Table<Plant>()
                .Where(p => p.FloweringMonths.Contains("Jun"))
                .ToListAsync();

            return plants;
        }

        public async Task<List<Plant>> AddPlantsAsync()
        {
            //return a list of plants saved to the Plant table in the database
            List<Plant> plants = await database.Table<Plant>().ToListAsync();

            // Test code to see if I could manually add. This works.
            int result = 0;
            result = await database.InsertAsync(new Plant { PlantName = "Shrinking Violet " + DateTime.Now.ToString() });
            plants = await database.Table<Plant>().ToListAsync();

            return plants;
        }



        //Would put any queries against the database here


    }
}
