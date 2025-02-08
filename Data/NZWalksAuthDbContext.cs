using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZ_Walk.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //هذا يضمن أن أي إعدادات أساسية تم تعريفها في
            //IdentityDbContext أو DbContext
            //يتم تنفيذها أولًا قبل تنفيذ الكود المخصص
            base.OnModelCreating(builder);

            var readerRoleId = "7096d965-d82f-46ee-83dd-542774f949a3";
            var writerRoleId = "601dfa4a-a03d-4f14-8f4a-eb60578091e8";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = readerRoleId,
                    ConcurrencyStamp = readerRoleId,
                    Name = "Reader",
                    NormalizedName="Reader".ToUpper()
                },
                new IdentityRole
                {
                    Id =writerRoleId,
                    ConcurrencyStamp=writerRoleId,
                    Name= "Writer",
                    NormalizedName="Writer".ToUpper()
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
