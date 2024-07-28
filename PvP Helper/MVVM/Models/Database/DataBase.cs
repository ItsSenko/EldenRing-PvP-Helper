using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.MVVM.Models.Database
{
    public class DataBase<T> : IDataBase<T>
    {
        public delegate void OnPagesChangedHandler(int pages);
        public event OnPagesChangedHandler OnPagesChanged;

        public delegate void OnCurrentPageChangedHandler (int currentPage);
        public event OnCurrentPageChangedHandler OnCurrentPageChanged;

        public delegate void OnItemsChangedHandler(IEnumerable<T> items);
        public event OnItemsChangedHandler OnShownItemsChanged;
        public event OnItemsChangedHandler OnItemsChanged;
        
        public string Name { get; set; }

        private int _pages;
        public int Pages
        {
            get { return _pages; }
            set { _pages = value; OnPagesChanged?.Invoke(value); }
        }

        private int _currentPage;
        public int CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; OnCurrentPageChanged?.Invoke(value); }
        }
        public int MaxPerPage { get; set; }

        private IEnumerable<T> _items;

        public IEnumerable<T> Items
        {
            get { return _items; }
            set { _items = value; OnItemsChanged?.Invoke(value); }
        }


        private IEnumerable<T> _currentItemsOnPage;

        public IEnumerable<T> CurrentItemsOnPage
        {
            get { return _currentItemsOnPage; }
            set { _currentItemsOnPage = value; OnShownItemsChanged?.Invoke(value); }
        }


        public DataBase(string name, IEnumerable<T> items, int maxPerPage)
        {
            Name = name;
            MaxPerPage = maxPerPage;

            Items = items;

            Pages = (int)MathF.Ceiling(items.Count() / maxPerPage);
            CurrentPage = 0;

            CurrentItemsOnPage = Items.Chunk(MaxPerPage).ToList()[CurrentPage];

            OnCurrentPageChanged += OnCurrPageChanged;
        }

        public virtual void OnCurrPageChanged(int page)
        {
            CurrentItemsOnPage = Items.Chunk(MaxPerPage).ToList()[CurrentPage];
        }

        public void NextPage()
        {
            int nextIndex = CurrentPage + 1;

            if (nextIndex >= Pages)
                return;

            CurrentPage = nextIndex;
        }

        public void PrevPage()
        {
            int nextIndex = CurrentPage - 1;

            if (nextIndex < 0)
                return;

            CurrentPage = nextIndex;
        }

        public void GoToPage(int pageIndex)
        {
            if (pageIndex < 0 || pageIndex >= Pages)
                throw new Exception($"Index out of range. {pageIndex}");

            CurrentPage = pageIndex;
        }
    }
}
