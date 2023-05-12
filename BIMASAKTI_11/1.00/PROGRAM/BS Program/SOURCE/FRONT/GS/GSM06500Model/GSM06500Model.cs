using GSM06500Common;
using R_APIClient;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
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

        public string lcPropertyId {  get; set; }

        public GSM06500Model(
            string pcHttpClientName = DEFAULT_HTTP,
            string pcRequestServiceEndPoint = DEFAULT_ENDPOINT,
            bool plSendWithContext = true,
            bool plSendWithToken = true)
            : base(pcHttpClientName, pcRequestServiceEndPoint, plSendWithContext, plSendWithToken)
        {
        }

        public IAsyncEnumerable<GSM06500DTO> TermOfPayment()
        {
            throw new NotImplementedException();
        }
        public GSM06500ListDTO GetallTermOfpaymentOriginal()
        {
            throw new NotImplementedException();
        }
        public async Task<GSM06500ListDTO> GetTermOfPaymentListAsyncModel( string lcPropertyId)
        {
            var loEx = new R_Exception();
            GSM06500ListDTO loResult = null;
            //lcCompany = "ABCDE";
            //lcuserLoginId = "Admin";
            try
            {
                //R_BlazorFrontEnd.R_FrontContext.R_SetStreamingContext(R_GlobalVar.CCOMPANY_ID, lcCompany);
                //R_BlazorFrontEnd.R_FrontContext.R_SetStreamingContext(R_GlobalVar.CUSER_LOGIN_ID, lcuserLoginId);
                R_BlazorFrontEnd.R_FrontContext.R_SetStreamingContext(ContextConstant.CPROPERTY_ID, lcPropertyId);

                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GSM06500ListDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGSM06500.GetallTermOfpaymentOriginal),
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
        public async Task <GSM06500PropertyListDTO> GetPropertyAsyncModel()
        {
            var loEx = new R_Exception();
            GSM06500PropertyListDTO loResult = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GSM06500PropertyListDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGSM06500.GetAllPropertyList),
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

        public GSM06500PropertyListDTO GetAllPropertyList()
        {
            throw new NotImplementedException();
        }

        //public async Task<GSM06500DTO> GetTermOfPaymentOneRecordAsyncModel()
        //{
        //    var loEx = new R_Exception();
        //    GSM06500DTO loResult = null;
        //    //lcCompany = "ABCDE";
        //    //lcuserLoginId = "Admin";
        //    try
        //    {
        //        R_HTTPClientWrapper.httpClientName = _HttpClientName;
        //        loResult = await R_HTTPClientWrapper.R_APIRequestObject<GSM06500DTO>(
        //            _RequestServiceEndPoint,
        //            nameof(IGSM06500.R_ServiceGetRecord),
        //            _SendWithContext,
        //            _SendWithToken);
        //    }
        //    catch (Exception ex)
        //    {
        //        loEx.Add(ex);
        //    }
        //    loEx.ThrowExceptionIfErrors();
        //    return loResult;

        //}





    }
}
