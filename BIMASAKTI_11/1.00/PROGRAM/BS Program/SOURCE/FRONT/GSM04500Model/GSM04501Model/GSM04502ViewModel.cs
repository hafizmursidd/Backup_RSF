using GSM04500Common;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace GSM04501Model
{
    public class GSM04502ViewModel : R_ViewModel<GSM04510GOADeptDTO>
    {
        private GSM04502Model _model = new GSM04502Model();
        public ObservableCollection<GSM04510GOADeptDTO> GOADeptList = new ObservableCollection<GSM04510GOADeptDTO>();
        public GSM04510GOADeptDTO GOADept { get; set; } = new GSM04510GOADeptDTO();

        public async Task GetAllJournalGrupGOADeptAsync(string lcJournalGRPType, string lcPropertyId, string lcJournalGRPCode)
        {
            R_Exception loException = new R_Exception();
            try
            {
                var loResult = await _model.GetAllGOADeptAsync(lcJournalGRPType, lcPropertyId, lcJournalGRPCode);
                GOADeptList = new ObservableCollection<GSM04510GOADeptDTO>(loResult.ListData);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        }

        public async Task <GSM04510GOADeptDTO> GetGOADeptOneRecord (GSM04510GOADeptDTO poEntity)
        {
            var loEx = new R_Exception();
            GSM04510GOADeptDTO loResult = null;
            try
            {
                var loParam = new GSM04510GOADeptDTO
                {
                    CCOMPANY_ID = poEntity.CCOMPANY_ID,
                    CUSER_ID = poEntity.CUSER_ID,
                    CPROPERTY_ID = poEntity.CPROPERTY_ID,
                    CJRNGRP_TYPE = poEntity.CJRNGRP_TYPE,
                    CJRNGRP_CODE = poEntity.CJRNGRP_CODE,
                    CGOA_CODE = poEntity.CGOA_CODE,
                    CDEPT_CODE = poEntity.CDEPT_CODE
                };
                loResult = await _model.R_ServiceGetRecordAsync(loParam);
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
