using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MIS.DAL;
using MIS.IsPlatform.Framework.CachedDataProviders;
using MIS.IsPlatform.Framework.EntryInterval;
using MIS.IsPlatform.Framework.Grid;
using MIS.IsPlatform.Framework.Mvc;
using MIS.IsPlatform.Framework.Operation;
using MIS.IsPlatform.Framework.TimeZone;
using MIS.IsPlatform.Framework.WebApi;
using MIS.IsPlatform.Interface.Areas.Common.Business.EntryInterval;
using MIS.IsPlatform.Interface.Areas.LoadAndHaulDataEntry.Business.ProductionEntryInterval;
using MIS.IsPlatform.Interface.Areas.LoadAndHaulDataEntry.DAL;
using MIS.IsPlatform.Interface.Areas.LoadAndHaulDataEntry.Dtc;

namespace MIS.IsPlatform.Interface.Areas.LoadAndHaulDataEntry.Controllers.Api {
  public class ProductionEntryController : ApiController {
    private readonly IOperationInfoProvider _operationInfoProvider;
    private readonly IEntryIntervalProvider _entryIntervalProvider;
    private readonly ITimeZoneInfoProvider _timezoneInfoProvider;
    private readonly IProductionEntryUtility _productionEntryUtility;

    private const int MAX_LOADS = 1000;

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
    [Route ("api/loadandhauldataentry/{operation}/data-entry-tabs")]
    public IHttpActionResult Get (string operation, bool isActive = true) {
      var operationInfo = GetOperationInfo (operation);

      var dataEntryTabs = isActive ?
        VwRfProductionDataEntryTab.SelectAllActive (operationInfo.Operation) :
        VwRfProductionDataEntryTab.SelectAll ();

      return Ok (dataEntryTabs.OrderBy (p => p.TabOrder).OrderBy (p => p.Label));
    }

