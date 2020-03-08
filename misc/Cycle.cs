using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using MIS.DAL;
using MIS.DAL.Helpers;
using MIS.IsPlatform.Framework.EntryInterval;
using MIS.IsPlatform.Framework.Grid;
using MIS.IsPlatform.Interface.Areas.Common.Business.EntryInterval;
using MIS.IsPlatform.Interface.Areas.LoadAndHaulDataEntry.Business;
using MIS.IsPlatform.Interface.Areas.LoadAndHaulDataEntry.Business.ProductionEntryInterval;
using MIS.IsPlatform.Interface.Areas.MMRSDataEntryHelper.Business;
using MIS.IsPlatform.Interface.Areas.MMRSReferenceData.Business;

namespace MIS.IsPlatform.Interface.Areas.LoadAndHaulDataEntry.DAL {
  [Mapping (Table = "mmrs.CYCLE", ConnectionString = "MMRS")]
  public class Cycle : DALType, IMovementTable {
    [MappedProperty (Field = "DATEOP", PrimaryKey = true)]
    public DateTime DateOp { get { return GetData<DateTime> ("DateOp"); } set { SetData ("DateOp", value); } }

    [MappedProperty (Field = "DATETIMESTART", PrimaryKey = true)]
    public DateTime DateTimeStart { get { return GetData<DateTime> ("DateTimeStart"); } set { SetData ("DateTimeStart", value); } }

    [MappedProperty (Field = "OPERATION", PrimaryKey = true, Length = 8, NotNullString = true)]
    public string Operation { get { return GetData<string> ("Operation"); } set { SetData ("Operation", value); } }

    [MappedProperty (Field = "LOADER", PrimaryKey = true, Length = 8, NotNullString = true)]
    public string Loader { get { return GetData<string> ("Loader"); } set { SetData ("Loader", value); } }

    [MappedProperty (Field = "HAULER", PrimaryKey = true, Length = 8, NotNullString = true)]
    public string Hauler { get { return GetData<string> ("Hauler"); } set { SetData ("Hauler", value); } }

    [MappedProperty (Field = "LOADEROPERATORID", PrimaryKey = true, Length = 8, NotNullString = true)]
    public string LoaderOperatorId { get { return GetData<string> ("LoaderOperatorId"); } set { SetData ("LoaderOperatorId", value); } }

    [MappedProperty (Field = "HAULEROPERATORID", PrimaryKey = true, Length = 8, NotNullString = true)]
    public string HaulerOperatorId { get { return GetData<string> ("HaulerOperatorId"); } set { SetData ("HaulerOperatorId", value); } }

    [MappedProperty (Field = "ORIGIN", PrimaryKey = true, Length = 32, NotNullString = true)]
    public string Origin { get { return GetData<string> ("Origin"); } set { SetData ("Origin", value); } }

    [MappedProperty (Field = "DESTINATION", PrimaryKey = true, Length = 32, NotNullString = true)]
    public string Destination { get { return GetData<string> ("Destination"); } set { SetData ("Destination", value); } }

    [MappedProperty (Field = "MATERIAL", PrimaryKey = true, Length = 8, NotNullString = true)]
    public string Material { get { return GetData<string> ("Material"); } set { SetData ("Material", value); } }

    [MappedProperty (Field = "DATASOURCE", Length = 2, NotNullString = true)]
    public string Datasource { get { return GetData<string> ("Datasource"); } set { SetData ("Datasource", value); } }

    [MappedProperty (Field = "SHIFT", Length = 1, NotNullString = true)]
    public string Shift { get { return GetData<string> ("Shift"); } set { SetData ("Shift", value); } }

    [MappedProperty (Field = "PAYLOADWEIGHT")]
    public double? PayloadWeight { get { return GetData<double?> ("PayloadWeight"); } set { SetData ("PayloadWeight", value); } }

    [MappedProperty (Field = "PAYLOADVOLUME")]
    public double? PayloadVolume { get { return GetData<double?> ("PayloadVolume"); } set { SetData ("PayloadVolume", value); } }

    [MappedProperty (Field = "NOMINALWEIGHT")]
    public double? NominalWeight { get { return GetData<double?> ("NominalWeight"); } set { SetData ("NominalWeight", value); } }

    [MappedProperty (Field = "NOMINALVOLUME")]
    public double? NominalVolume { get { return GetData<double?> ("NominalVolume"); } set { SetData ("NominalVolume", value); } }

