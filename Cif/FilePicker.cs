//***********************************************************************
//                        CIF program
//                Created by Daniel M. Kaminski
//                        Lublin 2023
//                     Under GNU licence
//***********************************************************************

using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Java.IO;
using Microsoft.Xna.Framework;
using Plugin.FilePicker;
using System;
using System.Diagnostics;
using Activity = Android.App.Activity;

namespace cif
{

    [Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    [Preserve(AllMembers = true)]
    public class FilePicker : Activity
    {
        public string fileName;
        public string data;
         public void OnCreate(Bundle savedInstanceState)
        {
            if(savedInstanceState != null)
            base.OnCreate(savedInstanceState);

            fileName = "";
            data = "";


              //intent.PutExtra(Intent.ExtraAlternateIntents, new String[] {
            //    "*.cif", //  
            // });

    
            var intent = new Intent(Intent.ActionGetContent);
            intent.SetType("*/*");

            intent.AddCategory(Intent.CategoryOpenable);
            try
            {
                StartActivityForResult(Intent.CreateChooser(intent, "Select file"), 0);
            }
            catch (Exception exAct)
            {
                System.Diagnostics.Debug.Write(exAct);
            }
        }


        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {

            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Canceled)
            {
                // Notify user file picking was cancelled.
                //  OnFilePickCancelled();
                Finish();
            }
            else
            {
                System.Diagnostics.Debug.Write(data.Data);
                try
                {
                    var _uri = data.Data;

                    var filePath = IOUtil.getPath(Application.Context, _uri);

                    if (string.IsNullOrEmpty(filePath))
                        filePath = _uri.Path;

                    var reader = new System.IO.StreamReader(filePath, System.Text.Encoding.UTF8);
                    var stream = reader.ReadToEnd();

                    data = (Intent)stream.ToString();

                    reader.Close();

                     fileName = GetFileName(Application.Context, _uri);

                }
                catch (Exception readEx)
                {
                    // Notify user file picking failed.
                    //    OnFilePickCancelled();
                    System.Diagnostics.Debug.Write(readEx);
                }
                finally
                {
                    //                 Finish();
                }
            }
        }

        string GetFileName(Context ctx, Android.Net.Uri uri)
        {

            string[] projection = { MediaStore.MediaColumns.DisplayName };

            var cr = ctx.ContentResolver;
            var name = "";
            var metaCursor = cr.Query(uri, projection, null, null, null);

            if (metaCursor != null)
            {
                try
                {
                    if (metaCursor.MoveToFirst())
                    {
                        name = metaCursor.GetString(0);
                    }
                }
                finally
                {
                    metaCursor.Close();
                }
            }
            return name;
        }
    }
}
