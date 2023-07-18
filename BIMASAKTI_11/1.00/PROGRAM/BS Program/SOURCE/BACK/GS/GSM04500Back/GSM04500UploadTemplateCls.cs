using R_BackEnd;
using R_Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSM04500Common;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata;
using R_CommonFrontBackAPI;
using System.Transactions;
using System.Windows.Input;
using System.Text.Json;

namespace GSM04500Back
{
    public class GSM04500UploadTemplateCls : R_IAttachFile, R_IBatchProcess
    {
        public GSM04500ListDTO GetUploadJournalGroupList(GSM04500DBParameter poParameter)
        {
            R_Exception loException = new R_Exception();
            GSM04500ListDTO loResult = null;

            try
            {
                R_Db loDb = new R_Db();
                DbConnection loConn = loDb.GetConnection();
                DbCommand loCmd = loDb.GetCommand();
                string lcQuery =
                    $"SELECT * FROM GSM_JRNGRP (NOLOCK) WHERE CCOMPANY_ID =@CCOMPANY_ID AND CPROPERTY_ID = @CPROPERTY_ID " +
                    "AND CJRNGRP_TYPE =@CJRNGRP_TYPE ";
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 20, poParameter.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", DbType.String, 25, poParameter.CPROPERTY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CJRNGRP_TYPE", DbType.String, 5, poParameter.CJRNGRP_TYPE);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                var lotemp = R_Utility.R_ConvertTo<GSM04500DTO>(loDataTable).ToList();
                loResult.ListData = lotemp;

            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();

            return loResult;
        }

        public void R_AttachFile(R_AttachFilePar poAttachFile)
        {
            R_Exception loException = new R_Exception();
            string lcPropertyId;
            string lcJournalGroupType;
            string lcCmd;
            string lcQuery = "";
            var loDb = new R_Db();
            DbConnection loConn = loDb.GetConnection();
            var loCommand = loDb.GetCommand();
            Dictionary<string, string> loMapping = new Dictionary<string, string>();
            List<GSM04500UploadFromUserDTO> loResult = null;

            try
            {
                var loTempObject = R_NetCoreUtility.R_DeserializeObjectFromByte<List<GSM04500UploadFromUserDTO>>(poAttachFile.BigObject);
                var loObject = R_Utility.R_ConvertCollectionToCollection<GSM04500UploadFromUserDTO, GSM04500UploadFromUserRequestDTO>(loTempObject);

                //get parameter
                var loVar = poAttachFile.UserParameters.Where((x) => x.Key.Equals(ContextConstant.CPROPERTY_ID)).FirstOrDefault().Value;
                var loVar2 = poAttachFile.UserParameters.Where((x) => x.Key.Equals(ContextConstant.CJRNGRP_TYPE)).FirstOrDefault().Value;
                lcPropertyId = ((System.Text.Json.JsonElement)loVar).GetString();
                lcJournalGroupType = ((System.Text.Json.JsonElement)loVar2).GetString();

                using (var TransScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    lcQuery = $"CREATE TABLE #JRNLGROUP " +
                              $"(NO INT " +
                              $"JournalGroup VARCHAR(8), " +
                              $"JournalGroupName VARCHAR(80), " +
                              $"EnableAccrual BIT, " +
                              $"ValidFlag BIT) ";

                    loDb.SqlExecNonQuery(lcQuery, loConn, false);

                    loDb.R_BulkInsert<GSM04500UploadFromUserRequestDTO>((SqlConnection)loConn, "#JRNLGROUP", loObject);

                    lcQuery = "RSP_LM_VALIDATE_UPLOAD_JOURNAL_GROUP";
                    loCommand.CommandText = lcQuery;
                    loCommand.CommandType = CommandType.StoredProcedure;
                    loDb.R_AddCommandParameter(loCommand, "@CCOMPANY_ID", DbType.String, 8, poAttachFile.Key.COMPANY_ID);
                    loDb.R_AddCommandParameter(loCommand, "@USER_ID", DbType.String, 25, poAttachFile.Key.USER_ID);

                    loDb.R_AddCommandParameter(loCommand, "@CPROPERTY_ID", DbType.String, 20, lcPropertyId);
                    loDb.R_AddCommandParameter(loCommand, "@CJOURNAL_GROUP_TYPE ", DbType.String, 20, lcJournalGroupType);
                    loDb.R_AddCommandParameter(loCommand, "@KEY_GUID", DbType.String, 55, poAttachFile.Key.KEY_GUID);

                    loDb.SqlExecNonQuery(loConn, loCommand, false);

                    lcQuery = $"SELECT CCOMPANY_ID FROM GST_XML_RESULT WHERE CCOMPANY_ID = @CCOMPANY_ID AND CUSER_ID = @CUSER_ID AND CKEY_GUID = @CKEY_GUID ";
                    loCommand.CommandText = lcQuery;

                    var loDataTable = loDb.SqlExecQuery(loConn, loCommand, false);

                    var loDataErrorValidate = R_Utility.R_ConvertTo<GSM04500DTO>(loDataTable).ToList();

                    if (loDataErrorValidate.Count > 0)
                    {

                        lcQuery = "EXECUTE RSP_ConvertXMLToTable @CCOMPANY_ID, @CUSER_ID, @CKEY_GUID";
                        loCommand.CommandText = lcQuery;

                        var loDataTableResult = loDb.SqlExecQuery(loConn, loCommand, false);

                        var loTempResult = R_Utility.R_ConvertTo<GSM04500UploadErrorValidateDTO>(loDataTableResult).ToList();

                        var loConvertJson = JsonSerializer.Serialize(loTempResult);

                        throw new Exception(loConvertJson);
                    }

                    TransScope.Complete();
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
                        loConn.Close();

                    loConn.Dispose();
                    loConn = null;
                }
            }

            loException.ThrowExceptionIfErrors();
        }

