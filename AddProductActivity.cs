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
using Android.Content.PM;
using Android.Provider;
using Android.Graphics;
using Java.IO;
using SQLite;
using System.IO;
using Android.Views.InputMethods;

namespace ENVision
{
    [Activity(Label = "Add Products")]
    public class AddProductActivity : ParentActivity
    {
        Java.IO.File dir, file;
        String filepath = "";
        Bitmap bitmap;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.AddProductActivity);
            initMenu();
            
            Button b_more = FindViewById<Button>(Resource.Id.b_more);
            b_more.Click += delegate
            {
                CreateProduct();
            };

            Button b_finish = FindViewById<Button>(Resource.Id.b_finish);
            b_finish.Click += delegate
            {
                CreateProduct();

                Intent intent = new Intent(this, typeof(LoadingActivity));
                StartActivity(intent);
            };

            Button b_picture = FindViewById<Button>(Resource.Id.b_picture);
            b_picture.Click += delegate
            {
                try
                {
                    if (IsThereAnAppToTakePictures())
                    {
                        CreateDirectoryForPictures();

                        Intent intent = new Intent(MediaStore.ActionImageCapture);
                        file = new Java.IO.File(dir, String.Format("productPhoto_{0}.jpg", Guid.NewGuid()));
                        intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(file));
                        StartActivityForResult(intent, 0);
                    }
                }
                catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Caught!" + e.Message);
                }
            };
            
        }
        
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            try
            {
                Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                Android.Net.Uri contentUri = Android.Net.Uri.FromFile(file);
                mediaScanIntent.SetData(contentUri);
                SendBroadcast(mediaScanIntent);
                
                ImageView _imageView = FindViewById<ImageView>(Resource.Id.iv_picture);

                //bitmap = BitmapFactory.DecodeFile(file.Path);
                bitmap = file.Path.LoadAndResizeBitmap(100,100);
                filepath = file.Path;

                if (bitmap != null)
                {
                    //System.Diagnostics.Debug.WriteLine("Image set");
                    _imageView.SetImageBitmap(bitmap);
                    bitmap = null;
                }
                else
                    System.Diagnostics.Debug.WriteLine("Cannot set");

                // Dispose of the Java side bitmap.
                GC.Collect();
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Caught Exception: " + e.Message);
            }
        }

        private void CreateDirectoryForPictures()
        {
            dir = new Java.IO.File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "ENVisionApp");
            System.Diagnostics.Debug.WriteLine("DIRCREATED: " + dir.ToURI().ToString());
            if (!dir.Exists())
                dir.Mkdirs();
        }

        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities =
                PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        private void CreateProduct()
        {
            EditText et_product = FindViewById<EditText>(Resource.Id.et_name);
            System.Diagnostics.Debug.WriteLine("FILEPATH: " + filepath);
            Product p = new Product();
            p.ProductCost = 0;
            p.ProductName = et_product.Text;
            p.ProductPicture = filepath;
            p.ProductPrice = 0;
            p.ProductStock = 0;

            /*
            if(p.ProductName.Equals(""))
            {
                new AlertDialog.Builder(this).SetTitle("Error").SetMessage("Please provide a product name").Create().Show();
                return;
            }
            */

            DBHandle handle = new DBHandle();
            SQLiteConnection con = handle.getConnection();
            con.Insert(p);

            et_product.Text = "";
        }
    }
}