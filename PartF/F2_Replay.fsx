#load "F1_Inventory.fsx"

type undefined = exn

module Replay =
    open F1_Inventory.Inventory

    type InventoryState = undefined

    type ApplyEvent = InventoryState -> StockroomEvent -> InventoryState
    type ExecuteCommand = StockroomCommand -> InventoryState -> StockroomEvent list
    type HandleCommand = StockroomCommand -> unit
