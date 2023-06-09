using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMM06000Common;
using LMM06000Model.ViewModel;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;

namespace LMM06000Front
{
    public partial class LMM06000
    {
        private R_Conductor _conductorPropertyRef;
        private LMM06000ViewModel BillingRuleViewModel = new();

        private R_ConductorGrid _conductorUnitTypeRef;
        private R_Grid<LMM06000UnitTypeDTO> _gridUnitTypeRef;

        private R_ConductorGrid _conductorBillingRuleRef;
        private R_Grid<LMM06000BillingRuleDTO> _gridBillingRuleRef;

        public LMM06000BillingRuleDTO loBillingRuleParam = new LMM06000BillingRuleDTO();

        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();
            try
            {
                await PropertyDropdown_ServiceGetListRecord(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        private async Task PropertyDropdown_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                await BillingRuleViewModel.GetPropertyList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);
        }

        private async Task PropertyDropdown_OnChange(object poParam)
        {
            var loEx = new R_Exception();
            try
            {
                await _gridUnitTypeRef.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }
        
        private async Task GetListRecordUnitType(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                await BillingRuleViewModel.GetAllUnitType();
                eventArgs.ListEntityResult = BillingRuleViewModel.UnitTypeList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        private async Task Grid_Display(R_DisplayEventArgs eventArgs)
        {
            if (eventArgs.ConductorMode == R_eConductorMode.Normal)
            {
                 var loParam = (LMM06000UnitTypeDTO)eventArgs.Data;

                 loBillingRuleParam.CPROPERTY_ID = loParam.CPROPERTY_ID;
                 loBillingRuleParam.CUNIT_TYPE_ID = loParam.CUNIT_TYPE_ID;
                 
                 await _gridBillingRuleRef.R_RefreshGrid(null);
            }
        }
        
        private async Task GetListRecordBillingRule(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                await BillingRuleViewModel.GetAllBillingRule(loBillingRuleParam.CPROPERTY_ID, loBillingRuleParam.CUNIT_TYPE_ID);
                eventArgs.ListEntityResult = BillingRuleViewModel.BillingRuleList;
                var temp = BillingRuleViewModel.BillingRuleList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}
