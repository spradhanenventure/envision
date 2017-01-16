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
using Java.Text;
using Java.Util;
using static Android.App.DatePickerDialog;

namespace ENVision
{
    [Activity(Label = "Sales")]
    public class POSActivity : ParentActivity, DatePickerDialog.IOnDateSetListener
    {
        List<DataTwoItems> items;
        Button b_date;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.POSActivity);
            initMenu();

            handleDatePicker();

            updateListBox();

            Button b_weekly = FindViewById<Button>(Resource.Id.b_weekly);
            b_weekly.Click += delegate
            {
                Intent intent = new Intent(this, typeof(WeeklyReportActivity));
                StartActivity(intent);
            };
        }

        public void handleDatePicker()
        {
            b_date = FindViewById<Button>(Resource.Id.b_date);
            SimpleDateFormat dateFormatter = new SimpleDateFormat("yyyy-MM-dd", Locale.Us);

            String today = Intent.GetStringExtra("date") ?? dateFormatter.Format(Calendar.GetInstance(Locale.Us).Time);

            if (b_date.Text.Equals("0"))
                b_date.Text = today;

            b_date.Click += delegate
            {
                DateTime currently = DateTime.Parse(b_date.Text);
                DatePickerDialog datePicker = new DatePickerDialog(this, this, currently.Year, currently.Month-1, currently.Day);
                datePicker.Show();
            };
        }

        public void updateListBox()
        {
            DBHandle handle = new DBHandle();
            SQLiteConnection connection = handle.getConnection();

            var query = connection.Query<Product>("SELECT * FROM Product");

            items = new List<DataTwoItems>();
            
            foreach (Product p in query)
            {
                String saleAmount = "0";

                var q = connection.Query<Sales>("SELECT * FROM Sales WHERE ProductID = ? AND SaleDate = ?", p.ID, b_date.Text + " 00:00:00");
                if(q.Count > 0)
                {
                    Sales s = q.First<Sales>();
                    saleAmount = s.SaleQuantity.ToString();
                }

                DataTwoItems dt = new DataTwoItems(p.ID, p.ProductPicture, p.ProductName, saleAmount);
                dt.item3 = p.ProductCost.ToString();
                dt.item4 = p.ProductPrice.ToString();
                dt.item5 = p.ProductStock.ToString();
                dt.item6 = p.ProductPicture;
                items.Add(dt);
            }

            LVPOSAdapter adapter = new LVPOSAdapter(this, items, b_date.Text);

            ListView lv = FindViewById<ListView>(Resource.Id.lv_products);
            lv.Adapter = adapter;
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            SimpleDateFormat dateFormatter = new SimpleDateFormat("yyyy-MM-dd", Locale.Us);
            Calendar newDate = Calendar.GetInstance(Locale.Us);
            newDate.Set(year, monthOfYear, dayOfMonth);

            b_date.Text = dateFormatter.Format(newDate.Time);
            //Action action = delegate { b_date.Text = dateFormatter.Format(newDate.Time); };
            //b_date.Post(action);

            updateListBox();
        }
    }
}