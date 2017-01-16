using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Java.IO;
using Java.Lang;
using System.IO;

namespace ENVision
{
    [Activity(Label = "ENVision", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            //SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            //Button button = FindViewById<Button>(Resource.Id.MyButton);

            //button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };
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

                inputStream.Close();

                if (content.IndexOf("online") == -1)
                    logged = false;
            }
            catch (System.IO.FileNotFoundException e)
            {
                logged = false;
                System.Diagnostics.Debug.WriteLine("Exception: " + e.Message.ToString());
            }

            Intent intent;

            if (logged)
                intent = new Intent(this, typeof(POSActivity));
            else
                intent = new Intent(this, typeof(LoginActivity));

            StartActivity(intent);
        }
    }
}

