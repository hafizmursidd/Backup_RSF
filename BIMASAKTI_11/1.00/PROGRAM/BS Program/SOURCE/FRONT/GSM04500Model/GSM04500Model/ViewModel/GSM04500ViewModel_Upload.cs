﻿using GSM04500Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd;
using System.Collections.ObjectModel;
using R_CommonFrontBackAPI;
using R_BlazorFrontEnd.Helpers;
using R_BlazorFrontEnd.Excel;
using R_ProcessAndUploadFront;
using R_APICommonDTO;
using System.Linq;
using System.IO;
using System.Globalization;
using System.Text.Json;

namespace GSM04500Model.ViewModel
{
    public class GSM04500ViewModel_Upload : R_IProcessProgressStatus
    {

        public GSM004500ParamDTO CurrentObjectParam = new GSM004500ParamDTO();
        public string Message = "";
        public int Percentage = 0;
        public bool BtnSave = true;
        public string SourceFileName = "";
        public bool IsOverWrite = false;
        public bool IsErrorEmptyFile = false;
        public GSM04500ModelUploadTemplate _modelUpload = new GSM04500ModelUploadTemplate();
        public ObservableCollection<GSM04500UploadErrorValidateDTO> JournalGroupValidateUploadError { get; set; } = new ObservableCollection<GSM04500UploadErrorValidateDTO>();
        public ObservableCollection<GSM04500DTO> DataJournalGroupList { get; set; } = new ObservableCollection<GSM04500DTO>();
        public List<GSM04500UploadToDBDTO> loUploadLJournalGroupList = new List<GSM04500UploadToDBDTO>();
      
        
        public Action StateChangeAction { get; set; }



