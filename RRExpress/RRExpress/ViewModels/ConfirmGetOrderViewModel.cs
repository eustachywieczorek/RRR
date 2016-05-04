﻿using RRExpress.Attributes;
using RRExpress.Service.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRExpress.ViewModels {

    /// <summary>
    /// 确认接单
    /// </summary>
    [Regist(InstanceMode.Singleton)]
    public class ConfirmGetOrderViewModel : BaseVM {
        public override string Title {
            get {
                return "订单详情";
            }
        }


        private Order _data = null;
        public Order Data {
            get {
                return this._data;
            }
            set {
                this._data = value;
                this.NotifyOfPropertyChange(() => this.Data);
            }
        }
    }
}