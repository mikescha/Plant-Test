using System;
using System.Collections.Generic;
using System.Text;

namespace Plant_Test
{
    public class Data
    {
    }

    public interface IDatabaseConnection
    {
        SQLite.SQLiteConnection DbConnection();
    }

}
