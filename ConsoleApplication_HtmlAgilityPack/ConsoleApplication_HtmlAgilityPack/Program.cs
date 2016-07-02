using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.IO;

namespace ConsoleApplication_HtmlAgilityPack
{
    class Program
    {
        static void Main(string[] args)
        {
            //指定來源網頁
            WebClient url = new WebClient();
            //將網頁來源資料暫存到記憶體內
            MemoryStream ms = new MemoryStream(url.DownloadData("http://www.coolpc.com.tw/evaluate.php"));
            // 使用預設編碼讀入 HTML 
            HtmlDocument doc = new HtmlDocument();
            doc.Load(ms, Encoding.Default);

            if (doc != null)
            {
                var list_type = new List<string>();
                HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//select[@name='n4']//optgroup");

                foreach (HtmlNode node in nodes)
                {
                    var type = node.Attributes["label"].Value;
                    list_type.Add(type);
                }

                List<string> list_name = doc.DocumentNode.SelectSingleNode("//select[@name='n4']").InnerText.Split('\n').ToList();

                //刪除不必要的非商品選項
                list_name.RemoveRange(0, 3);
                list_name.RemoveAt(list_name.Count - 1);

                var models = new List<Product>();
                int number = 0;
                for (int i = 0; i < list_name.Count; i++)
                {
                    string type = list_type[number];
                    string name = list_name[i];

                    if (name == "")
                    {
                        number++;
                    }
                    else
                    {
                        models.Add(new Product()
                        {
                            type = type,
                            name = name
                        });

                        Console.WriteLine("類型：{0} ,", type);
                        Console.WriteLine("名稱：{0}", name);
                    }
                }

                Console.ReadLine();
            }

        }
    }

    class Product
    {
        public string type { get; set; }

        public string name { get; set; }
    }
}
