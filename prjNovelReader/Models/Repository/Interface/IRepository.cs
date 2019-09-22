using System;
using System.Linq;
using System.Linq.Expressions;

namespace prjNovelReader.Models.Repository.Interface
{
    public interface IRepository<TEntity> : IDisposable
        //條件約束，必須為class型別
        where TEntity : class
    {
        void Create(TEntity instance);

        void Update(TEntity instance);

        void Delete(TEntity instance);

        TEntity Get(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> GetSome(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> GetAll();

        void SaveChanges();
    }
}