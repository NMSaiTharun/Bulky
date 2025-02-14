using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }

        public void Update(Product obj)
        {
            var productObj=_db.Products.FirstOrDefault(p => p.Id == obj.Id);
            if (productObj != null)
            {
                productObj.Title=obj.Title;
                productObj.ISBN=obj.ISBN;              
                productObj.Price = obj.Price;
                productObj.Price50=obj.Price50;
                productObj.Price100=obj.Price100;
                productObj.ListPrice=obj.ListPrice;
                productObj.Description=obj.Description;
                productObj.CategoryId=obj.CategoryId;
                productObj.Author=obj.Author;   
                if(obj.ImageUrl!=null)
                {
                    productObj.ImageUrl=obj.ImageUrl;
                }
            }
            //_db.Products.Update(obj);
        }

    }
}
