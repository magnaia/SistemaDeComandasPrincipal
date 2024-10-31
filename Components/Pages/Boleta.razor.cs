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
    public partial class Boleta
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

        protected IEnumerable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Boletum> boleta;

        protected RadzenDataGrid<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Boletum> grid0;
        protected override async Task OnInitializedAsync()
        {
            boleta = await BUSHIDOSUSHIWOKService.GetBoleta(new Query { Expand = "Sucursal,TipoPago" });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddBoletum>("Add Boletum", null);
            await grid0.Reload();
        }

        protected async Task EditRow(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Boletum args)
        {
            await DialogService.OpenAsync<EditBoletum>("Edit Boletum", new Dictionary<string, object> { {"BolId", args.BolId} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Boletum boletum)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await BUSHIDOSUSHIWOKService.DeleteBoletum(boletum.BolId);

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
                    Detail = $"Unable to delete Boletum"
                });
            }
        }
    }
}