using R_BusinessObjectFront;
using System;
using System.Collections.Generic;
using LMM06000Common;
using R_APIClient;
using R_BlazorFrontEnd.Exceptions;
using System.Threading.Tasks;

namespace LMM06000Model
{
    public class LMM06000Model : R_BusinessObjectServiceClientBase<LMM06000BillingRuleDTO>, ILMM06000
    {
        private const string DEFAULT_HTTP = "R_DefaultServiceUrlLM";
        private const string DEFAULT_ENDPOINT = "api/LMM06000";
        private const string DEFAULT_MODULE = "LM";
        public LMM06000Model(
            string pcHttpClientName = DEFAULT_HTTP, 
            string pcRequestServiceEndPoint = DEFAULT_ENDPOINT,
            string pcModuleName = DEFAULT_MODULE, 
            bool plSendWithContext = true, 
            bool plSendWithToken = true) 
            : base(pcHttpClientName, pcRequestServiceEndPoint, pcModuleName, plSendWithContext, plSendWithToken)
        {
        }

        public IAsyncEnumerable<LMM06000BillingRuleDTO> BillingRuleListStream()
        {
            throw new NotImplementedException();
        }

        public LMM06000PropertyListDTO GetAllPropertyList()
        {
            throw new NotImplementedException();
        }

        public LMM06000UnitTypeListDTO GetAllUnitTypeList()
        {
            throw new NotImplementedException();
        }

        public async Task<LMM06000PropertyListDTO> GetPropertyAsyncModel()
        {
            var loEx = new R_Exception();
            LMM06000PropertyListDTO loResult = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<LMM06000PropertyListDTO>(
                    _RequestServiceEndPoint,
                    nameof(ILMM06000.GetAllPropertyList),
                    DEFAULT_MODULE,
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
        public async Task<LMM06000UnitTypeListDTO> GetUnitTypeAsyncModel( string lcPropertyId)
        {
            var loEx = new R_Exception();
            LMM06000UnitTypeListDTO loResult = null;

            try
            {
                R_BlazorFrontEnd.R_FrontContext.R_SetStreamingContext(ContextConstant.CPROPERTY_ID, lcPropertyId);

                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<LMM06000UnitTypeListDTO>(
                    _RequestServiceEndPoint,
                    nameof(ILMM06000.GetAllUnitTypeList),
                    DEFAULT_MODULE,
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
        
        public async Task<LMM06000BillingRuleListDTO> GetBillingRuleListAsyncModel(string lcPropertyId, string lcUnitTypeId)
        {
            var loEx = new R_Exception();
            LMM06000BillingRuleListDTO loResult = new LMM06000BillingRuleListDTO();
            bool lcLACTIVE_ONLY = false;
    
            try
            {
                R_BlazorFrontEnd.R_FrontContext.R_SetStreamingContext(ContextConstant.CPROPERTY_ID, lcPropertyId);
                R_BlazorFrontEnd.R_FrontContext.R_SetStreamingContext(ContextConstant.CUNIT_TYPE_ID, lcUnitTypeId);
                R_BlazorFrontEnd.R_FrontContext.R_SetStreamingContext(ContextConstant.LACTIVE_ONLY, lcLACTIVE_ONLY);

                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                var loTemp = await R_HTTPClientWrapper.R_APIRequestStreamingObject<LMM06000BillingRuleDTO>(
                    _RequestServiceEndPoint,
                    nameof(ILMM06000.BillingRuleListStream),
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);
                
                loResult.Data = loTemp;
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
