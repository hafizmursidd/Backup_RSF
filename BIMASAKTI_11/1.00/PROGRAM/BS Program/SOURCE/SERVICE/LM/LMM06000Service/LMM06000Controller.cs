using LMM06000Back;
using LMM06000Common;
using Microsoft.AspNetCore.Mvc;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;

namespace LMM06000Service
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LMM06000Controller : ControllerBase, ILMM06000
    {
        [HttpPost]
        public R_ServiceGetRecordResultDTO<LMM06000BillingRuleDTO> R_ServiceGetRecord(R_ServiceGetRecordParameterDTO<LMM06000BillingRuleDTO> poParameter)
        {
            var loEx = new R_Exception();
            var loRtn = new R_ServiceGetRecordResultDTO<LMM06000BillingRuleDTO>();

            try
            {
                var loCls = new LMM06000Cls();
                //poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                //poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;
                //poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                //poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;
                //poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;

                poParameter.Entity.CCOMPANY_ID = "RCD";
                poParameter.Entity.CUSER_ID = "hmc";
                poParameter.Entity.CPROPERTY_ID = "JBMPC";
                poParameter.Entity.CUNIT_TYPE_ID = "2BRoom";
                poParameter.Entity.CBILLING_RULE_CODE = "RULES01";

                loRtn.data = loCls.R_GetRecord(poParameter.Entity);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public R_ServiceSaveResultDTO<LMM06000BillingRuleDTO> R_ServiceSave(R_ServiceSaveParameterDTO<LMM06000BillingRuleDTO> poParameter)
        {
            R_Exception loException = new R_Exception();
            R_ServiceSaveResultDTO<LMM06000BillingRuleDTO> loRtn = null;
            LMM06000Cls loCls;

            try
            {
                loCls = new LMM06000Cls();
                loRtn = new R_ServiceSaveResultDTO<LMM06000BillingRuleDTO>();

                poParameter.Entity.CCOMPANY_ID = "RCD";
                poParameter.Entity.CPROPERTY_ID = "JBMPC";
                poParameter.Entity.CUNIT_TYPE_ID = "1BRoom";
                poParameter.Entity.CBILLING_RULE_CODE = "RULES03";
                poParameter.Entity.CBILLING_RULE_NAME = "Iuran Bulan Keetig";

                poParameter.Entity.LWITH_DP = false;
                poParameter.Entity.IDP_PERCENTAGE = 0;
                poParameter.Entity.IDP_INTERVAL = 0;
                poParameter.Entity.CDP_PERIOD_MODE = "";
                poParameter.Entity.LIINSTALLMENT = false;
                poParameter.Entity.IINSTALLMENT_PERCENTAGE = 0;
                poParameter.Entity.IINSTALLMENT_INTERVAL = 0;
                poParameter.Entity.CINSTALLMENT_PERIOD_MODE = "";
                poParameter.Entity.LBANK_CREDIT = false;
                poParameter.Entity.IBANK_CREDIT_PERCENTAGE = 0;
                poParameter.Entity.IBANK_CREDIT_INTERVAL = 0;

                poParameter.Entity.LACTIVE = true;
                poParameter.Entity.CUSER_ID = "hmc";

                loRtn.data = loCls.R_Save(poParameter.Entity, poParameter.CRUDMode);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            };
        EndBlock:
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }
        [HttpPost]
        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<LMM06000BillingRuleDTO> poParameter)
        {
            R_Exception loException = new R_Exception();
            R_ServiceDeleteResultDTO loRtn = null;
            LMM06000Cls loCls;
            LMM06000BillingRuleDTO loDbParameter;

            try
            {
                loCls = new LMM06000Cls();
                loRtn = new R_ServiceDeleteResultDTO();
                poParameter.Entity.CCOMPANY_ID = "RCD";
                poParameter.Entity.CPROPERTY_ID = "JBMPC";
                poParameter.Entity.CUNIT_TYPE_ID = "1BRoom";
                poParameter.Entity.CBILLING_RULE_CODE = "RULES01";
                poParameter.Entity.CBILLING_RULE_NAME = "Iuran Pertama";

                poParameter.Entity.LWITH_DP = false;
                poParameter.Entity.IDP_PERCENTAGE = 0;
                poParameter.Entity.IDP_INTERVAL = 0;
                poParameter.Entity.CDP_PERIOD_MODE = "";
                poParameter.Entity.LIINSTALLMENT = false;
                poParameter.Entity.IINSTALLMENT_PERCENTAGE = 0;
                poParameter.Entity.IINSTALLMENT_INTERVAL = 0;
                poParameter.Entity.CINSTALLMENT_PERIOD_MODE = "";
                poParameter.Entity.LBANK_CREDIT = false;
                poParameter.Entity.IBANK_CREDIT_PERCENTAGE = 0;
                poParameter.Entity.IBANK_CREDIT_INTERVAL = 0;

                poParameter.Entity.LACTIVE = true;
                poParameter.Entity.CUSER_ID = "hmc";

                loCls.R_Delete(poParameter.Entity);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            };
            EndBlock:
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public LMM06000PropertyListDTO GetAllPropertyList()
        {
            var loEx = new R_Exception();
            LMM06000PropertyListDTO loRtn = null;
            var loParameter = new LMM06000DBParameter();

            try
            {
                var loCls = new LMM06000Cls();
                loRtn = new LMM06000PropertyListDTO();

                loParameter.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loParameter.CUSER_ID = R_BackGlobalVar.USER_ID;

                //loParameter.CCOMPANY_ID = "RCD";
                //loParameter.CUSER_ID = "admin";

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
        public LMM06000UnitTypeListDTO GetAllUnitTypeList()
        {
            var loEx = new R_Exception();
            LMM06000UnitTypeListDTO loRtn = null;
            var loParameter = new LMM06000DBParameter();
            try
            {
                var loCls = new LMM06000Cls();
                loRtn = new LMM06000UnitTypeListDTO();

                //loParameter.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                //loParameter.CUSER_ID = R_BackGlobalVar.USER_ID;
                //loParameter.CPROPERTY_ID = R_BackGlobalVar.COMPANY_ID;
                //loParameter.CUNIT_TYPE_CATEGORY_ID = R_BackGlobalVar.USER_ID;

                loParameter.CCOMPANY_ID = "RCD";
                loParameter.CUSER_ID = "hmc";
                loParameter.CPROPERTY_ID = "JBMPC";
                loParameter.CUNIT_TYPE_CATEGORY_ID = "";

                var loResult = loCls.GetAllUnitTypeList(loParameter);
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
        public IAsyncEnumerable<LMM06000BillingRuleDTO> BillingRuleListStream()
        {
            var loEx = new R_Exception();
            LMM06000DBParameter loDbParameter;
            R_Exception loException = new R_Exception();
            IAsyncEnumerable<LMM06000BillingRuleDTO> loRtn = null;
            List<LMM06000BillingRuleDTO> loRtnTemp;

            try
            {
                loDbParameter = new LMM06000DBParameter();

                loDbParameter.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID; ;
                loDbParameter.CUSER_ID = R_BackGlobalVar.USER_ID;
                loDbParameter.CPROPERTY_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CPROPERTY_ID);
                loDbParameter.CUNIT_TYPE_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CUNIT_TYPE_ID);
                loDbParameter.LACTIVE_ONLY = R_Utility.R_GetStreamingContext<bool>(ContextConstant.LACTIVE_ONLY);

                //loDbParameter.CCOMPANY_ID = "RCD";
                //loDbParameter.CUSER_ID = "hmc";
                //loDbParameter.CPROPERTY_ID = "JBMPC";
                //loDbParameter.CUNIT_TYPE_ID = "1BRoom";
                //loDbParameter.LACTIVE_ONLY = false;

                var loCls = new LMM06000Cls();
                loRtnTemp = loCls.BillingRuleList(loDbParameter);
                loRtn = Get_BillingRuleList(loRtnTemp);

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;

        }
        private async IAsyncEnumerable<LMM06000BillingRuleDTO> Get_BillingRuleList(List<LMM06000BillingRuleDTO> poParameter)
        {
            foreach (var item in poParameter)
            {
                yield return item;
            }
        }

    }
}