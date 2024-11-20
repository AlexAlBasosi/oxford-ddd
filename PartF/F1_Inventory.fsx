module Inventory =
    type ItemBarcode = ItemBarcode of string
    type EmployeeId = EmployeeId of string
    type Timestamp = Timestamp of System.DateTime

    type InventoryChange =
        { Barcode: ItemBarcode
          StaffMember: EmployeeId
          Timestamp: Timestamp }

    type StockroomCommand =
        | AddToInventory of InventoryChange
        | UseFromInventory of InventoryChange
        | DiscardFromInventory of InventoryChange

    type StockroomEvent =
        | AddedToInventory of InventoryChange
        | UsedFromInventory of InventoryChange
        | DiscardedFromInventory of InventoryChange
