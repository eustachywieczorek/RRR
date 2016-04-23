﻿using AsNum.XFControls;
using Caliburn.Micro;
using RRExpress.Api.V1.Methods;
using RRExpress.Service.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RRExpress.ViewModels {
    public abstract class OrderList : BaseVM, ISelectable {
        public bool IsSelected { get; set; }

        public ICommand SelectCommand { get; set; }


        public ICommand RefreshCmd { get; }

        public ICommand LoadMoreCmd { get; }

        public BindableCollection<Order> Datas {
            get; set;
        }

        /// <summary>
        /// Item1: 是否有错误， Item2: 结果集
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public abstract Task<Tuple<bool, IEnumerable<Order>>> GetDatas(int page);

        protected bool HasFirstLoaded = false;
        protected int NextPage = 0;


        public OrderList() {
            this.SelectCommand = new Command(async () => {
                if (!this.HasFirstLoaded) {
                    this.HasFirstLoaded = true;
                    await this.LoadData(true);
                }
            });

            this.Datas = new BindableCollection<Order>();

            this.LoadMoreCmd = new Command(async () => {
                await this.LoadData();
            });

            this.RefreshCmd = new Command(async () => {
                await this.LoadData(true);
            });
        }

        private async Task LoadData(bool isReload = false) {
            //if (this.IsBusy) {
            //    //ListView.IsRefreshing 绑定到这个属性上, 造成双向绑定,所以, 不能用它作为判断
            //    //return;
            //}

            this.IsBusy = true;

            var page = isReload ? 0 : this.NextPage;
            var result = await this.GetDatas(page);

            if (!result.Item1 && result.Item2 != null && result.Item2.Count() > 0) {
                if (isReload)
                    this.Datas.Clear();

                this.NextPage = page + 1;
                this.Datas.AddRange(result.Item2);
            }
            this.IsBusy = false;
        }
    }
}
