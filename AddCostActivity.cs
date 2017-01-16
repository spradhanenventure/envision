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
    [Activity(Label = "Manage Cost")]
    public class AddCostActivity : ParentActivity
    {
        List<DataTwoItems> items;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.AddCostActivity);
            initMenu();

            DBHandle handle = new DBHandle();
            SQLiteConnection connection = handle.getConnection();
            
            var query = connection.Query<Product>("SELECT * FROM Product");

            items = new List<DataTwoItems>();

            foreach (Product p in query)
            {
                DataTwoItems dt = new DataTwoItems(p.ID, p.ProductPicture, p.ProductName, p.ProductCost.ToString());
                dt.item3 = p.ProductPrice.ToString();
                dt.item4 = p.ProductStock.ToString();
                dt.item5 = p.ProductPicture;
                items.Add(dt);
            }
            
            LVTextButtonAdapter adapter = new LVTextButtonAdapter(this, items, new Intent(this, typeof(EditSingleCostActivity)));

            ListView lv = FindViewById<ListView>(Resource.Id.lv_products);
            lv.Adapter = adapter;
            
            //Handling Events
            Button b_next = FindViewById<Button>(Resource.Id.b_next);

            b_next.Click += delegate
            {
                Intent intent = new Intent(this, typeof(AddPriceActivity));
                StartActivity(intent);
            };
        }
    }
}