    [HttpGet]
    [RequireAuthentication]
    [Route ("api/loadandhauldataentry/{operation}/shift-records/{tabKey}/{intervalString}")]
    public IHttpActionResult Get (
      string operation,
      string tabKey,
      string intervalString,
      int? pageSize = null,
      int? pageNumber = null, [FromUri] ColumnFilter[] filters = null, [FromUri] SortBy[] sorting = null) {
      var operationInfo = GetOperationInfo (operation);

      var tabInfo = VwRfProductionDataEntryTab.Find (operationInfo.Operation, tabKey);
      if (tabInfo == null) {
        throw new ArgumentException ();
      }

      // Only shift interval supported at the moment.
      if (!tabInfo.IntervalType.Equals ("Shift", StringComparison.InvariantCultureIgnoreCase)) {
        throw new ArgumentException ();
      }

      var shiftInterval = (ShiftInterval) _entryIntervalProvider.GetInterval (tabInfo.IntervalType.ToUpper (), intervalString);

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

    [HttpPost]
    [RequireAuthentication]
    [Route ("api/loadandhauldataentry/{operation}/shift-records/{tabKey}/{intervalString}")]
    public IHttpActionResult Post (string operation, string tabKey, string intervalString, [FromBody] CycleDtc record) {
      var operationInfo = GetOperationInfo (operation);

      var tabInfo = VwRfProductionDataEntryTab.Find (operationInfo.Operation, tabKey);
      if (tabInfo == null) {
        throw new ArgumentException ();
      }

      // Only shift interval supported at the moment.
      if (!tabInfo.IntervalType.Equals ("Shift", StringComparison.InvariantCultureIgnoreCase)) {
        throw new ArgumentException ();
      }

      // Maximum # of loads exceeded
      if (record.Loads > MAX_LOADS) {
        throw new Exception ("$Too many loads entered.Maximum { maxLoads } loads per record.");
      }

      var intervalType = tabInfo.IntervalType.ToUpper ();
      var shiftInterval = (ShiftInterval) _entryIntervalProvider.GetInterval (intervalType, intervalString);

      var productionEntryIntervalFactory = new ProductionEntryIntervalFactory (_operationInfoProvider, _timezoneInfoProvider, _productionEntryUtility);

      var productionEntryIntervalType = (ProductionEntryIntervalType) Enum.Parse (typeof (ProductionEntryIntervalType), tabInfo.IntervalType);
      var entryIntervalProvider = productionEntryIntervalFactory.GetProvider (productionEntryIntervalType);

      //var intervals = entryIntervalProvider.GetIntervals(operationInfo.Operation, shiftInterval.DateOp, shiftInterval.Shift);

      //if (!bool.TryParse(showAll, out bool finalShowAll)) finalShowAll = false;

      //ViewData["ShowAllIntervals"] = finalShowAll;

      //Process the interval
      //int finalIntervalId;
      //if (string.IsNullOrEmpty(intervalId) || !int.TryParse(intervalId, out finalIntervalId))
      //    finalIntervalId = 0;

      ////if the show all box is checked, then we ignore the passed interval id
      //if (finalShowAll) finalIntervalId = -1;

      //get the related interval 
      var productionEntryInterval = entryIntervalProvider.GetInterval (
        operationInfo.Operation,
        shiftInterval.DateOp,
        shiftInterval.Shift,
        0);

      //if (!finalShowAll)
      //{
      //try
      //{
      //    productionEntryInterval = entryIntervalProvider.GetInterval(
      //        operationInfo.Operation,
      //        shiftInstance.DateOp,
      //        shiftInstance.Shift,
      //        record.IntervalId);
      //}
      //catch (Exception)
      //{
      //    //if a bad interval id is passed in, then default to the first interval for the shift.
      //    productionEntryInterval = entryIntervalProvider.GetInterval(
      //        operationInfo.Operation,
      //        shiftInstance.DateOp,
      //        shiftInstance.Shift, 0);
      //}

      //ViewData["IntervalId"] = finalIntervalId;

      //}

      //ViewData["IntervalSelect"] = new SelectList(intervals, "IntervalId", "Label", finalIntervalId);

      //Data for shift selector view
      //ViewData["Operation"] = operationInfo.Operation;
      //if (operationsContext.Length > 1) ViewData["OperationSelect"] = new SelectList(operationsContext, "Operation", "Description", operationInfo.Description);
      //ViewData["ShiftSelect"] = new SelectList(shiftInfos, "Shift", "Label", finalShift.Shift);

      //ViewData["Shift"] = finalShift.Shift;
      //ViewData["DateOp"] = finalDateOp.ToShortDateString();

      //var includedFleetTypes = new[]
      //{
      //    FleetTypeValidator.HaulerFleetType, FleetTypeValidator.LoaderFleetType,
      //    FleetTypeValidator.ExcavatorFleetType, FleetTypeValidator.ShovelFleetType
      //};

      //var fleetTypes = Request.QueryString.GetValues("FleetType");
      ////Validate fleettypes, prepare select list
      //var fleetTypeRecords = Array.FindAll(RfFleetType.SelectAll(),
      //                                     f =>
      //                                     Array.Find(includedFleetTypes, i => Utility.CompareSqlStrings(i, f.FleetType)) != null);

      //var selectedFleetTypeRecords = fleetTypes != null && fleetTypes.Length != 0
      //                                   ? Array.FindAll(fleetTypeRecords, ur => Array.Find(fleetTypes, u => Utility.CompareSqlStrings(u, ur.FleetType)) != null) : fleetTypeRecords;

      //var fleetTypeSelect = new BetterCheckBoxList<RfFleetType>(fleetTypeRecords, "FleetType", "Description", selectedFleetTypeRecords);

      //ViewData["FleetTypeSelect"] = fleetTypeSelect;
      //ViewData["SelectedFleetTypes"] = Array.ConvertAll(selectedFleetTypeRecords, u => u.FleetType);

      //var fleetTypeUrl = "&FleetType=" + String.Join("&FleetType=", Array.ConvertAll(selectedFleetTypeRecords, u => u.FleetType));

      ////Next and previous shift links
      //if (Request.Url != null)
      //{
      //    var url = Request.Url.AbsolutePath;
      //    var nextShift = operationInfo.GetNextShift(shiftInstance);
      //    var prevShift = operationInfo.GetPrevShift(shiftInstance);

      //    ViewData["Url"] = url;

      //    var urlTail = string.Format("{0}&ShowAllIntervals={1}&tabKey={2}", fleetTypeUrl, finalShowAll, tabKey);

      //    if (intervalType == ProductionEntryIntervalType.Hourly && !finalShowAll && entryInterval != null)
      //    {
      //        var nextInterval = entryIntervalProvider.GetNextInterval(shiftInstance.Operation, shiftInstance.DateOp, shiftInstance.Shift, finalIntervalId);
      //        var prevInterval = entryIntervalProvider.GetPrevInterval(shiftInstance.Operation, shiftInstance.DateOp, shiftInstance.Shift, finalIntervalId);

      //        ViewData["NextIntervalUrl"] =
      //            string.Format("{0}?operation={1}&dateOp={2}&shift={3}&intervalId={4}{5}", url,
      //                          operationInfo.Operation, nextInterval.IntervalShift.DateOp.ToShortDateString(),
      //                          nextInterval.IntervalShift.Shift, nextInterval.IntervalId, urlTail);
      //        ViewData["PrevIntervalUrl"] =
      //            string.Format("{0}?operation={1}&dateOp={2}&shift={3}&intervalId={4}{5}", url,
      //                          operationInfo.Operation, prevInterval.IntervalShift.DateOp.ToShortDateString(),
      //                          prevInterval.IntervalShift.Shift, prevInterval.IntervalId, urlTail);
      //    }

      //    var defaultIntervalId = finalShowAll ? -1 : 0;

      //    ViewData["NextShiftUrl"] = string.Format("{0}?operation={1}&dateOp={2}&shift={3}&intervalId={4}{5}", url,
      //                                             operationInfo.Operation, nextShift.DateOp.ToShortDateString(),
      //                                             nextShift.Shift, defaultIntervalId, urlTail);
      //    ViewData["PrevShiftUrl"] = string.Format("{0}?operation={1}&dateOp={2}&shift={3}&intervalId={4}{5}", url,
      //                                             operationInfo.Operation, prevShift.DateOp.ToShortDateString(),
      //                                             prevShift.Shift, defaultIntervalId, urlTail);

      //    ViewData["NextDayUrl"] = string.Format("{0}?operation={1}&dateOp={2}&shift={3}&intervalId={4}{5}", url,
      //                             operationInfo.Operation, shiftInstance.DateOp.AddDays(1).ToShortDateString(),
      //                             shiftInstance.Shift, defaultIntervalId, urlTail);
      //    ViewData["PrevDayUrl"] = string.Format("{0}?operation={1}&dateOp={2}&shift={3}&intervalId={4}{5}", url,
      //                                             operationInfo.Operation, shiftInstance.DateOp.AddDays(-1).ToShortDateString(),
      //                                             shiftInstance.Shift, defaultIntervalId, urlTail);
      //}

      //const int maxLoads = 1000;

      //IOperationInfo operationInfo;
      //ShiftInstance shiftInstance;
      //IProductionEntryIntervalProvider entryIntervalProvider;
      //ProductionEntryInterval entryInterval;
      //ValidateArguments(out operationInfo, out shiftInstance, out entryIntervalProvider, out entryInterval);

      var gradeItems = RfProductionDataEntryGrade.SelectAll (operation, tabKey).Select (g => g.GradeItem).ToArray ();

      //var newValues = ObjectBinder.BindDictionary<FlexibleRowObject, int>(Request.Form, "NewValues");
      //var oldValues = ObjectBinder.BindDictionary<FlexibleRowObject, int>(Request.Form, "OldValues");

      using (var connection = Utility.GetConnection<Cycle> ()) {
        using (var transaction = connection.BeginTransaction ()) {
          // Get movement rows            
          var rows = entryIntervalProvider.ConvertToMovementRows (shiftInterval, productionEntryInterval, tabKey, newValues, oldValues);

          var row = rows.Where (r => r.Id == rowIndex).FirstOrDefault ();

          result.AddValidationErrors (rowIndex, entryIntervalProvider.ValidateMovementRow (connection, transaction, row));

          if (!result.IsErrorFree () || result.RowResults.Exists (row => row.ValidationErrors.Count > 0))
            return result;

          foreach (var row in rows) {
            try {

              var deleteRow = row.OldValue != null ? row.OldValue : row;

              var deleteGrades = new DeleteAll<CycleGrade> ()
                .WherePropertyEquals ("Operation", deleteRow.Operation)
                .WherePropertyEquals ("Loader", deleteRow.Loader)
                .WherePropertyEquals ("Hauler", deleteRow.Hauler)
                .WherePropertyEquals ("LoaderOperatorId", deleteRow.LoaderOperatorId)
                .WherePropertyEquals ("HaulerOperatorId", deleteRow.HaulerOperatorId)
                .WherePropertyEquals ("Origin", deleteRow.Origin)
                .WherePropertyEquals ("Destination", deleteRow.Destination)
                .WherePropertyEquals ("Material", deleteRow.Material)
                .WherePropertyEquals ("DateOp", deleteRow.DateOp)
                .WherePropertyEquals ("Shift", deleteRow.Shift)
                .WherePropertyEquals ("Datasource", deleteRow.Datasource)
                .WherePropertyIn ("GradeItem", gradeItems);

              if (deleteRow.DateTimeStart.HasValue)
                deleteGrades.WherePropertyEquals ("DateTimeStart", deleteRow.DateTimeStart.Value);

              deleteGrades.Execute (connection, transaction);

              var deleteCycles = new DeleteAll<Cycle> ()
                .WherePropertyEquals ("Operation", deleteRow.Operation)
                .WherePropertyEquals ("Loader", deleteRow.Loader)
                .WherePropertyEquals ("Hauler", deleteRow.Hauler)
                .WherePropertyEquals ("LoaderOperatorId", deleteRow.LoaderOperatorId)
                .WherePropertyEquals ("HaulerOperatorId", deleteRow.HaulerOperatorId)
                .WherePropertyEquals ("Origin", deleteRow.Origin)
                .WherePropertyEquals ("Destination", deleteRow.Destination)
                .WherePropertyEquals ("Material", deleteRow.Material)
                .WherePropertyEquals ("DateOp", deleteRow.DateOp)
                .WherePropertyEquals ("Shift", deleteRow.Shift)
                .WherePropertyEquals ("Datasource", deleteRow.Datasource);

              if (deleteRow.DateTimeStart.HasValue)
                deleteCycles.WherePropertyEquals ("DateTimeStart", deleteRow.DateTimeStart.Value);

              deleteCycles.Execute (connection, transaction);

              int cycleCount = 0;

              foreach (var cycle in row.Cycles) {
                cycleCount++;
                Utility.SaveSafe (connection, transaction, cycle);

                foreach (var gradeItem in cycle.GradeValues) {
                  Utility.SaveSafe (connection, transaction, CreateCycleGrade (cycle, gradeItem.Key, gradeItem.Value));
                  if (cycleCount.Equals (row.Cycles.Count ()) && !string.IsNullOrEmpty (tab.SourceGradeType)) {
                    var grade = new SelectAll<VwProductionDataEntryGrade> ()
                      .WherePropertyEquals ("Operation", cycle.Operation)
                      .WherePropertyEquals ("DateOp", cycle.DateOp)
                      .WherePropertyEquals ("Origin", cycle.Origin)
                      .WherePropertyEquals ("Material", cycle.Material)
                      .WherePropertyEquals ("GradeItem", gradeItem.Key)
                      .First (connection, transaction);

                    Utility.SaveSafe (connection, transaction, CreateGrade (cycle, tab.SourceGradeType, grade));
                  }
                }
              }

              result.AddRowSaved (row.Id);
              result.AddRowUpdated (row.Id, new FlexibleRowObject (row));

              ManualDataEntryLog.Log (transaction,
                row.IsNew ? ManualDataEntryLog.AdditionLogType : ManualDataEntryLog.UpdateLogType, shiftInstance.Operation,
                LoadAndHaulManualEntryLog.LogArea, LoadAndHaulManualEntryLog.LogGroup, "Production Entry", shiftInstance.DateOp, shiftInstance.Shift, null, "Data edited.");
            } catch (Exception ex) {
              result.AddRowError (row.Id, GridRowError.Make (ex));
            }
          }

          if (!result.IsErrorFree () || result.RowResults.Exists (row => row.ValidationErrors.Count > 0))
            return result;

          // Add record to ManualDataEntryProcesQueue to recalculate the grades
          Utility.SaveSafe (connection, transaction, new ManualDataEntryProcessQueue () { Operation = shiftInstance.Operation, DateOp = shiftInstance.DateOp, DateTimeAdded = DateTime.UtcNow });

          transaction.Commit ();
        }
      }

      return result;
    }

    private IOperationInfo GetOperationInfo (string operation) {
      var operationInfo = _operationInfoProvider.GetOperationInfos ()
        .Where (p => string.Equals (p.Operation, operation, StringComparison.CurrentCultureIgnoreCase))
        .FirstOrDefault ();

      if (operationInfo == null) {
        throw new Exception ("No operation context available for current page. This may mean that no entry tab is configured/active.");
      }

      return operationInfo;
    }

    //        //private Dictionary<string, object> ConvertToFlexibleRowObject(VwCycleGrade[] grades, IList<Cycle> movementTable)
    //        //{
    //        //    var row = new FlexibleRowObject(movementTable);

    //        //    var matchedGrades = grades.Where(p => MatchMovementKey(p.GetData<byte[]>("MovementKey"), movementTable.GetData<byte[]>("MovementKey")));

    //        //    foreach (var g in matchedGrades)
    //        //        row.SetValue("GradeItem" + g.GetData<string>("GradeItem"), g.GetData<double?>("GradeValue"));

    //        //    return row;
    //        //}

    //        //private Dictionary<string, object> GetDic(IEnumerable<DALType> values)
    //        //{
    //        //    var dic = new Dictionary<string, object>();

    //        //    foreach (var value in values)
    //        //    {
    //        //        if (value.Value == null) dic[value.] = null;
    //        //        else _nvc[value.Key] = Convert.ToString(value.Value);
    //        //    }
    //        //}

    //private bool MatchMovementKey(byte[] m1, byte[] m2)
    //{
    //    if (m1.Length != m2.Length)
    //        return false;

    //    if (ReferenceEquals(m1, m2))
    //        return true;

    //    for (var i = 0; i < m1.Length; i++)
    //        if (m1[i] != m2[i])
    //            return false;

    //    return true;
    //}

    //private ProductionEntryInterval[] GetIntervals(string operation, DateTime dateOp, string shift)
    //{
    //    var operationInfo = _operationInfoProvider.GetOperationInfo(operation);
    //    var shiftInstance = operationInfo.GetShift(dateOp, shift);

    //    return new ProductionEntryInterval[1]
    //    {
    //        new ProductionEntryInterval(
    //            operationInfo,
    //            _timezoneInfoProvider,
    //            0,
    //            shiftInstance.ShiftDescription,
    //            shiftInstance.InstanceStartTime,
    //            shiftInstance.InstanceEndTime,
    //            shiftInstance.InstanceStartTimeUtc,
    //            shiftInstance.InstanceEndTimeUtc,
    //            shiftInstance)
    //    };
    //}

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