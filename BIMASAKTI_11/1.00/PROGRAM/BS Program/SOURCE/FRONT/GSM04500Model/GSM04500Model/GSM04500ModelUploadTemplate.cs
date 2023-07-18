using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GSM04500Common;
using R_APIClient;
using R_BlazorFrontEnd.Exceptions;
using R_BusinessObjectFront;

namespace GSM04500Model
{
    public class GSM04500ModelUploadTemplate : R_BusinessObjectServiceClientBase<GSM04500DTO>, IGSM04500UploadTemplate
    {
        private const string DEFAULT_HTTP = "R_DefaultServiceUrl";
        private const string DEFAULT_ENDPOINT = "api/GSM04500UploadTemplate";
        private const string DEFAULT_MODULE = "GS";

        public GSM04500ModelUploadTemplate(
            string pcHttpClientName = DEFAULT_HTTP,
            string pcRequestServiceEndPoint = DEFAULT_ENDPOINT,
            bool plSendWithContext = true,
            bool plSendWithToken = true) :
            base(pcHttpClientName, pcRequestServiceEndPoint, DEFAULT_MODULE, plSendWithContext, plSendWithToken)
        {
        }

        public GSM04500UploadFileDTO DownloadTemplateFile()
        {
            throw new NotImplementedException();
        }

        public GSM04500ListDTO GetJournalGroupUploadList()
        {
            throw new NotImplementedException();
        }

        public async Task<GSM04500UploadFileDTO> DownloadTemplateFileAsync()
        {
            var loEx = new R_Exception();
            GSM04500UploadFileDTO loResult = new GSM04500UploadFileDTO();

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                var loTempResult = await R_HTTPClientWrapper.R_APIRequestObject<GSM04500UploadFileDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGSM04500UploadTemplate.DownloadTemplateFile),
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);

                loResult = loTempResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }

        public async Task<GSM04500ListDTO> GetJournalGroupUploadListAsync()
        {
            var loEx = new R_Exception();
            GSM04500ListDTO loResult = new GSM04500ListDTO();

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                var loTempResult = await R_HTTPClientWrapper.R_APIRequestObject<GSM04500ListDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGSM04500UploadTemplate.GetJournalGroupUploadList),
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);

                loResult = loTempResult;
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
