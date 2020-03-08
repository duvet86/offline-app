using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MIS.DAL;
using MIS.IsPlatform.Framework.CachedDataProviders;
using MIS.IsPlatform.Framework.EntryInterval;
using MIS.IsPlatform.Framework.Grid;
using MIS.IsPlatform.Framework.Mvc;
using MIS.IsPlatform.Framework.TimeZone;
using MIS.IsPlatform.Framework.WebApi;
using MIS.IsPlatform.Interface.Areas.Common.Business.EntryInterval;
using MIS.IsPlatform.Interface.Areas.LoadAndHaulDataEntry.Business;
using MIS.IsPlatform.Interface.Areas.LoadAndHaulDataEntry.Business.ProductionEntryInterval;
using MIS.IsPlatform.Interface.Areas.LoadAndHaulDataEntry.DAL;

namespace MIS.IsPlatform.Interface.Areas.LoadAndHaulDataEntry.Controllers.Api {
  public class ProductionEntryController : ApiController {
    private readonly IOperationInfoProvider _operationInfoProvider;
    private readonly IEntryIntervalProvider _entryIntervalProvider;
    private readonly ITimeZoneInfoProvider _timezoneInfoProvider;
    private readonly IProductionEntryUtility _productionEntryUtility;

    public ProductionEntryController (
      IOperationInfoProvider operationInfoProvider,
      IEntryIntervalProvider entryIntervalProvider,
      ITimeZoneInfoProvider timezoneInfoProvider,
      IProductionEntryUtility productionEntryUtility) {
      _operationInfoProvider = operationInfoProvider;
      _entryIntervalProvider = entryIntervalProvider;
      _timezoneInfoProvider = timezoneInfoProvider;
      _productionEntryUtility = productionEntryUtility;
    }

    [HttpGet]
    [RequireAuthentication]
    [Route ("api/loadandhauldataentry/shift-records")]
    public IHttpActionResult Get (
      string operation,
      string intervalString,
      string tabKey,
      int? pageSize = null,
      int? pageNumber = null, [FromUri] ColumnFilter[] filters = null, [FromUri] SortBy[] sorting = null) {
      var shiftInterval = (ShiftInterval) _entryIntervalProvider.GetInterval ("SHIFT", intervalString);

      var records = Cycle.SelectByIntervalShift (operation, shiftInterval, tabKey, filters, sorting)
        .ToList ();

      records = ExtractRows (records);

      var page = pageNumber.HasValue ?
        records.SelectPageZeroBased (pageSize.Value, pageNumber.Value, out int pageCount) :
        records;

      //_productionEntryUtility.ApplyColumnMasks(operation, page.Cast<IMovementTable>().ToList(), intervals);

      //var gradeItems = RfProductionDataEntryGrade.SelectAll(operation, tabKey)
      //    .Select(gi => gi.GradeItem)
      //    .ToArray();

      //var grades = gradeItems.Length > 0
      //    ? VwCycleGrade.SelectAll(operation, shiftInterval.DateOp, shiftInterval.Shift, gradeItems, filters)
      //    : new VwCycleGrade[0];

      // Convert these records to flexible row objects so that grade pivot can be added
      //var flexibleRows = new List<FlexibleRowObject>();

      //foreach (var record in page)
      //{
      //    var row = _productionEntryUtility.ConvertToFlexibleRowObject(grades, record);
      //    flexibleRows.Add(row);
      //}

      return Ok (page);
    }

    //private Dictionary<string, object> ConvertToFlexibleRowObject(VwCycleGrade[] grades, IList<Cycle> movementTable)
    //{
    //    var row = new FlexibleRowObject(movementTable);

    //    var matchedGrades = grades.Where(p => MatchMovementKey(p.GetData<byte[]>("MovementKey"), movementTable.GetData<byte[]>("MovementKey")));

    //    foreach (var g in matchedGrades)
    //        row.SetValue("GradeItem" + g.GetData<string>("GradeItem"), g.GetData<double?>("GradeValue"));

    //    return row;
    //}

    //private Dictionary<string, object> GetDic(IEnumerable<DALType> values)
    //{
    //    var dic = new Dictionary<string, object>();

    //    foreach (var value in values)
    //    {
    //        if (value.Value == null) dic[value.] = null;
    //        else _nvc[value.Key] = Convert.ToString(value.Value);
    //    }
    //}

    private bool MatchMovementKey (byte[] m1, byte[] m2) {
      if (m1.Length != m2.Length)
        return false;

      if (ReferenceEquals (m1, m2))
        return true;

      for (var i = 0; i < m1.Length; i++)
        if (m1[i] != m2[i])
          return false;

      return true;
    }

    private ProductionEntryInterval[] GetIntervals (string operation, DateTime dateOp, string shift) {
      var operationInfo = _operationInfoProvider.GetOperationInfo (operation);
      var shiftInstance = operationInfo.GetShift (dateOp, shift);

      return new ProductionEntryInterval[1] {
        new ProductionEntryInterval (
          operationInfo,
          _timezoneInfoProvider,
          0,
          shiftInstance.ShiftDescription,
          shiftInstance.InstanceStartTime,
          shiftInstance.InstanceEndTime,
          shiftInstance.InstanceStartTimeUtc,
          shiftInstance.InstanceEndTimeUtc,
          shiftInstance)
      };
    }

    private List<Cycle> ExtractRows (List<Cycle> cycleRows) {
      var rows = new List<Cycle> ();

      Cycle existingRow;
      foreach (var row in cycleRows) {
        existingRow = rows.Find (x =>
          x.Operation.Equals (row.Operation) &&
          x.Loader.Equals (row.Loader) &&
          x.Hauler.Equals (row.Hauler) &&
          x.LoaderOperatorId.Equals (row.LoaderOperatorId) &&
          x.HaulerOperatorId.Equals (row.HaulerOperatorId) &&
          x.Origin.Equals (row.Origin) &&
          x.Destination.Equals (row.Destination) &&
          x.Material.Equals (row.Material) &&
          x.DateOp.Equals (row.DateOp));

        if (existingRow == null) {
          rows.Add (row);
        } else {
          existingRow.Loads += row.Loads;
          existingRow.NominalVolume += row.NominalVolume;
          existingRow.NominalWeight += row.NominalWeight;
          existingRow.PayloadVolume += row.PayloadVolume;
          existingRow.PayloadWeight += row.PayloadWeight;
        }
      }

      return rows;
    }
  }
}