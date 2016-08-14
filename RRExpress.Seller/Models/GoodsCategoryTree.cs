﻿using RRExpress.Common;
using RRExpress.Seller.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.ComponentModel;

namespace RRExpress.Seller.Models {

    public class GoodsCategoryTreeNode : TreeNode<GoodsCategory, GoodsCategoryTreeNode, int>, INotifyPropertyChanged {

        private bool _isSelected = false;
        public bool IsSelected {
            get {
                return this._isSelected;
            }
            set {
                this._isSelected = value;
                this.NotifyOfPropertyChange("IsSelected");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyOfPropertyChange(string prop) {
            if (this.PropertyChanged != null)
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
