using DynMvp.Data;

namespace WPF.ThicknessMeasure.Model
{
    public class ModelBuilder : DynMvp.Data.ModelBuilder
    {
        #region 메서드
        public override ModelBase CreateModel()
        {
            return new WPF.ThicknessMeasure.Model.Model();
        }

        public override DynMvp.Data.ModelDescription CreateModelDescription()
        {
            return new WPF.ThicknessMeasure.Model.ModelDescription();
        }
        #endregion
    }
}
