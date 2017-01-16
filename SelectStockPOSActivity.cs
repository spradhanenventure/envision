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
    [Activity(Label = "Select Quantity")]
    public class SelectStockPOSActivity : Activity
    {
        int qty;
        int max;
        int id;
        string date;

        DBHandle handle;
        SQLiteConnection connection;

        Product product;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.SelectStockPOSActivity);

            id = Intent.GetIntExtra("id", 0);
            qty = Intent.GetIntExtra("qty", 0);
            max = Intent.GetIntExtra("max", 0);
            date = Intent.GetStringExtra("date") ?? "";

            handle = new DBHandle();
            connection = handle.getConnection();

            product = connection.Get<Product>(id);

            Button b_plus = FindViewById<Button>(Resource.Id.b_plus);
            Button b_minus = FindViewById<Button>(Resource.Id.b_minus);
            Button b_cancel = FindViewById<Button>(Resource.Id.b_cancel);
            Button b_save = FindViewById<Button>(Resource.Id.b_save);
            TextView tv_qty = FindViewById<TextView>(Resource.Id.tv_qty);
            TextView tv_label = FindViewById<TextView>(Resource.Id.tv_label);

            tv_label.Text = "Select Quantity (Stock: " + product.ProductStock + ")";

            b_plus.Click += delegate
            {
                if(qty < max)
                    qty++;

                tv_qty.Text = qty + "";
            };

            b_minus.Click += delegate
            {
                if(qty > 0)
                    qty--;

                tv_qty.Text = qty + "";
            };

            b_cancel.Click += delegate
            {
                Intent intent = new Intent(this, typeof(POSActivity));
                intent.PutExtra("date", date);
                StartActivity(intent);
            };

            b_save.Click += delegate
            {
                String[] tmp = date.Split('-');
                int year = int.Parse(tmp[0]);
                int month = int.Parse(tmp[1]);
                int day = int.Parse(tmp[2]);
                
                product.ProductStock -= qty;
                connection.Update(product);

                var q = connection.Query<Sales>("SELECT * FROM Sales WHERE ProductID = ? AND SaleDate = ?", id, date + " 00:00:00");

                if (q.Count > 0) //Update
                {
                    Sales s = q.First<Sales>();
                    s.SaleQuantity = qty;

                    connection.Update(s);
                }
                else //Insert
                {
                    Sales s = new Sales();
                    s.ProductID = id;
                    s.SaleDate = new DateTime(year, month, day);
                    s.SaleCost = product.ProductCost;
                    s.SalePrice = product.ProductPrice;
                    s.SaleQuantity = qty;

                    connection.Insert(s);
                }

                Intent intent = new Intent(this, typeof(POSActivity));
                intent.PutExtra("date", date);
                StartActivity(intent);
            };
        }
    }
}