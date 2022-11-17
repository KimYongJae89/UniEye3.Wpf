using DynMvp.Data;
using DynMvp.InspectData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Unieye.WPF.Base.Layout.Models;
using UniScanC.Controls.Views;
using UniScanC.Data;

namespace UniScanC.Controls.ViewModels
{
    public class PatternSizeControlViewModel : CustomizeControlViewModel, INotifyModelChanged, INotifyProductResultChanged
    {
        #region 생성자
        public PatternSizeControlViewModel() : base(typeof(PatternSizeControlViewModel))
        {

        }
        #endregion


        #region 속성(LayoutControlViewModel)
        #endregion


        #region 속성
        private SizeF patternSize = new SizeF();
        public SizeF PatternSize
        {
            get => patternSize;
            set => Set(ref patternSize, value);
        }
        #endregion


        #region 메서드
        public void OnUpdateModel(ModelBase modelBase)
        {
            System.Windows.Threading.Dispatcher dispatcher = Application.Current.Dispatcher;
            if (!dispatcher.CheckAccess())
            {
                dispatcher.Invoke(new Action(() => OnUpdateModel(modelBase)));
                return;
            }
            UpdateModel(modelBase);
        }

        public void UpdateModel(ModelBase modelBase)
        {
            PatternSize = new SizeF();
        }

        public void OnUpdateResult(IEnumerable<ProductResult> productResults, CancellationTokenSource taskCancelToken = null)
        {
            System.Windows.Threading.Dispatcher dispatcher = Application.Current.Dispatcher;
            if (!dispatcher.CheckAccess())
            {
                dispatcher.Invoke(new Action(() => OnUpdateResult(productResults, taskCancelToken)));
                return;
            }
            UpdateResult(productResults, taskCancelToken);
        }

        public void UpdateResult(IEnumerable<ProductResult> productResults, CancellationTokenSource taskCancelToken = null)
        {
            if (productResults == null || productResults.Count() == 0)
            {
                PatternSize = new SizeF();
                return;
            }

            try
            {
                var filteredList = new List<InspectResult>();
                foreach (ProductResult productResult in productResults)
                {
                    if (productResult is InspectResult inspectResult)
                    {
                        filteredList.Add(inspectResult);
                    }
                }

                if (filteredList.Count == 0)
                {
                    return;
                }

                List<InspectResult> sizeList = filteredList.FindAll(x => x.PatternSize.Width > 0 && x.PatternSize.Height > 0);
                if (sizeList.Count() > 0)
                {
                    SizeF size = sizeList.Last().PatternSize;
                    PatternSize = new SizeF(size.Width, size.Height);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("PatternSizeControl Exception : {0}", ex.Message);
            }
        }

        public override UserControl CreateControlView()
        {
            return new PatternSizeControlView();
        }
        #endregion
    }
}
