using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
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
    [Route ("api/loadandhauldataentry/{operation}/shift-records/{tabKey}")]
    public IHttpActionResult Get (
      string operation,
      string tabKey,
      string intervalString,
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

    //        [HttpPost]
    //        [RequireAuthentication]
    //        [Route("api/loadandhauldataentry/{operation}/shift-records/{tabKey}")]
    //        public IHttpActionResult Post(string operation, string tabKey, [FromBody] CycleDtc record)
    //        {
    //            //var operationInfoProvider = CachedDataProvider.Instance.Get<IOperationInfoProvider>();
    //            //var timezoneInfoProvider = CachedDataProvider.Instance.Get<ITimeZoneInfoProvider>();
    //            //var productionEntryUtility = new ProductionEntryUtility();

    //            //string operation, string dateOp, string shift, 
    //            //var operation = Request["Operation"];
    //            //var dateOp = Request["DateOp"];
    //            //var shift = Request["Shift"];
    //            //var intervalId = Request["IntervalId"];
    //            //var showAll = Request["ShowAllIntervals"];
    //            //var tabKey = Request["TabKey"];

    //            //Ensure the selection is valid for the current operation, if not specified or invalid default to first operation of context
    //            //operationInfo = null;
    //            //IOperationInfo[] operationContext;
    ////#if !ISPLATFORM2
    ////            operationContext = CurrentSiteMapItem.Section.GetOperations();
    ////#else
    ////            operationContext = CurrentSiteMapItem.GetOperations();
    ////#endif
    //            //Only list operations having at least 1 tab in the select list
    //            var suitableOperations = VwRfProductionDataEntryTab.SelectAll().Where(p => p.Active);

    //            //Select only operations with at least 1 active tab
    //            var operationsContext = _operationInfoProvider.GetOperationInfos()
    //                .Where(p => suitableOperations.Any(q => string.Equals(p.Operation, q.Operation, StringComparison.CurrentCultureIgnoreCase)));

    //            var operationInfo = operationsContext
    //                .Where(p => string.Equals(p.Operation, operation, StringComparison.CurrentCultureIgnoreCase))
    //                .FirstOrDefault();

    //            if (operationInfo == null)
    //            {
    //                throw new Exception("No operation context available for current page.  This may mean that no entry tab is configured/active.");
    //            }

    //            //var tab = string.IsNullOrEmpty(tabKey)
    //            //    ? VwRfProductionDataEntryTab.SelectDefault(operationInfo.Operation)
    //            //    : VwRfProductionDataEntryTab.FindActive(operationInfo.Operation, tabKey);

    //            //if (tab == null && !string.IsNullOrEmpty(tabKey))
    //            //{
    //            //    throw new Exception($"Specified tab '{tabKey}' does not exist for operation '{operationInfo.Operation}'");
    //            //}
    //            //if (tab == null)
    //            //{
    //            //    throw new Exception("No production entry tabs configured for this operation.");
    //            //}

    //            //var intervalType = (ProductionEntryIntervalType)Enum.Parse(typeof(ProductionEntryIntervalType), tab.IntervalType);

    //            //var operationConfig = new OperationConfiguration(operationInfo.Operation);

    //            //var defaultDate = false;
    //            //var defaultShift = false;

    //            //Process the date, default to current date
    //            //DateTime finalDateOp;
    //            //if (string.IsNullOrEmpty(dateOp) || !DateTime.TryParse(dateOp, out finalDateOp))
    //            //{
    //            //    defaultDate = true;
    //            //    finalDateOp = DateTime.SpecifyKind(DateTime.Now.Date, DateTimeKind.Unspecified);
    //            //}

    //            ////Process the shift string
    //            //var shiftInfos = operationInfo.GetShiftInfos(finalDateOp, finalDateOp);
    //            //var finalShift = Array.Find(shiftInfos, s => string.Equals(s.Shift, shift, StringComparison.CurrentCultureIgnoreCase));
    //            //if (finalShift == null)
    //            //{
    //            //    defaultShift = true;
    //            //    finalShift = shiftInfos.FirstOrDefault();
    //            //}

    //            //if (finalShift == null) throw new Exception("No shift information was found for specified operation");

    //            var shiftInstance = operationInfo.GetShift(record.DateOp, record.Shift);

    //            //if this date and shift were selected by default, and the shiftlag is not 0, then we want to offset the shift. 
    //            //if (operationConfig.EntryShiftLag > 0 && defaultDate && defaultShift)
    //            //{
    //            //    for (var i = 0; i < operationConfig.EntryShiftLag; i++)
    //            //        shiftInstance = operationInfo.GetPrevShift(shiftInstance);

    //            //    var newCurrentShift = shiftInstance.Shift;

    //            //    //update the previously selected defaults. 
    //            //    finalDateOp = shiftInstance.DateOp;
    //            //    finalShift = Array.Find(shiftInfos, s => string.Equals(s.Shift, newCurrentShift, StringComparison.CurrentCultureIgnoreCase));
    //            //}

    //            //ViewData["IntervalMode"] = intervalType == ProductionEntryIntervalType.Hourly;

    //            var entryIntervalFactory = new ProductionEntryIntervalFactory(_operationInfoProvider, _timezoneInfoProvider, _productionEntryUtility);

    //            var entryIntervalProvider = entryIntervalFactory.GetProvider(intervalType);

    //            var intervals = entryIntervalProvider.GetIntervals(shiftInstance.Operation, shiftInstance.DateOp, shiftInstance.Shift);

    //            //if (!bool.TryParse(showAll, out bool finalShowAll)) finalShowAll = false;

    //            //ViewData["ShowAllIntervals"] = finalShowAll;

    //            //Process the interval
    //            //int finalIntervalId;
    //            //if (string.IsNullOrEmpty(intervalId) || !int.TryParse(intervalId, out finalIntervalId))
    //            //    finalIntervalId = 0;

    //            ////if the show all box is checked, then we ignore the passed interval id
    //            //if (finalShowAll) finalIntervalId = -1;

    //            //get the related interval 
    //            ProductionEntryInterval entryInterval = null;
    //            //if (!finalShowAll)
    //            //{
    //            try
    //            {
    //                entryInterval = entryIntervalProvider.GetInterval(shiftInstance.Operation, shiftInstance.DateOp, shiftInstance.Shift, record.IntervalId);
    //            }
    //            catch (Exception)
    //            {
    //                //if a bad interval id is passed in, then default to the first interval for the shift.
    //                entryInterval = entryIntervalProvider.GetInterval(shiftInstance.Operation, shiftInstance.DateOp, shiftInstance.Shift, 0);
    //            }

    //                //ViewData["IntervalId"] = finalIntervalId;

    //            //}

    //            //ViewData["IntervalSelect"] = new SelectList(intervals, "IntervalId", "Label", finalIntervalId);

    //            //Data for shift selector view
    //            //ViewData["Operation"] = operationInfo.Operation;
    //            //if (operationsContext.Length > 1) ViewData["OperationSelect"] = new SelectList(operationsContext, "Operation", "Description", operationInfo.Description);
    //            //ViewData["ShiftSelect"] = new SelectList(shiftInfos, "Shift", "Label", finalShift.Shift);

    //            //ViewData["Shift"] = finalShift.Shift;
    //            //ViewData["DateOp"] = finalDateOp.ToShortDateString();

    //            //var includedFleetTypes = new[]
    //            //{
    //            //    FleetTypeValidator.HaulerFleetType, FleetTypeValidator.LoaderFleetType,
    //            //    FleetTypeValidator.ExcavatorFleetType, FleetTypeValidator.ShovelFleetType
    //            //};

    //            //var fleetTypes = Request.QueryString.GetValues("FleetType");
    //            ////Validate fleettypes, prepare select list
    //            //var fleetTypeRecords = Array.FindAll(RfFleetType.SelectAll(),
    //            //                                     f =>
    //            //                                     Array.Find(includedFleetTypes, i => Utility.CompareSqlStrings(i, f.FleetType)) != null);

    //            //var selectedFleetTypeRecords = fleetTypes != null && fleetTypes.Length != 0
    //            //                                   ? Array.FindAll(fleetTypeRecords, ur => Array.Find(fleetTypes, u => Utility.CompareSqlStrings(u, ur.FleetType)) != null) : fleetTypeRecords;

    //            //var fleetTypeSelect = new BetterCheckBoxList<RfFleetType>(fleetTypeRecords, "FleetType", "Description", selectedFleetTypeRecords);

    //            //ViewData["FleetTypeSelect"] = fleetTypeSelect;
    //            //ViewData["SelectedFleetTypes"] = Array.ConvertAll(selectedFleetTypeRecords, u => u.FleetType);

    //            //var fleetTypeUrl = "&FleetType=" + String.Join("&FleetType=", Array.ConvertAll(selectedFleetTypeRecords, u => u.FleetType));

    //            ////Next and previous shift links
    //            //if (Request.Url != null)
    //            //{
    //            //    var url = Request.Url.AbsolutePath;
    //            //    var nextShift = operationInfo.GetNextShift(shiftInstance);
    //            //    var prevShift = operationInfo.GetPrevShift(shiftInstance);

    //            //    ViewData["Url"] = url;

    //            //    var urlTail = string.Format("{0}&ShowAllIntervals={1}&tabKey={2}", fleetTypeUrl, finalShowAll, tabKey);

    //            //    if (intervalType == ProductionEntryIntervalType.Hourly && !finalShowAll && entryInterval != null)
    //            //    {
    //            //        var nextInterval = entryIntervalProvider.GetNextInterval(shiftInstance.Operation, shiftInstance.DateOp, shiftInstance.Shift, finalIntervalId);
    //            //        var prevInterval = entryIntervalProvider.GetPrevInterval(shiftInstance.Operation, shiftInstance.DateOp, shiftInstance.Shift, finalIntervalId);

    //            //        ViewData["NextIntervalUrl"] =
    //            //            string.Format("{0}?operation={1}&dateOp={2}&shift={3}&intervalId={4}{5}", url,
    //            //                          operationInfo.Operation, nextInterval.IntervalShift.DateOp.ToShortDateString(),
    //            //                          nextInterval.IntervalShift.Shift, nextInterval.IntervalId, urlTail);
    //            //        ViewData["PrevIntervalUrl"] =
    //            //            string.Format("{0}?operation={1}&dateOp={2}&shift={3}&intervalId={4}{5}", url,
    //            //                          operationInfo.Operation, prevInterval.IntervalShift.DateOp.ToShortDateString(),
    //            //                          prevInterval.IntervalShift.Shift, prevInterval.IntervalId, urlTail);
    //            //    }

    //            //    var defaultIntervalId = finalShowAll ? -1 : 0;

    //            //    ViewData["NextShiftUrl"] = string.Format("{0}?operation={1}&dateOp={2}&shift={3}&intervalId={4}{5}", url,
    //            //                                             operationInfo.Operation, nextShift.DateOp.ToShortDateString(),
    //            //                                             nextShift.Shift, defaultIntervalId, urlTail);
    //            //    ViewData["PrevShiftUrl"] = string.Format("{0}?operation={1}&dateOp={2}&shift={3}&intervalId={4}{5}", url,
    //            //                                             operationInfo.Operation, prevShift.DateOp.ToShortDateString(),
    //            //                                             prevShift.Shift, defaultIntervalId, urlTail);

    //            //    ViewData["NextDayUrl"] = string.Format("{0}?operation={1}&dateOp={2}&shift={3}&intervalId={4}{5}", url,
    //            //                             operationInfo.Operation, shiftInstance.DateOp.AddDays(1).ToShortDateString(),
    //            //                             shiftInstance.Shift, defaultIntervalId, urlTail);
    //            //    ViewData["PrevDayUrl"] = string.Format("{0}?operation={1}&dateOp={2}&shift={3}&intervalId={4}{5}", url,
    //            //                                             operationInfo.Operation, shiftInstance.DateOp.AddDays(-1).ToShortDateString(),
    //            //                                             shiftInstance.Shift, defaultIntervalId, urlTail);
    //            //}

    //            //const int maxLoads = 1000;

    //            //IOperationInfo operationInfo;
    //            //ShiftInstance shiftInstance;
    //            //IProductionEntryIntervalProvider entryIntervalProvider;
    //            //ProductionEntryInterval entryInterval;
    //            //ValidateArguments(out operationInfo, out shiftInstance, out entryIntervalProvider, out entryInterval);

    //            var tab = RfProductionDataEntryTab.Find(operation, tabKey);
    //            if (tab == null && !string.IsNullOrEmpty(tabKey))
    //            {
    //                throw new Exception($"Specified tab '{tabKey}' does not exist for operation '{operationInfo.Operation}'");
    //            }
    //            if (tab == null)
    //            {
    //                throw new Exception("No production entry tabs configured for this operation.");
    //            }

    //            var gradeItems = RfProductionDataEntryGrade.SelectAll(operation, tabKey).Select(g => g.GradeItem).ToArray();

    //            //var newValues = ObjectBinder.BindDictionary<FlexibleRowObject, int>(Request.Form, "NewValues");
    //            //var oldValues = ObjectBinder.BindDictionary<FlexibleRowObject, int>(Request.Form, "OldValues");

    //            var result = new GridActionResult();

    //            using (var connection = Utility.GetConnection<Cycle>())
    //            {
    //                using (var transaction = connection.BeginTransaction())
    //                {
    //                    // Get movement rows            
    //                    var rows = entryIntervalProvider.ConvertToMovementRows(shiftInstance, entryInterval, tabKey, newValues, oldValues);

    //                    // Basic validation
    //                    foreach (var rowIndex in newValues.Keys)
    //                    {
    //                        var loadsRaw = newValues[rowIndex].Result["Loads"];
    //                        var loads = !string.IsNullOrEmpty(loadsRaw) ? int.Parse(loadsRaw) : (int?)null;

    //                        // Maximum # of loads exceeded
    //                        if (loads > maxLoads)
    //                            newValues[rowIndex].ValidationErrors.Add(new ValidationError("Loads",
    //                                                                                         string.Format(
    //                                                                                             "Too many loads entered. Maximum {0} loads per record.",
    //                                                                                             maxLoads)));

    //                        // Automatic validation errors picked up e.g. data types etc
    //                        result.AddValidationErrors(rowIndex, newValues[rowIndex].ValidationErrors);

    //                        var row = rows.Where(r => r.Id == rowIndex).FirstOrDefault();

    //                        result.AddValidationErrors(rowIndex, entryIntervalProvider.ValidateMovementRow(connection, transaction, row));
    //                    }

    //                    if (!result.IsErrorFree() || result.RowResults.Exists(row => row.ValidationErrors.Count > 0))
    //                        return result;

    //                    foreach (var row in rows)
    //                    {
    //                        try
    //                        {

    //                            var deleteRow = row.OldValue != null ? row.OldValue : row;

    //                            var deleteGrades = new DeleteAll<CycleGrade>()
    //                                .WherePropertyEquals("Operation", deleteRow.Operation)
    //                                .WherePropertyEquals("Loader", deleteRow.Loader)
    //                                .WherePropertyEquals("Hauler", deleteRow.Hauler)
    //                                .WherePropertyEquals("LoaderOperatorId", deleteRow.LoaderOperatorId)
    //                                .WherePropertyEquals("HaulerOperatorId", deleteRow.HaulerOperatorId)
    //                                .WherePropertyEquals("Origin", deleteRow.Origin)
    //                                .WherePropertyEquals("Destination", deleteRow.Destination)
    //                                .WherePropertyEquals("Material", deleteRow.Material)
    //                                .WherePropertyEquals("DateOp", deleteRow.DateOp)
    //                                .WherePropertyEquals("Shift", deleteRow.Shift)
    //                                .WherePropertyEquals("Datasource", deleteRow.Datasource)
    //                                .WherePropertyIn("GradeItem", gradeItems);

    //                            if (deleteRow.DateTimeStart.HasValue)
    //                                deleteGrades.WherePropertyEquals("DateTimeStart", deleteRow.DateTimeStart.Value);

    //                            deleteGrades.Execute(connection, transaction);

    //                            var deleteCycles = new DeleteAll<Cycle>()
    //                                .WherePropertyEquals("Operation", deleteRow.Operation)
    //                                .WherePropertyEquals("Loader", deleteRow.Loader)
    //                                .WherePropertyEquals("Hauler", deleteRow.Hauler)
    //                                .WherePropertyEquals("LoaderOperatorId", deleteRow.LoaderOperatorId)
    //                                .WherePropertyEquals("HaulerOperatorId", deleteRow.HaulerOperatorId)
    //                                .WherePropertyEquals("Origin", deleteRow.Origin)
    //                                .WherePropertyEquals("Destination", deleteRow.Destination)
    //                                .WherePropertyEquals("Material", deleteRow.Material)
    //                                .WherePropertyEquals("DateOp", deleteRow.DateOp)
    //                                .WherePropertyEquals("Shift", deleteRow.Shift)
    //                                .WherePropertyEquals("Datasource", deleteRow.Datasource);

    //                            if (deleteRow.DateTimeStart.HasValue)
    //                                deleteCycles.WherePropertyEquals("DateTimeStart", deleteRow.DateTimeStart.Value);

    //                            deleteCycles.Execute(connection, transaction);

    //                            int cycleCount = 0;

    //                            foreach (var cycle in row.Cycles)
    //                            {
    //                                cycleCount++;
    //                                Utility.SaveSafe(connection, transaction, cycle);

    //                                foreach (var gradeItem in cycle.GradeValues)
    //                                {
    //                                    Utility.SaveSafe(connection, transaction, CreateCycleGrade(cycle, gradeItem.Key, gradeItem.Value));
    //                                    if (cycleCount.Equals(row.Cycles.Count()) && !string.IsNullOrEmpty(tab.SourceGradeType))
    //                                    {
    //                                        var grade = new SelectAll<VwProductionDataEntryGrade>()
    //                                            .WherePropertyEquals("Operation", cycle.Operation)
    //                                            .WherePropertyEquals("DateOp", cycle.DateOp)
    //                                            .WherePropertyEquals("Origin", cycle.Origin)
    //                                            .WherePropertyEquals("Material", cycle.Material)
    //                                            .WherePropertyEquals("GradeItem", gradeItem.Key)
    //                                            .First(connection, transaction);

    //                                        Utility.SaveSafe(connection, transaction, CreateGrade(cycle, tab.SourceGradeType, grade));
    //                                    }
    //                                }
    //                            }

    //                            result.AddRowSaved(row.Id);
    //                            result.AddRowUpdated(row.Id, new FlexibleRowObject(row));

    //                            ManualDataEntryLog.Log(transaction,
    //                                row.IsNew ? ManualDataEntryLog.AdditionLogType : ManualDataEntryLog.UpdateLogType, shiftInstance.Operation,
    //                                LoadAndHaulManualEntryLog.LogArea, LoadAndHaulManualEntryLog.LogGroup, "Production Entry", shiftInstance.DateOp, shiftInstance.Shift, null, "Data edited.");
    //                        }
    //                        catch (Exception ex)
    //                        {
    //                            result.AddRowError(row.Id, GridRowError.Make(ex));
    //                        }
    //                    }

    //                    if (!result.IsErrorFree() || result.RowResults.Exists(row => row.ValidationErrors.Count > 0))
    //                        return result;

    //                    // Add record to ManualDataEntryProcesQueue to recalculate the grades
    //                    Utility.SaveSafe(connection, transaction, new ManualDataEntryProcessQueue() { Operation = shiftInstance.Operation, DateOp = shiftInstance.DateOp, DateTimeAdded = DateTime.UtcNow });

    //                    transaction.Commit();
    //                }
    //            }

    //            return result;
    //        }

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