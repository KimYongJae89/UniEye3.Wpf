namespace UniScanC.Models
{
    public class ModelBuilder : DynMvp.Data.ModelBuilder
    {
        public override DynMvp.Data.ModelBase CreateModel()
        {
            return new Model();
        }

        public override DynMvp.Data.ModelDescription CreateModelDescription()
        {
            return new ModelDescription();
        }
    }
}
