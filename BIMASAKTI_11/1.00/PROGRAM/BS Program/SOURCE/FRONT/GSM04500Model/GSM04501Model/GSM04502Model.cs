using GSM04500Common;
using R_APIClient;
using R_BlazorFrontEnd.Exceptions;
using R_BusinessObjectFront;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GSM04501Model
{
    public class GSM04502Model : R_BusinessObjectServiceClientBase<GSM04510GOADeptDTO>, IGSM04510GOADept
    {
        private const string DEFAULT_HTTP = "R_DefaultServiceUrl";
        private const string DEFAULT_ENDPOINT = "api/GSM04510GOADept";

        public GSM04502Model(
            string pcHttpClientName = DEFAULT_HTTP,
            string pcRequestServiceEndPoint = DEFAULT_ENDPOINT,
            bool plSendWithContext = true,
            bool plSendWithToken = true)
            : base(pcHttpClientName, pcRequestServiceEndPoint, plSendWithContext, plSendWithToken)
        {
        }


        public async Task<GSM04510GOADeptListDTO> GetAllGOADeptAsync(string lcJournalGRPType, string lcPropertyId, string lcJournalGRPCode)
        {
            var loEx = new R_Exception();
            GSM04510GOADeptListDTO loResult = new GSM04510GOADeptListDTO();
            try
            {

                R_BlazorFrontEnd.R_FrontContext.R_SetStreamingContext(ContextConstant.CJRNGRP_TYPE, lcJournalGRPType);
                R_BlazorFrontEnd.R_FrontContext.R_SetStreamingContext(ContextConstant.CPROPERTY_ID, lcPropertyId);
                R_BlazorFrontEnd.R_FrontContext.R_SetStreamingContext(ContextConstant.CJOURNAL_GRP_CODE, lcJournalGRPCode);

                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                var loTmp = await R_HTTPClientWrapper.R_APIRequestStreamingObject<GSM04510GOADeptDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGSM04510GOADept.JOURNAL_GRP_GOA_DEPT_LIST),
                    _SendWithContext,
                    _SendWithToken);
                loResult.ListData = loTmp;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }

        public IAsyncEnumerable<GSM04510GOADeptDTO> JOURNAL_GRP_GOA_DEPT_LIST()
        {
            throw new NotImplementedException();
        }
    }
}
