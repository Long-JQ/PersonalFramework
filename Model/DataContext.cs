using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Model
{
    public class DataContext: DbContext
    {
        public DataContext():base("DataContext")
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleClass> ArticleClasses { get; set; }
        public DbSet<Notice> Notices { get; set; }
    }
}