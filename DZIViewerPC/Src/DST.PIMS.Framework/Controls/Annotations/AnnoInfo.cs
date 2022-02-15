using DST.Controls.Controls;
using GalaSoft.MvvmLight;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DST.PIMS.Framework.Controls
{
    [NotifyAspect]
    public class AnnoInfo : ObservableObject
    {
        private AnnoBase _target;
        private int PreviousZindex { get; set; }
        public bool IsDrawing { get; set; }
        /// <summary>
        /// 所有标记集合
        /// </summary>

        public FullyObservableCollection<AnnoBase> AnnoList { get; set; } = new FullyObservableCollection<AnnoBase>();
        /// <summary>
        /// 选中的目标
        /// </summary>
        public AnnoBase Target
        {
            get => _target;
            set
            {
                if (_target != value)
                {
                    if (_target != null) // 还原上一个标记的Zindex
                    {
                        Canvas.SetZIndex(_target, PreviousZindex);
                    }
                    if (value != null) // 记录并设置当前选中的标记的Zindex
                    {
                        PreviousZindex = Canvas.GetZIndex(value);
                        Canvas.SetZIndex(value, 99);
                    }
                    _target?.UnRegisterThumbEvent();
                    value?.RegisterThumbEvent();
                }
                _target = value;
                _target?.Focus();
            }
        }
        public AnnoInfo()
        {
            AnnoList.ItemPropertyChanged += (s, e) =>
            {
                RaisePropertyChanged(nameof(AnnoList));
            };
        }

        /// <summary>
        /// 新增标记框
        /// </summary>
        public AnnoBase NewAnno { get; set; }
        /// <summary>
        /// 新增标记框时记录起点
        /// </summary>
        public Point? OriginPoint { get; set; }
    }
}
