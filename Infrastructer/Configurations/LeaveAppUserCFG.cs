using Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructer.Configurations
{
    internal class LeaveAppUserCFG : IEntityTypeConfiguration<LeaveAppUser>
    {
        public void Configure(EntityTypeBuilder<LeaveAppUser> builder)
        {
            builder.Property(x => x.DateofResponse).IsRequired(false);
            builder.Property(x => x.StartDate).IsRequired();
            builder.Property(x => x.NumberOfRequestedDays).IsRequired();
        }
    }
}
