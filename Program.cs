using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.OleDb;

namespace DotNetPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    public class Document { }

    public interface ConvertibleToEntity<TEntity>
    { 
        TEntity Convert();
    }

    public class DocumentRepresentation : ConvertibleToEntity<Document>
    {
        public Document Convert()
        {
            return new Document();
        }
    }

    public abstract class SheetContext : DbContext
    {
        public abstract object GetAt(int x, int y);
        public abstract void SetAt(int x, int y);
    }

    public class FileSheetContext : SheetContext
    {
        private readonly string filePath;

        public FileSheetContext(string filePath)
        {
            this.filePath = filePath;
        }

        public override object GetAt(int x, int y)
        {
            return 1;
        }

        public override void SetAt(int x, int y)
        {
            
        }
    }

    public abstract class SheetRepository
    {      
        private readonly SheetContext context;

        public SheetRepository(SheetContext context)
        {
            this.context = context;
        }

        
    }
    public class DocRepository : SheetRepository
    { 
        public DocRepository(SheetContext context) : base(context) { }
    }


    public class MigrationDelegates
    {
        public delegate void MigrationEvent(object source, EventArgs args);
        public delegate void MigrationItemEvent(object source, EventArgs args);
    }

    public abstract class MigrationFacade
    {
        public event MigrationDelegates.MigrationEvent OnMigrationStarted;
        public event MigrationDelegates.MigrationItemEvent OnItemProcessed;
        public event MigrationDelegates.MigrationEvent OnMigrationEnded;

        Repository source;
        Repository target;

        public virtual void Start()
        {
            OnMigrationStarted(this, null);
            ValidateSource();
            ValidateTarget();
            
            foreach (var item in source.Get())
            {
                ValidateItem(item);
                MigrateItem(item);
                OnItemProcessed(item, null);
            }
            OnMigrationEnded(this, null); 
        }

        protected abstract void ValidateSource();
        protected abstract void ValidateTarget();
        
        protected abstract bool ValidateItem(ImportableItem item);
        protected abstract bool MigrateItem(ImportableItem item);
    }
}
