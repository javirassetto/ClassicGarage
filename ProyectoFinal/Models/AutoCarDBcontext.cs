using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using static Microsoft.IO.RecyclableMemoryStreamManager;

namespace ProyectoFinal.Models
{
    public partial class AutoCarDBcontext : IdentityDbContext
    {
        public AutoCarDBcontext()
        {
        }

        //esto hace que nuestro dbContext herede de los servicios la config de BD
        public AutoCarDBcontext(DbContextOptions<AutoCarDBcontext> options)
            : base(options)
        {
        }

        public virtual DbSet<Marca> Marcas { get; set; }
        public virtual DbSet<Modelo> Modelos { get; set; }       
        public virtual DbSet<Tipo> Tipos { get; set; }
        public virtual DbSet<Categoria> Categorias { get; set; }

        public virtual DbSet<Repuesto> Repuestos { get; set; }

        public virtual DbSet<Vehiculo> Vehiculos { get; set; }
        public virtual DbSet<Venta> Ventas { get; set; }

        public virtual DbSet<VentaVehiculo> VentasVehiculos { get; set; }
             
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
            // modelBuilder.Entity<VentaVehiculo>().HasKey(v => new { v.Id });
            modelBuilder.Entity<Venta>().Property(t => t.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<VentaVehiculo>().Property(t => t.Id).ValueGeneratedOnAdd();
            base.OnModelCreating(modelBuilder); // necesario para Identity

                  modelBuilder.Entity<Modelo>()
                .HasOne(m => m.Marca)
                .WithMany(ma => ma.Modelos)
                .HasForeignKey(m => m.MarcaId)
                .OnDelete(DeleteBehavior.Restrict); // 🔴 cambio aquí

                // Relación Vehiculo -> Modelo
                modelBuilder.Entity<Vehiculo>()
                    .HasOne(v => v.Modelo)
                    .WithMany()
                    .HasForeignKey(v => v.ModeloId)
                    .OnDelete(DeleteBehavior.Restrict); // 🔴 cambio aquí

                // Relación Vehiculo -> Marca
                modelBuilder.Entity<Vehiculo>()
                    .HasOne(v => v.Marca)
                    .WithMany()
                    .HasForeignKey(v => v.MarcaId)
                    .OnDelete(DeleteBehavior.Restrict); // 🔴 cambio aquí


        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);





        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{ 
        //   optionsBuilder.UseSqlServer("name=conexion");
        //}
    }
}
  