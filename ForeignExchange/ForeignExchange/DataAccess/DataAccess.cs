namespace ForeignExchange.DataAccess
{
    using ForeignExchange.Interfaces;
    using ForeignExchange.Models;
    using SQLite.Net;
    using System;
    using Xamarin.Forms;

    public class DataAccess : IDisposable
    {
        private SQLiteConnection connection;

        public DataAccess()
        {
            var config = DependencyService.Get <IConfig>();
            connection = new SQLiteConnection(config.Platform,
                System.IO.Path.Combine(config.DirectoryDB, "RatesDB.db3"));
            connection.CreateTable<Rate>();
        }

        public void Insert<T>(T model)
        {
            connection.Insert(model);
        }

        public void Update<T>(T model)
        {
            connection.Update(model);
        }

        public void Delete<T>(T model)
        {
            connection.Delete(model);
        }

        public void Dispose()
        {
            connection.Close();
            connection.Dispose();
        }
    }
}
