using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CPOEORdIten.ModelEn
{
   public class LastOrdItmModel
    {
        public String Epi { get; set; }
        public String OEORI_RowId { get; set; }
        public DateTime? OEORI_SttDat { get; set; }
        public String OEORI_SttTim { get; set; }

        public String OEORI_SttDatString
        {
            get
            {
                if (OEORI_SttDat != null)
                {
                    return OEORI_SttDat.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                return "";
            }
        }
    }
}
