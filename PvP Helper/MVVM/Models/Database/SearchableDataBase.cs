using PvPHelper.MVVM.Models.Search;
using PvPHelper.MVVM.Models.Search.SortOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.MVVM.Models.Database
{
    public class SearchableDataBase<T> : DataBase<T>
    {
        public SearchAlgorithm<T> searchAlg { get; private set; }

        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; searchAlg.SearchString = value; }
        }


        public SearchableDataBase(string name, IEnumerable<T> items, int maxPerPage) : base(name, items, maxPerPage) 
        {
            searchAlg = new(items, new ClosestMatchSort<T>());
            searchAlg.OnItemsChanged += SearchAlg_OnItemsChanged;

            OnItemsChanged += OnDataBaseItemsChanged;
        }

        private void OnDataBaseItemsChanged(IEnumerable<T> items)
        {
            searchAlg.Items = items;
        }

        public override void OnCurrPageChanged(int page)
        {
            CurrentItemsOnPage = searchAlg.ShownItems.Chunk(MaxPerPage).ToList()[CurrentPage];
        }

        private void SearchAlg_OnItemsChanged(IEnumerable<T> items)
        {
            if (items.Count() < 1 || searchAlg.ShownItems.Count() < 1)
            {
                CurrentItemsOnPage = items;
                return;
            }

            var chunk = items.Chunk(MaxPerPage);
            var page = chunk.ElementAtOrDefault(CurrentPage);

            if (page == null)
            {
                CurrentPage = 0;
                page = chunk.ElementAtOrDefault(CurrentPage);
            }

            Pages = chunk.Count();

            CurrentItemsOnPage = chunk.ToList()[CurrentPage];            
        }

    }
}
