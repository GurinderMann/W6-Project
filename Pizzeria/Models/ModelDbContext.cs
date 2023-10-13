using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Pizzeria.Models
{
    public partial class ModelDbContext : DbContext
    {
        public ModelDbContext()
            : base("name=ModelDbContext")
        {
        }

        public virtual DbSet<T_Dettaglio> T_Dettaglio { get; set; }
        public virtual DbSet<T_Ordine> T_Ordine { get; set; }
        public virtual DbSet<T_Prodotti> T_Prodotti { get; set; }
        public virtual DbSet<T_Utenti> T_Utenti { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<T_Ordine>()
                .Property(e => e.Totale)
                .HasPrecision(19, 4);

            modelBuilder.Entity<T_Ordine>()
                .HasMany(e => e.T_Dettaglio)
                .WithRequired(e => e.T_Ordine)
                .HasForeignKey(e => e.FKOrdine)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<T_Prodotti>()
                .Property(e => e.Costo)
                .HasPrecision(19, 4);

            modelBuilder.Entity<T_Prodotti>()
                .HasMany(e => e.T_Dettaglio)
                .WithRequired(e => e.T_Prodotti)
                .HasForeignKey(e => e.FKProdotto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<T_Utenti>()
                .HasMany(e => e.T_Ordine)
                .WithRequired(e => e.T_Utenti)
                .HasForeignKey(e => e.FKUtente)
                .WillCascadeOnDelete(false);
        }
    }
}
