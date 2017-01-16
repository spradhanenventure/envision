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
    [Activity(Label = "Edit Price")]
    public class EditSinglePriceActivity : Activity
    {
        private int id;
        private String name;
        private float amount;

        DBHandle handle;
        Product product;
        SQLiteConnection connection;

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

            handle = new DBHandle();
            connection = handle.getConnection();

            product = connection.Get<Product>(id);

            tv_label.Text = "Enter price of " + name + " (Cost: "+product.ProductCost+"):";

            if(amount > 0)
                et_value.Text = amount + "";

            b_cancel.Click += delegate
            {
                Intent intent = new Intent(this, typeof(AddPriceActivity));
                StartActivity(intent);
            };

            b_save.Click += delegate
            {
                product.ProductPrice = float.Parse((et_value.Text.Equals("")) ? "0" : et_value.Text);

                if(product.ProductPrice <= product.ProductCost)
                {
                    new AlertDialog.Builder(this)
                   .SetMessage("Price must be higher than the cost!")
                   .Show();
                    return;
                }

                var query = connection.Query<Product>("SELECT * FROM Product where ID = ?", id);
                connection.Update(product);

                Intent intent = new Intent(this, typeof(AddPriceActivity));
                StartActivity(intent);
            };
        }
    }
}