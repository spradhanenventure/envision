using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace ENVision
{
    class DBHandle
    {
        static SQLiteConnection connection;
        static bool opened = false;

        public DBHandle()
        {
            if (!opened)
            {
                var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                path = System.IO.Path.Combine(path, "ENVision.db");
                System.Diagnostics.Debug.WriteLine("DBPATH: " + path);
                connection = new SQLiteConnection(path);
            }

            opened = true;
        }

        public SQLiteConnection getConnection()
        {
            return connection;
        }

        public void createDatabase()
        {
            connection.CreateTable<Product>();
            connection.CreateTable<Sales>();
        }
    }
}