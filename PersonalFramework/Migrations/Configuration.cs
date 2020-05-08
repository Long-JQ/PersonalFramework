namespace PersonalFramework.Migrations
{
    using PersonalFramework.Service;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Model.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Model.DataContext context)
        {
            var role1 = new Model.Role { RoleName = "管理员" };
            var role2 = new Model.Role { RoleName = "运营" };
            var role3 = new Model.Role { RoleName = "用户" };
            context.Roles.Add(role1);
            context.Roles.Add(role2);
            context.Roles.Add(role3);

            var salt = DeCrypt.SetSalt();
            context.Users.Add(new Model.User { UserName = "张三",RoleID = role3.ID, RoleName = role3.RoleName,Salt = salt, Password= DeCrypt.SetPassWord("123", salt) });

            salt = DeCrypt.SetSalt();
            var admin = new Model.Admin { AdminName = "admin",RoleID = role1.ID, RoleName = role1.RoleName, Salt = salt, Password = DeCrypt.SetPassWord("123", salt) };
            context.Admins.Add(admin);
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
