using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Unieye.WPF.Base.Helpers
{
    public sealed class ResXWebRequestFactory : IWebRequestCreate
    {
        public const string Scheme = "resx";
        private static ResXWebRequestFactory _factory = new ResXWebRequestFactory();

        private ResXWebRequestFactory()
        {
        }

        // call this before anything else
        public static void Register()
        {
            WebRequest.RegisterPrefix(Scheme, _factory);
        }

        WebRequest IWebRequestCreate.Create(Uri uri)
        {
            return new ResXWebRequest(uri);
        }

        private class ResXWebRequest : WebRequest
        {
            public ResXWebRequest(Uri uri)
            {
                Uri = uri;
            }

            public Uri Uri { get; set; }

            public override WebResponse GetResponse()
            {
                return new ResXWebResponse(Uri);
            }
        }

        private class ResXWebResponse : WebResponse
        {
            public ResXWebResponse(Uri uri)
            {
                Uri = uri;
            }

            public Uri Uri { get; set; }

            public override Stream GetResponseStream()
            {
                Assembly asm;
                if (string.IsNullOrEmpty(Uri.Host))
                {
                    asm = Assembly.GetEntryAssembly();
                }
                else
                {
                    asm = Assembly.Load(Uri.Host);
                }

                int filePos = Uri.LocalPath.LastIndexOf('/');
                string baseName = Uri.LocalPath.Substring(1, filePos - 1);
                string name = Uri.LocalPath.Substring(filePos + 1);

                var rm = new ResourceManager(baseName, asm);
                object obj = rm.GetObject(name);

                if (obj is Stream stream)
                {
                    return stream;
                }

                // System.Drawing.Bitmap
                if (obj is Bitmap bmp)
                {
                    stream = new MemoryStream();
                    bmp.Save(stream, bmp.RawFormat);
                    bmp.Dispose();
                    stream.Position = 0;
                    return stream;
                }
                return null;
            }
        }
    }
}
