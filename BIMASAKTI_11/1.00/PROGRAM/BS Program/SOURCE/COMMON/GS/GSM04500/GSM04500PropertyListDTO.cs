using R_APICommonDTO;
using System.Collections.Generic;

namespace GSM04500Common
{
    public class GSM04500PropertyListDTO : R_APIResultBaseDTO
    {
        public List<GSM04500PropertyDTO> Data { get; set; }
    }

}
