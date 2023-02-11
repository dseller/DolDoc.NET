using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DolDoc.Editor.Compositor;
using DolDoc.Editor.Core;
using DolDoc.Editor.Entries;
using DolDoc.Renderer.OpenGL;

namespace DolDoc.Examples
{

    public class WebDocument : Document
    {
        private readonly string url;
        private readonly Dictionary<string, object> dataBag;

        public WebDocument(string url)
        {
            dataBag = new Dictionary<string, object>();
            this.url = url;
            Load(GetRequest(url));
        }

        public override object GetData(string key)
        {
            if (dataBag.ContainsKey(key))
                return dataBag[key];
            return null;
        }

        public void RefreshPage() => Load(GetRequest(url));

        public override void ButtonClicked(Button btn)
        {
            Load(PostRequest(url + $"/{btn.Aux}"));
        }

        public override void FieldChanged(string name, object value)
        {
            if (dataBag.ContainsKey(name))
                dataBag[name] = value;
            else
                dataBag.Add(name, value);
        }

        private string GetRequest(string url)
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
            var c = new HttpClient(handler);
            
            var body = Task.Run(async () =>
            {
                var response = await c.GetAsync(url);
                return await response.Content.ReadAsStringAsync();
            }).GetAwaiter().GetResult();

            return body.Replace("\r", string.Empty);
        }
        
        private string PostRequest(string url)
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
            var c = new HttpClient(handler);
            
            var body = Task.Run(async () =>
            {
                var formContent = new FormUrlEncodedContent(dataBag.Select(db => new KeyValuePair<string, string>(db.Key, db.Value.ToString())));
                var response = await c.PostAsync(url, formContent);
                return await response.Content.ReadAsStringAsync();
            }).GetAwaiter().GetResult();

            return body.Replace("\r", string.Empty);
        }
    }
    
    public static class Program
    {
        public static void Main(string[] args)
        {
            var window = new OpenTKWindow();

            var document = new WebDocument("https://localhost:7074");
            document.OnMacro += entry =>
            {
            };

            var compositor = Compositor.Initialize(window, 1600, 1200, document);
            compositor.OnKeyPress += key =>
            {
                if (key == Key.F5)
                    document.RefreshPage();;
            };
            compositor.Start();
        }

        
    }
}