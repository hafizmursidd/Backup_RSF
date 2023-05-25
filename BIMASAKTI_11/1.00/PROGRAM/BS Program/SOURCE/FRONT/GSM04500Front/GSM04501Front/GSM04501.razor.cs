using GSM04500Common;
using GSM04501Model;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Exceptions;

namespace GSM04501Front
{
    public partial class GSM04501
    {
        private GSM04501ViewModel journalGOAViewModel = new();
        private R_ConductorGrid _conJournalGOARef;
        private R_Grid<GSM04510GOADTO> _gridRef;

        //private GSM04502ViewModel _GOADeptViewModel = new();
        //private R_Grid<GSM04510GOADeptDTO> _gridDeptUserRef;
        //private R_ConductorGrid _conGOADeptRef;

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
        private async Task R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            string lcJournalGRPType = null;
            string lcPropertyId = null;
            string lcJournalGRPCode = null;
            try
            {
                //lcGroupId ===> 10 untuk Tab Service
                lcJournalGRPType = "10";
                lcPropertyId = "JBMPC";
                lcJournalGRPCode = "A";
                await journalGOAViewModel.GetAllJournalGrupGOAAsync(lcJournalGRPType, lcPropertyId, lcJournalGRPCode);
                eventArgs.ListEntityResult = journalGOAViewModel.GOAList;
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
                var loParam = (GSM04510GOADTO)eventArgs.Data;
                eventArgs.Result = await journalGOAViewModel.GetGOAOneRecord(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        private async Task ServiceSave(R_ServiceSaveEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loParam = (GSM04510GOADTO)eventArgs.Data;
                loParam.CJRNGRP_TYPE = "10";
                loParam.CPROPERTY_ID = "JBMPC";
                loParam.CJRNGRP_CODE = "A";

                await journalGOAViewModel.SaveGOA(loParam, eventArgs.ConductorMode);
                //eventArgs.Result = journalGOAViewModel.GOA;

                await _gridRef.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}