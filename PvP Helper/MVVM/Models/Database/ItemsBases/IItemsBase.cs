using PvPHelper.MVVM.Models.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.MVVM.Models.Database.ItemsBases
{
    public interface IItemsBase<T>
    {
        public string Name { get; set; }
        public List<T> Items { get; set; }
        public void Update(object? sender, SearchableDataBase<T> alg);
    }
}
