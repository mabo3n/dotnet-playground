using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Collections;

namespace DotNetPlayground
{
    public abstract class BaseIterator<T> : IEnumerator<T>
    {
        private int currentIndex = -1;
        private T currentItem;
        T IEnumerator<T>.Current => currentItem;
        public object Current => currentItem;

        public bool MoveNext()
        {
            currentIndex++;

            if (HasNext(currentIndex))
            {
                currentItem = GetCurrent(currentIndex);
                return true;
            }
            
            return false;
        }

        public abstract bool HasNext(int index);
        public abstract T GetCurrent(int index);

        public virtual void Reset()
        {
            currentIndex = -1;
        }
        
        public virtual void Dispose()
        {
            Reset();
        }
    }

    public class SingleValueIterator<T> : BaseIterator<T>
    {
        private T[] values; // sheet
        public override T GetCurrent(int index)
        {
            return values[index];
        }

        public override bool HasNext(int index)
        {
            return index < values.Length - 1;
        }
    }

    public class CompoundIterator<T> : BaseIterator<BaseIterator<T>>
    {
        // private BaseIterator<BaseIterator<T>> childIterators;
        private BaseIterator<T>[] childIterators;

        public override BaseIterator<T> GetCurrent(int index)
        {
            return childIterators[index];
        }

        public override bool HasNext(int index)
        {
            return index < childIterators.Length - 1;
        }
    }

    class Program
    {
        
        static void Main(string[] args)
        {
            var columnIterator = new SingleValueIterator<object>();
            var lineIterator = new CompoundIterator<object>();
        }
    }
}
