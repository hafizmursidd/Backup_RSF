using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GSM04500Common;
using Microsoft.AspNetCore.Mvc;
using R_Common;

namespace GSM04500Service
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GSM04500UploadTemplateController : ControllerBase, IGSM04500UploadTemplate 
    {
        [HttpPost]
        public GSM04500UploadFileDTO DownloadTemplateFile()
        {
            var loEx = new R_Exception();
            var loRtn = new GSM04500UploadFileDTO();

            try
            {
                var loAsm = Assembly.GetExecutingAssembly();
                var lcResourceFile = "GSM04500SERVICE.File.JournalGroup.xlsx";
                using (Stream resFilestream = loAsm.GetManifestResourceStream(lcResourceFile))
                {
                    var ms = new MemoryStream();
                    resFilestream.CopyTo(ms);
                    var bytes = ms.ToArray();

                    loRtn.FileBytes = bytes;
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }
    }
}
