using DynMvp.Data;
using DynMvp.UI;

namespace WPF.ThicknessMeasure.Model
{
    public class ModelManager : DynMvp.Data.ModelManager
    {
        #region 생성자
        public ModelManager(DynMvp.Data.ModelBuilder modelBuilder) : base(modelBuilder)
        {
        }
        #endregion

        #region 메서드
        public override ModelBase OpenModel(DynMvp.Data.ModelDescription modelDesc, IReportProgress reportProgress)
        {
            CloseModel();

            currentModel = null;

            var modelBuilder = ModelBuilder as ModelBuilder;
            ModelBase model = modelBuilder.CreateModel();
            if (model == null)
            {
                return null;
            }

            model.ModelPath = GetModelPath(modelDesc.Name);
            model.ModelDescription = modelDesc;

            if (model.OpenModel(reportProgress) == false)
            {
                model.BuildModel();
            }

            currentModel = model;

            ModelOpened(currentModel);

            return currentModel;
        }
        #endregion
    }
}
