using GSM04500Common;
using GSM04500Model;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.MessageBox;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;

namespace GSM04500Front
{
    public partial class GSM04500BaseJournalGroup
    {
        private GSM04500ViewModel journalGroupViewModel = new();
        private R_ConductorGrid _conJournalGroupRef;
        private R_Grid<GSM04500DTO> _gridRef;
        private R_Conductor _conductorRef;

        [Parameter] public string JournalGRPType { get; set; } = "10";
        [Parameter] public string PropertyId { get; set; }
        [Parameter] public string JournalGRPCode { get; set; }
        [Parameter] public string GOA_CODE { get; set; }
        [Parameter] public string GOA_Name { get; set; }

        public GSM04500DTO manipulate = new GSM04500DTO();

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
                var loTemp = journalGroupViewModel.JournalGroupList.FirstOrDefault();
                PropertyId = loTemp.CPROPERTY_ID;
                JournalGRPCode = loTemp.CJRNGRP_CODE;
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
            }
        }
        #endregion

        //MAIN TAB

        private void Before_Open_AccountSetting(R_BeforeOpenTabPageEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(GSM04500AccountSetting);

            var loparam = new GSM04500AccountSetting();
            loparam.JournalGRPType = JournalGRPType;
            loparam.PropertyId = PropertyId;
            loparam.JournalGRPCode = JournalGRPCode;

            eventArgs.Parameter = loparam;
        }
        private async Task ChangeTabMain(R_TabStripTab arg)
        {
            var loEx = new R_Exception();
            try
            {
                switch (arg.Title)
                {
                    case "Service":
                        JournalGRPType = "10";
                        break;
                    case "Utility":
                        JournalGRPType = "11";
                        break;
                    case "Deposit":
                        JournalGRPType = "12";
                        break;
                    case "Customer":
                        JournalGRPType = "20";
                        break;
                    case "Product":
                        JournalGRPType = "30";
                        break;
                    case "Expenditure":
                        JournalGRPType = "40";
                        break;
                    case "Supplier":
                        JournalGRPType = "50";
                        break;

                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        //CHILD TAB
        private async Task ChangeTabChild(R_TabStripTab arg)
        {
            var loEx = new R_Exception();
            try
            {

                //var a = JournalGRPType;
                //var B = JournalGRPCode;
                //var c = PropertyId;
                //lakukan manipulate disini
                //case "Deposit":
                //    JournalGRPType = "12";
                //    break;
                //case "Customer":
                //    JournalGRPType = "20";
                //    break;
                //case "Product":
                //    JournalGRPType = "30";
                //    break;
                //case "Expenditure":
                //    JournalGRPType = "40";
                //    break;
                //case "Supplier":
                //    JournalGRPType = "50";
                //    break;


            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

    }
}
