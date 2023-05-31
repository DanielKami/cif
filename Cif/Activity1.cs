
//***********************************************************************
//                        CIF program
//                Created by Daniel M. Kaminski
//                        Lublin 2023
//                     Under GNU licence
//***********************************************************************

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;


namespace Cif
{
    [Activity(
        Label = "@string/app_name",
        MainLauncher = true,
        Icon = "@drawable/icon",
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.FullUser,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden
    )]
    public class Activity1 : AndroidGameActivity
    {
        private MainProgram1 _game;
        private View _view;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            //	
            Intent intent;
            if (!Android.OS.Environment.IsExternalStorageManager)
            {
                Toast.MakeText(this, "Cif uses file access only to download *.cif files!", ToastLength.Long).Show();

                intent = new Intent();
                intent.SetAction(Android.Provider.Settings.ActionManageAppAllFilesAccessPermission);

                Android.Net.Uri uri = Android.Net.Uri.FromParts("package", this.PackageName, null);

                intent.SetData(uri);
                StartActivity(intent);
            }

            //intent = new Intent(Intent.ActionOpenDocumentTree);
            //StartActivityForResult(intent, 0);


            _game = new MainProgram1();
            _view = _game.Services.GetService(typeof(View)) as View;

            SetContentView(_view);
            _game.Run();
        }



        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

        }




        public override void OnWindowFocusChanged(
    Boolean bHasFocus)
        {
            base.OnWindowFocusChanged(bHasFocus);

            if (bHasFocus)
                SetWindowLayout();
        }


        private void SetWindowLayout()
        {
            if (Window != null)
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
                {
                    IWindowInsetsController wicController = Window.InsetsController;


                    Window.SetDecorFitsSystemWindows(false);
                    Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

                    if (wicController != null)
                    {
                        wicController.Hide(WindowInsets.Type.Ime());
                        wicController.Hide(WindowInsets.Type.NavigationBars());
                    }
                }
                else
                {
#pragma warning disable CS0618

                    Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

                    Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(SystemUiFlags.Fullscreen |
                                                                           SystemUiFlags.HideNavigation |
                                                                           SystemUiFlags.Immersive |
                                                                           SystemUiFlags.ImmersiveSticky |
                                                                           SystemUiFlags.LayoutHideNavigation |
                                                                           SystemUiFlags.LayoutStable |
                                                                           SystemUiFlags.LowProfile);
#pragma warning restore CS0618
                }
            }
        }




    }


}
