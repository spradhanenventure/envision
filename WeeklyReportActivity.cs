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
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Xamarin.Android;
using Java.Text;
using Java.Util;
using SQLite;
using BarChart;
using Android.Graphics;
using Java.IO;
using Android.Util;

namespace ENVision
{
    [Activity(Label = "Weekly Reports")]
    public class WeeklyReportActivity : ParentActivity
    {
        SimpleDateFormat dateFormatter = new SimpleDateFormat("yyyy-MM-dd", Locale.Us);
        String startDate, endDate;
        DateTime start, end;
        BarChartView chart;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.WeeklyReportActivity);
            initMenu();

            endDate = dateFormatter.Format(Calendar.GetInstance(Locale.Us).Time);

            end = DateTime.ParseExact(endDate, "yyyy-MM-dd", null);
            int dateSubtractor = -7;
            start = end.AddDays(dateSubtractor);
            Calendar newDate = Calendar.GetInstance(Locale.Us);
            newDate.Set(start.Year, start.Month - 1, start.Day);

            startDate = dateFormatter.Format(newDate.Time);
            
            initCharts();

            Button b_share = FindViewById<Button>(Resource.Id.b_share);
            b_share.Click += delegate
            {
                View rootView = FindViewById(Resource.Id.fl_chart);
                Bitmap bitmap = getScreenShot(rootView);
                String filePath = store(bitmap, "screenshot-"+ Guid.NewGuid() + ".png");
                shareImage(filePath);
            };

            Button b_weekly = FindViewById<Button>(Resource.Id.b_weekly);
            b_weekly.Click += delegate
            {
                Intent intent = new Intent(this, typeof(ReportActivity));
                StartActivity(intent);
            };
        }

        public Bitmap getScreenShot(View view)
        {
            chart.DrawingCacheEnabled = true;
            
            Bitmap bitmap = Bitmap.CreateBitmap(chart.DrawingCache);
            chart.DrawingCacheEnabled = false;
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
            intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse("file:///"+file));
            try
            {
                StartActivity(Intent.CreateChooser(intent, "Share Screenshot"));
            }
            catch (ActivityNotFoundException e)
            {
                Toast.MakeText(this, "No App Available", ToastLength.Short).Show();
            }
        }

        private void initCharts()
        {
            chart = new BarChartView(this);

            DBHandle handle = new DBHandle();
            SQLiteConnection connection = handle.getConnection();

            var products = connection.Query<Product>("SELECT * FROM Product ");

            BarModel[] data = new BarModel[products.Count()];
            int i = 0;

            foreach (Product p in products)
            {
                List<Sales> sales = connection.Query<Sales>("SELECT * FROM Sales WHERE SaleDate >= ? AND SaleDate <= ? AND ProductID = ?", startDate + " 00:00:00", endDate + " 23:59:59", p.ID);

                int totalQuantity = 0;

                foreach(Sales sale in sales)
                {
                    totalQuantity += sale.SaleQuantity;
                }

                String name = p.ProductName;
                if (name.Length > 10)
                    name = name.Substring(0, 8) + ".";

                data[i++] = new BarModel() { Value = totalQuantity, Legend = name };
            }

            chart.BarColor = Color.ParseColor("#6bcebb");
            chart.SetBackgroundColor(Color.White);
            chart.LegendColor = Color.Black;
            chart.LegendHidden = false;
            chart.LegendFontSize = 19;

            chart.AutoLevelsEnabled = false;

            for(float j=0; j<=50; j += 5)
                chart.AddLevelIndicator(j, j + "");

            chart.ItemsSource = data;

            chart.BarWidth = 80;
            chart.MinimumValue = 0;
            chart.MaximumValue = 50;
            
            FrameLayout frame = FindViewById<FrameLayout>(Resource.Id.fl_chart);
            frame.AddView(chart, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));
        }
    }
}