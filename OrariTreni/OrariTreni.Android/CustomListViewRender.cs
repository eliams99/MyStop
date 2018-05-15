using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using OrariTreni.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.ListView), typeof(CustomListViewRender))]
namespace OrariTreni.Droid
{
    class CustomListViewRender : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                FieldInfo[] fields = typeof(ListViewRenderer).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                var refresh = (SwipeRefreshLayout)fields.First(x => x.Name == "_refresh").GetValue(this);
                refresh.SetColorSchemeColors(this.Context.Resources.GetIntArray(Resource.Array.SwipeRefreshColors));
            }
        }
    }
}