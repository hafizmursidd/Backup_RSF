﻿using R_CommonFrontBackAPI;
using System.Collections.Generic;

namespace GSM06500Common
{
    public interface IGSM06500 : R_IServiceCRUDBase<GSM06500DTO>
    {
        IAsyncEnumerable<GSM06500DTO> TermOfPayment();
        GSM06500ListDTO GetallTermOfpaymentOriginal();
        GSM06500PropertyListDTO GetAllPropertyList();
    }
}
