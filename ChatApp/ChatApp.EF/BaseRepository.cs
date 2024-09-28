using ChatApp.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ChatApp.EF
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected ApplicationDbContext _context;
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<T> GetAll(string[]? includes = null)
        {
            return _context.Set<T>().ToList();
        }

        public T GetById(int id, string[]? includes = null)
        {
            var entity = _context.Set<T>().Find(id);
            if (entity == null)
                throw new Exception("There aren't any entity at this id");
            return entity;




        }
        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
                throw new Exception("this is no entity has this id");
            return entity;
        }

        public T Find(Expression<Func<T, bool>> match, string[]? includes = null)
        {
            /*
            IQueryable<T> query = _context.Set<T>();
            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);
            return query.SingleOrDefault(match);*/


            IQueryable<T> query = _context.Set<T>();
            if (includes != null)
                foreach (var include in includes)
                    {

                        int index = include.IndexOf(">");
                        if (index != -1)
                        {

                        ///*
                        //    string word1 = include.Substring(0, index);
                        //    string word2 = include.Substring(++index);
                        //    Console.WriteLine(word1);
                        //    Console.WriteLine(word2);
                        //    query = query.AsQueryable()
                        //        .Include($"{word1}")
                        //        .Include($"{word1}.{word2}");
                        //*/
                            string[] word = eagers(include);
                            int i = 0;
                            while (word[i] != null)
                            {
                                Console.WriteLine(word[i]);
                                query = query.Include(word[i]);
                            i++;
                            }
                        }
                        else
                        {
                            Console.WriteLine("failed");
                            query = query.Include(include);
                        }

                    }

            var entity = query.FirstOrDefault(match);
            if (entity == null)
                throw new Exception("this is no entity has this expression");
            return entity;
        }

        public T Find1(Expression<Func<T, bool>> match, string[]? includes = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);
            var entity = query.FirstOrDefault(match);
            if (entity == null)
                throw new Exception("this is no entity has this expression");
            return entity;
        }

        public T FindLast(Expression<Func<T, int>> match, string[]? includes = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);
            return query.OrderBy(match).Last();
        }
        public IEnumerable<T> FindAll(Expression<Func<T, bool>> match, string[]? includes = null)
        {
            
            IQueryable<T> query = _context.Set<T>().Where(match);
            if (includes != null)
                foreach (var item in query)
                    foreach (var include in includes)
                    {
                        
                        int index = include.IndexOf(">");
                        if (index != -1)
                        {
                            string word1 = include.Substring(0, index);
                            string word2 = include.Substring(++index);
                            Console.WriteLine(word1);
                            Console.WriteLine(word2);
                            query = query.AsQueryable()
                                .Include($"{word1}")
                                .Include($"{word1}.{word2}");
                                
                        }
                        else
                        {
                            Console.WriteLine("failed");
                            query = query.Include(include);
                        }
                            
                    }
                        
            return query;
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> match, string[]? includes = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (includes != null)
                foreach (var item in query)
                    foreach (var include in includes)
                        query = query.Include(include);
            return query.Where(match);
        }
        public IEnumerable<T> FindAllAL(Expression<Func<T, bool>> match, string[]? includes = null)// find all after doing eager loading
        {

            IQueryable<T> query = _context.Set<T>();
            if (includes != null)
                foreach (var item in query)
                    foreach (var include in includes)
                        query = query.Include(include);
            return query.Where(match);
        }

        public IEnumerable<T> IncludeAll(IEnumerable<T> query,string[]? includes = null)
        {
            IQueryable<T> newQuery = query.AsQueryable();
            if (includes != null)
                foreach (var include in includes)
                    newQuery = newQuery.Include(include);
            return newQuery;
        }
        public T IncludeOne(T entity, string[]? includes = null)
        {
            var list = new List<T>();
            list.Add(entity);
            IQueryable<T> newQuery = list.AsQueryable();
            if (includes != null)
                foreach (var include in includes)
                    newQuery = newQuery.Include(include);
            return newQuery.First();
        }

        public T Insert(T entity)
        {
            _context.Set<T>().Add(entity);
            return entity;
        }

        public async Task<T> InsertAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }



        public T Update(T entity)
        {
            _context.Update(entity);
            return entity;
        }
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public void DeleteRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }
        public void Attach(T entity)
        {
            _context.Set<T>().Attach(entity);
        }
        public int Count()
        {
            return _context.Set<T>().Count();
        }
        public int Count(Expression<Func<T, bool>> match)
        {
            return _context.Set<T>().Count(match);
        }
        public void DeleteAll()
        {
            var entities = _context.Set<T>().ToList();
            _context.Set<T>().RemoveRange(entities);
        }

        public abstract T GetByIdWith(int id);

        public abstract T GetByIdWith(int id, string[] includes);


        private string[] eagers(string x)
        {
            //string x = Console.ReadLine();
            int index = x.IndexOf(">");
            string[] word = new string[10];
            string[] sum = new string[10];
            int i = 0;
            while (index != -1)
            {
                word[i] = x.Substring(0, index);
                i++;
                x = x.Substring(++index);
                index = x.IndexOf(">");
            }
            word[i] = x;
            Console.WriteLine(word[0]);
            sum[0] = word[0];
            for (int j = 1; j < word.Length; j++)
            {
                if (word[j] == null)
                    break;
                Console.WriteLine(word[j]);
                sum[j] = sum[j - 1] + "." + word[j];
            }
            Console.WriteLine();
            for (int z = 0; z < sum.Length; z++)
            {
                if (sum[z] == null)
                    break;
                Console.WriteLine($"sum[{z}] = " + sum[z]);
            }
            return sum;
        }
    }
}
