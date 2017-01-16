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
    [Activity(Label = "Login")]
    public class LoginActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.LoginActivity);
            
            Button button = FindViewById<Button>(Resource.Id.btn_login);
            button.Click += delegate {
                EditText et = FindViewById<EditText>(Resource.Id.et_phone);

                string phone = et.Text;
                
                var documentPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                var path = Path.Combine(documentPath, "envision_user.txt");
                
                var outputStream = new StreamWriter(path, false);
                outputStream.WriteLine(phone + ":online");
                outputStream.Close();

                DBHandle handle = new DBHandle();
                handle.createDatabase();

                Intent intent = new Intent(this, typeof(AddProductActivity));
                StartActivity(intent);
            };
        }

        protected override void OnResume()
        {
            base.OnResume();

            try
            {
                bool logged = false;

                var documentPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                var path = Path.Combine(documentPath, "envision_user.txt");

                var inputStream = new StreamReader(path);
                string content = inputStream.ReadToEnd();

                if (content.IndexOf("online") != -1)
                    logged = true;

                inputStream.Close();

                if (logged)
                {
                    Intent intent = new Intent(this, typeof(POSActivity));
                    StartActivity(intent);
                }
            }
            catch (System.IO.FileNotFoundException e)
            {
            }
        }


    }
}