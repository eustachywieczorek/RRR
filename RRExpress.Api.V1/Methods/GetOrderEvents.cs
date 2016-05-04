﻿using RRExpress.Service.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using RRExpress.Common.Attributes;

namespace RRExpress.Api.V1.Methods {
    public class GetOrderEvents : RRExpressV1BaseMethod<IEnumerable<OrderEvent>> {
        public override HttpMethod HttpMethod {
            get {
                return HttpMethod.Get;
            }
        }

        public override string Module {
            get {
                return "MyOrders/GetEvents";
            }
        }

        [Param]
        public string OrderNO { get; set; }
    }
}