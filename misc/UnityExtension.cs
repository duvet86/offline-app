#if ISPLATFORM2
using Microsoft.Practices.Unity;
using MIS.IsPlatform.Framework.TimeZone;
using MIS.IsPlatform.Framework.Unity;
using MIS.IsPlatform.Interface.Areas.LoadAndHaulDataEntry.Business.AuxiliaryEntryConfig;
using MIS.IsPlatform.Interface.Areas.LoadAndHaulDataEntry.Business.ProductionEntryInterval;
using MIS.IsPlatform.Interface.Areas.MMRSTimeZoneInfoProvider.Business;

namespace MIS.IsPlatform.Interface.Areas.LoadAndHaulDataEntry {
  public class UnityExtension : AreaUnityContainerExtension {
    protected override void Initialize () {
      Container.RegisterType<IAuxiliaryEntryConfigProvider, AuxiliaryEntryConfigProvider> ();
      Container.RegisterType<IAuxiliaryEntryProvider, AuxiliaryEntryProvider> ();

      Container.RegisterType<IProductionEntryUtility, ProductionEntryUtility> ();
      Container.RegisterType<ITimeZoneInfoProvider, TimeZoneInfoProvider> ();
    }
  }
}
#endif