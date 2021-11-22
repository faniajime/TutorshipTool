using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WebApplication156456.Context
{
    public partial class SesionHorarioContext : DbContext
    {
        public SesionHorarioContext()
            : base("name=SesionHorarioContext")
        {
        }

        public virtual DbSet<SesionHorario> sesiones { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SesionHorario>()
                .Property(e => e.text)
                .IsUnicode(false);
        }
    }
}
