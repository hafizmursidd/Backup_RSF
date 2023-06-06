﻿using GSM04500Common;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using R_BlazorFrontEnd.Enums;

namespace GSM04501Model
{
    public class GSM04502ViewModel : R_ViewModel<GSM04510GOADeptDTO>
    {
        private GSM04502Model _modelGOADept = new GSM04502Model();
        public ObservableCollection<GSM04510GOADeptDTO> GOADeptList = new ObservableCollection<GSM04510GOADeptDTO>();
        public GSM04510GOADeptDTO GOADept { get; set; } = new GSM04510GOADeptDTO();
        public string GroupOfAccount { get; set; }
        
        
        public async Task GetGOAAllByDept(GSM04510GOADTO poEntity)
        {
            var loEx = new R_Exception();
            try
            {
                var loResult = await _modelGOADept.GetAllGOADeptAsync(poEntity);
                GroupOfAccount = poEntity.GROUPOFACCOUNT;
                GOADeptList = new ObservableCollection<GSM04510GOADeptDTO>(loResult.ListData);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
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
                loResult = await _modelGOADept.R_ServiceGetRecordAsync(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }
        
        public async Task<GSM04510GOADeptDTO> SaveGOADept(GSM04510GOADeptDTO poNewEntity, R_eConductorMode peConductorMode)
        {
            var loEx = new R_Exception();
            GSM04510GOADeptDTO loResult = null;

            try
            {
                R_FrontContext.R_SetStreamingContext(ContextConstant.CJRNGRP_TYPE, poNewEntity.CJRNGRP_TYPE);
                R_FrontContext.R_SetStreamingContext(ContextConstant.CPROPERTY_ID, poNewEntity.CPROPERTY_ID);
                R_FrontContext.R_SetStreamingContext(ContextConstant.CJOURNAL_GRP_CODE, poNewEntity.CJRNGRP_CODE);
                R_FrontContext.R_SetStreamingContext(ContextConstant.CGOA_CODE, poNewEntity.CGOA_CODE);


                loResult = await _modelGOADept.R_ServiceSaveAsync(poNewEntity, (eCRUDMode)peConductorMode);
                GOADept = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }
        public async Task DeleteOneRecordGOADept(GSM04510GOADeptDTO poProperty)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = new GSM04510GOADeptDTO()
                {
                    CCOMPANY_ID = poProperty.CCOMPANY_ID,
                    CPROPERTY_ID = poProperty.CPROPERTY_ID,
                    CJRNGRP_TYPE = poProperty.CJRNGRP_TYPE,
                    CJRNGRP_CODE = poProperty.CJRNGRP_CODE,
                    CGOA_CODE = poProperty.CGOA_CODE,
                    CDEPT_CODE = poProperty.CDEPT_CODE,
                    CGLACCOUNT_NO = poProperty.CGLACCOUNT_NO,
                    CUSER_ID = poProperty.CUSER_ID
                };
                await _modelGOADept.R_ServiceDeleteAsync(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

    }
}