﻿using System;
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
    public class ApiConnection : DependencyObject
    {              
        public static readonly DependencyProperty UriProperty = DependencyProperty.Register(nameof(Uri), typeof(string), typeof(ApiConnection),
            new PropertyMetadata(null, async (d, e) =>
            {
                var u = d.GetValue(UriProperty) as string;
                try
                {
                    Uri uri = new Uri(u);
                    HttpClient client = new HttpClient();
                    var response = await client.GetAsync(uri);
                    d.SetValue(ApiConnection.ValueProperty, await response.Content.ReadAsStringAsync());
                }
                catch { }
 
            }));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(ApiConnection));
    }
    [MarkupExtensionReturnType(typeof(string))]
    public class StringApiBinding : MarkupExtension
    {
        public BindingBase Endpoint { get; set; }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            IProvideValueTarget provider = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            ApiConnection connection = new ApiConnection();
            
            BindingOperations.SetBinding(connection, ApiConnection.UriProperty, Endpoint);
            Binding binding = new Binding("Value")
            {
                Source = connection
            };
            BindingOperations.SetBinding((DependencyObject)provider.TargetObject, (DependencyProperty)provider.TargetProperty, binding);
            return binding.ProvideValue(serviceProvider);
        }
        
    }
}
