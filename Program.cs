using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Collections;

namespace DotNetPlayground
{
    public interface IDataSource
    {
        (int start, int end) XRange { get; }
        (int start, int end) YRange { get; }
        object GetValue(int x, int y);
        IEnumerable<object> GetXValues(int y);
        IEnumerable<object> GetYValues(int x);
    }

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

    public class LineIterator : BaseIterator<IEnumerable<object>>
    {
        private readonly IDataSource source;

        public LineIterator(IDataSource source)
        {
            this.source = source;
        }

        public override IEnumerable<object> GetCurrent(int index)
        {
            return source.GetXValues(index);
        }

        public override bool HasNext(int index)
        {
            return index >= source.XRange.start
                && index <= source.XRange.end;
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
            //var columnIterator = new SingleValueIterator<object>();
            //var lineIterator = new CompoundIterator<object>();
        }
    }
}
