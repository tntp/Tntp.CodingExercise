using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Tntp.WebApplication.Tests
{
    public class MockDbSet<T> : IDbSet<T> where T : class
    {
        private readonly List<T> _entities = new List<T>();

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public Expression Expression
        {
            get { return _entities.AsQueryable().Expression; }
        }

        public ObservableCollection<T> Local
        {
            get { throw new NotImplementedException(); }
        }

        public IQueryProvider Provider
        {
            get { return _entities.AsQueryable().Provider; }
        }

        public T Add(T entity)
        {
            _entities.Add(entity);
            return entity;
        }

        public T Attach(T entity)
        {
            throw new NotImplementedException();
        }

        public T Create()
        {
            throw new NotImplementedException();
        }

        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
        {
            throw new NotImplementedException();
        }

        public T Find(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _entities.GetEnumerator();
        }

        public T Remove(T entity)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _entities.GetEnumerator();
        }
    }
}