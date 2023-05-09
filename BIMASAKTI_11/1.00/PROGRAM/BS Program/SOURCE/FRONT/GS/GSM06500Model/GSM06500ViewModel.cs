using GSM06500Common;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace GSM06500Model
{
    public class GSM06500ViewModel : R_ViewModel<GSM06500DTO>
    {
        private GSM06500Model _model = new GSM06500Model();
        public ObservableCollection<GSM06500DTO> TermofPaymentList = new ObservableCollection<GSM06500DTO>();

        public async Task GetAllTermOfPayment(string pcCompanyId, string pcPropertyId, string pcUserLoginId)
        {
            R_Exception loException = new R_Exception();
            try
            {
                var loResult = await _model.GetTermOfPaymentListAsyncModel(pcCompanyId, pcPropertyId, pcUserLoginId);
                TermofPaymentList =new ObservableCollection<GSM06500DTO>(loResult.ListData);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

        EndBlock:
            loException.ThrowExceptionIfErrors();
        }

    }
}
