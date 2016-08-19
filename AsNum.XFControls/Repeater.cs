﻿using AsNum.XFControls.Binders;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;
using System;

namespace AsNum.XFControls {

    public class Repeater : Layout<View> {


        #region ItemTemplate
        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create("ItemTemplate",
                typeof(DataTemplate),
                typeof(Repeater),
                null
                );

        public DataTemplate ItemTemplate {
            get {
                return this.GetValue(ItemTemplateProperty) as DataTemplate;
            }

            set {
                this.SetValue(ItemTemplateProperty, value);
            }
        }
        #endregion

        #region ItemsSource
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create("ItemsSource",
                typeof(IEnumerable),
                typeof(Repeater),
                null,
                BindingMode.OneWay,
                propertyChanged: ItemsChanged);


        public IEnumerable ItemsSource {
            get {
                return this.GetValue(ItemsSourceProperty) as IEnumerable;
            }
            set {
                this.SetValue(ItemsSourceProperty, value);
            }
        }

        private static void ItemsChanged(BindableObject bindable, object oldValue, object newValue) {
            var rp = (Repeater)bindable;
            var v = newValue as INotifyCollectionChanged;
            if (v != null)
                rp.InitCollection(v);
            else {
                rp.RemoveAll();
                rp.Add((IEnumerable)newValue);
            }
        }
        #endregion

        #region SelectedItem
        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create("SelectedItem",
                typeof(object),
                typeof(Repeater),
                null,
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: SelectedItemChanged
                );


        public object SelectedItem {
            get {
                return this.GetValue(SelectedItemProperty);
            }
            set {
                this.SetValue(SelectedItemProperty, value);
            }
        }

        private static void SelectedItemChanged(BindableObject bindable, object oldValue, object newValue) {
            var rp = (Repeater)bindable;
        }
        #endregion

        #region itemTemplateSelector 模板选择器
        public static readonly BindableProperty ItemTemplateSelectorProperty =
            BindableProperty.Create("ItemTemplateSelector",
                typeof(DataTemplateSelector),
                typeof(TabbedView),
                null);

        public DataTemplateSelector ItemTemplateSelector {
            get {
                return (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty);
            }
            set {
                SetValue(ItemTemplateSelectorProperty, value);
            }
        }
        #endregion

        #region Orientation
        public static readonly BindableProperty OrientationProperty =
            BindableProperty.Create(nameof(Orientation),
                typeof(RepeaterOrientation),
                typeof(Repeater),
                RepeaterOrientation.HorizontalWrap,
                propertyChanged: OrientationChanged
                );

        public RepeaterOrientation Orientation {
            get {
                return (RepeaterOrientation)this.GetValue(OrientationProperty);
            }
            set {
                this.SetValue(OrientationProperty, value);
            }
        }

        private static void OrientationChanged(BindableObject bindable, object oldValue, object newValue) {
            var repeater = (Repeater)bindable;
            repeater.SetContainer();
        }

        #endregion

        private Command TapCmd { get; }

        private Layout<View> Container { get; set; }

        public Repeater() {
            this.SetContainer();
            this.TapCmd = new Command(o => {
                this.SelectedItem = o;
            });
        }


        private void SetContainer() {
            this.BatchBegin();
            var old = this.Container;
            IList<View> subViews = null;
            if (old != null) {
                subViews = old.Children;
            }

            var container = RepeaterContainerFactory.Get(this.Orientation);
            this.Container = container.Layout;
            this.Children.Add(this.Container);

            if (subViews != null) {
                foreach (var sub in subViews) {
                    sub.Parent = null;
                    this.Container.Children.Add(sub);
                }
            }

            if (old != null) {
                this.Children.Remove(old);
            }

            this.BatchCommit();
        }

        private void InitCollection(INotifyCollectionChanged datas) {
            datas.CollectionChanged += Datas_CollectionChanged;
        }

        private void Datas_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            switch (e.Action) {
                case NotifyCollectionChangedAction.Add:
                    this.Add(e.NewItems, e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    this.Remove(e.OldItems, e.OldStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Move:
                    Debugger.Break();
                    break;
                case NotifyCollectionChangedAction.Replace:
                    Debugger.Break();
                    break;
                case NotifyCollectionChangedAction.Reset:
                    this.RemoveAll();
                    this.Add(this.ItemsSource);
                    break;
            }
        }

        private void Add(IEnumerable datas, int startIdx = 0) {
            if (datas == null)
                return;

            foreach (var d in datas) {
                var v = this.GetChildView(d); //this.ItemTemplate.CreateContent() as View;
                this.Container.Children.Insert(startIdx++, v);
                v.Parent = this;
            }
        }

        private void Remove(IList datas, int startIdx) {
            if (datas == null)
                return;

            foreach (var d in datas) {
                this.Container.Children.RemoveAt(startIdx++);
            }
        }

        private void RemoveAll() {
            var children = this.Container.Children.ToList();
            foreach (var c in children)
                this.Container.Children.Remove(c);
        }


        /// <summary>
        /// 从模板/模板选择器创建子视图
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private View GetChildView(object data) {
            View view = null;
            if (this.ItemTemplate != null || this.ItemTemplateSelector != null) {
                if (this.ItemTemplateSelector != null) {
                    // SelectTemplate 的第二个参数，即 TemplateSelector 的 OnSelectTemplate 方法的 container 参数
                    view = (View)this.ItemTemplateSelector.SelectTemplate(data, this).CreateContent();
                } else if (this.ItemTemplate != null)
                    view = (View)this.ItemTemplate.CreateContent();

                if (view != null) {
                    view.BindingContext = data;
                    TapBinder.SetCmd(view, this.TapCmd);
                    TapBinder.SetParam(view, data);
                }
            }

            if (view == null)
                view = new Label() { Text = "111" };

            return view;
        }

        protected override void LayoutChildren(double x, double y, double width, double height) {
            this.Container.Layout(new Rectangle(x, y, width, height));
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint) {
            var size = this.Container.Measure(widthConstraint, heightConstraint);
            return base.OnMeasure(size.Request.Width, size.Request.Height);
        }
    }

    public enum RepeaterOrientation {
        Vertical = 0,
        Horizontal = 1,
        HorizontalWrap = 2
    }
}
