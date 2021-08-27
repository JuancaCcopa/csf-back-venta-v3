using System.Collections.Generic;

namespace Net.Connection
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected IConnectionSQL context { get; set; }

        public RepositoryBase(IConnectionSQL context)
        {
            this.context = context;
        }
        //public IEnumerable<T> FindAll(T entity, string storeProcedure)
        //{
        //    return context.ExecuteSqlViewAll<T>(storeProcedure, entity);
        //}

        //public IEnumerable<T> FindByCondition(object entity, string storeProcedure)
        //{
        //    return context.ExecuteSqlViewFindByCondition<T>(storeProcedure, entity);
        //}

        //public T FindById(T entity, string storeProcedure)
        //{
        //    return context.ExecuteSqlViewId<T>(storeProcedure, entity);
        //}

        //public int Create(T entity, string storeProcedure)
        //{
        //    return (int)context.ExecuteSqlInsert<T>(storeProcedure, entity);
        //}

        //public void Delete(T entity, string storeProcedure)
        //{
        //    context.ExecuteSqlDelete<T>(storeProcedure, entity);
        //}

        //public void Update(T entity, string storeProcedure)
        //{
        //    this.context.ExecuteSqlUpdate<T>(storeProcedure, entity);
        //}
    }
}
