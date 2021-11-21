using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WebApplication156456
{
    public partial class HorarioContext : DbContext
    {
        public HorarioContext()
            : base("name=HorarioContext")
        {
        }

        public virtual DbSet<HorarioModel> Horarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HorarioModel>()
                .Property(e => e.text)
                .IsUnicode(false);
        }
    }
}
