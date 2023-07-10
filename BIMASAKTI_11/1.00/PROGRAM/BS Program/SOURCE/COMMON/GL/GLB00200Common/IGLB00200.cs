using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GLB00200Common
{
    public interface IGLB00200 : R_IServiceCRUDBase<GLB00200DTO>
    {
        GLB00200MinMaxYearDTO GetMinMaxYear();
        IAsyncEnumerable<GLB00200DTO> ReversingJournalProcessListStream();
        GLB00200JournalDetailListDTO DetailReversingJournalProcessList();
    }
}