using System;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MongoDB.Data
{
    public static class MongoDbProvider
    {
        public static MongoDatabase db
        {
            get
            {
                var connectionstring = dbSetting.Default.MONGOLAB_URI;
                string dbName = MongoUrl.Create(connectionstring).DatabaseName;
                MongoServer dbServer = MongoServer.Create(connectionstring);
                MongoDatabase dbConnection = dbServer.GetDatabase(dbName, SafeMode.True);
                return dbConnection;
            }
        }

        public static void SaveData<T>(T product)
        {
            try
            {
                MongoDbProvider.db.GetCollection<T>(typeof(T).Name).Save(product, SafeMode.True);
            }
            catch (MongoConnectionException)
            {
                throw new MongoCommandException("Cannot access database. Please try again later");
            }
        }

        public static IQueryable<T> LoadData<T>()
        {
            try
            {
                return MongoDbProvider.db.GetCollection<T>(typeof(T).Name).AsQueryable<T>();
            }
            catch (MongoConnectionException)
            {
                throw new MongoCommandException("Cannot access database. Please try again later");
            }
        }
    }
}
