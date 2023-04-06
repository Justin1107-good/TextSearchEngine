

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TextSearchEngine.Models;

namespace TextSearchEngine
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
        }
        public virtual DbSet<Essay> Essays { get; set; }
        public virtual DbSet<UserInfo> UserInfos { get; set; }

        public virtual DbSet<MarkDown> MarkDowns { get; set; }
    }
}
