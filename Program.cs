using System;
using System.Collections.Generic;

namespace DotNetPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    public interface IGridDataSource
    {
        object GetAt(int x, int y);
    }

    public interface IImportableItem { }
    public class InternalDocumentItem : IImportableItem { }

    public interface ImportableItemRepository<T> where T : IImportableItem
    { 
        IEnumerable<T> Get(Func<T, bool> predicate = null);
    }
    public class InternalDocumentItemRepository : ImportableItemRepository<InternalDocumentItem>
    {
        public IEnumerable<InternalDocumentItem> Get(
            Func<InternalDocumentItem, bool> predicate = null)
        {
            throw new NotImplementedException();
        }
    }

    public static class ImportJobDelegates
    {
        public delegate void ImportJobEvent(object source, EventArgs args);
        public delegate void ImportItemEvent(object source, EventArgs args);
    }

    public abstract class ImportJobFacade
    {
        public event ImportJobDelegates.ImportJobEvent OnJobStarted;
        public event ImportJobDelegates.ImportItemEvent OnItemProcessed;
        public event ImportJobDelegates.ImportJobEvent OnJobEnded;

        InternalDocumentItemRepository source;

        public virtual void Start()
        {
            OnJobStarted(this, null);
            ValidateSource();
            ValidateTarget();
            
            foreach (var item in source.Get())
            {
                ValidateItem(item);
                MigrateItem(item);
                OnItemProcessed(item, null);
            }
            OnJobEnded(this, null); 
        }

        protected abstract void ValidateSource();
        protected abstract void ValidateTarget();
        
        protected abstract bool ValidateItem(IImportableItem item);
        protected abstract bool MigrateItem(IImportableItem item);
    }
}
