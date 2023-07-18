using System;
using System.Collections.Generic;
using System.Text;

namespace GSM04500Common
{
    public class GSM04500UploadFromUserDTO
    {
        public string CJRNGRP_CODE { get; set; } 
        public string CJRNGRP_NAME { get; set; } 
        public bool LACCRUAL { get; set; } 
        public string  CNOTES { get; set; } 
        public bool Var_Exists { get; set; }
        public bool Var_Overwrite { get; set; }
    }

    public class GSM04500UploadFromUserRequestDTO
    {
        public string CJRNGRP_CODE { get; set; } 
        public string CJRNGRP_NAME { get; set; }
        public bool LACCRUAL { get; set; } 
        public string CNOTES { get; set; } 

    }

    public class GSM04500UploadErrorValidateDTO
    {
        public string CJRNGRP_CODE { get; set; }
        public string CJRNGRP_NAME { get; set; }
        public bool LACCRUAL { get; set; }
        public string CNOTES { get; set; }
        public bool Var_Exists { get; set; }
        public bool Var_Overwrite { get; set; }
    }
}
