using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorClientHelper;
using GSM04500Common;
using GSM04500Model.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.Enums;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.MessageBox;
using R_BlazorFrontEnd.Exceptions;

namespace GSM04500Front
{
    public partial class GSM04500Upload : R_Page
    {
        private R_eFileSelectAccept[] accepts = { R_eFileSelectAccept.Excel };
        private GSM04500ViewModel_Upload _viewModel = new();
        [Inject] IClientHelper clientHelper { get; set; }

        private R_Grid<GSM04500UploadErrorValidateDTO> JournalGroup_gridRef;

        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                var Param = (GSM004500ParamDTO)poParameter;
                _viewModel.CurrentObjectParam = Param;

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }
        private async Task _SourceUpload_OnChange(InputFileChangeEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                //import excel from user
                var loMS = new MemoryStream();
                await eventArgs.File.OpenReadStream().CopyToAsync(loMS);
                var loFileByte = loMS.ToArray();

                await JournalGroup_gridRef.R_RefreshGrid(loFileByte);
                if (_viewModel.VisibleError)
                {
                    await R_MessageBox.Show("", "Error Validate Data", R_eMessageBoxButtonType.OK);
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }

        private async Task Upload_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                _viewModel.fileByte = (byte[])eventArgs.Parameter;
                await _viewModel.AttachFile((byte[])eventArgs.Parameter, clientHelper.CompanyId, clientHelper.UserId);

                eventArgs.ListEntityResult = _viewModel.JournalGroupValidateUploadError;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);

        }
    }
}
