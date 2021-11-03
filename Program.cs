using System;
using System.IO;
using System.Net;
using System.Xml;

namespace XmlRead
{
    class Program
    {


        static void Main(string[] args)
        {
            WebClient webClient = new WebClient();

            var imageFolderPath = Path.Combine( Directory.GetCurrentDirectory(), "images");

            //var xmlPath = System.IO.Path.GetFullPath("xml-export-2.xml");
            var xmlPath = "https://www.meliketatar.com/xml-export";

            XmlDocument xml = new XmlDocument();
            xml.Load(xmlPath);

            int counter = 0;

            XmlNodeList xnList = xml.SelectNodes("/Products/Product");
            foreach (XmlNode product in xnList)
            {
                string barcode = "";

                var firstVariant = product.SelectNodes("Variants/Variant")[0];
                if (firstVariant == null)
                    barcode = product["ProductBarcode"].InnerText;
                else
                    barcode = firstVariant["VariantBarcode"].InnerText;

                XmlNodeList images = product.SelectNodes("Images/*");
                for (int i = 0; i < images.Count; i++)
                {
                    string url = images[i].InnerText;
                    string ext = url.Substring(url.LastIndexOf("."));
                    string imageName = String.Format("{0}-{1}{2}", barcode, (i + 1).ToString(), ext);

                    webClient.DownloadFile(url, Path.Combine(imageFolderPath, imageName));
                }

                counter++;
            }

            Console.WriteLine("Toplam ürün sayısı = " + counter.ToString());
            Console.ReadKey();
        }
    }
}
