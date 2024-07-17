using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.MVVM.Models.Database
{
    public interface IDataBase<T>
    {
        public string Name { get; set; }
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
        public IEnumerable<T> Items { get; set; }
        public void NextPage();
        public void PrevPage();
        public void GoToPage(int pageIndex);
    }
}
