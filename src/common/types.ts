interface ShiftDetailDtc {
  Operation: string;
  StartDate: Date;
  Shift: string;
  Description: string;
  ShiftStart: number;
  ScheduledDuration: number;
}

interface ShiftSetDtc {
  StartDate: Date;
  Shifts: ShiftDetailDtc[];
}

export interface OperationDtc {
  Operation: string;
  Description: string;
  TimeZone: string;
  DaylightSavingCode: string;
  OperatingSpaceId?: string;
  ShiftSets: ShiftSetDtc[];
}

export interface DataEntryRecord {
  Loader: string;
  LoaderOperatorId: string;
  Origin: string;
  Material: string;
  Destination: string;
  Loads?: number;
  NominalVolume?: number;
  NominalWeight?: number;
  DateTimeModified?: Date;
  ModifiedBy?: string;
  Warning?: string;
}