        public void R_BatchProcess(R_BatchProcessPar poBatchProcessPar)
        {
            R_Exception loException = new R_Exception();
            string lcQuery = "";
            var loDb = new R_Db();
            DbConnection loConn = loDb.GetConnection();
            var loCommand = loDb.GetCommand();

            try
            {
                var loTempObject = R_NetCoreUtility.R_DeserializeObjectFromByte<List<GSM04500UploadErrorValidateDTO>>(poBatchProcessPar.BigObject);
                var loObject = loTempObject.Select(loTemp => new GSM04500UploadFromUserRequestDTO
                {
                    CJRNGRP_CODE = loTemp.CJRNGRP_CODE,
                    CJRNGRP_NAME = loTemp.CJRNGRP_NAME, 
                    LACCRUAL= loTemp.LACCRUAL,
                    CNOTES = loTemp.CNOTES
                }).ToList();

                var loVar = poBatchProcessPar.UserParameters.Where((x) => x.Key.Equals(ContextConstant.CPROPERTY_ID)).FirstOrDefault().Value;
                var lcPropertyId = ((System.Text.Json.JsonElement)loVar).GetString();

                var loVar2 = poBatchProcessPar.UserParameters.Where((x) => x.Key.Equals(ContextConstant.CJRNGRP_TYPE)).FirstOrDefault().Value;
                var lcJournalGroupType = ((System.Text.Json.JsonElement)loVar2).GetString();

                var loTempVar = poBatchProcessPar.UserParameters.Where((x) => x.Key.Equals(ContextConstant.COVERWRITE)).FirstOrDefault().Value;
                var lbOverwrite = ((System.Text.Json.JsonElement)loTempVar).GetBoolean();

                using (var TransScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    lcQuery = $"CREATE TABLE #JRNLGROUP " +
                              $"(NO INT " +
                              $"JournalGroup VARCHAR(8), " +
                              $"JournalGroupName VARCHAR(80), " +
                              $"EnableAccrual BIT, " +
                              $"ValidFlag BIT) ";

                    loDb.SqlExecNonQuery(lcQuery, loConn, false);

                    loDb.R_BulkInsert<GSM04500UploadFromUserRequestDTO>((SqlConnection)loConn, "#JRNLGROUP", loObject);

                    lcQuery = "RSP_GS_UPLOAD_JOURNAL_GROUP";
                    loCommand.CommandText = lcQuery;

                    loDb.R_AddCommandParameter(loCommand, "@CCOMPANY_ID", DbType.String, 8, poBatchProcessPar.Key.COMPANY_ID);
                    loDb.R_AddCommandParameter(loCommand, "@CUSER_ID", DbType.String, 20, poBatchProcessPar.Key.USER_ID);

                    loDb.R_AddCommandParameter(loCommand, "@CPROPERTY_ID", DbType.String, 20, lcPropertyId);
                    loDb.R_AddCommandParameter(loCommand, "@CJOURNAL_GROUP_TYPE", DbType.String, 20, lcJournalGroupType);
                    loDb.R_AddCommandParameter(loCommand, "@CKEY_GUID", DbType.String, 20, poBatchProcessPar.Key.KEY_GUID);
                    loDb.R_AddCommandParameter(loCommand, "@LOVERWRITE", DbType.Boolean, 20, lbOverwrite);

                    loDb.SqlExecNonQuery(loConn, loCommand, false);

                    TransScope.Complete();
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
                        loConn.Close();

                    loConn.Dispose();
                    loConn = null;
                }
            }

            loException.ThrowExceptionIfErrors();
        }
    }
}
