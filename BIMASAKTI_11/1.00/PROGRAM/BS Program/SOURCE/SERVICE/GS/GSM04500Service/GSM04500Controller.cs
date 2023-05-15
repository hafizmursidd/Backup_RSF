using GSM04500Back;
using GSM04500Common;
using Microsoft.AspNetCore.Mvc;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;

namespace GSM04500Service
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GSM04500Controller : ControllerBase, IGSM04500
    {
        [HttpPost]
        public GSM04500PropertyListDTO GetAllPropertyList()
        {
            var loEx = new R_Exception();
            GSM04500PropertyListDTO loRtn = null;
            var loParameter = new GSM04500DBParameter();

            try
            {
                var loCls = new GSM04500Cls();
                loRtn = new GSM04500PropertyListDTO();

                //loParameter.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                //loParameter.CUSER_ID = R_BackGlobalVar.CUSER_ID;

                loParameter.CCOMPANY_ID = "RCD";
                loParameter.CUSER_ID = "hmc";

                var loResult = loCls.GetAllPropertyList(loParameter);
                loRtn.Data = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<GSM04500DTO> poParameter)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public R_ServiceGetRecordResultDTO<GSM04500DTO> R_ServiceGetRecord(R_ServiceGetRecordParameterDTO<GSM04500DTO> poParameter)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public R_ServiceSaveResultDTO<GSM04500DTO> R_ServiceSave(R_ServiceSaveParameterDTO<GSM04500DTO> poParameter)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public IAsyncEnumerable<GSM04500DTO> GET_JOURNAL_GRP_LIST_STREAM()
        {
            var loEx = new R_Exception();
            GSM04500DBParameter loDbParameter;
            R_Exception loException = new R_Exception();
            IAsyncEnumerable<GSM04500DTO> loRtn = null;
            List<GSM04500DTO> loRtnTemp;

            try
            {
                loDbParameter = new GSM04500DBParameter();

                loDbParameter.CCOMPANY_ID = "RCD";
                loDbParameter.CUSER_ID = "hmc";
                loDbParameter.CPROPERTY_ID = "JBMPC";
                loDbParameter.CJRNGRP_TYPE = "10";

                var loCls = new GSM04500Cls();

                loRtnTemp = loCls.JOURNAL_GROUP_LIST_SERVICE(loDbParameter);
                loRtn = GET_JOURNAL_GROUP_LIST_SERVICE(loRtnTemp);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        private async IAsyncEnumerable<GSM04500DTO> GET_JOURNAL_GROUP_LIST_SERVICE(List<GSM04500DTO> poParameter)
        {
            foreach (var item in poParameter)
            {
                yield return item;
            }
        }
    }
}