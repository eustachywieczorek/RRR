﻿using RRExpress.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRExpress.ViewModels {
    [Regist(InstanceMode.Singleton)]
    public class MapViewModel : BaseVM {
        public override string Title {
            get {
                return "地图";
            }
        }
    }
}