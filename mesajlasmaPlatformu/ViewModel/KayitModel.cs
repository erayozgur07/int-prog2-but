using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

namespace mesajlasmaPlatformu.ViewModel
{
    public class KayitModel
    {
        public string kayitId { get; set; }
        public string kayitGrupId { get; set; }
        public string kayitKisiId { get; set; }
        public string kayitMesajId { get; set; }

        public KisilerModel kisiBilgi { get; set; }
        public MesajlarModel mesajBilgi { get; set; }

    }
}