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
    public partial class DetalleRolls
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

        protected IEnumerable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleRoll> detalleRolls;

        protected RadzenDataGrid<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleRoll> grid0;
        protected override async Task OnInitializedAsync()
        {
            detalleRolls = await BUSHIDOSUSHIWOKService.GetDetalleRolls(new Query { Expand = "Ingrediente,DetalleProducto" });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddDetalleRoll>("Add DetalleRoll", null);
            await grid0.Reload();
        }

        protected async Task EditRow(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleRoll args)
        {
            await DialogService.OpenAsync<EditDetalleRoll>("Edit DetalleRoll", new Dictionary<string, object> { {"RollId", args.RollId} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleRoll detalleRoll)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await BUSHIDOSUSHIWOKService.DeleteDetalleRoll(detalleRoll.RollId);

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
                    Detail = $"Unable to delete DetalleRoll"
                });
            }
        }
    }
}