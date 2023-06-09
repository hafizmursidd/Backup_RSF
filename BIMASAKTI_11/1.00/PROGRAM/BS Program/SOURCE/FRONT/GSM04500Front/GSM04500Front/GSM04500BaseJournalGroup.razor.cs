using GSM04500Common;
using GSM04500Model;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.MessageBox;
using R_BlazorFrontEnd.Exceptions;

namespace GSM04500Front
{
    public partial class GSM04500BaseJournalGroup
    {
        private GSM04500ViewModel journalGroupViewModel = new();
        private R_ConductorGrid _conJournalGroupRef;
        private R_Grid<GSM04500DTO> _gridRef;
        private R_Conductor _conductorRef;

        [Parameter] public string JournalGRPType { get; set; } = "10";
        [Parameter] public string PropertyId { get; set; }
        [Parameter] public string JournalGRPCode { get; set; }
        [Parameter] public string GOA_CODE { get; set; }
        [Parameter] public string GOA_Name { get; set; }

        public GSM04500DTO manipulate = new GSM04500DTO();

        //MAIN TAB
        private async Task ChangeTabMain(R_TabStripTab arg)
        {
            var loEx = new R_Exception();
            try
            {
                switch (arg.Title)
                {
                    case "Service":
                        JournalGRPType = "10";
                        break;
                    case "Utility":
                        JournalGRPType = "11";
                        break;
                    case "Deposit":
                        JournalGRPType = "12";
                        break;
                    case "Customer":
                        JournalGRPType = "20";
                        break;
                    case "Product":
                        JournalGRPType = "30";
                        break;
                    case "Expenditure":
                        JournalGRPType = "40";
                        break;
                    case "Supplier":
                        JournalGRPType = "50";
                        break;

                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        //CHILD TAB
        private async Task ChangeTabChild(R_TabStripTab arg)
        {
            var loEx = new R_Exception();
            try
            {

                //var a = JournalGRPType;
                //var B = JournalGRPCode;
                //var c = PropertyId;
                //lakukan manipulate disini
                //case "Deposit":
                //    JournalGRPType = "12";
                //    break;
                //case "Customer":
                //    JournalGRPType = "20";
                //    break;
                //case "Product":
                //    JournalGRPType = "30";
                //    break;
                //case "Expenditure":
                //    JournalGRPType = "40";
                //    break;
                //case "Supplier":
                //    JournalGRPType = "50";
                //    break;


            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

    }
}
