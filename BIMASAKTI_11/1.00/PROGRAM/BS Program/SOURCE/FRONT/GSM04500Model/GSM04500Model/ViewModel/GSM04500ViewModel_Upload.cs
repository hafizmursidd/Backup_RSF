using GSM04500Common;
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
        public bool VisibleError = false;
        public byte[] fileByte = null;
        public bool BtnSave = true;
        public string SourceFileName = "";
        public bool IsOverWrite = false;
        public bool IsErrorEmptyFile = false;
        public GSM04500ModelUploadTemplate _modelUpload = new GSM04500ModelUploadTemplate();
        public ObservableCollection<GSM04500UploadErrorValidateDTO> JournalGroupValidateUploadError { get; set; } = new ObservableCollection<GSM04500UploadErrorValidateDTO>();
        public ObservableCollection<GSM04500DTO> DataJournalGroupList { get; set; } = new ObservableCollection<GSM04500DTO>();
        // public ObservableCollection<GSM04500UploadErrorValidateDTO> JournalGroupValidateUploadError { get; set; } = new ObservableCollection<LMM06501ErrorValidateDTO>();
        //public List<GSM04500UploadFromExcelDTO> loUploadJRNLGRPList = new List<GSM04500UploadFromExcelDTO>();
        public List<GSM04500UploadToDBDTO> loUploadLJournalGroupList = new List<GSM04500UploadToDBDTO>();
        
        #region ReadExcel

        public void ReadExcelFile()
        {
            var loEx = new R_Exception();
            List<GSM04500UploadFromExcelDTO> loExtract = new List<GSM04500UploadFromExcelDTO>();
            try
            {
                //Read From EXCEL
                var loExcel = new R_Excel();

                var loDataSet = loExcel.R_ReadFromExcel(fileByte, new string[] { "JournalGroup" });
                var loResult = R_FrontUtility.R_ConvertTo<GSM04500UploadFromExcelDTO>(loDataSet.Tables[0]);
                loExtract = new List<GSM04500UploadFromExcelDTO>(loResult);

                //Convert to DTO for DB
                loUploadLJournalGroupList = loExtract.Select(x => new GSM04500UploadToDBDTO()
                {
                    CJRNGRP_CODE = x.JournalGroup,
                    CJRNGRP_NAME = x.JournalGroupName,
                    LACCRUAL = x.EnableAccrual
                }).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                var MatchingError = loEx.ErrorList.FirstOrDefault(x => x.ErrDescp == "SkipNumberOfRowsStart was out of range: 0");
                IsErrorEmptyFile = MatchingError != null;
            }
            loEx.ThrowExceptionIfErrors();
        }

        #endregion

        #region Attach
        // public async Task AttachFile(byte[] poExcelByte, string pcCompanyId, string pcUserId)
        public async Task AttachFile()
        {
            var loEx = new R_Exception();
            R_UploadPar loUploadPar;
            R_ProcessAndUploadClient loCls;
            List<GSM04500UploadToDBDTO> Bigobject;
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
                loUploadPar = new R_UploadPar();
                loUploadPar.COMPANY_ID = CurrentObjectParam.CCOMPANY_ID;
                loUploadPar.USER_ID = CurrentObjectParam.CUSER_ID;
                loUploadPar.UserParameters = loUserParameters;
                loUploadPar.ClassName = "GSM04500Back.GSM04500UploadTemplateCls";
                loUploadPar.BigObject = loUploadLJournalGroupList;

                await loCls.R_AttachFile<List<GSM04500UploadToDBDTO>>(loUploadPar);

                 await ValidateDataList(loUploadLJournalGroupList);

                VisibleError = false;
                BtnSave = true;

            }
            catch (Exception ex)
            {
                loEx.Add(ex);

                var DeserializeList = JsonSerializer.Deserialize<List<GSM04500UploadErrorValidateDTO>>(loEx.ErrorList[0].ErrDescp);

                if (DeserializeList.Count > 0)
                    JournalGroupValidateUploadError = new ObservableCollection<GSM04500UploadErrorValidateDTO>(DeserializeList);

                BtnSave = false;
                VisibleError = true;
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
                    item.Var_Exists = loMasterData.Contains(item.CJRNGRP_CODE);
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
                var loData = loTempParam.Select(loTemp => new GSM04500UploadErrorValidateDTO()
                {
                    JournalGroup = loTemp.CJRNGRP_CODE,
                    JournalGroupName = loTemp.CJRNGRP_NAME,
                    EnableAccrual = loTemp.LACCRUAL,
                    ErrorMessage = loTemp.CNOTES,
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

                ListFromExcel = JournalGroupValidateUploadError.ToList<GSM04500UploadErrorValidateDTO>();

                //preapare Batch Parameter
                loBatchPar = new R_BatchParameter();

                loBatchPar.COMPANY_ID = pcCompanyId;
                loBatchPar.USER_ID = pcUserId;
                loBatchPar.UserParameters = loUserParameneters;
                loBatchPar.ClassName = "GSM04500Back.GSM04500UploadTemplateCls";
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


        #region Status
        public async Task ProcessComplete(string pcKeyGuid, eProcessResultMode poProcessResultMode)
        {
            if (poProcessResultMode == eProcessResultMode.Success)
            {
                Message = string.Format("Process Complete and success with GUID {0}", pcKeyGuid);
            }

            if (poProcessResultMode == eProcessResultMode.Fail)
            {
                Message = string.Format("Process Complete but fail with GUID {0}", pcKeyGuid);
            }

            await Task.CompletedTask;
        }

        public async Task ProcessError(string pcKeyGuid, R_APIException ex)
        {
            Message = string.Format("Process Error with GUID {0}", pcKeyGuid);
            await Task.CompletedTask;
        }

        public async Task ReportProgress(int pnProgress, string pcStatus)
        {
            Message = string.Format("Process Progress {0} with status {1}", pnProgress, pcStatus);

            Percentage = pnProgress;
            Message = string.Format("Process Progress {0} with status {1}", pnProgress, pcStatus);

            await Task.CompletedTask;
        }
        #endregion
    }
}
