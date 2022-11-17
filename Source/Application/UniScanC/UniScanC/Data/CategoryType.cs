using Unieye.WPF.Base.Helpers;
using UniScanC.Enums;

namespace UniScanC.Data
{
    public class CategoryType : Observable
    {
        private ECategoryTypeName type;
        public ECategoryTypeName Type
        {
            get => type;
            set => Set(ref type, value);
        }

        private bool use;
        public bool Use
        {
            get => use;
            set => Set(ref use, value);
        }

        private object data;
        public object Data
        {
            get => data;
            set => Set(ref data, value);
        }

        public CategoryType() { }

        public CategoryType(ECategoryTypeName _type)
        {
            Type = _type;
        }

        public CategoryType(CategoryType srcType)
        {
            Type = srcType.Type;
            Use = srcType.Use;
            Data = srcType.Data;
        }

        public CategoryType Clone()
        {
            var newData = new CategoryType();
            newData.CopyFrom(this);

            return newData;
        }

        public void CopyFrom(CategoryType srcData)
        {
            Type = srcData.Type;
            Use = srcData.Use;
            Data = srcData.Data;
        }
    }
}
