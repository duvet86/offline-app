import Dexie from "dexie";
import { DataEntryRecord } from "../common/types";

export default class DataEntryDatabase extends Dexie {
  // Declare implicit table properties.
  // (just to inform Typescript. Instanciated by Dexie in stores() method)
  public formData: Dexie.Table<DataEntryRecord, number>; // number = type of the primkey
  //...other tables goes here...

  constructor() {
    super("DataEntryDatabase");

    this.version(1).stores({
      formData:
        "&[operation+loader+operator+source+material+destination], operation"
      //...other tables goes here...
    });

    // The following line is needed if your typescript
    // is compiled using babel instead of tsc:
    this.formData = this.table("formData");
  }
}
