﻿using ProtoBuf;

namespace RRExpress.Service.Entity {

    [ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields, EnumPassthru = true)]
    public class Ad {

        public int ID { get; set; }

        public string Img { get; set; }

        public string Title { get; set; }

        public string Href { get; set; }

        public string Tag { get; set; }

        public AdTypes AdType { get; set; }
    }
}
