using System;

namespace Task16
{
    public class GenericItemEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Item
        /// </summary>
        public T Item { get; private set; }
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="item"></param>
        public GenericItemEventArgs(T item)
        {
            this.Item = item;
        }
    }
}
