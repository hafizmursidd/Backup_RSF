using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GLB00200Common;
using GLB00200Model.ViewModel;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;

namespace GLB00200Front
{
    public partial class GLB00200
    {
        private GLB00200ViewModel _viewModelGLB00200 = new();
        private R_Conductor _conductorPeriod;

        private R_Grid<GLB00200DTO> _gridReversing;
        private R_ConductorGrid _conductorReversingJournal;

            //  public GLB00200DTO CurrentReversingJournal = new GLB00200DTO();



        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();
            try
            {
                await ServiceGetMinMaxYear();
                await GetMonth();
                await _gridReversing.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        private async Task ServiceGetMinMaxYear()
        {
            var loEx = new R_Exception();

            try
            {
                await _viewModelGLB00200.GetMinMaxYear();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }
        public async Task GetMonth()
        {
            _viewModelGLB00200.GetMonthList = new List<GetMonthDTO>();

            for (int i = 1; i <= 12; i++)
            {
                string monthId = i.ToString("D2");
                GetMonthDTO month = new GetMonthDTO { Id = monthId };
                _viewModelGLB00200.GetMonthList.Add(month);
            }

        }
        private async Task OnChangedMonth(object param)
        {
            _viewModelGLB00200.PeriodMonth = (string)param;
        }
        private async Task GetList_ReversingJournal(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {

                await _viewModelGLB00200.GetAllReversingJournalProcess();
                eventArgs.ListEntityResult = _viewModelGLB00200.ReversingJournalProcessList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        private async Task GetList_ReversingJournalWithCustomPeriod()
        {
            var loEx = new R_Exception();
            try
            {

                await _viewModelGLB00200.GetAllReversingJournalProcess();
                //await _gridReversing.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        private async Task GetList_ReversingJournalWithSearch()
        {
            var loEx = new R_Exception();
            try
            {
                if (string.IsNullOrEmpty(_viewModelGLB00200.lcSearchText))
                {
                    loEx.Add(new Exception("Please input keyword to search!"));
                    goto EndBlock;
                }
                if (!string.IsNullOrEmpty(_viewModelGLB00200.lcSearchText)
                    && _viewModelGLB00200.lcSearchText.Length < 3)
                {
                    loEx.Add(new Exception("Minimum search keyword is 3 characters!"));
                    goto EndBlock;
                }
                await _viewModelGLB00200.GetAllReversingJournalProcess();

                if (_viewModelGLB00200.ReversingJournalProcessList.Count() < 1)
                {
                    loEx.Add(new Exception("Data Not Found!"));
                    goto EndBlock;
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
        EndBlock:
            loEx.ThrowExceptionIfErrors();
        }

        private async Task Service_Display(R_DisplayEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                if (eventArgs.ConductorMode == R_eConductorMode.Normal)
                {
                    _viewModelGLB00200.CurrentReversingJournal = (GLB00200DTO)eventArgs.Data;
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        private async Task ButtonDetail_GetList_ReversingJournal(R_BeforeOpenPopupEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                eventArgs.TargetPageType = typeof(GLB00200Detail);
                eventArgs.Parameter = _viewModelGLB00200.CurrentReversingJournal;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}
