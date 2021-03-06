﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using RRExpress.Api.V1;
using RRExpress.Api.V1.Methods;
using RRExpress.Common;
using RRExpress.Service.Entity;
using System.Collections.Generic;
using System.Reflection;

namespace RRExpress.ApiClient.Test {
    [TestClass]
    public class AdTest {

        [TestInitialize]
        public void Init() {
            var asms = new List<Assembly>() {
                typeof(RRExpressV1BaseMethod<>).GetTypeInfo().Assembly,
                typeof(AdTest).GetTypeInfo().Assembly
            };
            ApiClientOption.Default.UseSandbox = true;
            ApiClient.Init(asms);
        }

        [TestMethod]
        public void GetAds() {
            var mth = new GetADs() {
                Type = AdTypes.MobileAdMiddle
            };

            var datas = ApiClient.Instance.Value.Execute(mth).Result;
        }

        [TestMethod]
        public void GetNewRequest() {
            var mth = new GetNewOrders();
            var datas = ApiClient.Instance.Value.Execute(mth).Result;
        }


        [TestMethod]
        public void NullInstanceAssignTest() {
            AdTest a = null;
            AdTest b = a;
            a = new AdTest();
        }
    }
}
