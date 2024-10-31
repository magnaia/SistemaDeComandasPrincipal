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
    public partial class EditOrden
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

        [Parameter]
        public int OrdId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            orden = await BUSHIDOSUSHIWOKService.GetOrdenByOrdId(OrdId);

            boletaForOrdBoleta = await BUSHIDOSUSHIWOKService.GetBoleta();

            productosForOrdPlato = await BUSHIDOSUSHIWOKService.GetProductos();
        }
        protected bool errorVisible;
        protected SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Orden orden;

        protected IEnumerable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Boletum> boletaForOrdBoleta;

        protected IEnumerable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Producto> productosForOrdPlato;

        protected async Task FormSubmit()
        {
            try
            {
                await BUSHIDOSUSHIWOKService.UpdateOrden(OrdId, orden);
                DialogService.Close(orden);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}