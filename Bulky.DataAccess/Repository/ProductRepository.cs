using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class ProductRepository : Repository<Product> , IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }
        public void Update(Product obj)
        {
           _db.Products.Update(obj);
            var objdb = _db.Products.FirstOrDefault(u=>u.Id == obj.Id);
            if (objdb != null)
            {
                objdb.Title = obj.Title;
                objdb.Description = obj.Description;
                objdb.Model = obj.Model;
                objdb.ListPrice = obj.ListPrice;
                objdb.Price = obj.Price;
                objdb.Price50 = obj.Price50;
                objdb.Price100 = obj.Price100;
                objdb.CategoryId = obj.CategoryId;

                if(obj.ImageUrl != null)
                {
                    objdb.ImageUrl = obj.ImageUrl; 
                }
            }

        }
    }
}
