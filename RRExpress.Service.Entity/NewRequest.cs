﻿using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRExpress.Service.Entity {
    /// <summary>
    /// 新单
    /// </summary>
    [ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields, EnumPassthru = true)]
    public class NewRequest {

        public int ID { get; set; }

        public string GoodsName { get; set; }

        public decimal Price { get; set; }

        public long DistanceToMe { get; set; }

        public long DistanceToTarget { get; set; }

        public string FromAddr { get; set; }

        public string TargetAddr { get; set; }

        public string Time { get; set; }
    }
}