        #region Attach
        public async Task ValidateFile()
        {
            var loEx = new R_Exception();
            R_BatchParameter loUploadPar;
            R_ProcessAndUploadClient loCls;
            List<R_KeyValue> loUserParameters;

            try
            {
                loUserParameters = new List<R_KeyValue>();
                loUserParameters.Add(new R_KeyValue() { Key = ContextConstant.CPROPERTY_ID, Value = CurrentObjectParam.CPROPERTY_ID });
                loUserParameters.Add(new R_KeyValue() { Key = ContextConstant.CJRNGRP_TYPE, Value = CurrentObjectParam.CJRNGRP_TYPE });
                loUserParameters.Add(new R_KeyValue() { Key = ContextConstant.COVERWRITE, Value = IsOverWrite });

                //Instantiate ProcessClient
                loCls = new R_ProcessAndUploadClient(
                    pcModuleName: "GS",
                    plSendWithContext: true,
                    plSendWithToken: true,
                    pcHttpClientName: "R_DefaultServiceUrl",
                    poProcessProgressStatus: this);

                //prepare Batch Parameter
                loUploadPar = new R_BatchParameter();
                loUploadPar.COMPANY_ID = CurrentObjectParam.CCOMPANY_ID;
                loUploadPar.USER_ID = CurrentObjectParam.CUSER_ID;
                loUploadPar.UserParameters = loUserParameters;
                loUploadPar.ClassName = "GSM04500Back.GSM04500ValidateUploadTemplateCls";
                loUploadPar.BigObject = loUploadLJournalGroupList;

                await loCls.R_BatchProcess<List<GSM04500UploadToDBDTO>>(loUploadPar,2);

                await ValidateDataList(loUploadLJournalGroupList);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
        }
        public async Task ValidateDataList(List<GSM04500UploadToDBDTO> poEntity)
        {
            var loEx = new R_Exception();

            try
            {
                await GetJournalGroupList();

                //cek apakah sudah ada di database
                var loMasterData = DataJournalGroupList.Where(x => x.CPROPERTY_ID == CurrentObjectParam.CPROPERTY_ID).Select(x => x.CJRNGRP_CODE).ToList();

                var loData = poEntity.Select(item =>
                {
                    item.Var_Exists = loMasterData.Contains(item.JournalGroup);
                    return item;
                }).ToList();

                await ConvertGrid(loData);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

        }
        public async Task GetJournalGroupList()
        {
            var loEx = new R_Exception();

            try
            {
                R_FrontContext.R_SetContext(ContextConstant.CPROPERTY_ID, CurrentObjectParam.CPROPERTY_ID);
                R_FrontContext.R_SetContext(ContextConstant.CJRNGRP_TYPE, CurrentObjectParam.CJRNGRP_TYPE);

                var loResult = await _modelUpload.GetJournalGroupUploadListAsync();

                DataJournalGroupList = new ObservableCollection<GSM04500DTO>(loResult.ListData);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        public async Task ConvertGrid(List<GSM04500UploadToDBDTO> poEntity)
        {
            var loEx = new R_Exception();

            try
            {
                var loTempParam = poEntity;
                //get data from excel
                var loData = loTempParam.Select(loTemp => new GSM04500UploadErrorValidateDTO()
                {
                    JournalGroup = loTemp.JournalGroup,
                    JournalGroupName = loTemp.JournalGroupName,
                    EnableAccrual = loTemp.EnableAccrual,
                    Var_Exists = loTemp.Var_Exists,
                    ErrorMessage = loTemp.CNotes,
                }).ToList();

                JournalGroupValidateUploadError = new ObservableCollection<GSM04500UploadErrorValidateDTO>(loData);

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        private async Task GetError(string pcKeyGuid)
        {
            R_APIException loException;


            try
            {
                var loError = await _modelUpload.GetErrorProcessAsync(pcKeyGuid);

                foreach (var item in JournalGroupValidateUploadError)
                {
                   item.ErrorMessage = loError.Where(x => x.JournalGroup == item.JournalGroup).Select(x => x.ErrorMessage).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task SaveFileBulkFile(string pcCompanyId, string pcUserId)
        {
            var loEx = new R_Exception();
            R_BatchParameter loBatchPar;
            R_ProcessAndUploadClient loCls;
            List<GSM04500UploadErrorValidateDTO> ListFromExcel;
            List<R_KeyValue> loUserParameneters;

            try
            {
                // set Param
                loUserParameneters = new List<R_KeyValue>();
                loUserParameneters.Add(new R_KeyValue() { Key = ContextConstant.CPROPERTY_ID, Value = CurrentObjectParam.CPROPERTY_ID });
                loUserParameneters.Add(new R_KeyValue() { Key = ContextConstant.CJRNGRP_TYPE, Value = CurrentObjectParam.CJRNGRP_TYPE });
                loUserParameneters.Add(new R_KeyValue() { Key = ContextConstant.COVERWRITE, Value = IsOverWrite });

                //Instantiate ProcessClient
                loCls = new R_ProcessAndUploadClient(
                    pcModuleName: "GS",
                    plSendWithContext: true,
                    plSendWithToken: true,
                    pcHttpClientName: "R_DefaultServiceUrl",
                    poProcessProgressStatus: this);

                //Set Data
                if (JournalGroupValidateUploadError.Count == 0)
                    return;

                ListFromExcel = JournalGroupValidateUploadError.ToList();

                //preapare Batch Parameter
                loBatchPar = new R_BatchParameter();

                loBatchPar.COMPANY_ID = pcCompanyId;
                loBatchPar.USER_ID = pcUserId;
                loBatchPar.UserParameters = loUserParameneters;
                loBatchPar.ClassName = "GSM04500Back.GSM04500UploadJournalGroupCls";
                loBatchPar.BigObject = ListFromExcel;
                await loCls.R_BatchProcess<List<GSM04500UploadErrorValidateDTO>>(loBatchPar, ListFromExcel.Count);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        #endregion


        #region Progress Bar

        public async Task ProcessComplete(string pcKeyGuid, eProcessResultMode poProcessResultMode)
        {
            if (poProcessResultMode == eProcessResultMode.Success)
            {
                Message = string.Format("Process Complete and success with GUID {0}", pcKeyGuid);
                //VisibleError = false;
                //BtnSave = true;

                await ValidateDataList(loUploadLJournalGroupList.ToList());
            }

            if (poProcessResultMode == eProcessResultMode.Fail)
            {
                BtnSave = false;
                Message = string.Format("Process Complete but fail with GUID {0}", pcKeyGuid);
                await GetError(pcKeyGuid);
            }

            await Task.CompletedTask;
        }

        public async Task ProcessError(string pcKeyGuid, R_APIException ex)
        {
            foreach (R_APICommonDTO.R_Error item in ex.ErrorList)
            {
                Message = string.Format($"{item.ErrDescp}");
            }

            StateChangeAction();

            await Task.CompletedTask;
        }

        public async Task ReportProgress(int pnProgress, string pcStatus)
        {
            Message = string.Format("Process Progress {0} with status {1}", pnProgress, pcStatus);

            Percentage = pnProgress;
            Message = string.Format("Process Progress {0} with status {1}", pnProgress, pcStatus);

            StateChangeAction();

            await Task.CompletedTask;
        }
        #endregion
    }
}
