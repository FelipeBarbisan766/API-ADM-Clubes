using API_PI_ADM_Clubes.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API_PI_ADM_Clubes.Infrastructure.Configuration
{
    public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.HasKey(a => a.Id);
            
            builder.HasOne(a => a.Court)
                .WithMany(b => b.Schedules)
                .HasForeignKey(a => a.CourtId);
        }
    }
}
