using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Net.Http;
using System.Windows.Data;

namespace run.Transformation.Xaml
{
    [MarkupExtensionReturnType(typeof(string))]
    public class StringApiBinding : MarkupExtension
    {
        [ConstructorArgument("endpoint")]
        public string Endpoint { get; set; }
        public StringApiBinding(string endopoint) { Endpoint = endopoint; }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            HttpClient client = new HttpClient();
            var response = client.GetAsync(Endpoint);
            response.Wait();
            response.Result.EnsureSuccessStatusCode();
            var content = response.Result.Content.ReadAsStringAsync();
            content.Wait();
            return content.Result;
        }
        
    }
}
