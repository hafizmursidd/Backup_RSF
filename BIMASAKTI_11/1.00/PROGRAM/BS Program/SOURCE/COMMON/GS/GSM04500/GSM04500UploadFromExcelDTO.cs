using System;
using System.Collections.Generic;
using System.Text;

namespace GSM04500Common
{
    public class GSM04500UploadFromExcelDTO
    {
        public int No { get; set; }
        public string JournalGroup { get; set; }
        public string JournalGroupName { get; set; }
        public bool EnableAccrual { get; set; }
    //    public string Notes { get; set; }
        public bool ValidFlag { get; set; }

    }

    public class GSM04500UploadToDBDTO
    {
        public string CJRNGRP_CODE { get; set; }
        public string CJRNGRP_NAME { get; set; }
        public bool LACCRUAL { get; set; }
        public string CNOTES { get; set; }
        public bool Var_Exists { get; set; }

    }
    public class GSM04500UploadErrorValidateDTO
    {
        public int No { get; set; }
        public string JournalGroup { get; set; }
        public string JournalGroupName { get; set; }
        public bool EnableAccrual { get; set; }
        public bool Var_Exists { get; set; } = false;
        public bool Var_Selected { get; set; } = true;
        public bool Var_Overwrite { get; set; } = false;
        public string ErrorMessage { get; set; }
    }
}
