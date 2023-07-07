using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMM06000Common;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;

namespace LMM06000Model.ViewModel
{
    public class LMM06000ViewModel : R_ViewModel<LMM06000BillingRuleDTO>
    {
        private LMM06000Model _model = new LMM06000Model();

        public ObservableCollection<LMM06000UnitTypeDTO> UnitTypeList = 
            new ObservableCollection<LMM06000UnitTypeDTO>();
        public ObservableCollection<LMM06000BillingRuleDTO> BillingRuleList = 
            new ObservableCollection<LMM06000BillingRuleDTO>();

        public LMM06000BillingRuleDTO BillingRuleDetail = new LMM06000BillingRuleDTO();
        public List<LMM06000PropertyDTO> PropertyList { get; set; } = new List<LMM06000PropertyDTO>();
        public string PropertyValueContext = "";
        public string UnitTypeValueContext = "";
        public List<LMM06000PeriodDTO> PeriodList {get; set;} = new List<LMM06000PeriodDTO>();

        public LMM06000ActiveInactiveDTO ActiveInactiveEntity = new LMM06000ActiveInactiveDTO();

        public async Task GetPropertyList()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _model.GetPropertyAsyncModel();
                PropertyList = loResult.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task GetAllUnitType()
        {
            R_Exception loException = new R_Exception();
            try
            {
                var loResult = await _model.GetUnitTypeAsyncModel(PropertyValueContext);
                UnitTypeList = new ObservableCollection<LMM06000UnitTypeDTO>(loResult.Data);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        }

        public async Task GetAllBillingRule(string pcProperty, string pcUnitType)
        {
            R_Exception loException = new R_Exception();
            try
            {
                var loResult = await _model.GetBillingRuleListAsyncModel(pcProperty, pcUnitType);
                var varTempt = loResult.Data;

                UnitTypeValueContext = pcUnitType;
                BillingRuleList = new ObservableCollection<LMM06000BillingRuleDTO>(loResult.Data);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        }

        public async Task<LMM06000BillingRuleDTO> GetBillingRuleOneRecord(LMM06000BillingRuleDTO poProperty)
        {
            var loEx = new R_Exception();
            LMM06000BillingRuleDTO loResult = null;
            try
            {
                var loParam = new LMM06000BillingRuleDTO
                {
                    CCOMPANY_ID = poProperty.CCOMPANY_ID,
                    CUSER_ID = poProperty.CUSER_ID,
                    CPROPERTY_ID = poProperty.CPROPERTY_ID,
                    CUNIT_TYPE_ID = poProperty.CUNIT_TYPE_ID,
                    CBILLING_RULE_CODE = poProperty.CBILLING_RULE_CODE
                };
                loResult = await _model.R_ServiceGetRecordAsync(loParam);
                BillingRuleDetail = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }
        public async Task GetPeriodList()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _model.GetPeriodAsyncModel();
                PeriodList = loResult.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task SaveUnitType_BillingRule(LMM06000BillingRuleDTO poEntity, eCRUDMode peCRUDMode)
        {
            var loEx = new R_Exception();
            try
            {
                var loResult = await _model.R_ServiceSaveAsync(poEntity, peCRUDMode);
                BillingRuleDetail = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        public async Task DeleteUnitType_BillingRule (LMM06000BillingRuleDTO poEntity)
        {
            var loEx = new R_Exception();
            try
            {
                var loParam = new LMM06000BillingRuleDTO
                {
                    CCOMPANY_ID = poEntity.CCOMPANY_ID,
                    CPROPERTY_ID = poEntity.CPROPERTY_ID,
                    CUNIT_TYPE_ID = poEntity.CUNIT_TYPE_ID,
                    CBILLING_RULE_CODE = poEntity.CBILLING_RULE_CODE,
                    CBILLING_RULE_NAME = poEntity.CBILLING_RULE_NAME,

                    LBOOKING_FEE = poEntity.LBOOKING_FEE,
                    CBOOKING_FEE_CHARGE_ID = poEntity.CBOOKING_FEE_CHARGE_ID,
                   
                    LWITH_DP = poEntity.LWITH_DP,
                    IDP_PERCENTAGE = poEntity.IDP_PERCENTAGE,
                    IDP_INTERVAL = poEntity.IDP_INTERVAL,
                    CDP_PERIOD_MODE = poEntity.CDP_PERIOD_MODE,
                    CDP_CHARGE_ID = poEntity.CDP_CHARGE_ID,

                    LINSTALLMENT = poEntity.LINSTALLMENT,
                    IINSTALLMENT_PERCENTAGE = poEntity.IINSTALLMENT_PERCENTAGE,
                    IINSTALLMENT_INTERVAL = poEntity.IINSTALLMENT_INTERVAL,
                    CINSTALLMENT_PERIOD_MODE = poEntity.CINSTALLMENT_PERIOD_MODE,
                    CINSTALLMENT_CHARGE_ID = poEntity.CINSTALLMENT_CHARGE_ID,

                    LBANK_CREDIT = poEntity.LBANK_CREDIT,
                    IBANK_CREDIT_PERCENTAGE = poEntity.IBANK_CREDIT_PERCENTAGE,
                    IBANK_CREDIT_INTERVAL = poEntity.IBANK_CREDIT_INTERVAL,

                    LACTIVE = poEntity.LACTIVE,
                    CUSER_ID = poEntity.CUSER_ID
                };
                await _model.R_ServiceDeleteAsync(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        public async Task ActiveInactiveProcessAsync()
        {
            R_Exception loException = new R_Exception();

            try
            {
                R_FrontContext.R_SetContext(ContextConstant.CPROPERTY_ID, ActiveInactiveEntity.PROPERTY_ID);
                R_FrontContext.R_SetContext(ContextConstant.CUNIT_TYPE_ID, ActiveInactiveEntity.CUNIT_TYPE_ID);
                R_FrontContext.R_SetContext(ContextConstant.CBILLING_RULE_CODE, ActiveInactiveEntity.CBILLING_RULE_CODE);
                R_FrontContext.R_SetContext(ContextConstant.LACTIVE, ActiveInactiveEntity.LACTIVE);

                await _model.SetActiveInactiveAsync();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();
        }
    }
}
