using GSM06500Common;
using R_APIClient;
using R_BlazorFrontEnd.Exceptions;
using R_BusinessObjectFront;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GSM06500Model
{
    public class GSM06500Model : R_BusinessObjectServiceClientBase<GSM06500DTO>, IGSM06500
    {
        private const string DEFAULT_HTTP = "R_DefaultServiceUrl";
        private const string DEFAULT_ENDPOINT = "api/GSM06500";

        public GSM06500Model(
            string pcHttpClientName = DEFAULT_HTTP,
            string pcRequestServiceEndPoint = DEFAULT_ENDPOINT,
            bool plSendWithContext = false,
            bool plSendWithToken = false)
            : base(pcHttpClientName, pcRequestServiceEndPoint, plSendWithContext, plSendWithToken)
        {
        }

        public IAsyncEnumerable<GSM06500DTO> TermOfPayment()
        {
            throw new NotImplementedException();
        }
        public async Task<GSM06500ListDTO> GetTermOfPaymentListAsyncModel(string pcCompanyId, string pcPropertyId, string pcUserLoginId)
        {
            var loEx = new R_Exception();
            GSM06500ListDTO loResult = null;

            try
            {
                R_BlazorFrontEnd.R_FrontContext.R_SetStreamingContext(ContextConstant.CCOMPANY_ID, pcCompanyId);
                R_BlazorFrontEnd.R_FrontContext.R_SetStreamingContext(ContextConstant.CPROPERTY_ID, pcPropertyId);
                R_BlazorFrontEnd.R_FrontContext.R_SetStreamingContext(ContextConstant.CUSER_LOGIN_ID, pcUserLoginId);
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GSM06500ListDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGSM06500.TermOfPayment),
                    _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;

        }
    }
}