    //Field depricated. DO NOT USE. 
    //[MappedProperty(Field = "WEIGHT")]
    //public double? Weight { get { return GetData<double?>("Weight"); } set { SetData("Weight", value); } }

    //Field depricated. DO NOT USE. 
    //[MappedProperty(Field = "VOLUME")]
    //public double? Volume { get { return GetData<double?>("Volume"); } set { SetData("Volume", value); } }

    [MappedProperty (Field = "BULKDENSITY")]
    public double? BulkDensity { get { return GetData<double?> ("BulkDensity"); } set { SetData ("BulkDensity", value); } }

    [MappedProperty (Field = "LOADS")]
    public int? Loads { get { return GetData<int?> ("Loads"); } set { SetData ("Loads", value); } }

    [MappedProperty (Field = "PRODUCTIONTYPE", Length = 8, NotNullString = true)]
    public string ProductionType { get { return GetData<string> ("ProductionType"); } set { SetData ("ProductionType", value); } }

    [MappedProperty (Field = "CYCLETYPE", Length = 8)]
    public string CycleType { get { return GetData<string> ("CycleType"); } set { SetData ("CycleType", value); } }

    [MappedProperty (Field = "DATETIMEFINISH")]
    public DateTime? DateTimeFinish { get { return GetData<DateTime?> ("DateTimeFinish"); } set { SetData ("DateTimeFinish", value); } }

    [MappedProperty (Field = "DATETIMELOADSTART")]
    public DateTime? DateTimeLoadStart { get { return GetData<DateTime?> ("DateTimeFinish"); } set { SetData ("DateTimeLoadStart", value); } }

    [MappedProperty (Field = "DATETIMELOADFINISH")]
    public DateTime? DateTimeLoadFinish { get { return GetData<DateTime?> ("DateTimeLoadFinish"); } set { SetData ("DateTimeLoadFinish", value); } }

    [MappedProperty (Field = "DATETIMEDUMPSTART")]
    public DateTime? DateTimeDumpStart { get { return GetData<DateTime?> ("DateTimeDumpStart"); } set { SetData ("DateTimeDumpStart", value); } }

    [MappedProperty (Field = "DATETIMEDUMPFINISH")]
    public DateTime? DateTimeDumpFinish { get { return GetData<DateTime?> ("DateTimeDumpFinish"); } set { SetData ("DateTimeDumpFinish", value); } }

    [MappedProperty (Field = "DISTANCEFORWARD")]
    public double? DistanceForward { get { return GetData<double?> ("DistanceForward"); } set { SetData ("DistanceForward", value); } }

    [MappedProperty (Field = "WEIGHTCALCMETHOD")]
    public int? WeightCalcMethod { get { return GetData<int?> ("WeightCalcMethod"); } set { SetData ("WeightCalcMethod", value); } }

    [MappedProperty (Field = "VOLUMECALCMETHOD")]
    public int? VolumeCalcMethod { get { return GetData<int?> ("VolumeCalcMethod"); } set { SetData ("VolumeCalcMethod", value); } }

    [MappedProperty (Field = "DATETIMEMODIFIED")]
    public DateTime DateTimeModified { get { return GetData<DateTime> ("DateTimeModified"); } set { SetData ("DateTimeModified", value); } }

    [MappedProperty (Field = "MODIFIEDBY", Length = 64, NotNullString = false)]
    public string ModifiedBy { get { return GetData<string> ("ModifiedBy"); } set { SetData ("ModifiedBy", value); } }

    [MappedProperty (Field = "DATAENTRYTAB", Length = 4)]
    public string DataEntryTab { get { return GetData<string> ("DataEntryTab"); } set { SetData ("DataEntryTab", value); } }

    [MappedProperty (Field = "REPORTADJUSTMENT")]
    public double? ReportAdjustment { get { return GetData<double?> ("ReportAdjustment"); } set { SetData ("ReportAdjustment", value); } }

    [MappedProperty (Field = "NOMINALADJUSTEDWEIGHT")]
    public double? NominalAdjustedWeight { get { return GetData<double?> ("NominalAdjustedWeight"); } set { SetData ("NominalAdjustedWeight", value); } }

    [MappedProperty (Field = "MOVEMENTKEY", Computed = true)]
    public byte[] MovementKey { get { return GetData<byte[]> ("MovementKey"); } set { SetData ("MovementKey", value); } }

