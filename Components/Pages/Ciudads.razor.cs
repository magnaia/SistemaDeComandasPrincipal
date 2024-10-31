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
    public partial class Ciudads
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

        protected IEnumerable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ciudad> ciudads;

        protected RadzenDataGrid<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ciudad> grid0;
        protected override async Task OnInitializedAsync()
        {
            ciudads = await BUSHIDOSUSHIWOKService.GetCiudads();
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddCiudad>("Add Ciudad", null);
            await grid0.Reload();
        }

        protected async Task EditRow(SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ciudad args)
        {
            await DialogService.OpenAsync<EditCiudad>("Edit Ciudad", new Dictionary<string, object> { {"CiuId", args.CiuId} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ciudad ciudad)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await BUSHIDOSUSHIWOKService.DeleteCiudad(ciudad.CiuId);

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
                    Detail = $"Unable to delete Ciudad"
                });
            }
        }
    }
}