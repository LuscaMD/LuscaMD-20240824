using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GestaoColaboradores.Models.Entities;

namespace GestaoColaboradores.Context
{
    public class GestaoColaboradoresContext : DbContext
    {
        public GestaoColaboradoresContext (DbContextOptions<GestaoColaboradoresContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Colaborador>()
                .HasOne(c => c.Unidade)
                .WithMany(u => u.Colaboradores)
                .HasForeignKey(c => c.CodigoUnidade) 
                .HasPrincipalKey(u => u.CodigoUnidade)
                .OnDelete(DeleteBehavior.Cascade); 

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Colaborador> Colaborador { get; set; }

        public DbSet<Unidade> Unidade { get; set; } 

        public DbSet<Usuario> Usuario { get; set; }

    }
}
