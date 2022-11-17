using DynMvp.Base;
using DynMvp.Vision;
using DynMvp.Vision.Cuda;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.InspectFlow.Models;
using Unieye.WPF.Base.Override;
using ICommand = System.Windows.Input.ICommand;
using SystemManager = Unieye.WPF.Base.Override.SystemManager;

namespace Unieye.WPF.Base.InspectFlow.ViewModels
{
    internal class InspectionFlowViewModel : Observable
    {
        private ImageSource imageSource;
        public ImageSource ImageSource
        {
            get => imageSource;
            set => Set(ref imageSource, value);
        }

        private Type selectedAlgorithmType;
        public Type SelectedAlgorithmType
        {
            get => selectedAlgorithmType;
            set => Set(ref selectedAlgorithmType, value);
        }

        private FlowAlgorithmModel selectedAlgorithm;
        public FlowAlgorithmModel SelectedAlgorithm
        {
            get => selectedAlgorithm;
            set => Set(ref selectedAlgorithm, value);
        }

        public ObservableRangeCollection<Type> DefinesAlgorithmList { get; set; } = new ObservableRangeCollection<Type>();

        private ICommand addAlgorithmCommand;
        public ICommand AddAlgorithmCommand => addAlgorithmCommand ?? (addAlgorithmCommand = new RelayCommand<InspectionFlowModel>(AddAlgorithm));

        private void AddAlgorithm(InspectionFlowModel InspectionFlowModel)
        {
            if (SelectedAlgorithmType == null)
            {
                return;
            }

            InspectionFlowModel.FlowAlgorithms.Add(Activator.CreateInstance(SelectedAlgorithmType) as FlowAlgorithmModel);
        }

        private ICommand deleteAlgorithmCommand;
        public ICommand DeleteAlgorithmCommand => deleteAlgorithmCommand ?? (deleteAlgorithmCommand = new RelayCommand<InspectionFlowModel>(DeleteAlgorithm));

        private void DeleteAlgorithm(InspectionFlowModel InspectionFlowModel)
        {
            if (SelectedAlgorithm == null)
            {
                return;
            }

            InspectionFlowModel.FlowAlgorithms.Remove(SelectedAlgorithm);
        }

        private ICommand imageProcessingCommand;
        public ICommand ImageProcessingCommand => imageProcessingCommand ?? (imageProcessingCommand = new RelayCommand<InspectionFlowModel>(ImageProcessing));

        private void ImageProcessing(InspectionFlowModel InspectionFlowModel)
        {
            if (ImageSource == null)
            {
                return;
            }

            var bitmapSource = ImageSource as BitmapSource;

            Bitmap bitmap = ImageHelper.BitmapSourceToBitmap(bitmapSource.Clone(), System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            var image = Image2D.ToImage2D(bitmap);
            image.ConvertFromDataPtr();

            ImageBuilder imageBuilder = ImageBuilder.CudaImageBuilder;
            AlgoImage algoImage = imageBuilder.Build(image, ImageType.Grey);

            AlgoImage resultImage = InspectionFlowModel.Inspect(algoImage);
            algoImage.Dispose();

            ImageSource = resultImage?.ToBitmapSource();
        }

        private ICommand okCommand;
        public ICommand OkCommand => okCommand ?? (okCommand = new RelayCommand<Window>(Ok));

        private void Ok(Window window)
        {
            window.Close();
        }

        public InspectionFlowViewModel()
        {
            BindingOperations.EnableCollectionSynchronization(DefinesAlgorithmList, DefinesAlgorithmList);

            var systemManager = SystemManager.Instance() as SystemManager;

            DefinesAlgorithmList.AddRange(FlowAlgorithmModel.GetAlgorithmTypeList());

            //var FlowAlgorithmModelHandler = systemManager.ServiceProvider.GetRequiredService<FlowAlgorithmModelHandler>();
            //foreach (var model in FlowAlgorithmModelHandler.FlowAlgorithms)
            //    DefinesAlgorithmList.Add(model.GetType());
        }
    }
}
