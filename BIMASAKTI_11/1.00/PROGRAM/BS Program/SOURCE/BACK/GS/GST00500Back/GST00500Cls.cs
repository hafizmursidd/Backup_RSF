using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using GST00500Common;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;

namespace GST00500Back
{
    public class GST00500Cls : R_BusinessObject<GST00500DTO>
    {
        protected override GST00500DTO R_Display(GST00500DTO poEntity)
        {
            throw new NotImplementedException();
        }

        protected override void R_Saving(GST00500DTO poNewEntity, eCRUDMode poCRUDMode)
        {
            throw new NotImplementedException();
        }

        protected override void R_Deleting(GST00500DTO poEntity)
        {
            throw new NotImplementedException();
        }

        public List<GST00500DTO> Approval_Inbox_List(GST00500DBParameter poEntity)
        {
            var loEx = new R_Exception();
            var loResult = new List<GST00500DTO>();
            R_Db loDb;
            DbCommand loCmd;
            try
            {
                loDb = new R_Db();
                var loConn = loDb.GetConnection();

                var loCommand = loDb.GetCommand();

                var lcQuery = $"SELECT VAR_SELECTED = 0, A.*, C.CDESCRIPTION AS CAPPROVAL_STATUS_DESC, B.CTRANSACTION_NAME, " +
                              $"B.CTABLE_NAME, B.CPROGRAM_ID FROM GST_APPROVAL_I A (NOLOCK) " +
                              $"INNER JOIN GSM_TRANSACTION_CODE B (NOLOCK) " +
                              $"ON B.CCOMPANY_ID = A.CCOMPANY_ID AND B.CTRANSACTION_CODE = A.CTRANSACTION_CODE " +
                              $"INNER JOIN RFT_GET_GSB_CODE_INFO('SIAPP', '{poEntity.CCOMPANYID}', " +
                              $"'_GS_APPROVAL_STATUS', '', '{poEntity.CLANGUAGE_ID}') C " +
                              $"ON C.CCODE = A.CAPPROVAL_STATUS " +
                              $"WHERE A.CUSER_ID = '{poEntity.CUSER_ID}' AND A.CAPPROVAL_STATUS = '01'  AND A.CTRANSACTION_STATUS IN ('01','02') " +
                              $"ORDER BY B.CTRANSACTION_NAME, A.CREFERENCE_DATE ASC";

                loResult = loDb.SqlExecObjectQuery<GST00500DTO>(lcQuery, loConn, true);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }

        public GST00500UserNameDTO GetUserName(GST00500DBParameter poEntity)
        {
            var loEx = new R_Exception();
            var loResult = new GST00500UserNameDTO();
            R_Db loDb;
            DbCommand loCmd;
            try
            {
                loDb = new R_Db();
                var loConn = loDb.GetConnection();
                var loCommand = loDb.GetCommand();

                var lcQuery = $"SELECT B.CUSER_NAME FROM SAM_USER_COMPANY A (NOLOCK) " +
                              $"INNER JOIN SAM_USER B (NOLOCK) ON B.CUSER_ID = A.CUSER_ID " +
                              $"WHERE A.CCOMPANY_ID = '{poEntity.CCOMPANYID}' AND A.CUSER_ID = '{poEntity.CUSER_ID}' ";
                var loResultTemp = loDb.SqlExecQuery(lcQuery, loConn, true);
                loResult.CUSER_NAME = loResultTemp.Rows[0]["CUSER_NAME"].ToString();

                //   loResult = loDb.SqlExecObjectQuery<GST00500UserNameDTO>(lcQuery, loConn, true);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }

        public List<GST00500RejectDTO> GetReasonRejectList(GST00500DBParameter poParameter)
        {
            var loEx = new R_Exception();
            var loResult = new List<GST00500RejectDTO>();
            R_Db loDb;
            DbCommand loCmd;
            try
            {
                loDb = new R_Db();
                var loConn = loDb.GetConnection();

                var loCommand = loDb.GetCommand();

                var lcQuery = $"SELECT * FROM RFT_GET_GSB_CODE_INFO('SIAPP', " +
                              $"'{poParameter.CCOMPANYID}', '_GS_REJECTTRANS_REASON', '', " +
                              $"'{poParameter.CLANGUAGE_ID}') ";

                loResult = loDb.SqlExecObjectQuery<GST00500RejectDTO>(lcQuery, loConn, true);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }
        public List<GST00500ApprovalTransactionDTO> ProcessApprovalTransaction(List<GST00500DTO> poListTransaction, GST00500DBParameter poParameter)
        {
            R_Exception loException = new R_Exception();
            List<GST00500ApprovalTransactionDTO> listApprovalTransactionReturn = new();
            int lnCount;
            var loDb = new R_Db();
            DbConnection loConn = null;
            bool llStatusApprove;
            try
            {
                lnCount = 1;

                {
                    foreach (GST00500DTO item in poListTransaction)
                        try
                        {
                            GST00500ApprovalTransactionDTO lotemp = new();
                            using (TransactionScope TransScope = new TransactionScope(TransactionScopeOption.Required))
                            {
                                loConn = loDb.GetConnection();
                                llStatusApprove = UpdateEachApprovalStatus(item, poParameter, loConn);

                                if (llStatusApprove == false)
                                {
                                    lotemp.LSUCCESSED = llStatusApprove;
                                    lotemp.CCOMPANY_ID = poParameter.CCOMPANYID;
                                    lotemp.CTRANSACTION_CODE = item.CTRANSACTION_CODE;
                                    lotemp.CDEPT_CODE = item.CDEPT_CODE;
                                    lotemp.CREFERENCE_NO = item.CREFERENCE_NO;
                                    listApprovalTransactionReturn.Add(lotemp);
                                    goto EndDetail;
                                }

                                TransScope.Complete();
                            }
                        }
                        catch (Exception ex)
                        {
                            loException.Add(ex);
                        }

                    EndDetail:
                    lnCount++;
                }
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            finally
            {
                if (loConn != null)
                {
                    if (loConn.State != ConnectionState.Closed)
                    {
                        loConn.Close();
                    }

                    loConn.Dispose();
                }
            }

        EndBlock:
            loException.ThrowExceptionIfErrors();

            return listApprovalTransactionReturn;
        }
        private bool UpdateEachApprovalStatus(GST00500DTO poEntity, GST00500DBParameter poParameter, DbConnection poConnection)
        {
            var loEx = new R_Exception();
            R_Db loDb;
            DbConnection loConn = poConnection;
            DbCommand loCommand;
            string lcCmd;
            bool lbRtn = false;
            try
            {
                loDb = new R_Db();
                loCommand = loDb.GetCommand();

                lcCmd = $"Select CCOMPANY_ID From GST_APPROVAL_I  (Updlock) " +
                        $"Where CCOMPANY_ID = '{poParameter.CCOMPANYID}' And CTRANSACTION_CODE = '{poEntity.CTRANSACTION_NAME}' " +
                        $"And CDEPT_CODE = '{poEntity.CDEPT_CODE}'And CREFERENCE_NO = '{poEntity.CREFERENCE_NO}' And CTRANSACTION_STATUS In (01,02) " +

                        $"UPDATE GST_APPROVAL_I SET CAPPROVAL_STATUS= '02' " +
                        $"FROM GST_APPROVAL_I A (NOLOCK) WHERE A.CCOMPANY_ID= '{poParameter.CCOMPANYID}' AND A.CTRANSACTION_CODE = '{poEntity.CTRANSACTION_CODE}' " +
                        $"AND A.CDEPT_CODE= '{poEntity.CDEPT_CODE}' AND A.CREFERENCE_NO= '{poEntity.CREFERENCE_NO}' AND A.CUSER_ID = '{poParameter.CUSER_ID}' " +
                        $"AND A.CAPPROVAL_STATUS= '01'";
                loCommand.CommandText = lcCmd;

                loDb.SqlExecNonQuery(loConn, loCommand, false);
                lbRtn = true;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
        EndBlock:
            loEx.ThrowExceptionIfErrors();

            return lbRtn;
        }

        public List<GST00500ApprovalTransactionDTO> ProcessRejectTransaction(List<GST00500DTO> poListTransaction, GST00500DBParameter poParameter)
        {
            R_Exception loException = new R_Exception();
            List<GST00500ApprovalTransactionDTO> listApprovalTransactionReturn = new();
            int lnCount;
            var loDb = new R_Db();
            DbConnection loConn = null;
            bool llStatusApprove;
            try
            {
                lnCount = 1;

                {
                    foreach (GST00500DTO item in poListTransaction)
                        try
                        {
                            GST00500ApprovalTransactionDTO lotemp = new();
                            using (TransactionScope TransScope = new TransactionScope(TransactionScopeOption.Required))
                            {
                                loConn = loDb.GetConnection();
                                llStatusApprove = UpdateEachRejectStatus(item, poParameter, loConn);
                                
                                if (llStatusApprove == false)
                                {
                                    lotemp.LSUCCESSED = llStatusApprove;
                                    lotemp.CCOMPANY_ID = poParameter.CCOMPANYID;
                                    lotemp.CTRANSACTION_CODE = item.CTRANSACTION_CODE;
                                    lotemp.CDEPT_CODE = item.CDEPT_CODE;
                                    lotemp.CREFERENCE_NO = item.CREFERENCE_NO;
                                    listApprovalTransactionReturn.Add(lotemp);
                                    goto EndDetail;
                                }

                                TransScope.Complete();
                            }
                        }
                        catch (Exception ex)
                        {
                            loException.Add(ex);
                        }

                    EndDetail:
                    lnCount++;
                }
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            finally
            {
                if (loConn != null)
                {
                    if (loConn.State != ConnectionState.Closed)
                    {
                        loConn.Close();
                    }

                    loConn.Dispose();
                }
            }

        EndBlock:
            loException.ThrowExceptionIfErrors();

            return listApprovalTransactionReturn;
        }
        private bool UpdateEachRejectStatus(GST00500DTO poEntity, GST00500DBParameter poParam, DbConnection poConnection)
        {
            var loEx = new R_Exception();
            R_Db loDb;
            DbConnection loConn = poConnection;
            DbCommand loCommand;
            string lcCmd;
            bool lbRtn = false;
            try
            {
                loDb = new R_Db();
                loCommand = loDb.GetCommand();

                loDb.R_AddCommandParameter(loCommand, "CREASON_CODE", DbType.String, 50, poParam.CREASON_CODE);
                loDb.R_AddCommandParameter(loCommand, "TNOTES", DbType.String, 255, poParam.TNOTES);

                lcCmd = $"Select CCOMPANY_ID From GST_APPROVAL_I (Updlock) " +
                        $"Where CCOMPANY_ID = '{poParam.CCOMPANYID}' And CTRANSACTION_CODE = '{poEntity.CTRANSACTION_CODE}' " +
                        $"And CDEPT_CODE = '{poEntity.CDEPT_CODE}' And CREFERENCE_NO = '{poEntity.CREFERENCE_NO}' And CTRANSACTION_STATUS In (01,02); " +
                        $"Update GST_APPROVAL_I " +
                        $"Set CAPPROVAL_STATUS= '03' , CREASON_CODE= @CREASON_CODE , " +
                        $"TNOTES= @TNOTES FROM GST_APPROVAL_I A (NOLOCK)";

                loCommand.CommandText = lcCmd;

                loDb.SqlExecNonQuery(loConn, loCommand, false);
                lbRtn = true;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
        EndBlock:
            loEx.ThrowExceptionIfErrors();

            return lbRtn;
        }


    }
}
