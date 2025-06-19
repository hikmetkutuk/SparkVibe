using System.Linq.Expressions;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Base;

namespace Infrastructure.Persistence.EntityFramework.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        /* 📌 Applies global query filters to all entities that inherit from BaseEntity.
            This implementation automatically filters out soft-deleted entities by adding
            a query filter that checks if the 'Deleted' property is false. */
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(BaseEntity<>).IsAssignableFrom(entityType.ClrType)) continue;
            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var propertyMethodInfo = typeof(EF).GetMethod("Property")!
                .MakeGenericMethod(typeof(bool));
            var deletedProperty = Expression.Call(propertyMethodInfo, parameter, Expression.Constant("Deleted"));
            var compareExpression = Expression.Equal(deletedProperty, Expression.Constant(false));

            var lambda = Expression.Lambda(compareExpression, parameter);
            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
        }
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<ApplicationRole> ApplicationRoles { get; set; }
}