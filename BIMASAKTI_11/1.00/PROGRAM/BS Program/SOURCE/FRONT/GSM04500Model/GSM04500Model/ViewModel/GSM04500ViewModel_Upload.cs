using R_ProcessAndUploadFront;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GSM04500Common;
using R_APICommonDTO;
using R_CommonFrontBackAPI;
using R_BlazorFrontEnd.Exceptions;
using System.Linq;
using R_BlazorFrontEnd.Excel;
using R_BlazorFrontEnd.Helpers;
using System.Collections.ObjectModel;
using System.Text.Json;
using R_BlazorFrontEnd;

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

        public GSM04500ModelUploadTemplate _modelUpload = new GSM04500ModelUploadTemplate();
        public ObservableCollection<GSM04500UploadErrorValidateDTO> JournalGroupValidateUploadError { get; set; } = new ObservableCollection<GSM04500UploadErrorValidateDTO>();
        public ObservableCollection<GSM04500DTO> DataJournalGroupList { get; set; } = new ObservableCollection<GSM04500DTO>();
       // public ObservableCollection<GSM04500UploadErrorValidateDTO> JournalGroupValidateUploadError { get; set; } = new ObservableCollection<LMM06501ErrorValidateDTO>();




        #region MyRegion

        public async Task AttachFile(byte[] poExcelByte, string pcCompanyId, string pcUserId)
        {
            var loEx = new R_Exception();
            R_UploadPar loUploadPar;
            R_ProcessAndUploadClient loCls;
            List<GSM04500UploadFromUserDTO> Bigobject;
            List<R_KeyValue> loUserParameters;
            try
            {
                loUserParameters = new List<R_KeyValue>();
                loUserParameters.Add(new R_KeyValue() { Key = ContextConstant.CPROPERTY_ID, Value = CurrentObjectParam.CPROPERTY_ID });
                loUserParameters.Add(new R_KeyValue() { Key = ContextConstant.CJRNGRP_TYPE, Value = CurrentObjectParam.CJRNGRP_TYPE });

                //Instantiate ProcessClient
                loCls = new R_ProcessAndUploadClient(
                    pcModuleName: "GS",
                    plSendWithContext: true,
                    plSendWithToken: true,
                    pcHttpClientName: "R_DefaultServiceUrl",
                    poProcessProgressStatus: this);

                //add filebyte to DTO
                var loExcel = new R_Excel();

                var loDataSet = loExcel.R_ReadFromExcel(poExcelByte);

                var loResult = R_FrontUtility.R_ConvertTo<GSM04500UploadFromUserDTO>(loDataSet.Tables[0]);

                Bigobject = new List<GSM04500UploadFromUserDTO>(loResult);

                //prepare Batch Parameter
                loUploadPar = new R_UploadPar();
                loUploadPar.COMPANY_ID = CurrentObjectParam.CCOMPANY_ID;
                loUploadPar.USER_ID = CurrentObjectParam.CUSER_ID;
                loUploadPar.UserParameters = loUserParameters;
                loUploadPar.ClassName = "GSM04500Back.GSM04500UploadTemplateCls";
                loUploadPar.BigObject = Bigobject;

                await loCls.R_AttachFile<List<GSM04500UploadFromUserDTO>>(loUploadPar);

                await ValidateDataList(Bigobject);

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

        public async Task ValidateDataList(List<GSM04500UploadFromUserDTO> poEntity)
        {
            var loEx = new R_Exception();

            try
            {
                await GetJournalGroupfList();

                //var loMasterData = StaffGrid.Where(x => x.CPROPERTY_ID == PropertyValue).Select(x => x.CSTAFF_ID).ToList();

                //var loData = poEntity.Select(item =>
                //{
                //    item.Var_Exists = loMasterData.Contains(item.StaffId);
                //    return item;
                //}).ToList();

                //await ConvertGrid(loData);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

        }
        public async Task GetJournalGroupfList()
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
