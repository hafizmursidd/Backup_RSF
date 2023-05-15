
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System.Data.Common;
using System.Data;
using GSM04500Common;

namespace GSM04500Back
{
    public partial class GSM04500Cls : R_BusinessObject<GSM04500DTO>
    {
        protected override void R_Deleting(GSM04500DTO poEntity)
        {
            throw new NotImplementedException();
        }

        protected override GSM04500DTO R_Display(GSM04500DTO poEntity)
        {
            throw new NotImplementedException();
        }

        protected override void R_Saving(GSM04500DTO poNewEntity, eCRUDMode poCRUDMode)
        {
            throw new NotImplementedException();
        }

        public List<GSM04500DTO> JOURNAL_GROUP_LIST_SERVICE(GSM04500DBParameter poParameter)
        {
            R_Exception loException = new R_Exception();
            List<GSM04500DTO> loReturn = null;
            R_Db loDb;
            DbCommand loCmd;

            try
            {
                loDb = new R_Db();
                var loConn = loDb.GetConnection();

                loCmd = loDb.GetCommand();

                var lcQuery = @"RSP_GS_GET_JOURNAL_GRP_LIST";
                loCmd.CommandText = lcQuery;
                loCmd.CommandType = CommandType.StoredProcedure;
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 20, poParameter.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", DbType.String, 100, poParameter.CPROPERTY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CJOURNAL_GROUP_TYPE", DbType.String, 2, poParameter.CJRNGRP_TYPE);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_LOGIN_ID", DbType.String, 2, poParameter.CUSER_ID);

                var loReturnTemp = loDb.SqlExecQuery(loConn, loCmd, true);

                loReturn = R_Utility.R_ConvertTo<GSM04500DTO>(loReturnTemp).ToList();

            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();

            return loReturn;
        }

        public List<GSM04500PropertyDTO> GetAllPropertyList(GSM04500DBParameter poParameter)
        {
            R_Exception loException = new R_Exception();
            List<GSM04500PropertyDTO> loReturn = null;
            R_Db loDb;
            DbCommand loCmd;

            try
            {
                loDb = new R_Db();
                var loConn = loDb.GetConnection();
                loCmd = loDb.GetCommand();

                var lcQuery = @"RSP_GS_GET_PROPERTY_LIST";
                loCmd.CommandText = lcQuery;
                loCmd.CommandType = CommandType.StoredProcedure;
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poParameter.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 50, poParameter.CUSER_ID);

                var loReturnTemp = loDb.SqlExecQuery(loConn, loCmd, true);

                loReturn = R_Utility.R_ConvertTo<GSM04500PropertyDTO>(loReturnTemp).ToList();

            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();

            return loReturn;
        }
    }
}