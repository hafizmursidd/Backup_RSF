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
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Controls.MessageBox;
using R_BlazorFrontEnd.Enums;

namespace GSM06500Front
{
    public partial class GSM06500
    {
        private GSM06500ViewModel PaymentTermViewModel = new();
        private R_ConductorGrid _conGridPaymentRef;
        private R_Grid<GSM06500DTO> _gridRef;

        [Inject] private IClientHelper _clientHelper { get; set; }

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

        private async Task Grid_ServiceGetRecordAsync(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = (GSM06500DTO)eventArgs.Data;
                eventArgs.Result = await PaymentTermViewModel.GetTermOftermOneRecord(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        public async Task Grid_ServiceDelete(R_ServiceDeleteEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = (GSM06500DTO)eventArgs.Data;
                await PaymentTermViewModel.DeleteTermOfPayment(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
       
        //public async Task Conductor_BeforeDelete(R_BeforeDeleteEventArgs eventArgs)
        //{
        //    var loData = (GSM06500DTO)eventArgs.Data;

        //    if (loData.CPROPERTY_ID == 1)
        //    {
        //        eventArgs.Cancel = true;
        //        await R_MessageBox.Show("", "Cannot delete Product ID 1", R_eMessageBoxButtonType.OK);
        //    }
        //}
        public async Task Conductor_AfterDelete()
        {
            await R_MessageBox.Show("", "Delete Success", R_eMessageBoxButtonType.OK);
        }

    }
}
