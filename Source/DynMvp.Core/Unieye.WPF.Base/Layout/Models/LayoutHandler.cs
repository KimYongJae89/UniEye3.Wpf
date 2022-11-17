using DynMvp.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Override;
using UniEye.Base.Config;

namespace Unieye.WPF.Base.Layout.Models
{
    public class LayoutHandler : ObservableRangeCollection<LayoutPageHandler>
    {
        public List<Grid> ToLayoutGrid()
        {
            var gridList = new List<Grid>();
            foreach (LayoutPageHandler layoutPageHandler in this)
            {
                int columns = layoutPageHandler.Columns;
                int rows = layoutPageHandler.Rows;

                var grid = new Grid();
                grid.Tag = layoutPageHandler.PageName;

                for (int col = 0; col < columns; col++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                }

                for (int row = 0; row < rows; row++)
                {
                    grid.RowDefinitions.Add(new RowDefinition());
                }

                foreach (LayoutModel layoutModel in layoutPageHandler.ModelList)
                {
                    var contentPresenter = new ContentPresenter();
                    UserControl view = layoutModel.ControlViewModel.CreateControlView();
                    view.DataContext = layoutModel.ControlViewModel;
                    contentPresenter.Content = view;
                    contentPresenter.Margin = (Thickness)Application.Current.TryFindResource("XXSmallMargin");
                    Grid.SetRow(contentPresenter, layoutModel.Row - 1);
                    Grid.SetRowSpan(contentPresenter, layoutModel.RowSpan);
                    Grid.SetColumn(contentPresenter, layoutModel.Column - 1);
                    Grid.SetColumnSpan(contentPresenter, layoutModel.ColumnSpan);

                    grid.Children.Add(contentPresenter);
                }

                gridList.Add(grid);
            }

            return gridList;
        }

        public LayoutHandler Clone()
        {
            var layoutHandler = new LayoutHandler();
            layoutHandler.CopyFrom(this);
            return layoutHandler;
        }

        public void CopyFrom(LayoutHandler layoutHandler)
        {
            Clear();
            foreach (LayoutPageHandler layoutPageHandler in layoutHandler)
            {
                Add(layoutPageHandler.Clone());
            }
        }

        public void Save(string fileName = "Layout")
        {
            int index = 0;
            var directoryInfo = new DirectoryInfo(BaseConfig.Instance().ConfigPath);
            FileInfo[] files = directoryInfo.GetFiles($"{fileName}*", SearchOption.TopDirectoryOnly);
            foreach (FileInfo file in files)
            {
                File.Delete(file.FullName);
            }

            foreach (LayoutPageHandler layoutPageHandler in this)
            {
                layoutPageHandler.Save($"{fileName}{index++}");
            }
        }

        public void Load(string fileName = "Layout")
        {
            try
            {
                Clear();
                var directoryInfo = new DirectoryInfo(BaseConfig.Instance().ConfigPath);
                FileInfo[] files = directoryInfo.GetFiles($"{fileName}*", SearchOption.TopDirectoryOnly);
                foreach (FileInfo file in files)
                {
                    Add(new LayoutPageHandler());
                }

                int index = 0;
                foreach (LayoutPageHandler layoutPageHandler in this)
                {
                    layoutPageHandler.Load($"{files[index++].Name.Split('.').FirstOrDefault()}");
                }
            }
            catch (JsonSerializationException ex)
            {
                LogHelper.Error($"LayoutHandler::Load - {ex.Message}");
            }
        }
    }
}
