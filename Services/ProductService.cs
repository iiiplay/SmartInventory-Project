using SmartInventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SmartInventory.Services
{
    public static class ProductService
    {
        public static Dictionary<string, decimal[]> Statistics(List<Product> all)
        {
            // 分類 => 數量  "電子" =>45

            var stat = new Dictionary<string, decimal[]>();
            foreach (var p in all)
            {
                if (!stat.ContainsKey(p.Category))
                {
                    stat[p.Category] = [0, 0];
                }

                stat[p.Category][0] += p.Quantity;
                stat[p.Category][1] += p.TotalValue;
            }


            return stat;
        }

        public static (decimal, int) GetTotalValue(List<Product> all)
        {
            //Calc(all);
            decimal sum = 0;
            int qty = 0;
            foreach (var p in all) sum += p.TotalValue; // 每筆的 數量×單價
            foreach (var p in all) qty += p.Quantity; // 每筆的 數量×單價

            return (sum, qty);
        }

        public static List<Product> GetLowStack(List<Product> all)
        {
            int lowStock = 10;

            var result = new List<Product>();
            foreach (var p in all)
            {
                if (p.Quantity < lowStock)
                {
                    result.Add(p);
                }
            }

            return result;
        }


        public static readonly string[] Categories =
        { "電子", "生活", "文具", "食品"};


        public static List<Product> Search(List<Product> all, string keyword, string category)
        {
            //1. 判斷是否都為空字串
            if (keyword == string.Empty && category == string.Empty) return all;

            //2. 是否是搜尋關鍵字
            //3.是否是搜尋分類
            var result = new List<Product>();
            foreach (var p in all)
            {
                if (!p.Name.Contains(keyword)) continue;

                if (category == "全部" || p.Category.Contains(category))
                {
                    result.Add(p);
                }
            }

            //4.回傳篩選後的商品集合
            return result;
        }
    }
}
