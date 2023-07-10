using System.Data;
using System.Data.Common;
using System.Reflection.Metadata;
using System.Transactions;
using GLB00200Common;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;

namespace GLB00200Back
{
    public class GLB00200Cls : R_BusinessObject<GLB00200DTO>
    {
        protected override GLB00200DTO R_Display(GLB00200DTO poEntity)
        {
            throw new NotImplementedException();
        }

        protected override void R_Saving(GLB00200DTO poNewEntity, eCRUDMode poCRUDMode)
        {
            throw new NotImplementedException();
        }

        protected override void R_Deleting(GLB00200DTO poEntity)
        {
            throw new NotImplementedException();
        }

        public List<GLB00200DTO> ReversingJournalProcessList(GLB00200DBParameter poParameter)
        {
            R_Exception loException = new R_Exception();
            List<GLB00200DTO> loResult = null;
            R_Db loDb;
            DbCommand loCommand;
            try
            {
                loDb = new R_Db();
                var loConn = loDb.GetConnection();
                loCommand = loDb.GetCommand();
                var lcQuery = @"RSP_GL_SEARCH_ACTIVE_REVERSING_LIST";
                loCommand.CommandText = lcQuery;
                loCommand.CommandType = CommandType.StoredProcedure;

                loDb.R_AddCommandParameter(loCommand, "@CCOMPANY_ID", DbType.String, 50, poParameter.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCommand, "@CUSER_ID", DbType.String, 50, poParameter.CUSER_ID);
                loDb.R_AddCommandParameter(loCommand, "@CPERIOD", DbType.String, 6, poParameter.CPERIOD);
                loDb.R_AddCommandParameter(loCommand, "@CSEARCH_TEXT", DbType.String, 30, poParameter.CSEARCH_TEXT);
                loDb.R_AddCommandParameter(loCommand, "@CLANGUAGE_ID", DbType.String, 2, poParameter.CLANGUAGE_ID);

                var loReturnTemp = loDb.SqlExecQuery(loConn, loCommand, true);
                loResult = R_Utility.R_ConvertTo<GLB00200DTO>(loReturnTemp).ToList();

            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();
            return loResult;
        }

        public GLB00200MinMaxYearDTO GetMinMaxYear(GLB00200DBParameter poParameter)
        {
            R_Exception loException = new R_Exception();
            GLB00200MinMaxYearDTO loResult = null;
            R_Db loDb;
            DbCommand loCommand;
            try
            {
                loDb = new R_Db();
                var loConn = loDb.GetConnection();
                loCommand = loDb.GetCommand();
                var lcQuery =
                    @"SELECT IMIN_YEAR = CAST(MIN(CYEAR) AS INT),IMAX_YEAR = CAST(MAX(CYEAR) AS INT) FROM GSM_PERIOD(NOLOCK) WHERE CCOMPANY_ID = @CCOMPANY_ID";
                loCommand.CommandText = lcQuery;
                loCommand.CommandType = CommandType.Text;
                loDb.R_AddCommandParameter(loCommand, "@CCOMPANY_ID", DbType.String, 50, poParameter.CCOMPANY_ID);
                var loReturnTemp = loDb.SqlExecQuery(loConn, loCommand, true);
                loResult = R_Utility.R_ConvertTo<GLB00200MinMaxYearDTO>(loReturnTemp).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();
            return loResult;
        }

        public List<GLB00200JournalDetailDTO> GetDetail_ReversingJournalList(GLB00200DBParameter poParameter)
        {
            R_Exception loException = new R_Exception();
            List<GLB00200JournalDetailDTO> loResult = null;
            R_Db loDb;
            DbCommand loCommand;
            try
            {
                loDb = new R_Db();
                var loConn = loDb.GetConnection();
                loCommand = loDb.GetCommand();
                var lcQuery = @"RSP_GL_GET_JOURNAL_DETAIL_LIST";
                loCommand.CommandText = lcQuery;
                loCommand.CommandType = CommandType.StoredProcedure;

                loDb.R_AddCommandParameter(loCommand, "@CJRN_ID", DbType.String, 50, poParameter.CREC_ID);
                loDb.R_AddCommandParameter(loCommand, "@CLANGUAGE_ID", DbType.String, 2, poParameter.CLANGUAGE_ID);

                var loReturnTemp = loDb.SqlExecQuery(loConn, loCommand, true);
                loResult = R_Utility.R_ConvertTo<GLB00200JournalDetailDTO>(loReturnTemp).ToList();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();
            return loResult;
        }

        public List<GLB00200ResultProcessReversing> ReversingJournalProcess(List<GLB00200DTO> poListParameter, GLB00200DBParameter poParameter)
        {
            R_Exception loException = new R_Exception();
            List<GLB00200ResultProcessReversing> listReversingJournalProcessReturn = null;
            int lnCount;
            var loDb = new R_Db();
            DbConnection loConn = null;
            bool llStatusReversingJournal;
            try
            {
                lnCount = 1;
                foreach (var item in poListParameter)
                {
                    try
                    {
                        GLB00200ResultProcessReversing loTemp = new GLB00200ResultProcessReversing();
                        using (TransactionScope TransScope = new TransactionScope(TransactionScopeOption.Required))
                        {
                            loConn = loDb.GetConnection();
                            llStatusReversingJournal = ProcessEachReversingJournal(item, poParameter, loConn);

                            loTemp.CREF_NO = item.CREF_NO;

                            if (llStatusReversingJournal == false)
                            {
                                loTemp.LSUCCESSED = llStatusReversingJournal;
                                listReversingJournalProcessReturn.Add(loTemp);
                                goto EndDetail;
                            }
                            loTemp.LSUCCESSED = llStatusReversingJournal;
                            listReversingJournalProcessReturn.Add(loTemp);

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
            return listReversingJournalProcessReturn;
        }

        private bool ProcessEachReversingJournal(GLB00200DTO poEntity, GLB00200DBParameter poParameter,
            DbConnection poConnection)
        {
            var loEx = new R_Exception();
            R_Db loDb;
            DbConnection loConn = poConnection;
            DbCommand loCommand;
            string lcquery;
            bool lbRtn = false;
            try
            {
                loDb = new R_Db();
                loCommand = loDb.GetCommand();
                lcquery = @"RSP_GL_PROCESS_REVERSING_JRN";
                loCommand.CommandText = lcquery;
                loCommand.CommandType = CommandType.StoredProcedure;

                loDb.R_AddCommandParameter(loCommand, "@CCOMPANY_ID", DbType.String, 50, poParameter.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCommand, "@CUSER_ID", DbType.String, 50, poParameter.CUSER_ID);
                loDb.R_AddCommandParameter(loCommand, "@CDEPT_CODE", DbType.String, 25, poEntity.CDEPT_CODE);
                loDb.R_AddCommandParameter(loCommand, "@CREF_NO", DbType.String, 25, poEntity.CREF_NO);
                loDb.R_AddCommandParameter(loCommand, "@CREC_ID", DbType.String, 50, poEntity.CREC_ID);

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