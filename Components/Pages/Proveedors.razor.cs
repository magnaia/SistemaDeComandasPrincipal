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
    public partial class Proveedors
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

        protected IEnumerable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Proveedor> proveedors;

        protected RadzenDataGrid<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Proveedor> grid0;
        protected override async Task OnInitializedAsync()
        {
            proveedors = await BUSHIDOSUSHIWOKService.GetProveedors();
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddProveedor>("Add Proveedor", null);
            await grid0.Reload();
        }

        protected async Task EditRow(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Proveedor args)
        {
            await DialogService.OpenAsync<EditProveedor>("Edit Proveedor", new Dictionary<string, object> { {"ProvId", args.ProvId} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Proveedor proveedor)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await BUSHIDOSUSHIWOKService.DeleteProveedor(proveedor.ProvId);

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
                    Detail = $"Unable to delete Proveedor"
                });
            }
        }
    }
}