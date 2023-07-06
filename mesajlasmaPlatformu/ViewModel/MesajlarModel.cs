using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mesajlasmaPlatformu.ViewModel
{
    public class MesajlarModel
    {
        public string mesajId { get; set; }
        public string mesajText { get; set; }
        public string kimdenId { get; set; }
        public string kimeId { get; set; }
        public string grupId { get; set; }
        public int bulkMesaj { get; set; }
        public string kisiAdSoyad { get; internal set; }
    }
}