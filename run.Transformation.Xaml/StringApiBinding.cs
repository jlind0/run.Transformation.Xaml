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
    [MarkupExtensionReturnType(typeof(string))]
    public class StringApiBinding : MarkupExtension
    {
        public BindingBase Endpoint { get; set; }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            IProvideValueTarget target = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            var targetObj = target.TargetObject as DependencyObject;
            DependencyProperty prop = DependencyProperty.Register("Uri", typeof(string), target.TargetObject.GetType());
            BindingOperations.SetBinding(targetObj, prop, Endpoint);
            return Endpoint.ProvideValue(serviceProvider);
        }
        
    }
}
