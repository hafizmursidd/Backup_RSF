using GSM04500Common;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GSM04500Model
{
    public class GSM04500ViewModel : R_ViewModel<GSM04500DTO>
    {
        private GSM04500Model _model = new GSM04500Model();
        public List<GSM04500PropertyDTO> PropertyList { get; set; } = new List<GSM04500PropertyDTO>();
        public string PropertyValueContext = "";
        public ObservableCollection<GSM04500DTO> JournalGroupList = new ObservableCollection<GSM04500DTO>();
        public GSM04500DTO JournalGroup { get; set; } = new GSM04500DTO();


        public async Task GetPropertyList()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _model.GetPropertyListAsyncModel();
                PropertyList = loResult.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task GetAllJournalAsync(string lcGroupId)
        {
            R_Exception loException = new R_Exception();
            try
            {
                var loResult = await _model.GetAllJournalGroupListAsync(lcGroupId, PropertyValueContext);
                JournalGroupList = new ObservableCollection<GSM04500DTO>(loResult.ListData);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

        EndBlock:
            loException.ThrowExceptionIfErrors();
        }


        public async Task<GSM04500DTO> GetGroupJournaltOneRecord(GSM04500DTO poProperty)
        {
            var loEx = new R_Exception();
            GSM04500DTO loResult = null;

            try
            {
                var loParam = new GSM04500DTO
                {
                    CCOMPANY_ID = poProperty.CCOMPANY_ID,
                    CUSER_ID = poProperty.CUSER_ID,
                    CPROPERTY_ID = poProperty.CPROPERTY_ID,
                    CJRNGRP_TYPE = poProperty.CJRNGRP_TYPE,
                    CJRNGRP_CODE = poProperty.CJRNGRP_CODE
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

        public async Task DeleteOneRecordJournalGroup(GSM04500DTO poProperty)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = new GSM04500DTO
                {
                    CCOMPANY_ID = poProperty.CCOMPANY_ID,
                    CPROPERTY_ID = poProperty.CPROPERTY_ID,
                    CJRNGRP_TYPE = poProperty.CJRNGRP_TYPE,
                    CJRNGRP_CODE = poProperty.CJRNGRP_CODE,
                    CJRNGRP_NAME = poProperty.CJRNGRP_NAME,
                    LACCRUAL = poProperty.LACCRUAL
                };
                await _model.R_ServiceDeleteAsync(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        public async Task<GSM04500DTO> SaveJournalGroup(GSM04500DTO poNewEntity, R_eConductorMode peConductorMode)
        {
            var loEx = new R_Exception();
            GSM04500DTO loResult = null;

            try
            {
                poNewEntity.CPROPERTY_ID = PropertyValueContext;
                R_FrontContext.R_SetStreamingContext(ContextConstant.CJRNGRP_TYPE, poNewEntity.CJRNGRP_TYPE);
                loResult = await _model.R_ServiceSaveAsync(poNewEntity, (eCRUDMode)peConductorMode);
                JournalGroup = loResult;
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
