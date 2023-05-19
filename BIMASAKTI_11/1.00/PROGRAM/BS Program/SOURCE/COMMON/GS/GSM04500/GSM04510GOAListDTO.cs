using R_APICommonDTO;
using System.Collections.Generic;

namespace GSM04500Common
{
    public class GSM04510GOAListDTO : R_APIResultBaseDTO
    {
        public List<GSM04510GOADTO> ListData { get; set; }
    }
}
