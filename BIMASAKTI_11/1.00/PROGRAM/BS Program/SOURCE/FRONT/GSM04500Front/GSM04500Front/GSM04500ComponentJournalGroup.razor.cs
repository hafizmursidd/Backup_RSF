using GSM04500Common;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.MessageBox;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using GSM04500Model;
using R_BlazorFrontEnd.Enums;
using System.Diagnostics.Tracing;

namespace GSM04500Front
{
    public partial class GSM04500ComponentJournalGroup
    {

        private GSM04500ViewModel journalGroupViewModel = new();
        private R_ConductorGrid _conJournalGroupRef;
        private R_Grid<GSM04500DTO> _gridRef;
        private R_Conductor _conductorRef;

        private GSM04501ViewModel JournalGOAViewModel = new();
        private R_Grid<GSM04510GOADTO> _gridRefGOA;
        private R_ConductorGrid _conJournalGOARef;

        private GSM04502ViewModel GOADeptViewModel = new();
        private R_ConductorGrid _conGOADeptRef;
        private R_Grid<GSM04510GOADeptDTO> _gridGOADeptRef;

        [Parameter] public string JournalGRPType { get; set; } = "10";
        [Parameter]  public string PropertyId { get; set; }
        [Parameter]  public string JournalGRPCode { get; set; }
        [Parameter] public string GOA_CODE { get; set; }
        [Parameter] public string GOA_Name { get; set; }
        [Parameter] public string lcGroupOfAccount { get; set; }

        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();
            try
            {
                var aa = JournalGRPType;
                //await PropertyDropdown_ServiceGetListRecord(null);
                await _gridRef.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        /*
        private async Task PropertyDropdown_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                await journalGroupViewModel.GetPropertyList();
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
                await _gridRef.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);
        }
        */

        #region Journal Group
        private async Task R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            string lcGroupId = null;
            try
            {
                lcGroupId = JournalGRPType;
                await journalGroupViewModel.GetAllJournalAsync(lcGroupId);
                eventArgs.ListEntityResult = journalGroupViewModel.JournalGroupList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        private async Task R_ServiceGetRecordAsync(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = (GSM04500DTO)eventArgs.Data;
                eventArgs.Result = await journalGroupViewModel.GetGroupJournaltOneRecord(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task ServiceDelete(R_ServiceDeleteEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loParam = (GSM04500DTO)eventArgs.Data;
                await journalGroupViewModel.DeleteOneRecordJournalGroup(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        public async Task AfterDelete()
        {
            await R_MessageBox.Show("", "Delete Success", R_eMessageBoxButtonType.OK);
        }
        private async Task ServiceSave(R_ServiceSaveEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loParam = (GSM04500DTO)eventArgs.Data;
                loParam.CJRNGRP_TYPE = JournalGRPType;
                await journalGroupViewModel.SaveJournalGroup(loParam, eventArgs.ConductorMode);

                eventArgs.Result = journalGroupViewModel.JournalGroup;
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
                var loParam = (GSM04500DTO)eventArgs.Data;
                
                PropertyId = loParam.CPROPERTY_ID;
                JournalGRPCode = loParam.CJRNGRP_CODE;
                GOA_Name = loParam.CJRNGRP_NAME;

                await _gridRefGOA.R_RefreshGrid(null);
            }
        }
        #endregion

        #region Account Setting GOA

        private async Task R_ServiceGetListRecordGOAList(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var lcJournalGRPType = JournalGRPType;
                var lcPropertyId = PropertyId;
                var lcJournalGRPCode = JournalGRPCode;

                await JournalGOAViewModel.GetAllJournalGrupGOAAsync(lcJournalGRPType, lcPropertyId, lcJournalGRPCode);
                eventArgs.ListEntityResult = JournalGOAViewModel.GOAList;

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }
        private async Task R_ServiceGetRecordAsyncGOA(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loParam = (GSM04510GOADTO)eventArgs.Data;
                eventArgs.Result = await JournalGOAViewModel.GetGOAOneRecord(loParam);

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task ServiceSaveGOA(R_ServiceSaveEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loParam = (GSM04510GOADTO)eventArgs.Data;
                loParam.CJRNGRP_TYPE = "10";
                loParam.CPROPERTY_ID = "JBMPC";
                loParam.CJRNGRP_CODE = "A";

                await JournalGOAViewModel.SaveGOA(loParam, eventArgs.ConductorMode);
                eventArgs.Result = JournalGOAViewModel.GOA;

                await _gridRef.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        private async Task Grid_DisplayGOA(R_DisplayEventArgs eventArgs)
        {
            if (eventArgs.ConductorMode == R_eConductorMode.Normal)
            {
                var loParam = (GSM04510GOADTO)eventArgs.Data;
                GOA_CODE = loParam.CGOA_CODE;
                lcGroupOfAccount = loParam.GROUPOFACCOUNT;
                await GridGOADept_GetList(null);
                //_gridGOADeptRef.R_RefreshGrid(null);
            }
        }
        #endregion

        #region Account Setting GroupOfAccountDept
        private async Task GridGOADept_GetList(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                GSM04510GOADTO liParam = new GSM04510GOADTO();
                liParam.CJRNGRP_TYPE = JournalGRPType;
                liParam.CPROPERTY_ID =PropertyId;
                liParam.CJRNGRP_CODE = JournalGRPCode;
                liParam.CGOA_CODE = GOA_CODE;
                await GOADeptViewModel.GetGOAAllByDept(liParam);
                eventArgs.ListEntityResult = GOADeptViewModel.GOADeptList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }

        private async Task R_ServiceGetRecordGOADeptAsync(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loParam = (GSM04510GOADeptDTO)eventArgs.Data;
                eventArgs.Result = await GOADeptViewModel.GetGOADeptOneRecord(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        private async Task ServiceSaveGOADept(R_ServiceSaveEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loParam = (GSM04510GOADeptDTO)eventArgs.Data;
                loParam.CJRNGRP_TYPE = "10";
                loParam.CPROPERTY_ID = "JBMPC";
                loParam.CJRNGRP_CODE = "A";

                await GOADeptViewModel.SaveGOADept(loParam, eventArgs.ConductorMode);
                eventArgs.Result = GOADeptViewModel.GOADept;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        #endregion


    }
}
