using GSM04500Common;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSM04500Back
{
    public class GSM04510GOADeptCls : R_BusinessObject<GSM04510GOADeptDTO>
    {
        protected override void R_Deleting(GSM04510GOADeptDTO poEntity)
        {
            throw new NotImplementedException();
        }

        protected override GSM04510GOADeptDTO R_Display(GSM04510GOADeptDTO poEntity)
        {
            throw new NotImplementedException();
        }

        protected override void R_Saving(GSM04510GOADeptDTO poNewEntity, eCRUDMode poCRUDMode)
        {
            throw new NotImplementedException();
        }

        public List<GSM04510GOADeptDTO> JOURNAL_GROUP_GOA_DEPT_LIST(GSM04510GOADeptDBParameter poParameter)
        {
            R_Exception loException = new R_Exception();
            List<GSM04510GOADeptDTO> loReturn = null;
            R_Db loDb;
            DbCommand loCmd;

            try
            {
                loDb = new R_Db();
                var loConn = loDb.GetConnection();

                loCmd = loDb.GetCommand();
                //EXEC RSP_GS_GET_JOURNAL_GRP_GOA_DEPT_LIST
                //@CCOMPANY_ID, @CPROPERTY_ID, '10' , @CJOURNAL_GRP_CODE, @CGOA_CODE, @CUSER_LOGIN_ID
                var lcQuery = @"RSP_GS_GET_JOURNAL_GRP_GOA_DEPT_LIST";
                loCmd.CommandText = lcQuery;
                loCmd.CommandType = CommandType.StoredProcedure;
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 20, poParameter.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", DbType.String, 100, poParameter.CPROPERTY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CJOURNAL_GROUP_TYPE", DbType.String, 2, poParameter.CJRNGRP_TYPE);
                loDb.R_AddCommandParameter(loCmd, "@CJOURNAL_GROUP_CODE", DbType.String, 30, poParameter.CJOURNAL_GRP_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CGOA_CODE", DbType.String, 30, poParameter.CGOA_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_LOGIN_ID", DbType.String, 25, poParameter.CUSER_ID);

                var loReturnTemp = loDb.SqlExecQuery(loConn, loCmd, true);

                loReturn = R_Utility.R_ConvertTo<GSM04510GOADeptDTO>(loReturnTemp).ToList();

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
