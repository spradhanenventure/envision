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
using Java.Text;
using Java.Util;
using SQLite;
using Android.Graphics;

namespace ENVision
{
    [Activity(Label = "Sales Report")]
    public class ReportActivity : ParentActivity, DatePickerDialog.IOnDateSetListener
    {
        Button b_startDate;
        Button b_endDate;
        Button b_selected;

        SimpleDateFormat dateFormatter = new SimpleDateFormat("yyyy-MM-dd", Locale.Us);

        List<DataTwoItems> items;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.ReportActivity);
            initMenu();

            initDates();
            updateListBox();

            Button b_share = FindViewById<Button>(Resource.Id.b_share);
            b_share.Click += delegate
            {
                View rootView = FindViewById(Resource.Id.ll_report);
                Bitmap bitmap = getScreenShot(rootView);
                String filePath = store(bitmap, "screenshot-" + Guid.NewGuid() + ".png");
                shareImage(filePath);
            };
        }

        private void initDates()
        {
            b_startDate = FindViewById<Button>(Resource.Id.b_start);
            b_endDate = FindViewById<Button>(Resource.Id.b_end);

            b_endDate.Text = dateFormatter.Format(Calendar.GetInstance(Locale.Us).Time);

            DateTime today = DateTime.Parse(b_endDate.Text);
            //int dateSubtractor = ((int)today.DayOfWeek - 1) * -1;
            DateTime start = today.AddDays(-7);
            Calendar newDate = Calendar.GetInstance(Locale.Us);
            newDate.Set(start.Year, start.Month - 1, start.Day);

            b_startDate.Text = dateFormatter.Format(newDate.Time);

            b_startDate.Click += delegate
            {
                DateTime currently = DateTime.Parse(b_startDate.Text);
                DatePickerDialog datePicker = new DatePickerDialog(this, this, currently.Year, currently.Month - 1, currently.Day);
                b_selected = b_startDate;
                datePicker.Show();
            };

            b_endDate.Click += delegate
            {
                DateTime currently = DateTime.Parse(b_endDate.Text);
                DatePickerDialog datePicker = new DatePickerDialog(this, this, currently.Year, currently.Month - 1, currently.Day);
                b_selected = b_endDate;
                datePicker.Show();
            };
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            SimpleDateFormat dateFormatter = new SimpleDateFormat("yyyy-MM-dd", Locale.Us);
            Calendar newDate = Calendar.GetInstance(Locale.Us);
            newDate.Set(year, monthOfYear, dayOfMonth);

            b_selected.Text = dateFormatter.Format(newDate.Time);

            updateListBox();
        }

        private void updateListBox()
        {
            DBHandle handle = new DBHandle();
            SQLiteConnection connection = handle.getConnection();
            
            var q = connection.Query<Sales>("SELECT * FROM Sales WHERE SaleDate >= ? AND SaleDate <= ?", b_startDate.Text + " 00:00:00", b_endDate.Text + " 00:00:00");
            
            float totalEarned = 0;
            float totalSpent = 0;
            float profit = 0;
            double growth = 0;
            double loss = 0;

            if (q.Count > 0)
            {
                foreach(Sales sale in q)
                {
                    totalEarned += sale.SalePrice * sale.SaleQuantity;
                    //totalSpent += sale.SaleCost * sale.SaleQuantity;
                    //profit += ((sale.SalePrice - sale.SaleCost) * sale.SaleQuantity);
                    //double figure = Math.Round(profit / totalEarned * 100, 2);

                    //if (profit < 0)
                      //  loss = figure;
                    //else
                      //  growth = figure;
                }
            }

            var q2 = connection.Query<Product>("SELECT * FROM Product");

            if(q.Count > 0)
            {
                foreach(Product product in q2)
                {
                    totalSpent += product.ProductCost * product.ProductStock;
                }
            }

            TextView tvearned = FindViewById<TextView>(Resource.Id.tv_earned);
            tvearned.Text = totalEarned + "";

            TextView tvspent = FindViewById<TextView>(Resource.Id.tv_spent);
            tvspent.Text = totalSpent + "";

            TextView tvprofit = FindViewById<TextView>(Resource.Id.tv_profit);
            tvprofit.Text = (totalEarned - totalSpent) + "";

            TextView tvgrowth = FindViewById<TextView>(Resource.Id.tv_growth);
            tvgrowth.Text = growth + "%";

            TextView tvloss = FindViewById<TextView>(Resource.Id.tv_loss);
            tvloss.Text = loss + "%";
        }

        public Bitmap getScreenShot(View view)
        {
            View screenView = view.RootView;
            screenView.DrawingCacheEnabled = true;
            Bitmap bitmap = Bitmap.CreateBitmap(screenView.DrawingCache);
            screenView.DrawingCacheEnabled = false;
            return bitmap;
        }

        public String store(Bitmap bm, String fileName)
        {
            String sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/Screenshots";
            Java.IO.File dir = new Java.IO.File(sdCardPath);

            if (!dir.Exists())
                dir.Mkdirs();

            var filePath = "";
            try
            {
                filePath = System.IO.Path.Combine(sdCardPath, fileName);
                var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create);
                bm.Compress(Bitmap.CompressFormat.Png, 85, stream);
                stream.Close();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }

            return filePath;
        }

        private void shareImage(String file)
        {
            Intent intent = new Intent();
            intent.SetAction(Intent.ActionSend);
            intent.SetType("image/*");

            intent.PutExtra(Intent.ExtraSubject, "Sales");
            intent.PutExtra(Intent.ExtraText, "Check my weekly sales");
            intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse("file:///" + file));
            try
            {
                StartActivity(Intent.CreateChooser(intent, "Share Screenshot"));
            }
            catch (ActivityNotFoundException e)
            {
                Toast.MakeText(this, "No App Available", ToastLength.Short).Show();
            }
        }
    }
}