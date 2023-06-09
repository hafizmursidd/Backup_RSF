using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMM06000Common;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;

namespace LMM06000Model.ViewModel
{
    public class LMM06000ViewModel : R_ViewModel<LMM06000BillingRuleDTO>
    {
        private LMM06000Model _model = new LMM06000Model();

        public ObservableCollection<LMM06000UnitTypeDTO> UnitTypeList = 
            new ObservableCollection<LMM06000UnitTypeDTO>();
        public ObservableCollection<LMM06000BillingRuleDTO> BillingRuleList = 
            new ObservableCollection<LMM06000BillingRuleDTO>();

        public List<LMM06000PropertyDTO> PropertyList { get; set; } = new List<LMM06000PropertyDTO>();
        public string PropertyValueContext = "";

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
                BillingRuleList = new ObservableCollection<LMM06000BillingRuleDTO>(loResult.Data);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        }
    }
}
