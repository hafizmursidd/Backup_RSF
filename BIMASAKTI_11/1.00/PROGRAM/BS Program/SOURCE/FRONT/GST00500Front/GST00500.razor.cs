using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GST00500Common;
using GST00500Model.ViewModel;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.Grid;
using R_BlazorFrontEnd.Controls.MessageBox;
using R_BlazorFrontEnd.Controls.Tab;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;

namespace GST00500Front
{
    public partial class GST00500
    {
        #region Declare ViewModel

        private GST00500InboxViewModel _viewModelGST00500Inbox = new();
        #endregion

        private R_Grid<GST00500DTO> _gridInboxTransRef;

        private R_Conductor _conductorGetUserName;
        private R_ConductorGrid _conductorInboxTrans;

        private bool Approve = true;

        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();
            try
            {
                await ServiceGetUserName(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task ServiceGetUserName(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                await _viewModelGST00500Inbox.GetUserName();
                // await ServiceGetListInboxTransaction(null);
                await _gridInboxTransRef.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }

        #region Inbox

        private async Task ServiceGetListInboxTransaction(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                await _viewModelGST00500Inbox.GetAllInboxTransaction();
                eventArgs.ListEntityResult = _viewModelGST00500Inbox.InboxTransactionList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        #region ApproveButton
        //OnClickApprove()
        private async Task OnClickApprove()
        {
            var loEx = new R_Exception();
            try
            {
                Approve = true;
                await _conductorInboxTrans.R_SaveBatch();
                await this.Close(true, true);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        #endregion

        #region Reject Button
        //OnClickReject()
        private async Task R_Before_Open_Reject(R_BeforeOpenPopupEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {


                var loTemp = await R_MessageBox.Show("", "Are You sure want process these records? ", R_eMessageBoxButtonType.OKCancel);
                if (loTemp == R_eMessageBoxResult.OK)
                {
                    await _conductorInboxTrans.R_SaveBatch();
                    Approve = false;

                    eventArgs.Parameter = _viewModelGST00500Inbox.loInboxApprovaltBatchList;
                    eventArgs.TargetPageType = typeof(GST00500RejectPopUp);


                }
                await this.Close(true, true);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        private async Task R_After_Open_Reject(R_AfterOpenPopupEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                await _gridInboxTransRef.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        #endregion

        #region Save Batch
        private void BeforeSaveBatch(R_BeforeSaveBatchEventArgs events)
        {
            var loData = (List<GST00500DTO>)events.Data;

            if (loData.Count < 1)
            {
                var loTemp = R_MessageBox.Show("", "Please select at least one record", R_eMessageBoxButtonType.OK);
                events.Cancel = true;
            }
        }
        private async Task ServiceSaveBatch(R_ServiceSaveBatchEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                _viewModelGST00500Inbox.loInboxApprovaltBatchList = (List<GST00500DTO>)eventArgs.Data;

                //if APPROVE == TRUE, USER === Click button Approve
                if (Approve == true)
                {
                    await _viewModelGST00500Inbox.SaveApprovalBatch(Approve);
                    await _gridInboxTransRef.R_RefreshGrid(null);

                    if (!string.IsNullOrEmpty(_viewModelGST00500Inbox.ResultFailedTransaction))
                    {
                        R_MessageBox.Show("",
                            $"This Record With {_viewModelGST00500Inbox.ResultFailedTransaction} has been failed when update status Approved",
                            R_eMessageBoxButtonType.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        private void AfterSaveBatch(R_AfterSaveBatchEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                // R_MessageBox.Show("", $"Approve Transaction with Reference Number {_viewModelGST00500Inbox.ResultSuccesTransaction} Successfully", R_eMessageBoxButtonType.OK);

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        #endregion


        #endregion

        #region ChangeTab
        private R_TabPage _tabPageOutbox;
        private R_TabPage _tabPageDraft;

        private async Task ChangeTab(R_TabStripTab arg)
        {
            var loEx = new R_Exception();
            try
            {
                if (arg.Id == "TabInbox")
                {
                    await _gridInboxTransRef.R_RefreshGrid(null);
                }
                //else if (arg.Title == "Outbox")
                //{
                //    await _gridOutboxTransRef.R_RefreshGrid(null);
                //}
                //else if (arg.Title == "Draft")
                //{
                //    await _gridDraftTransRef.R_RefreshGrid(null);
                //}
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        private void R_Before_Open_TabPageOutbox(R_BeforeOpenTabPageEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(GST00500Outbox);
        }
        private void R_Before_Open_TabPageDraft(R_BeforeOpenTabPageEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(GST00500Draft);
        }


        private void R_After_Open_TabPage(R_AfterOpenTabPageEventArgs eventArgs)
        {

        }

        #endregion

    }
}