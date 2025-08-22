using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApi5.Entities.concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi5.DataAccess.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(x =>x.CreatedAt).IsRequired().HasDefaultValueSql("GetDate()"); 
            builder.Property(x =>x.IsDeleted).IsRequired().HasDefaultValue(false); 
            builder.Property(x=>x.Name).IsRequired().HasMaxLength(20);
            builder.Property(x => x.ImagePath).IsRequired();
        }
    }
}
