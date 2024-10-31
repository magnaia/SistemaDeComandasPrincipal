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
    public partial class AddDetalleRoll
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

        protected override async Task OnInitializedAsync()
        {
            detalleRoll = new SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleRoll();

            ingredientesForRollIngrediente = await BUSHIDOSUSHIWOKService.GetIngredientes();

            detalleProductosForRollDetproducto = await BUSHIDOSUSHIWOKService.GetDetalleProductos();
        }
        protected bool errorVisible;
        protected SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleRoll detalleRoll;

        protected IEnumerable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ingrediente> ingredientesForRollIngrediente;

        protected IEnumerable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.DetalleProducto> detalleProductosForRollDetproducto;

        protected async Task FormSubmit()
        {
            try
            {
                await BUSHIDOSUSHIWOKService.CreateDetalleRoll(detalleRoll);
                DialogService.Close(detalleRoll);
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