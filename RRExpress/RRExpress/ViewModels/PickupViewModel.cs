﻿using AsNum.XFControls;
using RRExpress.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RRExpress.Service.Entity;
using RRExpress.Api.V1.Methods;
using Xamarin.Forms;

namespace RRExpress.ViewModels {

    [Regist(InstanceMode.Singleton)]
    public class PickupViewModel : OrderList {

        public override string Title {
            get {
                return "收货";
            }
        }

        public ICommand PickupItCmd { get; set; }

        public PickupViewModel() {
            this.PickupItCmd = new Command((o) => {

            });
        }

        public override async Task<Tuple<bool, IEnumerable<Order>>> GetDatas(int page) {
            var mth = new GetMyOrders() {
                Page = page
            };

            var datas = await ApiClient.ApiClient.Instance.Value.Execute(mth);
            return new Tuple<bool, IEnumerable<Order>>(mth.HasError, datas);
        }
    }
}