using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSM06500Model;
using GSM06500Common;
using BlazorClientHelper;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Controls.Events;

namespace GSM06500Front
{
    public partial class GSM06500
    {
        private GSM06500ViewModel PaymentTermViewModel = new();
        private R_ConductorGrid _conGridPaymentRef;
        private R_Grid<GSM06500DTO> _gridRef;

        [Inject] private IClientHelper _clientHelper { get; set; }

        private string lcPropertyId = "ABC";
        private string lcCompany = "ABC";
        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();
            try
            {
                await _gridRef.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        private async Task Grid_R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                await PaymentTermViewModel.GetAllTermOfPaymentAsync();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private void Grid_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {

        }

    }
}
