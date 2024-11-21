[<- Back to Main](../README.md)

# Part F: Event Sourcing

Now, let's look at tracking inventory changes in the kitchen's stockroom.

## Exercise F1: Stockroom

Before we start defining the types of commands that can be taken in the stockroom, as well as the events that those commands trigger, let's first define some types:

```
module Inventory =
    type ItemBarcode = ItemBarcode of string
    type EmployeeId = EmployeeId of string
    type Timestamp = Timestamp of System.DateTime

    type InventoryChange =
        { Barcode: ItemBarcode
          StaffMember: EmployeeId
          Timestamp: Timestamp }
```

First, we define the `ItemBarcode`, `EmployeeId`, and `Timestamp` as these fields constitute the `InventoryChange` that is required by any actions related to inventory.

Then, we define the command and event themselves:

```
    type StockroomCommand =
        | AddToInventory of InventoryChange
        | UseFromInventory of InventoryChange
        | DiscardFromInventory of InventoryChange

    type StockroomEvent =
        | AddedToInventory of InventoryChange
        | UsedFromInventory of InventoryChange
        | DiscardedFromInventory of InventoryChange

```

Notice that the states defined in `StockroomCommand` are in the future tense - that's because we will be applying these commands to the state so that we may change the state. Similarly, `StockroomEvent` are in the past tense because we use events to replay the state both prior to executing the commands (if there were events prior to executing the commands), and apply the events triggered by the commands.

## Exercise F2: Replaying Events

Now, let's define the function signatures required to apply events to the state, execute commands against the state, and handle the commands:

```
type undefined = exn

module Replay =
    open F1_Inventory.Inventory

    type InventoryState = undefined

    type ApplyEvent = InventoryState -> StockroomEvent -> InventoryState
    type ExecuteCommand = StockroomCommand -> InventoryState -> StockroomEvent list
    type HandleCommand = StockroomCommand -> unit
```

First, we define the `InventoryState` type, which we will leave as `undefined`. What we would do is define an `eventStore` to store all the inventory events in. When we want to apply a command to the state, we would first need to determine what the current state of the system is.

To do this, `ApplyEvent` takes in the `InventoryState` and all the events that have taken place in the system, which is stored in the `eventStore`. We would iterate through this store, and for each event, call the `applyEvent`. The function then returns the state the system is in after applying the function. It will keep doing this for all the events in the `eventStore`, until it's done. Then the system is in the current state.

The `ExecuteCommand` takes the changes we want to make to the current state, as well as the current state of the system, and returns a list of `StockroomEvents` that will occur as a result of the command having been applied.

Finally, the `HandleCommand` is what orchestrates executing the command. It is the function that takes in the command to be executed against the current state, takes all the events from the event store and called `ApplyEvent` on each event to bring the state to the current state, and then calls the `ExecuteCommand` against the current state. It then adds the list of events output from that function back into the event store. It doesn't return anything or, in other words, `unit`.

In our case, suppose we want to discard an item from inventory. We would call `HandleCommand`, passing in `DiscardFromInventory` and the `Barcode` of the item, the `StaffMember` who wants to discard the item, and the `Timestamp` at which that barcode was scanned. It would then call `ApplyEvent` on all the events in the `eventStore`, until it reaches the current state. If the item hasn't been added to inventory, it might cause an issue. Otherwise, it can be safely thrown away. It will then store the `DiscardedFromInventory` event into the `eventStore`, and not need to return anything.

[<- Back to Main](../README.md)
