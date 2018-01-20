using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Net.Http;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

namespace run.Transformation.Xaml
{
    public class UriDependencyObject : DependencyObject
    {
        public string Uri
        {
            get { return GetValue(EndpointProperty) as string; }
            set { SetCurrentValue(EndpointProperty, value); }
        }
        public static readonly DependencyProperty EndpointProperty = DependencyProperty.Register(nameof(Uri), typeof(string), typeof(UriDependencyObject));
    }
    [MarkupExtensionReturnType(typeof(string))]
    public class StringApiBinding : MarkupExtension
    {
        public BindingBase Endpoint { get; set; }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            UriDependencyObject obj = new UriDependencyObject();
            BindingOperations.SetBinding(obj, UriDependencyObject.EndpointProperty, Endpoint);
            
            var uri = obj.GetValue(UriDependencyObject.EndpointProperty) as string;
            HttpClient client = new HttpClient();
            var response = client.GetAsync(uri);
            response.Wait();
            response.Result.EnsureSuccessStatusCode();
            var content = response.Result.Content.ReadAsStringAsync();
            content.Wait();
            return content.Result;
        }
        
    }
}