    public Dictionary<string, double?> GradeValues { get; set; }

    public int IntervalId { get; set; }

    // Temporary masking fields
    public string IntervalIdMask { get; set; }
    public string LoaderOperatorMask { get; set; }
    public string HaulerOperatorMask { get; set; }

    //note that if intervalid is null, it should select the whole shift
    public static Cycle[] Select (string operation, DateTime dateOp, string shift, ProductionEntryInterval entryInterval) {
      if (entryInterval == null)
        return Select (operation, dateOp, shift);

      using (var connection = Utility.GetConnection<Cycle> ()) {
        return new SelectAll<Cycle> ()
          .WherePropertyEquals ("Operation", operation)
          .WherePropertyBetween ("DateTimeStart", entryInterval.IntervalStartUtc, entryInterval.IntervalEndUtc)
          .WherePropertyEquals ("Datasource", DatasourceValidator.ManualEntry)
          .Execute (connection);
      }
    }

    public static Cycle[] Select (string operation, DateTime dateOp, string shift) {
      using (var connection = Utility.GetConnection<Cycle> ()) {
        return new SelectAll<Cycle> ()
          .WherePropertyEquals ("Operation", operation)
          .WherePropertyEquals ("DateOp", dateOp)
          .WherePropertyEquals ("Shift", shift)
          .WherePropertyEquals ("Datasource", DatasourceValidator.ManualEntry)
          .Execute (connection);
      }
    }

    public static Cycle[] Select (SqlConnection connection, SqlTransaction transaction, string operation, DateTime dateOp, string shift) {
      // Only select cycle records that have relevant tab keys to Load and Haul
      var allProdEntryTabs = VwRfProductionDataEntryTab.SelectAllActive (operation);
      var tabKeys = Array.ConvertAll (allProdEntryTabs, t => t.DataEntryTab);

      int totalRows;

      return new SelectAll<Cycle> ()
        .WherePropertyEquals ("Operation", operation)
        .WherePropertyEquals ("DateOp", dateOp)
        .WherePropertyEquals ("Shift", shift)
        .WherePropertyEquals ("Datasource", DatasourceValidator.ManualEntry)
        .WherePropertyIn ("DataEntryTab", tabKeys)
        .Execute (connection, transaction, out totalRows);
    }

    public static Cycle Select (SqlConnection connection, SqlTransaction transaction, MovementRow row) {
      return new SelectAll<Cycle> ()
        .WherePropertyEquals ("DateOp", row.DateOp)
        .WherePropertyEquals ("Shift", row.Shift)
        .WherePropertyEquals ("Operation", row.Operation)
        .WherePropertyEquals ("Loader", row.Loader)
        .WherePropertyEquals ("Hauler", row.Hauler)
        .WherePropertyEquals ("LoaderOperatorId", row.LoaderOperatorId)
        .WherePropertyEquals ("HaulerOperatorId", row.HaulerOperatorId)
        .WherePropertyEquals ("Origin", row.Origin)
        .WherePropertyEquals ("Destination", row.Destination)
        .WherePropertyEquals ("Material", row.Material)
        .WherePropertyEquals ("Datasource", row.Datasource)
        .WherePropertyEquals ("ProductionType", row.ProductionType)
        .First (connection, transaction);
    }

    public static Cycle[] Select (string operation, DateTime dateOp, string shift, string tabKey, ColumnFilter[] filters, SortBy[] sortBy) {
      using (var connection = Utility.GetConnection<Cycle> ()) {
        var list = new SelectAll<Cycle> ()
          .WherePropertyEquals ("Operation", operation)
          .WherePropertyEquals ("DateOp", dateOp)
          .WherePropertyEquals ("Shift", shift)
          .WherePropertyEquals ("DataEntryTab", tabKey)
          .WherePropertyEquals ("Datasource", DatasourceValidator.ManualEntry);

        ColumnFilter[] unknownFilters;
        GridHelpers.ApplyFiltersToSelectAll (list, new [] { new UnitHierarchyImplementation (new [] { operation }) }, filters, out unknownFilters);
        GridHelpers.ApplySortByToSelectAll (list, sortBy, "DateTimeModified");

        return list.Execute (connection);
      }
    }

