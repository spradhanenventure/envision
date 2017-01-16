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
    [Activity(Label = "Edit Cost")]
    public class EditSingleCostActivity : Activity
    {
        private int id;
        private String name;
        private float amount;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.EditScreenTVGeneric);

            Button b_cancel = FindViewById<Button>(Resource.Id.b_cancel);
            Button b_save = FindViewById<Button>(Resource.Id.b_save);
            TextView tv_label = FindViewById<TextView>(Resource.Id.tv_label);
            EditText et_value = FindViewById<EditText>(Resource.Id.et_value);

            id = Intent.GetIntExtra("id", 0);
            name = Intent.GetStringExtra("name");
            amount = Intent.GetFloatExtra("item", 0);

            tv_label.Text = "Enter cost of " + name + ":";

            if(amount > 0)
                et_value.Text = amount + "";

            b_cancel.Click += delegate 
            {
                Intent intent = new Intent(this, typeof(AddCostActivity));
                StartActivity(intent);
            };

            b_save.Click += delegate
            {
                DBHandle handle = new DBHandle();
                SQLiteConnection connection = handle.getConnection();

                var query = connection.Query<Product>("SELECT * FROM Product where ID = ?", id);

                Product p = query.First<Product>();
                
                p.ProductCost = float.Parse((et_value.Text.Equals(""))? "0" : et_value.Text);
                connection.Update(p);

                Intent intent = new Intent(this, typeof(AddCostActivity));
                StartActivity(intent);
            };
        }
    }
}