﻿using GSM04500Common;
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
using GSM04500Model.ViewModel;

namespace GSM04540Front
{
    public partial class GSM04540
    {
        private GSM04500ViewModel journalGroupViewModel = new();
        private R_ConductorGrid _conJournalGroupRef;
        private R_Grid<GSM04500DTO> _gridRef;
        private R_Conductor _conductorRef;

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
                //lcGroupId ===> 30 untuk Tab Product
                lcGroupId = "30";
                await journalGroupViewModel.GetAllJournalAsync(lcGroupId);
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
                //lcGroupId ===> 30 untuk Tab Product
                var loParam = (GSM04500DTO)eventArgs.Data;
                loParam.CJRNGRP_TYPE = "30";
                await journalGroupViewModel.SaveJournalGroup(loParam, eventArgs.ConductorMode);

                eventArgs.Result = journalGroupViewModel.JournalGroup;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}
