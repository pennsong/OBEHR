﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using OBEHR.Models;
using System.Linq.Expressions;

namespace OBEHR.Models.DAL
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        internal OBEHRContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(OBEHRContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        //public virtual IQueryable<TEntity> Get(
        //    Expression<Func<TEntity, bool>> filter = null,
        //    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        //    string includeProperties = "",
        //    bool noTrack = false
        //    )
        //{
        //    IQueryable<TEntity> query;
        //    if (noTrack)
        //    {
        //        query = dbSet.AsNoTracking();
        //    }
        //    else
        //    {
        //        query = dbSet;
        //    }

        //    if (filter != null)
        //    {
        //        query = query.Where(filter);
        //    }

        //    foreach (var includeProperty in includeProperties.Split
        //        (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        //    {
        //        query = query.Include(includeProperty);
        //    }

        //    if (orderBy != null)
        //    {
        //        return orderBy(query);
        //    }
        //    else
        //    {
        //        return query;
        //    }
        //}

        public virtual IQueryable<TEntity> Get(bool noTrack = false)
        {
            if (noTrack)
            {
                return dbSet.AsNoTracking();
            }
            else
            {
                return dbSet;
            }

        }

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}