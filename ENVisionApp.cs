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
using System.Threading.Tasks;
using Java.Lang;

namespace ENVision
{
    [Application(Label = "ENVisionApp", AllowBackup = true)]
    public class ENVisionApp : Application
    {
        public ENVisionApp(IntPtr handle, JniHandleOwnership ownerShip) : base(handle, ownerShip)
		{
        }

        public override void OnCreate()
        {
            try
            {
                base.OnCreate();

                AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;

                AppDomain currentDomain = AppDomain.CurrentDomain;
                currentDomain.UnhandledException += new UnhandledExceptionEventHandler(HandleExceptions);

                Java.Lang.Thread.DefaultUncaughtExceptionHandler = new ExceptHandler();

                TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

                //CalligraphyConfig.InitDefault(new CalligraphyConfig.Builder()
                //	.SetDefaultFontPath("fonts/flexo w01 regular.ttf")
                //	.SetFontAttrId(Resource.Attribute.fontPath)
                //	.Build());
            }
            catch (System.Exception ex)
            {
                //var top = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
                //var act = top.Activity;
                //Toast.MakeText(act, ex.ToString(), ToastLength.Short);

                LogException(ex);
            }
        }

        void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            //var top = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
            //var act = top.Activity;
            //Toast.MakeText(act, e.Exception.ToString(), ToastLength.Short);
            e.Handled = true;
            LogException(e.Exception);
        }

        void HandleExceptions(object sender, UnhandledExceptionEventArgs e)
        {
            //var top = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
            //var act = top.Activity;
            //Toast.MakeText(act, e.ExceptionObject.ToString(), ToastLength.Short);
            //LogException(e.ExceptionObject);
            LogException(new System.Exception(e.ExceptionObject?.ToString()));
        }

        void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            //var top = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
            //var act = top.Activity;
            //Toast.MakeText(act, e.Exception.ToString(), ToastLength.Short);
            LogException(e.Exception);
            e.SetObserved();
        }

        private void LogException(System.Exception exp)
        {
            try
            {
                //mTracker.Set("Exception", exp.Message);
                //mTracker.Set("Stacktrace", exp.StackTrace);
                string description = string.Format("Exception = {0}\nStackTrace = {1}", exp.Message, exp.StackTrace);
                //DefaultTracker.Send(new HitBuilders.ExceptionBuilder().SetDescription(description).Build());
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }

    public class ExceptHandler : Java.Lang.Object, Java.Lang.Thread.IUncaughtExceptionHandler
    {
        public void UncaughtException(Thread t, Throwable e)
        {
            //var top = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
            //var act = top.Activity;
            //Toast.MakeText(act, e.ToString(), ToastLength.Short);
        }
    }
}