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
    class Sales
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public int ProductID { get; set; }

        public DateTime SaleDate { get; set; }

        public int SaleQuantity { get; set; }

        public float SaleCost { get; set; }

        public float SalePrice { get; set; }

        public override string ToString()
        {
            return string.Format("[Sales: ID={0}, ProductID={1}, SaleDate={2}, SaleQuantity={3}, SaleCost={4}, SalePrice={5}]", ID, ProductID, SaleDate, SaleQuantity, SaleCost, SalePrice);
        }
    }
}