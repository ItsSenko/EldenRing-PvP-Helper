using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.MVVM.Models.Search
{
    public interface ISearchAlgorithm
    {
        string Name { get; }
    }
    public class SearchAlgorithm<T> : ISearchAlgorithm
    {
        public delegate void OnItemsChangedHandler(List<SearchItem<T>> items);
        public event OnItemsChangedHandler OnItemsChanged;
        public event OnItemsChangedHandler OnBaseItemsChanged;

        public delegate void OnSearchChangedHandler(string searchStr);
        public event OnSearchChangedHandler OnSearchChanged;

        public delegate void OnSortOrderChangedHandler(ISortOrder<T> newSortOrder);
        public event OnSortOrderChangedHandler OnSortOrderChanged;

        public string Name => "Default Search Algorithm";

        private string _searchStr;

        public string SearchString
        {
            get { return _searchStr; }
            set { _searchStr = value; OnSearchChanged?.Invoke(_searchStr); }
        }

        public List<ISortOrder<T>> AvailableOrders { get; set; }
        private ISortOrder<T> _order;

        public ISortOrder<T> Order
        {
            get { return _order; }
            set { _order = value; OnSortOrderChanged?.Invoke(_order); }
        }

        private List<SearchItem<T>> _items;

        public List<SearchItem<T>> Items
        {
            get { return _items; }
            set { _items = value; OnBaseItemsChanged?.Invoke(_items); }
        }


        private List<SearchItem<T>> _shownItems;

        public List<SearchItem<T>> ShownItems
        {
            get { return _shownItems; }
            private set 
            { 
                if (value != _shownItems)
                    OnItemsChanged?.Invoke(value);
                _shownItems = value;
            }
        }

        public SearchAlgorithm(List<SearchItem<T>> baseItems, ISortOrder<T> sortOrder)
        {
            Items = baseItems;
            ShownItems = baseItems;
            Order = sortOrder;

            OnBaseItemsChanged += OnBaseItemsChange;
            OnSearchChanged += OnSearchChange;
            OnSortOrderChanged += (order) => order.Sort(ShownItems, this);

            OnBaseItemsChange(Items);
        }

        private void OnBaseItemsChange(List<SearchItem<T>> items)
        {
            OnSearchChange(SearchString);
        }

        private void OnSearchChange(string searchStr)
        {
            if (Items == null)
            {
                ShownItems = new();
                return;
            }
            if (searchStr == null)
                searchStr = "";

            //Search Alg
            ShownItems = Items.Where(x => x.ToString().ToLower().Contains(searchStr.ToLower())).ToList();

            //Sort Order
            ShownItems = Order.Sort(ShownItems, this);
        }
    }
}
