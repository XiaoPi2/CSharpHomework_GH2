using Microsoft.EntityFrameworkCore;

namespace OrderApi.Models{
    public class OrderContext : DbContext{        
        public OrderContext(DbContextOptions<OrderContext> options)
            : base(options){
            this.Database.EnsureCreated(); //自动建库建表
        }
        public DbSet<Order> orders{get; set;}
//        public DbSet<OrderItem> orderItems{get; set;}
//        public DbSet<Goods> goods{get; set;}
        
    }
}