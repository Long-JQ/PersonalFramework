namespace PersonalFramework.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PersonalFramework.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(PersonalFramework.DataContext context)
        {
            context.Roles.Add(new Model.Role { RoleName = "1" });
            context.Roles.Add(new Model.Role { RoleName = "2" });
            context.Roles.Add(new Model.Role { RoleName = "3" });
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
