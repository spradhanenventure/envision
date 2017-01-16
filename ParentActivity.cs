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
using System.IO;

namespace ENVision
{
    public class ParentActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


        }

        public void initMenu()
        {
            Button button = FindViewById<Button>(Resource.Id.b_menu);
            button.Click += delegate {
                OpenOptionsMenu();
            };
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Drawable.Menu, menu);
            return base.OnPrepareOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Intent intent = null;
            
            switch (item.ItemId)
            {
                case Resource.Id.menu_new_product:
                    System.Diagnostics.Debug.WriteLine("Location2: PRODUCT");
                    intent = new Intent(this, typeof(AddProductActivity));
                    break;

                case Resource.Id.menu_set_costs:
                    System.Diagnostics.Debug.WriteLine("Location2: COST");
                    intent = new Intent(this, typeof(AddCostActivity));
                    break;

                case Resource.Id.menu_set_prices:
                    System.Diagnostics.Debug.WriteLine("Location2: PRICE");
                    intent = new Intent(this, typeof(AddPriceActivity));
                    break;

                case Resource.Id.menu_set_stock:
                    System.Diagnostics.Debug.WriteLine("Location2: STOCK");
                    intent = new Intent(this, typeof(AddStockActivity));
                    break;

                case Resource.Id.menu_sell:
                    System.Diagnostics.Debug.WriteLine("Location2: SELL");
                    intent = new Intent(this, typeof(POSActivity));
                    break;

                case Resource.Id.menu_wreport:
                    System.Diagnostics.Debug.WriteLine("Location2: Weekly Report");
                    intent = new Intent(this, typeof(WeeklyReportActivity));
                    break;

                case Resource.Id.menu_report:
                    System.Diagnostics.Debug.WriteLine("Location2: Report");
                    intent = new Intent(this, typeof(ReportActivity));
                    break;

                case Resource.Id.menu_signout:
                    System.Diagnostics.Debug.WriteLine("Location2: SignOut");
                    var documentPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                    var path = Path.Combine(documentPath, "envision_user.txt");

                    var outputStream = new StreamWriter(path, false);
                    outputStream.WriteLine("offline");
                    outputStream.Close();
                    
                    intent = new Intent(this, typeof(LoginActivity));
                    break;
            }
            
            StartActivity(intent);

            return base.OnOptionsItemSelected(item);
        }


        protected override void OnResume()
        {
            base.OnResume();

            bool logged = true;

            try
            {
                var documentPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                var path = Path.Combine(documentPath, "envision_user.txt");

                var inputStream = new StreamReader(path);
                string content = inputStream.ReadToEnd();

                if (content.IndexOf("online") == -1)
                    logged = false;

                System.Diagnostics.Debug.WriteLine("Login Status: " + content);

                inputStream.Close();

            }
            catch (System.IO.FileNotFoundException e)
            {
                logged = false;
            }

            if (!logged)
            {
                Intent intent = new Intent(this, typeof(LoginActivity));
                StartActivity(intent);
            }
        }
    }
}