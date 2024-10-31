using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK;

namespace SistemaBushidoSushiWok.Data
{
    public partial class BUSHIDOSUSHIWOKContext : DbContext
    {
        public BUSHIDOSUSHIWOKContext()
        {
        }

        public BUSHIDOSUSHIWOKContext(DbContextOptions<BUSHIDOSUSHIWOKContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Bodega>()
              .HasOne(i => i.Sucursal)
              .WithMany(i => i.Bodegas)
              .HasForeignKey(i => i.BodSucursal)
              .HasPrincipalKey(i => i.SucId);

            builder.Entity<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Boletum>()
              .HasOne(i => i.Sucursal)
              .WithMany(i => i.Boleta)
              .HasForeignKey(i => i.BolSucursal)
              .HasPrincipalKey(i => i.SucId);

            builder.Entity<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Boletum>()
              .HasOne(i => i.TipoPago)
              .WithMany(i => i.Boleta)
              .HasForeignKey(i => i.BolTipoPago)
              .HasPrincipalKey(i => i.TipaId);

            builder.Entity<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.CompraInsumo>()
              .HasOne(i => i.Bodega)
              .WithMany(i => i.CompraInsumos)
              .HasForeignKey(i => i.ComBodega)
              .HasPrincipalKey(i => i.BodId);

            builder.Entity<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.CompraInsumo>()
              .HasOne(i => i.Proveedor)
              .WithMany(i => i.CompraInsumos)
              .HasForeignKey(i => i.ComProveedor)
              .HasPrincipalKey(i => i.ProvId);

            builder.Entity<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleProducto>()
              .HasOne(i => i.Producto)
              .WithMany(i => i.DetalleProductos)
              .HasForeignKey(i => i.DetproProducto)
              .HasPrincipalKey(i => i.ProId);

            builder.Entity<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleRoll>()
              .HasOne(i => i.DetalleProducto)
              .WithMany(i => i.DetalleRolls)
              .HasForeignKey(i => i.RollDetproducto)
              .HasPrincipalKey(i => i.DeproId);

            builder.Entity<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleRoll>()
              .HasOne(i => i.Ingrediente)
              .WithMany(i => i.DetalleRolls)
              .HasForeignKey(i => i.RollIngrediente)
              .HasPrincipalKey(i => i.IngId);

            builder.Entity<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ingrediente>()
              .HasOne(i => i.TipoIngrediente)
              .WithMany(i => i.Ingredientes)
              .HasForeignKey(i => i.IngTipo)
              .HasPrincipalKey(i => i.TipingId);

            builder.Entity<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Mueble>()
              .HasOne(i => i.Sucursal)
              .WithMany(i => i.Muebles)
              .HasForeignKey(i => i.MuebSucursal)
              .HasPrincipalKey(i => i.SucId);

            builder.Entity<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Orden>()
              .HasOne(i => i.Boletum)
              .WithMany(i => i.Ordens)
              .HasForeignKey(i => i.OrdBoleta)
              .HasPrincipalKey(i => i.BolId);

            builder.Entity<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Orden>()
              .HasOne(i => i.Producto)
              .WithMany(i => i.Ordens)
              .HasForeignKey(i => i.OrdPlato)
              .HasPrincipalKey(i => i.ProId);

            builder.Entity<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Producto>()
              .HasOne(i => i.TipoPlato)
              .WithMany(i => i.Productos)
              .HasForeignKey(i => i.ProTipo)
              .HasPrincipalKey(i => i.TiplaId);

            builder.Entity<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Repartidor>()
              .HasOne(i => i.Sucursal)
              .WithMany(i => i.Repartidors)
              .HasForeignKey(i => i.RepSucursal)
              .HasPrincipalKey(i => i.SucId);

            builder.Entity<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Sucursal>()
              .HasOne(i => i.Ciudad)
              .WithMany(i => i.Sucursals)
              .HasForeignKey(i => i.SucCiudad)
              .HasPrincipalKey(i => i.CiuId);

            builder.Entity<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Trabajador>()
              .HasOne(i => i.Cargo)
              .WithMany(i => i.Trabajadors)
              .HasForeignKey(i => i.TraCargo)
              .HasPrincipalKey(i => i.CarId);

            builder.Entity<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Trabajador>()
              .HasOne(i => i.Sucursal)
              .WithMany(i => i.Trabajadors)
              .HasForeignKey(i => i.TraSucursal)
              .HasPrincipalKey(i => i.SucId);

            builder.Entity<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Stock>()
              .HasOne(i => i.Bodega)
              .WithMany(i => i.Stocks)
              .HasForeignKey(i => i.StoBodega)
              .HasPrincipalKey(i => i.BodId);

            builder.Entity<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Stock>()
              .HasOne(i => i.MateriaPrima)
              .WithMany(i => i.Stocks)
              .HasForeignKey(i => i.StoMateria)
              .HasPrincipalKey(i => i.MaprId);

            builder.Entity<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Vehiculo>()
              .HasOne(i => i.Sucursal)
              .WithMany(i => i.Vehiculos)
              .HasForeignKey(i => i.VehiSucursal)
              .HasPrincipalKey(i => i.SucId);
            this.OnModelBuilding(builder);
        }

        public DbSet<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Bodega> Bodegas { get; set; }

        public DbSet<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Boletum> Boleta { get; set; }

        public DbSet<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Cargo> Cargos { get; set; }

        public DbSet<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ciudad> Ciudads { get; set; }

        public DbSet<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.CompraInsumo> CompraInsumos { get; set; }

        public DbSet<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleProducto> DetalleProductos { get; set; }

        public DbSet<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleRoll> DetalleRolls { get; set; }

        public DbSet<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ingrediente> Ingredientes { get; set; }

        public DbSet<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.MateriaPrima> MateriaPrimas { get; set; }

        public DbSet<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Mueble> Muebles { get; set; }

        public DbSet<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Orden> Ordens { get; set; }

        public DbSet<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Producto> Productos { get; set; }

        public DbSet<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Proveedor> Proveedors { get; set; }

        public DbSet<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Repartidor> Repartidors { get; set; }

        public DbSet<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Sucursal> Sucursals { get; set; }

        public DbSet<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoIngrediente> TipoIngredientes { get; set; }

        public DbSet<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPago> TipoPagos { get; set; }

        public DbSet<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPlato> TipoPlatos { get; set; }

        public DbSet<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Trabajador> Trabajadors { get; set; }

        public DbSet<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Stock> Stocks { get; set; }

        public DbSet<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Vehiculo> Vehiculos { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    }
}