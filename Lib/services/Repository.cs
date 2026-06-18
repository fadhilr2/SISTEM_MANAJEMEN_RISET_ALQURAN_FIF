using System;
using System.Collections.Generic;
using System.Linq;

namespace Lib.services
{
    public class Repository<T>
    {
        private readonly List<T> _items = new();

        public Repository()
        {
        }

        public Repository(IEnumerable<T> items)
        {
            _items.AddRange(items);
        }

        public void Add(T item) => _items.Add(item);
        public bool Remove(T item) => _items.Remove(item);
        public IEnumerable<T> GetAll() => _items.AsReadOnly();
        public T? Find(Predicate<T> predicate) => _items.Find(predicate);
        public List<T> ToList() => _items.ToList();
    }
}
