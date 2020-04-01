namespace PersonalFramework.Migrations
{
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
            context.Roles.Add(new Model.Role { RoleName = "角色1" });
            context.Roles.Add(new Model.Role { RoleName = "角色2" });
            context.Roles.Add(new Model.Role { RoleName = "角色3" });

            context.Users.Add(new Model.User { UserName = "角色3", RoleName = "角色3",Password="123" });
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
