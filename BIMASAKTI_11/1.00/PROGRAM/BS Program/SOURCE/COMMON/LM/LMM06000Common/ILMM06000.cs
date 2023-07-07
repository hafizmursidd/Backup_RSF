using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMM06000Common
{
    public interface ILMM06000 : R_IServiceCRUDBase<LMM06000BillingRuleDTO>
    {
        IAsyncEnumerable<LMM06000BillingRuleDTO> BillingRuleListStream();
        LMM06000PropertyListDTO GetAllPropertyList();
        LMM06000UnitTypeListDTO GetAllUnitTypeList();
        LMM06000PeriodListDTO GetAllPeriodList();
        LMM06000ActiveInactiveDTO SetActiveInactive();
    }
}
