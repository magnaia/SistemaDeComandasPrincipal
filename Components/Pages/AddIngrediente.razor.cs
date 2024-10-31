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
    public partial class AddIngrediente
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
            ingrediente = new SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ingrediente();

            tipoIngredientesForIngTipo = await BUSHIDOSUSHIWOKService.GetTipoIngredientes();
        }
        protected bool errorVisible;
        protected SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.Ingrediente ingrediente;

        protected IEnumerable<SistemaBushidoSushiWok.Models.BUSHIDOSUSHIWOK.TipoIngrediente> tipoIngredientesForIngTipo;

        protected async Task FormSubmit()
        {
            try
            {
                await BUSHIDOSUSHIWOKService.CreateIngrediente(ingrediente);
                DialogService.Close(ingrediente);
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