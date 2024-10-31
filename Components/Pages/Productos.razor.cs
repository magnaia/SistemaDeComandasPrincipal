using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace SistemaBushidoSushiWok.Components.Pages
{
    public partial class Productos
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        public BUSHIDOSUSHIWOKService BUSHIDOSUSHIWOKService { get; set; }

        protected IEnumerable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Producto> productos;

        protected RadzenDataGrid<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Producto> grid0;
        protected override async Task OnInitializedAsync()
        {
            productos = await BUSHIDOSUSHIWOKService.GetProductos(new Query { Expand = "TipoPlato" });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddProducto>("Add Producto", null);
            await grid0.Reload();
        }
        protected async Task AddButtonClick2(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddTipoPlato>("Añadir tipo de producto", null);
            await grid0.Reload();
        }

        protected async Task EditRow(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Producto args)
        {
            await DialogService.OpenAsync<EditProducto>("Edit Producto", new Dictionary<string, object> { {"ProId", args.ProId} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Producto producto)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await BUSHIDOSUSHIWOKService.DeleteProducto(producto.ProId);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete Producto"
                });
            }
        }
    }
}