using GSM04500Common;
using GSM04500Model;
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

namespace GSM04500Front
{
    public partial class GSM04500Dinamis
    {

        private GSM04500ViewModel journalGroupViewModel = new();
        private R_ConductorGrid _conJournalGroupRef;
        private R_Grid<GSM04500DTO> _gridRef;
        private R_Conductor _conductorRef;

        [Parameter]
        public string JournalGRPType { get; set; }

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

        private async Task R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            string lcGroupId = null;
            try
            {
                //lcGroupId ===> 10 untuk Tab Service
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
            //lcGroupId ===> 10 untuk Tab Service
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
        private async Task ChangeTab(R_TabStripTab arg)
        {
            var loEx = new R_Exception();

            try
            {
                //await PropertyDropdown_ServiceGetListRecord(null);
                /*
                await _GSM05000NumberingViewModel.GetNumberingHeader();
                await _gridRefNumbering.R_RefreshGrid(null);
                */
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}
