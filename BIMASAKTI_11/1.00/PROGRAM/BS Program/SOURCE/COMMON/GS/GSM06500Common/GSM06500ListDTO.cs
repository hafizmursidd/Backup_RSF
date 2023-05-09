using R_APICommonDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace GSM06500Common
{
    public class GSM06500ListDTO : R_APIResultBaseDTO
    {
        public List<GSM06500DTO> ListData { get; set; }
    }

}
