using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using GLB00200Common;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;

namespace GLB00200Model.ViewModel
{
    public class GLB00200ViewModel : R_ViewModel<GLB00200DTO>
    {
        private GLB00200Model _modelGLB00200Model = new GLB00200Model();
        public ObservableCollection<GLB00200DTO> ReversingJournalProcessList =
            new ObservableCollection<GLB00200DTO>();

        public GLB00200MinMaxYearDTO lcGetPeriodYear = new GLB00200MinMaxYearDTO();
        public int PeriodYear = DateTime.Now.Year;
        public int PeriodMonth = DateTime.Now.Month;
        public string lcSearchText = "";

        public async Task GetMinMaxYear()
        {
            R_Exception loException = new R_Exception();
            try
            {
                var loResult = await _modelGLB00200Model.GetMinMaxYearAsyncModel();
                lcGetPeriodYear = loResult;
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

        }
        public async Task GetAllReversingJournalProcess()
        {
            R_Exception loException = new R_Exception();
            try
            {
                string lcPeriod = PeriodYear.ToString() + PeriodMonth.ToString("00");
                var loResult = await _modelGLB00200Model.GetReversingJournalProcessAsyncModel(lcPeriod, lcSearchText);
                ReversingJournalProcessList = new ObservableCollection<GLB00200DTO>(loResult.Data);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        }
    }
}
