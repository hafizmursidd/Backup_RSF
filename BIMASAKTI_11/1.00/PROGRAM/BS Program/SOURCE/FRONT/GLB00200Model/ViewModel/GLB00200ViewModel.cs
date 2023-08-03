using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using GLB00200Common;
using R_APICommonDTO;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;

namespace GLB00200Model.ViewModel
{
    public class GLB00200ViewModel : R_ViewModel<GLB00200DTO>
    {
        private GLB00200Model _modelGLB00200Model = new GLB00200Model();

        //List for Front
        public ObservableCollection<GLB00200DTO> ReversingJournalProcessList =
            new ObservableCollection<GLB00200DTO>();
        public ObservableCollection<GLB00200JournalDetailDTO> DetailReversingJournalList =
            new ObservableCollection<GLB00200JournalDetailDTO>();

        //List for Process
        public List<GLB00200DTO> loProcessReversingList = new List<GLB00200DTO>();
        //List for Convert
        public List<int> NO_Convert = new List<int>();

        public GLB00200InitalProcessDTO loGetInitialProcess = new GLB00200InitalProcessDTO();
        public int PeriodYear = DateTime.Now.Year;
        public string PeriodMonth = DateTime.Now.Month.ToString("D2");
        public string lcSearchText = "";
        public List<GetMonthDTO> GetMonthList { get; set; }
        public GLB00200DTO CurrentReversingJournal = new GLB00200DTO();

        public string ResultProcessList = null;
        public string ResultFailedProcessList = null;
        public bool ButtonEnable = false;

        public string Message = "";
        public int Percentage = 0;
        public Action StateChangeAction { get; set; }
        public async Task GetInitialprocess()
        {
            R_Exception loException = new R_Exception();
            try
            {
                var loResult = await _modelGLB00200Model.GetInitialProcessAsyncModel();
                loGetInitialProcess = loResult;
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
                string lcPeriod = PeriodYear.ToString() + PeriodMonth;
                var loResult = await _modelGLB00200Model.GetReversingJournalProcessAsyncModel(lcPeriod, lcSearchText);
                ReversingJournalProcessList = new ObservableCollection<GLB00200DTO>(loResult.Data);
                
                if (ReversingJournalProcessList.Count>0)
                {
                    ButtonEnable=true;
                }
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        }


        public async Task GetDetail_ReversingJournal()
        {
            R_Exception loException = new R_Exception();
            try
            {
                // var abc = CurrentReversingJournal;
                var Result =
                    await _modelGLB00200Model.GetDetail_ReversingJournalAsyncModel(CurrentReversingJournal.CREC_ID);
                DetailReversingJournalList = new ObservableCollection<GLB00200JournalDetailDTO>(Result.Data);
                ConvertBigIntToInt();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        }

        public async Task ConvertBigIntToInt()
        {
            R_Exception loException = new R_Exception();
            try
            {
                foreach (var item in DetailReversingJournalList)
                {
                    int temp = Convert.ToInt32(item.INO);
                    item.NO_Convert = temp;
                }
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

        }

        public async Task ProcessReversingJournal()
        {
            R_Exception loException = new R_Exception();
            List<GLB00200DTO> loInboxApprovaltBatchListSelected = new List<GLB00200DTO>();
            try
            {
                foreach (var item in loProcessReversingList)
                {
                    if (item.LSELECTED == true)
                    {
                        loInboxApprovaltBatchListSelected.Add(item);
                    }
                }

                if (loInboxApprovaltBatchListSelected.Count == 0)
                {
                    loException.Add(new Exception("Please select Reversing Journal to process!"));
                    goto EndDEtail;
                }
                R_FrontContext.R_SetStreamingContext(ContextConstant.LIST_REVERSING, loInboxApprovaltBatchListSelected);

                var loResult = await _modelGLB00200Model.GetProcessReversingJournalAsyncModel();
                var  ResultProcessTemp = loResult.Data;

               GetResultProcess(ResultProcessTemp);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        EndDEtail:
            loException.ThrowExceptionIfErrors();
        }

        public void GetResultProcess(List<GLB00200ResultProcessReversing> loEntity)
        {
            foreach (var item in loEntity)
            {
                if (item.LSUCCESSED == true)
                {
                    ResultProcessList +=(item.CREF_NO+", ");
                }
                else
                {
                    ResultFailedProcessList+=(item.CREF_NO + ", ");
                }
                //_ = item.LSUCCESSED ? ResultSuccesTransaction = "( " +item.CREFERENCE_NO + " )" 
                //    : ResultFailedTransaction = "( " + item.CREFERENCE_NO + " )";


            }

        }

        #region ProgressBar

        public async Task ProcessComplete(string pcKeyGuid, eProcessResultMode poProcessResultMode)
        {
            if (poProcessResultMode == eProcessResultMode.Success)
            {
                Message = string.Format("Process Complete and success with GUID {0}", pcKeyGuid);

                //VisibleError = false;
                //
                // = true;

               // await ValidateDataList(StaffValidateUpload.ToList());
            }

            if (poProcessResultMode == eProcessResultMode.Fail)
            {
                Message = string.Format("Process Complete but fail with GUID {0}", pcKeyGuid);
               // await GetError(pcKeyGuid);
            }

            StateChangeAction();
        }

        public async Task ProcessError(string pcKeyGuid, R_APIException ex)
        {
            Message = string.Format("Process Error with GUID {0}", pcKeyGuid);

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
