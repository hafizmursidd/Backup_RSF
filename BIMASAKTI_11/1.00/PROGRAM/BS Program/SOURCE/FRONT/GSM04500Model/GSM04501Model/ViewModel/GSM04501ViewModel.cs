using GSM04500Common;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace GSM04501Model
{
    public class GSM04501ViewModel : R_ViewModel<GSM04510GOADTO>
    {
        private GSM04501Model _model = new GSM04501Model();
        public ObservableCollection<GSM04510GOADTO> GOAList = new ObservableCollection<GSM04510GOADTO>();
        public GSM04510GOADTO GOA { get; set; } = new GSM04510GOADTO();
        public string journalGroup { get; set; }
        public async Task GetAllJournalGrupGOAAsync(string lcJournalGRPType, string lcPropertyId, string lcJournalGRPCode)
        {
            R_Exception loException = new R_Exception();
            try
            {
                var loResult = await _model.GetAllGOAListAsync(lcJournalGRPType, lcPropertyId, lcJournalGRPCode);
                journalGroup = GOA.CJRNGRP_CODE;
                GOAList = new ObservableCollection<GSM04510GOADTO>(loResult.ListData);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        }

        public async Task<GSM04510GOADTO> GetGOAOneRecord(GSM04510GOADTO poEntity)
        {
            var loEx = new R_Exception();
            GSM04510GOADTO loResult = null;
            try
            {
                var loParam = new GSM04510GOADTO
                {
                    CCOMPANY_ID = poEntity.CCOMPANY_ID,
                    CUSER_ID = poEntity.CUSER_ID,
                    CPROPERTY_ID = poEntity.CPROPERTY_ID,
                    CJRNGRP_TYPE = poEntity.CJRNGRP_TYPE,
                    CJRNGRP_CODE = poEntity.CJRNGRP_CODE,
                    CGOA_CODE = poEntity.CGOA_CODE
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

        public async Task<GSM04510GOADTO> SaveGOA(GSM04510GOADTO poNewEntity, R_eConductorMode peConductorMode)
        {
            var loEx = new R_Exception();
            GSM04510GOADTO loResult = null;

            try
            {
                R_FrontContext.R_SetStreamingContext(ContextConstant.CJRNGRP_TYPE, poNewEntity.CJRNGRP_TYPE);
                R_FrontContext.R_SetStreamingContext(ContextConstant.CPROPERTY_ID, poNewEntity.CPROPERTY_ID);
                R_FrontContext.R_SetStreamingContext(ContextConstant.CJOURNAL_GRP_CODE, poNewEntity.CJRNGRP_CODE);

                loResult = await _model.R_ServiceSaveAsync(poNewEntity, (eCRUDMode)peConductorMode);
                GOA = loResult;
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
