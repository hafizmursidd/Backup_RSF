using GLB00200Back;
using GLB00200Common;
using Microsoft.AspNetCore.Mvc;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;

namespace GLB00200Service
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GLB00200Controller : ControllerBase, IGLB00200
    {
        [HttpPost]
        public R_ServiceGetRecordResultDTO<GLB00200DTO> R_ServiceGetRecord(R_ServiceGetRecordParameterDTO<GLB00200DTO> poParameter)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public R_ServiceSaveResultDTO<GLB00200DTO> R_ServiceSave(R_ServiceSaveParameterDTO<GLB00200DTO> poParameter)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<GLB00200DTO> poParameter)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public GLB00200InitalProcessDTO GetInitialProcess()
        {
            R_Exception loException = new R_Exception();
            GLB00200DBParameter loDbParameter = new();
            GLB00200InitalProcessDTO loReturn = null;
            try
            {
                var loCls = new GLB00200Cls();
                loDbParameter.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbParameter.CCOMPANY_ID = "RCD";
                loReturn = loCls.InitialProcess(loDbParameter);

            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();

            return loReturn;
        }
        [HttpPost]
        public GLB00200JournalDetailListDTO DetailReversingJournalProcessList()
        {
            R_Exception loException = new R_Exception();
            GLB00200DBParameter loDbParameter;
            GLB00200JournalDetailListDTO loReturn = new();
            try
            {
                loDbParameter = new GLB00200DBParameter();
                loDbParameter.CLANGUAGE_ID = R_BackGlobalVar.CULTURE;
                loDbParameter.CREC_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CREC_ID);
                //loDbParameter.CLANGUAGE_ID = "EN";
                //loDbParameter.CREC_ID = "2C6AFFF4-848E-405F-9A0C-C6E9AD240748";

                var loCls = new GLB00200Cls();
                var temp = loCls.GetDetail_ReversingJournalList(loDbParameter);
                loReturn.Data = temp;

            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();

            return loReturn;
        }
        [HttpPost]
        public GLB00200ResultProcessReversingListDTO ProcessReversingJournal()
        {
            var loEx = new R_Exception();
            GLB00200ResultProcessReversingListDTO loReturn = new();
            List<GLB00200DTO> loReversingListSelectedData = new List<GLB00200DTO>();
            GLB00200DBParameter loParam = new GLB00200DBParameter();
            List<GLB00200ResultProcessReversing> loTemp;

            try
            {
                GLB00200Cls loCls = new GLB00200Cls();
                loParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loParam.CUSER_ID = R_BackGlobalVar.USER_ID;
                loReversingListSelectedData =
                    R_Utility.R_GetStreamingContext<List<GLB00200DTO>>(ContextConstant.LIST_REVERSING);

                loTemp = loCls.ReversingJournalProcess(loReversingListSelectedData, loParam);
                loReturn.Data = loTemp;

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
        EndBlock:
            loEx.ThrowExceptionIfErrors();
            return loReturn;

        }


        [HttpPost]
        public IAsyncEnumerable<GLB00200DTO> ReversingJournalProcessListStream()
        {
            R_Exception loException = new R_Exception();
            GLB00200DBParameter loDbParameter;
            List<GLB00200DTO> loReturnTemp;
            IAsyncEnumerable<GLB00200DTO> loRtn = null;
            try
            {
                loDbParameter = new GLB00200DBParameter();
                loDbParameter.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbParameter.CUSER_ID = R_BackGlobalVar.USER_ID;
                loDbParameter.CLANGUAGE_ID = R_BackGlobalVar.CULTURE;
                loDbParameter.CPERIOD = R_Utility.R_GetStreamingContext<string>(ContextConstant.CPERIOD);
                loDbParameter.CSEARCH_TEXT = R_Utility.R_GetStreamingContext<string>(ContextConstant.CSEARCH_TEXT);
                var loCls = new GLB00200Cls();
                loReturnTemp = loCls.ReversingJournalProcessList(loDbParameter);
                loRtn = Get_ReversingJournalProcessList(loReturnTemp);

            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();

            return loRtn;
        }



        private async IAsyncEnumerable<GLB00200DTO> Get_ReversingJournalProcessList(List<GLB00200DTO> poParameter)
        {
            foreach (var item in poParameter)
            {
                yield return item;
            }
        }
    }
}