    public static Cycle[] SelectByIntervalShift (
      string operation,
      ShiftInterval shiftInterval,
      string tabKey,
      ColumnFilter[] filters,
      SortBy[] sortBy) {
      using (var connection = Utility.GetConnection<Cycle> ()) {
        var list = new SelectAll<Cycle> ()
          .WherePropertyEquals ("Operation", operation)
          .WherePropertyEquals ("DateOp", shiftInterval.DateOp)
          .WherePropertyEquals ("Shift", shiftInterval.Shift)
          .WherePropertyBetween ("DateTimeStart", shiftInterval.GetIntervalStartTimeUtc (operation), shiftInterval.GetIntervalEndTimeUtc (operation))
          .WherePropertyEquals ("DataEntryTab", tabKey)
          .WherePropertyEquals ("Datasource", DatasourceValidator.ManualEntry);

        GridHelpers.ApplyFiltersToSelectAll (
          list,
          new [] {
            new UnitHierarchyImplementation (new [] { operation })
          },
          filters,
          out ColumnFilter[] unknownFilters);

        GridHelpers.ApplySortByToSelectAll (list, sortBy, "DateTimeModified");

        return list.Execute (connection);
      }
    }

    //note that if the interval id passed through is null, it returns the whole shift. 
    public static Cycle[] SelectByInterval (
      string operation,
      DateTime dateOp,
      string shift,
      string tabKey,
      ProductionEntryInterval entryInterval,
      ColumnFilter[] filters,
      SortBy[] sortBy) {
      if (entryInterval == null)
        return Select (operation, dateOp, shift, tabKey, filters, sortBy);

      using (var connection = Utility.GetConnection<Cycle> ()) {
        var list = new SelectAll<Cycle> ()
          .WherePropertyEquals ("Operation", operation)
          .WherePropertyEquals ("DateOp", dateOp)
          .WherePropertyEquals ("Shift", shift)
          .WherePropertyBetween ("DateTimeStart", entryInterval.IntervalStartUtc, entryInterval.IntervalEndUtc)
          .WherePropertyEquals ("DataEntryTab", tabKey)
          .WherePropertyEquals ("Datasource", DatasourceValidator.ManualEntry);

        ColumnFilter[] unknownFilters;
        GridHelpers.ApplyFiltersToSelectAll (list, new [] { new UnitHierarchyImplementation (new [] { operation }) }, filters, out unknownFilters);
        GridHelpers.ApplySortByToSelectAll (list, sortBy, "DateTimeModified");

        return list.Execute (connection);

      }
    }

    public static Cycle[] SelectByProduction (SqlConnection conn, SqlTransaction trans, Production prod) {
      int totalRows;

      return new SelectAll<Cycle> ()
        .WherePropertyEquals ("Operation", prod.Operation)
        .WherePropertyEquals ("Loader", prod.Loader)
        .WherePropertyEquals ("Hauler", prod.Hauler)
        .WherePropertyEquals ("LoaderOperatorId", prod.LoaderOperatorId)
        .WherePropertyEquals ("HaulerOperatorId", prod.HaulerOperatorId)
        .WherePropertyEquals ("Origin", prod.Origin)
        .WherePropertyEquals ("Destination", prod.Destination)
        .WherePropertyEquals ("Material", prod.Material)
        .WherePropertyEquals ("DateOp", prod.DateOp)
        .WherePropertyEquals ("Shift", prod.Shift)
        .WherePropertyEquals ("Datasource", prod.Datasource)
        .Execute (conn, trans, out totalRows);

    }

    public static Cycle[] SelectByPrimaryKey (DateTime dateTimestart, string operation, string loader, string hauler, string loaderOperatorId, string haulerOperatorId, string origin, string destination, string material, DateTime dateop) {

      using (var connection = Utility.GetConnection<Cycle> ()) {
        var list = new SelectAll<Cycle> ()
          .WherePropertyEquals ("Operation", operation)
          .WherePropertyEquals ("Loader", loader)
          .WherePropertyEquals ("Hauler", hauler)
          .WherePropertyEquals ("LoaderOperatorId", loaderOperatorId)
          .WherePropertyEquals ("HaulerOperatorId", haulerOperatorId)
          .WherePropertyEquals ("Origin", origin)
          .WherePropertyEquals ("Destination", destination)
          .WherePropertyEquals ("Material", material)
          .WherePropertyEquals ("DateOp", dateop);

        return list.Execute (connection);
      }
    }

  }
}