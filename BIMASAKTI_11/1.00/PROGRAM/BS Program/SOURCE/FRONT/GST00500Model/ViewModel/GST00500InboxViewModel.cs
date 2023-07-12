using R_BlazorFrontEnd;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using GST00500Common;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;

namespace GST00500Model.ViewModel
{
    public class GST00500InboxViewModel : R_ViewModel<GST00500DTO>
    {
        private GST00500InboxModel _modelGST00500 = new GST00500InboxModel();
        public ObservableCollection<GST00500DTO> InboxTransactionList =
            new ObservableCollection<GST00500DTO>();

        public GST00500UserNameDTO UserName = new GST00500UserNameDTO();
        public List<GST00500DTO> loInboxApprovaltBatchList = new List<GST00500DTO>();
        public List<GST00500RejectDTO> ReasonOfRejectList = new List<GST00500RejectDTO>();
        public GST00500RejectTransactionDTO ParamTransactionStatus = new GST00500RejectTransactionDTO();
        public List<GST00500ApprovalTransactionDTO> ApprovalTransacReturnFromBack = null;

        public string ResultSuccesTransaction = "";
        public string ResultFailedTransaction = "";

        public async Task GetAllInboxTransaction()
        {
            R_Exception loException = new R_Exception();
            try
            {
                var loResult = await _modelGST00500.GetInboxListAsyncModel();
                InboxTransactionList = new ObservableCollection<GST00500DTO>(loResult);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        }

        public async Task GetUserName()
        {
            R_Exception loException = new R_Exception();
            try
            {
                var loResult = await _modelGST00500.GetUserNameAsyncModel();
                UserName = loResult;
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        }

        public async Task SaveApprovalBatch(bool llApprove)
        {
            R_Exception loException = new R_Exception();
            List<GST00500DTO> loInboxApprovaltBatchListSelected = new List<GST00500DTO>();

            try
            {
                foreach (GST00500DTO item in loInboxApprovaltBatchList)
                {
                    if (item.LSELECTED == true)
                    {
                        item.VAR_SELECTED = 1;
                        loInboxApprovaltBatchListSelected.Add(item);
                    }
                }
                //Approve, else Reject
                if (llApprove == true)
                {
                    //flag transaction (true = approve, false = reject)
                    R_FrontContext.R_SetStreamingContext(ContextConstant.APPROVE_TRANSACTION, true);
                    R_FrontContext.R_SetStreamingContext(ContextConstant.LIST_TRANSACTION,
                        loInboxApprovaltBatchListSelected);
                }
                else
                {
                    //flag transaction (true = approve, false = reject)
                    R_FrontContext.R_SetStreamingContext(ContextConstant.APPROVE_TRANSACTION, false);
                    R_FrontContext.R_SetStreamingContext(ContextConstant.LIST_TRANSACTION, loInboxApprovaltBatchListSelected);
                    R_FrontContext.R_SetStreamingContext(ContextConstant.CREASON_CODE, ParamTransactionStatus.CREASON_CODE);
                    R_FrontContext.R_SetStreamingContext(ContextConstant.TNOTES, ParamTransactionStatus.TNOTES);
                }
                
               var loResult = await _modelGST00500.SaveTransactionListAsync();
               ApprovalTransacReturnFromBack = loResult.Data;

               //for notification 
               GetResultTransaction(ApprovalTransacReturnFromBack);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();
        }

        public async Task GetReasonRejectTransaction()
        {
            R_Exception loException = new R_Exception();
            try
            {
                var loResult = await _modelGST00500.GetReasonRejectListAsyncModel();
                ReasonOfRejectList = loResult.Data;
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        }

        public void GetResultTransaction(List<GST00500ApprovalTransactionDTO> loEntity)
        {
            foreach (var item in loEntity)
            {
                if (item.LSUCCESSED == true)
                {

                }
                else
                {
                    ResultFailedTransaction = $"{item.CCOMPANY_ID} And {item.CTRANSACTION_CODE} And " +
                                              $"{item.CDEPT_CODE} And {item.CREFERENCE_NO} ";
                }
                //_ = item.LSUCCESSED ? ResultSuccesTransaction = "( " +item.CREFERENCE_NO + " )" 
                //    : ResultFailedTransaction = "( " + item.CREFERENCE_NO + " )";


            }
        }
    }
}
