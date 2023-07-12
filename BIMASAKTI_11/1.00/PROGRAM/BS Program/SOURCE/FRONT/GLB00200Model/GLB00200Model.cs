using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GLB00200Common;
using R_APIClient;
using R_BlazorFrontEnd.Exceptions;
using R_BusinessObjectFront;

namespace GLB00200Model
{
    public class GLB00200Model : R_BusinessObjectServiceClientBase<GLB00200DTO>, IGLB00200
    {
        private const string DEFAULT_HTTP = "R_DefaultServiceUrlGL";
        private const string DEFAULT_ENDPOINT = "api/GLB00200";
        private const string DEFAULT_MODULE = "GL";
        public GLB00200Model(
            string pcHttpClientName = DEFAULT_HTTP, 
            string pcRequestServiceEndPoint = DEFAULT_ENDPOINT,
            string pcModuleName = DEFAULT_MODULE, 
            bool plSendWithContext = true, 
            bool plSendWithToken = true) 
            : base(pcHttpClientName, pcRequestServiceEndPoint, pcModuleName, plSendWithContext, plSendWithToken)
        {
        }

        public GLB00200MinMaxYearDTO GetMinMaxYear()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<GLB00200DTO> ReversingJournalProcessListStream()
        {
            throw new NotImplementedException();
        }

        public GLB00200JournalDetailListDTO DetailReversingJournalProcessList()
        {
            throw new NotImplementedException();
        }

        public GLB00200ResultProcessReversingListDTO ProcessReversingJournal()
        {
            throw new NotImplementedException();
        }

        public async Task<GLB00200MinMaxYearDTO> GetMinMaxYearAsyncModel()
        {
            var loEx = new R_Exception();
            GLB00200MinMaxYearDTO loResult = new GLB00200MinMaxYearDTO();
            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GLB00200MinMaxYearDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLB00200.GetMinMaxYear),
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
        public async Task<GLB00200ListDTO> GetReversingJournalProcessAsyncModel(string lcPeriod, string lcSearchText)
        {
            var loEx = new R_Exception();
            GLB00200ListDTO loResult = new GLB00200ListDTO();
            ;
            try
            {
                R_BlazorFrontEnd.R_FrontContext.R_SetStreamingContext(ContextConstant.CPERIOD, lcPeriod);
                R_BlazorFrontEnd.R_FrontContext.R_SetStreamingContext(ContextConstant.CSEARCH_TEXT, lcSearchText);

                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                var loTemp = await R_HTTPClientWrapper.R_APIRequestStreamingObject<GLB00200DTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLB00200.ReversingJournalProcessListStream),
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

        public async Task<GLB00200ResultProcessReversingListDTO> GetProcessReversingJournalAsyncModel()
        {
            var loEx = new R_Exception();
            GLB00200ResultProcessReversingListDTO loReturn = new GLB00200ResultProcessReversingListDTO();
            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                loReturn = await R_HTTPClientWrapper.R_APIRequestObject<GLB00200ResultProcessReversingListDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLB00200.ProcessReversingJournal),
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loReturn;
        }

        public async Task<GLB00200JournalDetailListDTO> GetDetail_ReversingJournalAsyncModel(string lcCREC_ID)
        {
            var loEx = new R_Exception();
            GLB00200JournalDetailListDTO loResult = null;
            try
            {
                R_BlazorFrontEnd.R_FrontContext.R_SetStreamingContext(ContextConstant.CREC_ID, lcCREC_ID);

                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GLB00200JournalDetailListDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLB00200.DetailReversingJournalProcessList),
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

    }
}
