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
    class Product
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string ProductName { get; set; }

        public float ProductCost { get; set; }

        public float ProductPrice { get; set; }

        public int ProductStock { get; set; }

        public string ProductPicture { get; set; }

        public override string ToString()
        {
            return string.Format("[Product: ID={0}, ProductName={1}, ProductCost={2}, ProductPrice={3}, ProductStock={4}, ProductPicture={5}]", ID, ProductName, ProductCost, ProductPrice, ProductStock, ProductPicture);
        }
    }
}