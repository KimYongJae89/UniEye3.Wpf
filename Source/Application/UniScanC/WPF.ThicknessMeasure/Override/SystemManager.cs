using DynMvp.InspectData;

namespace WPF.ThicknessMeasure.Override
{
    public class SystemManager : Unieye.WPF.Base.Override.SystemManager
    {
        #region 메서드
        public override void OnProductInspected(ProductResult productResult)
        {
            ExportData(productResult);
        }
        #endregion
    }
}
