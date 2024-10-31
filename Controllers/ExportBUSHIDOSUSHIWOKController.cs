using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using SistemaBushidoSushiWok.Data;

namespace SistemaBushidoSushiWok.Controllers
{
    public partial class ExportBUSHIDOSUSHIWOKController : ExportController
    {
        private readonly BUSHIDOSUSHIWOKContext context;
        private readonly BUSHIDOSUSHIWOKService service;

        public ExportBUSHIDOSUSHIWOKController(BUSHIDOSUSHIWOKContext context, BUSHIDOSUSHIWOKService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/bodegas/csv")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/bodegas/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBodegasToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetBodegas(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/bodegas/excel")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/bodegas/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBodegasToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetBodegas(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/boleta/csv")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/boleta/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBoletaToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetBoleta(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/boleta/excel")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/boleta/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBoletaToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetBoleta(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/cargos/csv")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/cargos/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCargosToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCargos(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/cargos/excel")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/cargos/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCargosToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCargos(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/ciudads/csv")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/ciudads/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCiudadsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCiudads(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/ciudads/excel")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/ciudads/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCiudadsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCiudads(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/comprainsumos/csv")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/comprainsumos/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCompraInsumosToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCompraInsumos(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/comprainsumos/excel")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/comprainsumos/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCompraInsumosToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCompraInsumos(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/detalleproductos/csv")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/detalleproductos/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportDetalleProductosToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetDetalleProductos(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/detalleproductos/excel")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/detalleproductos/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportDetalleProductosToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetDetalleProductos(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/detallerolls/csv")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/detallerolls/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportDetalleRollsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetDetalleRolls(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/detallerolls/excel")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/detallerolls/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportDetalleRollsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetDetalleRolls(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/ingredientes/csv")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/ingredientes/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportIngredientesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetIngredientes(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/ingredientes/excel")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/ingredientes/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportIngredientesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetIngredientes(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/materiaprimas/csv")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/materiaprimas/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMateriaPrimasToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetMateriaPrimas(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/materiaprimas/excel")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/materiaprimas/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMateriaPrimasToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetMateriaPrimas(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/muebles/csv")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/muebles/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMueblesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetMuebles(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/muebles/excel")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/muebles/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMueblesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetMuebles(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/ordens/csv")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/ordens/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportOrdensToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetOrdens(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/ordens/excel")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/ordens/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportOrdensToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetOrdens(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/productos/csv")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/productos/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportProductosToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetProductos(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/productos/excel")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/productos/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportProductosToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetProductos(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/proveedors/csv")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/proveedors/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportProveedorsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetProveedors(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/proveedors/excel")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/proveedors/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportProveedorsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetProveedors(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/repartidors/csv")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/repartidors/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportRepartidorsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetRepartidors(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/repartidors/excel")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/repartidors/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportRepartidorsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetRepartidors(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/sucursals/csv")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/sucursals/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSucursalsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetSucursals(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/sucursals/excel")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/sucursals/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSucursalsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetSucursals(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/tipoingredientes/csv")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/tipoingredientes/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTipoIngredientesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTipoIngredientes(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/tipoingredientes/excel")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/tipoingredientes/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTipoIngredientesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTipoIngredientes(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/tipopagos/csv")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/tipopagos/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTipoPagosToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTipoPagos(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/tipopagos/excel")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/tipopagos/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTipoPagosToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTipoPagos(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/tipoplatos/csv")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/tipoplatos/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTipoPlatosToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTipoPlatos(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/tipoplatos/excel")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/tipoplatos/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTipoPlatosToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTipoPlatos(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/trabajadors/csv")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/trabajadors/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTrabajadorsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTrabajadors(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/trabajadors/excel")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/trabajadors/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTrabajadorsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTrabajadors(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/stocks/csv")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/stocks/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStocksToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStocks(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/stocks/excel")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/stocks/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStocksToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStocks(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/vehiculos/csv")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/vehiculos/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportVehiculosToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetVehiculos(), Request.Query, false), fileName);
        }

        [HttpGet("/export/BUSHIDOSUSHIWOK/vehiculos/excel")]
        [HttpGet("/export/BUSHIDOSUSHIWOK/vehiculos/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportVehiculosToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetVehiculos(), Request.Query, false), fileName);
        }
    }
}
