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
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;

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

                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;
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
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;
                
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
        public LMM06000PeriodListDTO GetAllPeriodList()
        {
            var loEx = new R_Exception();
            LMM06000PeriodListDTO loRtn = null;

            var loParameter = new LMM06000DBParameter();

            try
            {
                var loCls = new LMM06000Cls();
                loRtn = new LMM06000PeriodListDTO();
                loParameter.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loParameter.CULTURE = R_BackGlobalVar.CULTURE;

                var loResult = loCls.GetAllPeriodList(loParameter);
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

                loParameter.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loParameter.CUSER_ID = R_BackGlobalVar.USER_ID;
                loParameter.CPROPERTY_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CPROPERTY_ID);
                loParameter.CUNIT_TYPE_CATEGORY_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CUNIT_TYPE_ID);
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
        public LMM06000ActiveInactiveDTO SetActiveInactive()
        {
            R_Exception loEx = new R_Exception();
            LMM06000ActiveInactiveParameterDb loDbPar = new LMM06000ActiveInactiveParameterDb();
            LMM06000ActiveInactiveDTO loRtn = new LMM06000ActiveInactiveDTO();
            LMM06000Cls loCls = new LMM06000Cls();

            try
            {
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CUSER_ID = R_BackGlobalVar.USER_ID;
                loDbPar.CPROPERTY_ID = R_Utility.R_GetContext<string>(ContextConstant.CPROPERTY_ID);
                loDbPar.CUNIT_TYPE_ID = R_Utility.R_GetContext<string>(ContextConstant.CUNIT_TYPE_ID);
                loDbPar.CBILLING_RULE_CODE = R_Utility.R_GetContext<string>(ContextConstant.CBILLING_RULE_CODE);
                loDbPar.LACTIVE = R_Utility.R_GetContext<bool>(ContextConstant.LACTIVE);

                loCls.SetActiveInactiveDb(loDbPar);
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
            IAsyncEnumerable<LMM06000BillingRuleDTO> loRtn = null;
            List<LMM06000BillingRuleDTO> loRtnTemp;

            try
            {
                loDbParameter = new LMM06000DBParameter();

                loDbParameter.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID; ;
                loDbParameter.CUSER_ID = R_BackGlobalVar.USER_ID;
                loDbParameter.CPROPERTY_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CPROPERTY_ID);
                loDbParameter.CUNIT_TYPE_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CUNIT_TYPE_ID);
                loDbParameter.LACTIVE_ONLY = false; //sesuai pernyataan dari bu Reni

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