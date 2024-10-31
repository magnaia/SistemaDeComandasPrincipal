using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using SistemaBushidoSushiWok.Data;

namespace SistemaBushidoSushiWok
{
    public partial class BUSHIDOSUSHIWOKService
    {
        BUSHIDOSUSHIWOKContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly BUSHIDOSUSHIWOKContext context;
        private readonly NavigationManager navigationManager;

        public BUSHIDOSUSHIWOKService(BUSHIDOSUSHIWOKContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportBodegasToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/bodegas/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/bodegas/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportBodegasToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/bodegas/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/bodegas/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnBodegasRead(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Bodega> items);

        public async Task<IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Bodega>> GetBodegas(Query query = null)
        {
            var items = Context.Bodegas.AsQueryable();

            items = items.Include(i => i.Sucursal);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnBodegasRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnBodegaGet(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Bodega item);
        partial void OnGetBodegaByBodId(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Bodega> items);


        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Bodega> GetBodegaByBodId(int bodid)
        {
            var items = Context.Bodegas
                              .AsNoTracking()
                              .Where(i => i.BodId == bodid);

            items = items.Include(i => i.Sucursal);
 
            OnGetBodegaByBodId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnBodegaGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnBodegaCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Bodega item);
        partial void OnAfterBodegaCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Bodega item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Bodega> CreateBodega(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Bodega bodega)
        {
            OnBodegaCreated(bodega);

            var existingItem = Context.Bodegas
                              .Where(i => i.BodId == bodega.BodId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Bodegas.Add(bodega);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(bodega).State = EntityState.Detached;
                throw;
            }

            OnAfterBodegaCreated(bodega);

            return bodega;
        }

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Bodega> CancelBodegaChanges(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Bodega item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnBodegaUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Bodega item);
        partial void OnAfterBodegaUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Bodega item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Bodega> UpdateBodega(int bodid, SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Bodega bodega)
        {
            OnBodegaUpdated(bodega);

            var itemToUpdate = Context.Bodegas
                              .Where(i => i.BodId == bodega.BodId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(bodega);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterBodegaUpdated(bodega);

            return bodega;
        }

        partial void OnBodegaDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Bodega item);
        partial void OnAfterBodegaDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Bodega item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Bodega> DeleteBodega(int bodid)
        {
            var itemToDelete = Context.Bodegas
                              .Where(i => i.BodId == bodid)
                              .Include(i => i.CompraInsumos)
                              .Include(i => i.Stocks)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnBodegaDeleted(itemToDelete);


            Context.Bodegas.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterBodegaDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportBoletaToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/boleta/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/boleta/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportBoletaToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/boleta/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/boleta/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnBoletaRead(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Boletum> items);

        public async Task<IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Boletum>> GetBoleta(Query query = null)
        {
            var items = Context.Boleta.AsQueryable();

            items = items.Include(i => i.Sucursal);
            items = items.Include(i => i.TipoPago);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnBoletaRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnBoletumGet(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Boletum item);
        partial void OnGetBoletumByBolId(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Boletum> items);


        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Boletum> GetBoletumByBolId(int bolid)
        {
            var items = Context.Boleta
                              .AsNoTracking()
                              .Where(i => i.BolId == bolid);

            items = items.Include(i => i.Sucursal);
            items = items.Include(i => i.TipoPago);
 
            OnGetBoletumByBolId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnBoletumGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnBoletumCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Boletum item);
        partial void OnAfterBoletumCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Boletum item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Boletum> CreateBoletum(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Boletum boletum)
        {
            OnBoletumCreated(boletum);

            var existingItem = Context.Boleta
                              .Where(i => i.BolId == boletum.BolId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Boleta.Add(boletum);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(boletum).State = EntityState.Detached;
                throw;
            }

            OnAfterBoletumCreated(boletum);

            return boletum;
        }

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Boletum> CancelBoletumChanges(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Boletum item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnBoletumUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Boletum item);
        partial void OnAfterBoletumUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Boletum item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Boletum> UpdateBoletum(int bolid, SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Boletum boletum)
        {
            OnBoletumUpdated(boletum);

            var itemToUpdate = Context.Boleta
                              .Where(i => i.BolId == boletum.BolId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(boletum);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterBoletumUpdated(boletum);

            return boletum;
        }

        partial void OnBoletumDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Boletum item);
        partial void OnAfterBoletumDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Boletum item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Boletum> DeleteBoletum(int bolid)
        {
            var itemToDelete = Context.Boleta
                              .Where(i => i.BolId == bolid)
                              .Include(i => i.Ordens)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnBoletumDeleted(itemToDelete);


            Context.Boleta.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterBoletumDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportCargosToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/cargos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/cargos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCargosToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/cargos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/cargos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCargosRead(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Cargo> items);

        public async Task<IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Cargo>> GetCargos(Query query = null)
        {
            var items = Context.Cargos.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnCargosRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCargoGet(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Cargo item);
        partial void OnGetCargoByCarId(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Cargo> items);


        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Cargo> GetCargoByCarId(int carid)
        {
            var items = Context.Cargos
                              .AsNoTracking()
                              .Where(i => i.CarId == carid);

 
            OnGetCargoByCarId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnCargoGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnCargoCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Cargo item);
        partial void OnAfterCargoCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Cargo item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Cargo> CreateCargo(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Cargo cargo)
        {
            OnCargoCreated(cargo);

            var existingItem = Context.Cargos
                              .Where(i => i.CarId == cargo.CarId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Cargos.Add(cargo);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(cargo).State = EntityState.Detached;
                throw;
            }

            OnAfterCargoCreated(cargo);

            return cargo;
        }

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Cargo> CancelCargoChanges(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Cargo item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCargoUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Cargo item);
        partial void OnAfterCargoUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Cargo item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Cargo> UpdateCargo(int carid, SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Cargo cargo)
        {
            OnCargoUpdated(cargo);

            var itemToUpdate = Context.Cargos
                              .Where(i => i.CarId == cargo.CarId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(cargo);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterCargoUpdated(cargo);

            return cargo;
        }

        partial void OnCargoDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Cargo item);
        partial void OnAfterCargoDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Cargo item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Cargo> DeleteCargo(int carid)
        {
            var itemToDelete = Context.Cargos
                              .Where(i => i.CarId == carid)
                              .Include(i => i.Trabajadors)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCargoDeleted(itemToDelete);


            Context.Cargos.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCargoDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportCiudadsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/ciudads/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/ciudads/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCiudadsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/ciudads/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/ciudads/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCiudadsRead(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ciudad> items);

        public async Task<IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ciudad>> GetCiudads(Query query = null)
        {
            var items = Context.Ciudads.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnCiudadsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCiudadGet(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ciudad item);
        partial void OnGetCiudadByCiuId(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ciudad> items);


        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ciudad> GetCiudadByCiuId(int ciuid)
        {
            var items = Context.Ciudads
                              .AsNoTracking()
                              .Where(i => i.CiuId == ciuid);

 
            OnGetCiudadByCiuId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnCiudadGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnCiudadCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ciudad item);
        partial void OnAfterCiudadCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ciudad item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ciudad> CreateCiudad(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ciudad ciudad)
        {
            OnCiudadCreated(ciudad);

            var existingItem = Context.Ciudads
                              .Where(i => i.CiuId == ciudad.CiuId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Ciudads.Add(ciudad);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(ciudad).State = EntityState.Detached;
                throw;
            }

            OnAfterCiudadCreated(ciudad);

            return ciudad;
        }

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ciudad> CancelCiudadChanges(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ciudad item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCiudadUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ciudad item);
        partial void OnAfterCiudadUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ciudad item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ciudad> UpdateCiudad(int ciuid, SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ciudad ciudad)
        {
            OnCiudadUpdated(ciudad);

            var itemToUpdate = Context.Ciudads
                              .Where(i => i.CiuId == ciudad.CiuId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(ciudad);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterCiudadUpdated(ciudad);

            return ciudad;
        }

        partial void OnCiudadDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ciudad item);
        partial void OnAfterCiudadDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ciudad item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ciudad> DeleteCiudad(int ciuid)
        {
            var itemToDelete = Context.Ciudads
                              .Where(i => i.CiuId == ciuid)
                              .Include(i => i.Sucursals)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCiudadDeleted(itemToDelete);


            Context.Ciudads.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCiudadDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportCompraInsumosToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/comprainsumos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/comprainsumos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCompraInsumosToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/comprainsumos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/comprainsumos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCompraInsumosRead(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.CompraInsumo> items);

        public async Task<IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.CompraInsumo>> GetCompraInsumos(Query query = null)
        {
            var items = Context.CompraInsumos.AsQueryable();

            items = items.Include(i => i.Bodega);
            items = items.Include(i => i.Proveedor);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnCompraInsumosRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCompraInsumoGet(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.CompraInsumo item);
        partial void OnGetCompraInsumoByComId(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.CompraInsumo> items);


        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.CompraInsumo> GetCompraInsumoByComId(int comid)
        {
            var items = Context.CompraInsumos
                              .AsNoTracking()
                              .Where(i => i.ComId == comid);

            items = items.Include(i => i.Bodega);
            items = items.Include(i => i.Proveedor);
 
            OnGetCompraInsumoByComId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnCompraInsumoGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnCompraInsumoCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.CompraInsumo item);
        partial void OnAfterCompraInsumoCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.CompraInsumo item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.CompraInsumo> CreateCompraInsumo(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.CompraInsumo comprainsumo)
        {
            OnCompraInsumoCreated(comprainsumo);

            var existingItem = Context.CompraInsumos
                              .Where(i => i.ComId == comprainsumo.ComId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.CompraInsumos.Add(comprainsumo);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(comprainsumo).State = EntityState.Detached;
                throw;
            }

            OnAfterCompraInsumoCreated(comprainsumo);

            return comprainsumo;
        }

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.CompraInsumo> CancelCompraInsumoChanges(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.CompraInsumo item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCompraInsumoUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.CompraInsumo item);
        partial void OnAfterCompraInsumoUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.CompraInsumo item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.CompraInsumo> UpdateCompraInsumo(int comid, SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.CompraInsumo comprainsumo)
        {
            OnCompraInsumoUpdated(comprainsumo);

            var itemToUpdate = Context.CompraInsumos
                              .Where(i => i.ComId == comprainsumo.ComId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(comprainsumo);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterCompraInsumoUpdated(comprainsumo);

            return comprainsumo;
        }

        partial void OnCompraInsumoDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.CompraInsumo item);
        partial void OnAfterCompraInsumoDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.CompraInsumo item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.CompraInsumo> DeleteCompraInsumo(int comid)
        {
            var itemToDelete = Context.CompraInsumos
                              .Where(i => i.ComId == comid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCompraInsumoDeleted(itemToDelete);


            Context.CompraInsumos.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCompraInsumoDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportDetalleProductosToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/detalleproductos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/detalleproductos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportDetalleProductosToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/detalleproductos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/detalleproductos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnDetalleProductosRead(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleProducto> items);

        public async Task<IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleProducto>> GetDetalleProductos(Query query = null)
        {
            var items = Context.DetalleProductos.AsQueryable();

            items = items.Include(i => i.Producto);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnDetalleProductosRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnDetalleProductoGet(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleProducto item);
        partial void OnGetDetalleProductoByDeproId(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleProducto> items);


        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleProducto> GetDetalleProductoByDeproId(int deproid)
        {
            var items = Context.DetalleProductos
                              .AsNoTracking()
                              .Where(i => i.DeproId == deproid);

            items = items.Include(i => i.Producto);
 
            OnGetDetalleProductoByDeproId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnDetalleProductoGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnDetalleProductoCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleProducto item);
        partial void OnAfterDetalleProductoCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleProducto item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleProducto> CreateDetalleProducto(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleProducto detalleproducto)
        {
            OnDetalleProductoCreated(detalleproducto);

            var existingItem = Context.DetalleProductos
                              .Where(i => i.DeproId == detalleproducto.DeproId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.DetalleProductos.Add(detalleproducto);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(detalleproducto).State = EntityState.Detached;
                throw;
            }

            OnAfterDetalleProductoCreated(detalleproducto);

            return detalleproducto;
        }

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleProducto> CancelDetalleProductoChanges(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleProducto item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnDetalleProductoUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleProducto item);
        partial void OnAfterDetalleProductoUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleProducto item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleProducto> UpdateDetalleProducto(int deproid, SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleProducto detalleproducto)
        {
            OnDetalleProductoUpdated(detalleproducto);

            var itemToUpdate = Context.DetalleProductos
                              .Where(i => i.DeproId == detalleproducto.DeproId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(detalleproducto);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterDetalleProductoUpdated(detalleproducto);

            return detalleproducto;
        }

        partial void OnDetalleProductoDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleProducto item);
        partial void OnAfterDetalleProductoDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleProducto item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleProducto> DeleteDetalleProducto(int deproid)
        {
            var itemToDelete = Context.DetalleProductos
                              .Where(i => i.DeproId == deproid)
                              .Include(i => i.DetalleRolls)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnDetalleProductoDeleted(itemToDelete);


            Context.DetalleProductos.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterDetalleProductoDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportDetalleRollsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/detallerolls/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/detallerolls/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportDetalleRollsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/detallerolls/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/detallerolls/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnDetalleRollsRead(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleRoll> items);

        public async Task<IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleRoll>> GetDetalleRolls(Query query = null)
        {
            var items = Context.DetalleRolls.AsQueryable();

            items = items.Include(i => i.DetalleProducto);
            items = items.Include(i => i.Ingrediente);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnDetalleRollsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnDetalleRollGet(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleRoll item);
        partial void OnGetDetalleRollByRollId(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleRoll> items);


        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleRoll> GetDetalleRollByRollId(int rollid)
        {
            var items = Context.DetalleRolls
                              .AsNoTracking()
                              .Where(i => i.RollId == rollid);

            items = items.Include(i => i.DetalleProducto);
            items = items.Include(i => i.Ingrediente);
 
            OnGetDetalleRollByRollId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnDetalleRollGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnDetalleRollCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleRoll item);
        partial void OnAfterDetalleRollCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleRoll item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleRoll> CreateDetalleRoll(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleRoll detalleroll)
        {
            OnDetalleRollCreated(detalleroll);

            var existingItem = Context.DetalleRolls
                              .Where(i => i.RollId == detalleroll.RollId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.DetalleRolls.Add(detalleroll);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(detalleroll).State = EntityState.Detached;
                throw;
            }

            OnAfterDetalleRollCreated(detalleroll);

            return detalleroll;
        }

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleRoll> CancelDetalleRollChanges(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleRoll item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnDetalleRollUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleRoll item);
        partial void OnAfterDetalleRollUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleRoll item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleRoll> UpdateDetalleRoll(int rollid, SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleRoll detalleroll)
        {
            OnDetalleRollUpdated(detalleroll);

            var itemToUpdate = Context.DetalleRolls
                              .Where(i => i.RollId == detalleroll.RollId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(detalleroll);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterDetalleRollUpdated(detalleroll);

            return detalleroll;
        }

        partial void OnDetalleRollDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleRoll item);
        partial void OnAfterDetalleRollDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleRoll item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleRoll> DeleteDetalleRoll(int rollid)
        {
            var itemToDelete = Context.DetalleRolls
                              .Where(i => i.RollId == rollid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnDetalleRollDeleted(itemToDelete);


            Context.DetalleRolls.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterDetalleRollDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportIngredientesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/ingredientes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/ingredientes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportIngredientesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/ingredientes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/ingredientes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnIngredientesRead(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ingrediente> items);

        public async Task<IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ingrediente>> GetIngredientes(Query query = null)
        {
            var items = Context.Ingredientes.AsQueryable();

            items = items.Include(i => i.TipoIngrediente);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnIngredientesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnIngredienteGet(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ingrediente item);
        partial void OnGetIngredienteByIngId(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ingrediente> items);


        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ingrediente> GetIngredienteByIngId(int ingid)
        {
            var items = Context.Ingredientes
                              .AsNoTracking()
                              .Where(i => i.IngId == ingid);

            items = items.Include(i => i.TipoIngrediente);
 
            OnGetIngredienteByIngId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnIngredienteGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnIngredienteCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ingrediente item);
        partial void OnAfterIngredienteCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ingrediente item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ingrediente> CreateIngrediente(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ingrediente ingrediente)
        {
            OnIngredienteCreated(ingrediente);

            var existingItem = Context.Ingredientes
                              .Where(i => i.IngId == ingrediente.IngId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Ingredientes.Add(ingrediente);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(ingrediente).State = EntityState.Detached;
                throw;
            }

            OnAfterIngredienteCreated(ingrediente);

            return ingrediente;
        }

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ingrediente> CancelIngredienteChanges(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ingrediente item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnIngredienteUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ingrediente item);
        partial void OnAfterIngredienteUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ingrediente item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ingrediente> UpdateIngrediente(int ingid, SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ingrediente ingrediente)
        {
            OnIngredienteUpdated(ingrediente);

            var itemToUpdate = Context.Ingredientes
                              .Where(i => i.IngId == ingrediente.IngId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(ingrediente);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterIngredienteUpdated(ingrediente);

            return ingrediente;
        }

        partial void OnIngredienteDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ingrediente item);
        partial void OnAfterIngredienteDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ingrediente item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ingrediente> DeleteIngrediente(int ingid)
        {
            var itemToDelete = Context.Ingredientes
                              .Where(i => i.IngId == ingid)
                              .Include(i => i.DetalleRolls)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnIngredienteDeleted(itemToDelete);


            Context.Ingredientes.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterIngredienteDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportMateriaPrimasToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/materiaprimas/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/materiaprimas/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportMateriaPrimasToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/materiaprimas/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/materiaprimas/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnMateriaPrimasRead(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.MateriaPrima> items);

        public async Task<IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.MateriaPrima>> GetMateriaPrimas(Query query = null)
        {
            var items = Context.MateriaPrimas.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnMateriaPrimasRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnMateriaPrimaGet(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.MateriaPrima item);
        partial void OnGetMateriaPrimaByMaprId(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.MateriaPrima> items);


        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.MateriaPrima> GetMateriaPrimaByMaprId(int maprid)
        {
            var items = Context.MateriaPrimas
                              .AsNoTracking()
                              .Where(i => i.MaprId == maprid);

 
            OnGetMateriaPrimaByMaprId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnMateriaPrimaGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnMateriaPrimaCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.MateriaPrima item);
        partial void OnAfterMateriaPrimaCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.MateriaPrima item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.MateriaPrima> CreateMateriaPrima(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.MateriaPrima materiaprima)
        {
            OnMateriaPrimaCreated(materiaprima);

            var existingItem = Context.MateriaPrimas
                              .Where(i => i.MaprId == materiaprima.MaprId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.MateriaPrimas.Add(materiaprima);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(materiaprima).State = EntityState.Detached;
                throw;
            }

            OnAfterMateriaPrimaCreated(materiaprima);

            return materiaprima;
        }

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.MateriaPrima> CancelMateriaPrimaChanges(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.MateriaPrima item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnMateriaPrimaUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.MateriaPrima item);
        partial void OnAfterMateriaPrimaUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.MateriaPrima item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.MateriaPrima> UpdateMateriaPrima(int maprid, SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.MateriaPrima materiaprima)
        {
            OnMateriaPrimaUpdated(materiaprima);

            var itemToUpdate = Context.MateriaPrimas
                              .Where(i => i.MaprId == materiaprima.MaprId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(materiaprima);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterMateriaPrimaUpdated(materiaprima);

            return materiaprima;
        }

        partial void OnMateriaPrimaDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.MateriaPrima item);
        partial void OnAfterMateriaPrimaDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.MateriaPrima item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.MateriaPrima> DeleteMateriaPrima(int maprid)
        {
            var itemToDelete = Context.MateriaPrimas
                              .Where(i => i.MaprId == maprid)
                              .Include(i => i.Stocks)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnMateriaPrimaDeleted(itemToDelete);


            Context.MateriaPrimas.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterMateriaPrimaDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportMueblesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/muebles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/muebles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportMueblesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/muebles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/muebles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnMueblesRead(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Mueble> items);

        public async Task<IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Mueble>> GetMuebles(Query query = null)
        {
            var items = Context.Muebles.AsQueryable();

            items = items.Include(i => i.Sucursal);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnMueblesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnMuebleGet(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Mueble item);
        partial void OnGetMuebleByMuebId(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Mueble> items);


        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Mueble> GetMuebleByMuebId(int muebid)
        {
            var items = Context.Muebles
                              .AsNoTracking()
                              .Where(i => i.MuebId == muebid);

            items = items.Include(i => i.Sucursal);
 
            OnGetMuebleByMuebId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnMuebleGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnMuebleCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Mueble item);
        partial void OnAfterMuebleCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Mueble item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Mueble> CreateMueble(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Mueble mueble)
        {
            OnMuebleCreated(mueble);

            var existingItem = Context.Muebles
                              .Where(i => i.MuebId == mueble.MuebId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Muebles.Add(mueble);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(mueble).State = EntityState.Detached;
                throw;
            }

            OnAfterMuebleCreated(mueble);

            return mueble;
        }

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Mueble> CancelMuebleChanges(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Mueble item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnMuebleUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Mueble item);
        partial void OnAfterMuebleUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Mueble item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Mueble> UpdateMueble(int muebid, SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Mueble mueble)
        {
            OnMuebleUpdated(mueble);

            var itemToUpdate = Context.Muebles
                              .Where(i => i.MuebId == mueble.MuebId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(mueble);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterMuebleUpdated(mueble);

            return mueble;
        }

        partial void OnMuebleDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Mueble item);
        partial void OnAfterMuebleDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Mueble item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Mueble> DeleteMueble(int muebid)
        {
            var itemToDelete = Context.Muebles
                              .Where(i => i.MuebId == muebid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnMuebleDeleted(itemToDelete);


            Context.Muebles.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterMuebleDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportOrdensToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/ordens/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/ordens/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportOrdensToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/ordens/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/ordens/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnOrdensRead(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Orden> items);

        public async Task<IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Orden>> GetOrdens(Query query = null)
        {
            var items = Context.Ordens.AsQueryable();

            items = items.Include(i => i.Boletum);
            items = items.Include(i => i.Producto);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnOrdensRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnOrdenGet(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Orden item);
        partial void OnGetOrdenByOrdId(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Orden> items);


        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Orden> GetOrdenByOrdId(int ordid)
        {
            var items = Context.Ordens
                              .AsNoTracking()
                              .Where(i => i.OrdId == ordid);

            items = items.Include(i => i.Boletum);
            items = items.Include(i => i.Producto);
 
            OnGetOrdenByOrdId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnOrdenGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnOrdenCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Orden item);
        partial void OnAfterOrdenCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Orden item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Orden> CreateOrden(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Orden orden)
        {
            OnOrdenCreated(orden);

            var existingItem = Context.Ordens
                              .Where(i => i.OrdId == orden.OrdId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Ordens.Add(orden);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(orden).State = EntityState.Detached;
                throw;
            }

            OnAfterOrdenCreated(orden);

            return orden;
        }

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Orden> CancelOrdenChanges(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Orden item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnOrdenUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Orden item);
        partial void OnAfterOrdenUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Orden item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Orden> UpdateOrden(int ordid, SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Orden orden)
        {
            OnOrdenUpdated(orden);

            var itemToUpdate = Context.Ordens
                              .Where(i => i.OrdId == orden.OrdId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(orden);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterOrdenUpdated(orden);

            return orden;
        }

        partial void OnOrdenDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Orden item);
        partial void OnAfterOrdenDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Orden item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Orden> DeleteOrden(int ordid)
        {
            var itemToDelete = Context.Ordens
                              .Where(i => i.OrdId == ordid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnOrdenDeleted(itemToDelete);


            Context.Ordens.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterOrdenDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportProductosToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/productos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/productos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportProductosToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/productos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/productos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnProductosRead(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Producto> items);

        public async Task<IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Producto>> GetProductos(Query query = null)
        {
            var items = Context.Productos.AsQueryable();

            items = items.Include(i => i.TipoPlato);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnProductosRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnProductoGet(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Producto item);
        partial void OnGetProductoByProId(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Producto> items);


        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Producto> GetProductoByProId(int proid)
        {
            var items = Context.Productos
                              .AsNoTracking()
                              .Where(i => i.ProId == proid);

            items = items.Include(i => i.TipoPlato);
 
            OnGetProductoByProId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnProductoGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnProductoCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Producto item);
        partial void OnAfterProductoCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Producto item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Producto> CreateProducto(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Producto producto)
        {
            OnProductoCreated(producto);

            var existingItem = Context.Productos
                              .Where(i => i.ProId == producto.ProId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Productos.Add(producto);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(producto).State = EntityState.Detached;
                throw;
            }

            OnAfterProductoCreated(producto);

            return producto;
        }

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Producto> CancelProductoChanges(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Producto item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnProductoUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Producto item);
        partial void OnAfterProductoUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Producto item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Producto> UpdateProducto(int proid, SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Producto producto)
        {
            OnProductoUpdated(producto);

            var itemToUpdate = Context.Productos
                              .Where(i => i.ProId == producto.ProId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(producto);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterProductoUpdated(producto);

            return producto;
        }

        partial void OnProductoDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Producto item);
        partial void OnAfterProductoDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Producto item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Producto> DeleteProducto(int proid)
        {
            var itemToDelete = Context.Productos
                              .Where(i => i.ProId == proid)
                              .Include(i => i.DetalleProductos)
                              .Include(i => i.Ordens)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnProductoDeleted(itemToDelete);


            Context.Productos.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterProductoDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportProveedorsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/proveedors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/proveedors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportProveedorsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/proveedors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/proveedors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnProveedorsRead(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Proveedor> items);

        public async Task<IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Proveedor>> GetProveedors(Query query = null)
        {
            var items = Context.Proveedors.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnProveedorsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnProveedorGet(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Proveedor item);
        partial void OnGetProveedorByProvId(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Proveedor> items);


        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Proveedor> GetProveedorByProvId(int provid)
        {
            var items = Context.Proveedors
                              .AsNoTracking()
                              .Where(i => i.ProvId == provid);

 
            OnGetProveedorByProvId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnProveedorGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnProveedorCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Proveedor item);
        partial void OnAfterProveedorCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Proveedor item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Proveedor> CreateProveedor(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Proveedor proveedor)
        {
            OnProveedorCreated(proveedor);

            var existingItem = Context.Proveedors
                              .Where(i => i.ProvId == proveedor.ProvId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Proveedors.Add(proveedor);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(proveedor).State = EntityState.Detached;
                throw;
            }

            OnAfterProveedorCreated(proveedor);

            return proveedor;
        }

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Proveedor> CancelProveedorChanges(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Proveedor item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnProveedorUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Proveedor item);
        partial void OnAfterProveedorUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Proveedor item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Proveedor> UpdateProveedor(int provid, SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Proveedor proveedor)
        {
            OnProveedorUpdated(proveedor);

            var itemToUpdate = Context.Proveedors
                              .Where(i => i.ProvId == proveedor.ProvId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(proveedor);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterProveedorUpdated(proveedor);

            return proveedor;
        }

        partial void OnProveedorDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Proveedor item);
        partial void OnAfterProveedorDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Proveedor item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Proveedor> DeleteProveedor(int provid)
        {
            var itemToDelete = Context.Proveedors
                              .Where(i => i.ProvId == provid)
                              .Include(i => i.CompraInsumos)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnProveedorDeleted(itemToDelete);


            Context.Proveedors.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterProveedorDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportRepartidorsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/repartidors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/repartidors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportRepartidorsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/repartidors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/repartidors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnRepartidorsRead(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Repartidor> items);

        public async Task<IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Repartidor>> GetRepartidors(Query query = null)
        {
            var items = Context.Repartidors.AsQueryable();

            items = items.Include(i => i.Sucursal);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnRepartidorsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnRepartidorGet(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Repartidor item);
        partial void OnGetRepartidorByRepId(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Repartidor> items);


        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Repartidor> GetRepartidorByRepId(int repid)
        {
            var items = Context.Repartidors
                              .AsNoTracking()
                              .Where(i => i.RepId == repid);

            items = items.Include(i => i.Sucursal);
 
            OnGetRepartidorByRepId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnRepartidorGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnRepartidorCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Repartidor item);
        partial void OnAfterRepartidorCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Repartidor item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Repartidor> CreateRepartidor(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Repartidor repartidor)
        {
            OnRepartidorCreated(repartidor);

            var existingItem = Context.Repartidors
                              .Where(i => i.RepId == repartidor.RepId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Repartidors.Add(repartidor);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(repartidor).State = EntityState.Detached;
                throw;
            }

            OnAfterRepartidorCreated(repartidor);

            return repartidor;
        }

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Repartidor> CancelRepartidorChanges(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Repartidor item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnRepartidorUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Repartidor item);
        partial void OnAfterRepartidorUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Repartidor item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Repartidor> UpdateRepartidor(int repid, SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Repartidor repartidor)
        {
            OnRepartidorUpdated(repartidor);

            var itemToUpdate = Context.Repartidors
                              .Where(i => i.RepId == repartidor.RepId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(repartidor);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterRepartidorUpdated(repartidor);

            return repartidor;
        }

        partial void OnRepartidorDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Repartidor item);
        partial void OnAfterRepartidorDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Repartidor item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Repartidor> DeleteRepartidor(int repid)
        {
            var itemToDelete = Context.Repartidors
                              .Where(i => i.RepId == repid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnRepartidorDeleted(itemToDelete);


            Context.Repartidors.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterRepartidorDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportSucursalsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/sucursals/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/sucursals/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSucursalsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/sucursals/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/sucursals/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSucursalsRead(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Sucursal> items);

        public async Task<IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Sucursal>> GetSucursals(Query query = null)
        {
            var items = Context.Sucursals.AsQueryable();

            items = items.Include(i => i.Ciudad);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnSucursalsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSucursalGet(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Sucursal item);
        partial void OnGetSucursalBySucId(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Sucursal> items);


        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Sucursal> GetSucursalBySucId(int sucid)
        {
            var items = Context.Sucursals
                              .AsNoTracking()
                              .Where(i => i.SucId == sucid);

            items = items.Include(i => i.Ciudad);
 
            OnGetSucursalBySucId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnSucursalGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnSucursalCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Sucursal item);
        partial void OnAfterSucursalCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Sucursal item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Sucursal> CreateSucursal(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Sucursal sucursal)
        {
            OnSucursalCreated(sucursal);

            var existingItem = Context.Sucursals
                              .Where(i => i.SucId == sucursal.SucId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Sucursals.Add(sucursal);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(sucursal).State = EntityState.Detached;
                throw;
            }

            OnAfterSucursalCreated(sucursal);

            return sucursal;
        }

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Sucursal> CancelSucursalChanges(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Sucursal item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnSucursalUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Sucursal item);
        partial void OnAfterSucursalUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Sucursal item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Sucursal> UpdateSucursal(int sucid, SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Sucursal sucursal)
        {
            OnSucursalUpdated(sucursal);

            var itemToUpdate = Context.Sucursals
                              .Where(i => i.SucId == sucursal.SucId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(sucursal);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterSucursalUpdated(sucursal);

            return sucursal;
        }

        partial void OnSucursalDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Sucursal item);
        partial void OnAfterSucursalDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Sucursal item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Sucursal> DeleteSucursal(int sucid)
        {
            var itemToDelete = Context.Sucursals
                              .Where(i => i.SucId == sucid)
                              .Include(i => i.Bodegas)
                              .Include(i => i.Boleta)
                              .Include(i => i.Muebles)
                              .Include(i => i.Repartidors)
                              .Include(i => i.Trabajadors)
                              .Include(i => i.Vehiculos)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnSucursalDeleted(itemToDelete);


            Context.Sucursals.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterSucursalDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTipoIngredientesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/tipoingredientes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/tipoingredientes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTipoIngredientesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/tipoingredientes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/tipoingredientes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTipoIngredientesRead(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoIngrediente> items);

        public async Task<IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoIngrediente>> GetTipoIngredientes(Query query = null)
        {
            var items = Context.TipoIngredientes.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnTipoIngredientesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTipoIngredienteGet(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoIngrediente item);
        partial void OnGetTipoIngredienteByTipingId(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoIngrediente> items);


        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoIngrediente> GetTipoIngredienteByTipingId(int tipingid)
        {
            var items = Context.TipoIngredientes
                              .AsNoTracking()
                              .Where(i => i.TipingId == tipingid);

 
            OnGetTipoIngredienteByTipingId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTipoIngredienteGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTipoIngredienteCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoIngrediente item);
        partial void OnAfterTipoIngredienteCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoIngrediente item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoIngrediente> CreateTipoIngrediente(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoIngrediente tipoingrediente)
        {
            OnTipoIngredienteCreated(tipoingrediente);

            var existingItem = Context.TipoIngredientes
                              .Where(i => i.TipingId == tipoingrediente.TipingId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TipoIngredientes.Add(tipoingrediente);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tipoingrediente).State = EntityState.Detached;
                throw;
            }

            OnAfterTipoIngredienteCreated(tipoingrediente);

            return tipoingrediente;
        }

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoIngrediente> CancelTipoIngredienteChanges(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoIngrediente item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTipoIngredienteUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoIngrediente item);
        partial void OnAfterTipoIngredienteUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoIngrediente item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoIngrediente> UpdateTipoIngrediente(int tipingid, SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoIngrediente tipoingrediente)
        {
            OnTipoIngredienteUpdated(tipoingrediente);

            var itemToUpdate = Context.TipoIngredientes
                              .Where(i => i.TipingId == tipoingrediente.TipingId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tipoingrediente);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTipoIngredienteUpdated(tipoingrediente);

            return tipoingrediente;
        }

        partial void OnTipoIngredienteDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoIngrediente item);
        partial void OnAfterTipoIngredienteDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoIngrediente item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoIngrediente> DeleteTipoIngrediente(int tipingid)
        {
            var itemToDelete = Context.TipoIngredientes
                              .Where(i => i.TipingId == tipingid)
                              .Include(i => i.Ingredientes)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTipoIngredienteDeleted(itemToDelete);


            Context.TipoIngredientes.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTipoIngredienteDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTipoPagosToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/tipopagos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/tipopagos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTipoPagosToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/tipopagos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/tipopagos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTipoPagosRead(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPago> items);

        public async Task<IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPago>> GetTipoPagos(Query query = null)
        {
            var items = Context.TipoPagos.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnTipoPagosRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTipoPagoGet(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPago item);
        partial void OnGetTipoPagoByTipaId(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPago> items);


        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPago> GetTipoPagoByTipaId(int tipaid)
        {
            var items = Context.TipoPagos
                              .AsNoTracking()
                              .Where(i => i.TipaId == tipaid);

 
            OnGetTipoPagoByTipaId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTipoPagoGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTipoPagoCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPago item);
        partial void OnAfterTipoPagoCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPago item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPago> CreateTipoPago(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPago tipopago)
        {
            OnTipoPagoCreated(tipopago);

            var existingItem = Context.TipoPagos
                              .Where(i => i.TipaId == tipopago.TipaId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TipoPagos.Add(tipopago);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tipopago).State = EntityState.Detached;
                throw;
            }

            OnAfterTipoPagoCreated(tipopago);

            return tipopago;
        }

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPago> CancelTipoPagoChanges(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPago item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTipoPagoUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPago item);
        partial void OnAfterTipoPagoUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPago item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPago> UpdateTipoPago(int tipaid, SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPago tipopago)
        {
            OnTipoPagoUpdated(tipopago);

            var itemToUpdate = Context.TipoPagos
                              .Where(i => i.TipaId == tipopago.TipaId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tipopago);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTipoPagoUpdated(tipopago);

            return tipopago;
        }

        partial void OnTipoPagoDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPago item);
        partial void OnAfterTipoPagoDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPago item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPago> DeleteTipoPago(int tipaid)
        {
            var itemToDelete = Context.TipoPagos
                              .Where(i => i.TipaId == tipaid)
                              .Include(i => i.Boleta)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTipoPagoDeleted(itemToDelete);


            Context.TipoPagos.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTipoPagoDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTipoPlatosToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/tipoplatos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/tipoplatos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTipoPlatosToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/tipoplatos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/tipoplatos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTipoPlatosRead(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPlato> items);

        public async Task<IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPlato>> GetTipoPlatos(Query query = null)
        {
            var items = Context.TipoPlatos.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnTipoPlatosRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTipoPlatoGet(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPlato item);
        partial void OnGetTipoPlatoByTiplaId(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPlato> items);


        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPlato> GetTipoPlatoByTiplaId(int tiplaid)
        {
            var items = Context.TipoPlatos
                              .AsNoTracking()
                              .Where(i => i.TiplaId == tiplaid);

 
            OnGetTipoPlatoByTiplaId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTipoPlatoGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTipoPlatoCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPlato item);
        partial void OnAfterTipoPlatoCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPlato item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPlato> CreateTipoPlato(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPlato tipoplato)
        {
            OnTipoPlatoCreated(tipoplato);

            var existingItem = Context.TipoPlatos
                              .Where(i => i.TiplaId == tipoplato.TiplaId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TipoPlatos.Add(tipoplato);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tipoplato).State = EntityState.Detached;
                throw;
            }

            OnAfterTipoPlatoCreated(tipoplato);

            return tipoplato;
        }

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPlato> CancelTipoPlatoChanges(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPlato item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTipoPlatoUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPlato item);
        partial void OnAfterTipoPlatoUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPlato item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPlato> UpdateTipoPlato(int tiplaid, SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPlato tipoplato)
        {
            OnTipoPlatoUpdated(tipoplato);

            var itemToUpdate = Context.TipoPlatos
                              .Where(i => i.TiplaId == tipoplato.TiplaId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tipoplato);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTipoPlatoUpdated(tipoplato);

            return tipoplato;
        }

        partial void OnTipoPlatoDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPlato item);
        partial void OnAfterTipoPlatoDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPlato item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoPlato> DeleteTipoPlato(int tiplaid)
        {
            var itemToDelete = Context.TipoPlatos
                              .Where(i => i.TiplaId == tiplaid)
                              .Include(i => i.Productos)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTipoPlatoDeleted(itemToDelete);


            Context.TipoPlatos.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTipoPlatoDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTrabajadorsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/trabajadors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/trabajadors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTrabajadorsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/trabajadors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/trabajadors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTrabajadorsRead(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Trabajador> items);

        public async Task<IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Trabajador>> GetTrabajadors(Query query = null)
        {
            var items = Context.Trabajadors.AsQueryable();

            items = items.Include(i => i.Cargo);
            items = items.Include(i => i.Sucursal);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnTrabajadorsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTrabajadorGet(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Trabajador item);
        partial void OnGetTrabajadorByTraId(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Trabajador> items);


        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Trabajador> GetTrabajadorByTraId(int traid)
        {
            var items = Context.Trabajadors
                              .AsNoTracking()
                              .Where(i => i.TraId == traid);

            items = items.Include(i => i.Cargo);
            items = items.Include(i => i.Sucursal);
 
            OnGetTrabajadorByTraId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTrabajadorGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTrabajadorCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Trabajador item);
        partial void OnAfterTrabajadorCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Trabajador item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Trabajador> CreateTrabajador(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Trabajador trabajador)
        {
            OnTrabajadorCreated(trabajador);

            var existingItem = Context.Trabajadors
                              .Where(i => i.TraId == trabajador.TraId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Trabajadors.Add(trabajador);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(trabajador).State = EntityState.Detached;
                throw;
            }

            OnAfterTrabajadorCreated(trabajador);

            return trabajador;
        }

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Trabajador> CancelTrabajadorChanges(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Trabajador item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTrabajadorUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Trabajador item);
        partial void OnAfterTrabajadorUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Trabajador item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Trabajador> UpdateTrabajador(int traid, SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Trabajador trabajador)
        {
            OnTrabajadorUpdated(trabajador);

            var itemToUpdate = Context.Trabajadors
                              .Where(i => i.TraId == trabajador.TraId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(trabajador);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTrabajadorUpdated(trabajador);

            return trabajador;
        }

        partial void OnTrabajadorDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Trabajador item);
        partial void OnAfterTrabajadorDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Trabajador item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Trabajador> DeleteTrabajador(int traid)
        {
            var itemToDelete = Context.Trabajadors
                              .Where(i => i.TraId == traid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTrabajadorDeleted(itemToDelete);


            Context.Trabajadors.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTrabajadorDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStocksToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/stocks/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/stocks/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStocksToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/stocks/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/stocks/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStocksRead(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Stock> items);

        public async Task<IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Stock>> GetStocks(Query query = null)
        {
            var items = Context.Stocks.AsQueryable();

            items = items.Include(i => i.Bodega);
            items = items.Include(i => i.MateriaPrima);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnStocksRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStockGet(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Stock item);
        partial void OnGetStockByStoId(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Stock> items);


        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Stock> GetStockByStoId(int stoid)
        {
            var items = Context.Stocks
                              .AsNoTracking()
                              .Where(i => i.StoId == stoid);

            items = items.Include(i => i.Bodega);
            items = items.Include(i => i.MateriaPrima);
 
            OnGetStockByStoId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStockGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStockCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Stock item);
        partial void OnAfterStockCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Stock item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Stock> CreateStock(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Stock stock)
        {
            OnStockCreated(stock);

            var existingItem = Context.Stocks
                              .Where(i => i.StoId == stock.StoId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Stocks.Add(stock);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(stock).State = EntityState.Detached;
                throw;
            }

            OnAfterStockCreated(stock);

            return stock;
        }

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Stock> CancelStockChanges(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Stock item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStockUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Stock item);
        partial void OnAfterStockUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Stock item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Stock> UpdateStock(int stoid, SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Stock stock)
        {
            OnStockUpdated(stock);

            var itemToUpdate = Context.Stocks
                              .Where(i => i.StoId == stock.StoId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(stock);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStockUpdated(stock);

            return stock;
        }

        partial void OnStockDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Stock item);
        partial void OnAfterStockDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Stock item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Stock> DeleteStock(int stoid)
        {
            var itemToDelete = Context.Stocks
                              .Where(i => i.StoId == stoid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStockDeleted(itemToDelete);


            Context.Stocks.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStockDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportVehiculosToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/vehiculos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/vehiculos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportVehiculosToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/bushidosushiwok/vehiculos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/bushidosushiwok/vehiculos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnVehiculosRead(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Vehiculo> items);

        public async Task<IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Vehiculo>> GetVehiculos(Query query = null)
        {
            var items = Context.Vehiculos.AsQueryable();

            items = items.Include(i => i.Sucursal);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnVehiculosRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnVehiculoGet(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Vehiculo item);
        partial void OnGetVehiculoByVehiId(ref IQueryable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Vehiculo> items);


        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Vehiculo> GetVehiculoByVehiId(int vehiid)
        {
            var items = Context.Vehiculos
                              .AsNoTracking()
                              .Where(i => i.VehiId == vehiid);

            items = items.Include(i => i.Sucursal);
 
            OnGetVehiculoByVehiId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnVehiculoGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnVehiculoCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Vehiculo item);
        partial void OnAfterVehiculoCreated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Vehiculo item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Vehiculo> CreateVehiculo(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Vehiculo vehiculo)
        {
            OnVehiculoCreated(vehiculo);

            var existingItem = Context.Vehiculos
                              .Where(i => i.VehiId == vehiculo.VehiId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Vehiculos.Add(vehiculo);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(vehiculo).State = EntityState.Detached;
                throw;
            }

            OnAfterVehiculoCreated(vehiculo);

            return vehiculo;
        }

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Vehiculo> CancelVehiculoChanges(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Vehiculo item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnVehiculoUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Vehiculo item);
        partial void OnAfterVehiculoUpdated(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Vehiculo item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Vehiculo> UpdateVehiculo(int vehiid, SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Vehiculo vehiculo)
        {
            OnVehiculoUpdated(vehiculo);

            var itemToUpdate = Context.Vehiculos
                              .Where(i => i.VehiId == vehiculo.VehiId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(vehiculo);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterVehiculoUpdated(vehiculo);

            return vehiculo;
        }

        partial void OnVehiculoDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Vehiculo item);
        partial void OnAfterVehiculoDeleted(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Vehiculo item);

        public async Task<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Vehiculo> DeleteVehiculo(int vehiid)
        {
            var itemToDelete = Context.Vehiculos
                              .Where(i => i.VehiId == vehiid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnVehiculoDeleted(itemToDelete);


            Context.Vehiculos.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterVehiculoDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}