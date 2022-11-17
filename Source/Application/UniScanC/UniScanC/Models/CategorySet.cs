using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unieye.WPF.Base.Helpers;

namespace UniScanC.Models
{
    public class CategorySet : Observable
    {
        private List<string> categoryList = new List<string>();
        public List<string> CategoryList
        {
            get => categoryList;
            set => Set(ref categoryList, value);
        }

        private string selectedCategory = "";
        public string SelectedCategory
        {
            get => selectedCategory;
            set => Set(ref selectedCategory, value);
        }
    }
}
