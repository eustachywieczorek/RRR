﻿using RRExpress.Service.Entity;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace RRExpress.Express.Converters {

    /// <summary>
    /// 订单状态转换为描述
    /// </summary>
    public class OrderStatusDescConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value == null || !(value is OrderStatus))
                return null;

            var s = (OrderStatus)value;
            switch (s) {
                case OrderStatus.New:
                    return "自由快递人抢单中";
                case OrderStatus.Geted:
                    return "正在赶往发货地";
                case OrderStatus.Picked:
                    return "正在赶往收货地";
                case OrderStatus.Deliveried:
                    return "已送达,等待发货人评价";
                case OrderStatus.Dispute:
                    return "订单产生纠纷,正在协商";
                case OrderStatus.Paied:
                    return "款项支付完成";
                case OrderStatus.Finished:
                    return "";
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
