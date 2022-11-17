using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unieye.WPF.Base.Helpers;
using WPF.UniScanIM.Override;

namespace WPF.UniScanIM.ViewModels
{
    public abstract class SystemConfigSettingViewModel : Observable
    {
        public SystemConfigSettingViewModel() { }

        public SystemConfig SystemConfig => SystemConfig.Instance;

        public virtual void Save()
        {
            try
            {
                System.Reflection.PropertyInfo[] srcProp = GetType().GetProperties();
                System.Reflection.PropertyInfo[] dstProp = SystemConfig.GetType().GetProperties();
                foreach (System.Reflection.PropertyInfo dst in dstProp)
                {
                    foreach (System.Reflection.PropertyInfo src in srcProp)
                    {
                        if (Equals(dst.Name, src.Name) && Equals(dst.PropertyType, src.PropertyType))
                        {
                            dst.SetValue(SystemConfig, src.GetValue(this));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Debug(LoggerType.Error, $"SettingViewModel::Save - {e.ToString()}");
            }
        }
        public virtual void Load()
        {
            try
            {
                System.Reflection.PropertyInfo[] srcProp = SystemConfig.GetType().GetProperties();
                System.Reflection.PropertyInfo[] dstProp = GetType().GetProperties();
                foreach (System.Reflection.PropertyInfo dst in dstProp)
                {
                    foreach (System.Reflection.PropertyInfo src in srcProp)
                    {
                        if (Equals(dst.Name, src.Name) && Equals(dst.PropertyType, src.PropertyType))
                        {
                            dst.SetValue(this, src.GetValue(SystemConfig));
                            OnPropertyChanged(dst.Name);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Debug(LoggerType.Error, $"SettingViewModel::Save - {e.ToString()}");
            }
        }
    }
}