using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task16
{
    public class DynamicList<T> : IEnumerable<T>
    {
         private const int DefaultCapacity = 4;
        private static readonly T[] EmptyArray = new T[0];
        private T[] _items;
        private int _size;
        private int _version;
        // Constructs a List. The list is initially empty and has a capacity 
        // of zero. Upon adding the first element to the list the capacity is
        // increased to 16, and then increased in multiples of two as required. 
        public DynamicList()
        {
            _items = EmptyArray;
        }

        // Constructs a List with a given initial capacity. The list is
        // initially empty, but will have room for the given number of elements 
        // before any reallocations are required. 
        public DynamicList(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(paramName: "capacity");
            }
            _items = new T[capacity];
        }

        // Constructs a List, copying the contents of the given collection. The
        // size and capacity of the new list will both be equal to the size of the 
        // given collection. 
        public DynamicList(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentException("Bad collection");
            }
            var c = collection as ICollection<T>;
            if (c != null)
            {
                int count = c.Count;
                _items = new T[count];
                c.CopyTo(_items, 0);
                _size = count;
            }
            else
            {
                _size = 0;
                _items = new T[DefaultCapacity];
                using (IEnumerator<T> en = collection.GetEnumerator())
                {
                    while (en.MoveNext())
                    {
                        Add(en.Current);
                    }
                }
            }
        }

        // Gets and sets the capacity of this list.  The capacity is the size of
        // the internal array used to hold items.  When set, the internal 
        // array of the list is reallocated to the given capacity. 
        public int Capacity
        {
            get { return _items.Length; }
            set
            {
                if (value != _items.Length)
                {
                    if (value < _size)
                    {
                        throw new ArgumentException("ArgumentOutOfRangeSmallCapacity");
                    }
                    if (value > 0)
                    {
                        var newItems = new T[value];
                        if (_size > 0)
                        {
                            Array.Copy(_items, 0, newItems, 0, _size);
                        }
                        _items = newItems;
                    }
                    else
                    {
                        _items = EmptyArray;
                    }
                }
            }
        }

        // Read-only property describing how many elements are in the List. 
        public int Count
        {
            get { return _size; }
        }

        // Sets or Gets the element at the given index.

        //

        public T this[int index]
        {
            get
            {
                // Fllowing trick can reduce the range check by one
                if ((uint) index >= (uint) _size)
                {
                    throw new ArgumentOutOfRangeException();
                }
                return _items[index];
            }
            set
            {
                if ((uint) index >= (uint) _size)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _items[index] = value;
                _version++;
            }
        }

        // Adds the given T to the end of this list. The size of the list is 
        // increased by one. If required, the capacity of the list is doubled
        // before adding the new element. 
        public void Add(T item)
        {
            OnBeforeItemAdded(this, new GenericItemEventArgs<T>(item));
            if (_size == _items.Length)
            {
                EnsureCapacity(_size + 1);
            }
            _items[_size++] = item;
            _version++;
            OnItemAdded(this, new GenericItemEventArgs<T>(item));
        }

        // Clears the contents of List.
        public void Clear()
        {
            if (_size > 0)
            {
                Array.Clear(_items, 0, _size);
                _size = 0;
            }
            _version++;
            OnItemsCleared(this, new EventArgs());
        }

        // Contains returns true if the specified element is in the List. 
        // It does a linear, O(n) search.  Equality is determined by calling 
        // item.Equals().
        public bool Contains(T item)
        {
            if (item == null)
            {
                for (int i = 0; i < _size; i++)
                    if (_items[i] == null)
                        return true;
                return false;
            }
            else
            {
                EqualityComparer<T> c = EqualityComparer<T>.Default;
                for (int i = 0; i < _size; i++)
                {
                    if (c.Equals(_items[i], item)) return true;
                }
                return false;
            }
        }

        // Copies this List into array, which must be of a
        // compatible array type.
        public void CopyTo(T[] array)
        {
            CopyTo(array, 0);
        }

        // Copies a section of this list to the given array at the given index.
        // The method uses the Array.Copy method to copy the elements.
        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            if (_size - index < count)
            {
                throw new ArgumentOutOfRangeException();
            }
            // Delegate rest of error checking to Array.Copy.
            Array.Copy(_items, index, array, arrayIndex, count);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            // Delegate rest of error checking to Array.Copy. 
            Array.Copy(_items, 0, array, arrayIndex, _size);
        }

        // Ensures that the capacity of this list is at least the given minimum
        // value. If the currect capacity of the list is less than min, the 
        // capacity is increased to twice the current capacity or to min,
        // whichever is larger.
        private void EnsureCapacity(int min)
        {
            if (_items.Length < min)
            {
                int newCapacity = _items.Length == 0 ? DefaultCapacity : _items.Length*2;
                if (newCapacity < min) newCapacity = min;
                Capacity = newCapacity;
            }
        }

        // Returns the index of the first occurrence of a given value in a range of 
        // this list. The list is searched forwards from beginning to end. 
        // The elements of the list are compared to the given value using the
        // T.Equals method. 
        // This method uses the Array.IndexOf method to perform the
        // search.
        public int IndexOf(T item)
        {
            return Array.IndexOf(_items, item, 0, _size);
        }

        // Removes the element at the given index. The size of the list is
        // decreased by one. 
        public bool Remove(T item)
        {
            OnBeforeItemRemoved(this, new GenericItemEventArgs<T>(item));
            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                OnItemRemoved(this, new EventArgs());
                return true;
            }
            return false;
        }

        // Removes the element at the given index. The size of the list is
        // decreased by one.
        public void RemoveAt(int index)
        {
            if ((uint) index >= (uint) _size)
            {
                throw new ArgumentOutOfRangeException();
            }
            _size--;
            if (index < _size)
            {
                Array.Copy(_items, index + 1, _items, index, _size - index);
            }
            _items[_size] = default(T);
            _version++;
        }

        // Sorts the elements in this list.  Uses the default comparer and
        // Array.Sort. 
        public void Sort()
        {
            Sort(0, Count, null);
        }

        // Sorts the elements in a section of this list. The sort compares the 
        // elements to each other using the given IComparer interface. If
        // comparer is null, the elements are compared to each other using 
        // the IComparable interface, which in that case must be implemented by all 
        // elements of the list.
        // This method uses the Array.Sort method to sort the elements.
        public void Sort(int index, int count, IComparer<T> comparer)
        {
            if (index < 0 || count < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (_size - index < count)
            {
                throw new ArgumentOutOfRangeException();
            }
            Array.Sort(_items, index, count, comparer);

            _version++;
        }

        #region Events

        /// <summary>
        ///     Raises when an item is added to the list.
        /// </summary>
        public event EventHandler<GenericItemEventArgs<T>> ItemAdded;

        /// <summary>
        ///     Raises before an item is added to the list.
        /// </summary>
        public event EventHandler<GenericItemEventArgs<T>> BeforeItemAdded;

        /// <summary>
        ///     Raises when an item is removed from the list.
        /// </summary>
        public event EventHandler<EventArgs> ItemRemoved;

        /// <summary>
        ///     Raises before an item is removed from the list.
        /// </summary>
        public event EventHandler<GenericItemEventArgs<T>> BeforeItemRemoved;

        /// <summary>
        ///     Raises when the items are cleared from the list.
        /// </summary>
        public event EventHandler<EventArgs> ItemsCleared;

        #endregion

        #region Event Methods

        /// <summary>
        ///     Raises when an Item is added to the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">GenericItemEventArgs</param>
        protected virtual void OnItemAdded(object sender, GenericItemEventArgs<T> e)
        {
            if (ItemAdded != null)
                ItemAdded(sender, e);
        }

        /// <summary>
        ///     Raises before an Item is added to the list.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GenericItemEventArgs</param>
        protected virtual void OnBeforeItemAdded(object sender, GenericItemEventArgs<T> e)
        {
            if (BeforeItemAdded != null)
                BeforeItemAdded(sender, e);
        }

        /// <summary>
        ///     Raises when an Item is removed from the list.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventsArgs</param>
        protected virtual void OnItemRemoved(object sender, EventArgs e)
        {
            if (ItemRemoved != null)
                ItemRemoved(sender, e);
        }

        /// <summary>
        ///     Raises before an Item is removed from the list.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GenericItemEventArgs</param>
        protected virtual void OnBeforeItemRemoved(object sender, GenericItemEventArgs<T> e)
        {
            if (BeforeItemRemoved != null)
                BeforeItemRemoved(sender, e);
        }

        /// <summary>
        ///     Raises when the Items are cleared from this list.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected virtual void OnItemsCleared(object sender, EventArgs e)
        {
            if (ItemsCleared != null)
                ItemsCleared(sender, e);
        }

        #endregion

        public IEnumerator<T> GetEnumerator()
        {
            var item = new List<T>(_items);
            return item.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
