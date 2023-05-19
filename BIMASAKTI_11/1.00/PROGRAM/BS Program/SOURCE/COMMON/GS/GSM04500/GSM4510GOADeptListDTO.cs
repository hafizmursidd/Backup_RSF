using R_APICommonDTO;
using System.Collections.Generic;

namespace GSM04500Common
{
    public class GSM4510GOADeptListDTO : R_APIResultBaseDTO
    {
        public List<GSM04510GOADeptDTO> ListData { get; set; }
    }
}
