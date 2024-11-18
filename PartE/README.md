[<- Back to Main](../README.md)

# Part E: Error Handling

To model the steps that a robot would take to go through the steps to make a pizza, let's start off by creating some types:

```
module RobotState =
    type RobotStateData = string

    type RobotState =
        | NewRobotState of RobotStateData
        | OwningTicket of RobotStateData
        | AssemblingPizza of RobotStateData
        | PuttingInOven of RobotStateData
        | WaitingUntilDone of RobotStateData
        | TakingOutOfOven of RobotStateData
        | ScanningTicket of RobotStateData

    type OwnTicket = RobotState -> RobotState
    type AssemblePizza = RobotState -> RobotState
    type PutInOven = RobotState -> RobotState
    type WaitUntilDone = RobotState -> RobotState
    type TakeOutOfOven = RobotState -> RobotState
    type ScanTicket = RobotState -> RobotState
```

First, we create the `RobotStateData` type as a string, which will be used by the `RobotState` to store any additional information about the state.

Then we model the `RobotState` as a union type of various states, which indicate the current step the robot is taking to make the pizza.

Then we create a few types to model the transitions between each state. Notice that at each step it goes from a `RobotState` to another `RobotState` until the pizza is made.

Now, let's add some implementation of these functions. First we create a helper function that extracts the `RobotStateData` from the `RobotState`:

```
    let extractRobotStateData =
        fun (robotState: RobotState) ->
            match robotState with
            | NewRobotState stateData -> stateData
            | OwningTicket stateData -> stateData
            | AssemblingPizza stateData -> stateData
            | PuttingInOven stateData -> stateData
            | WaitingUntilDone stateData -> stateData
            | TakingOutOfOven stateData -> stateData
            | ScanningTicket stateData -> stateData
```

It matches the current state the robot is in and returns its `RobotStateData`.

Now let's implement the rest of the functions:

```
    let ownTicket: OwnTicket =
        fun (state: RobotState) ->
            let robotStateData: RobotStateData = extractRobotStateData state
            OwningTicket robotStateData

    let assemblePizza: AssemblePizza =
        fun (state: RobotState) ->
            let robotStateData: RobotStateData = extractRobotStateData state
            AssemblingPizza robotStateData

    let putInOven: PutInOven =
        fun (state: RobotState) ->
            let robotStateData: RobotStateData = extractRobotStateData state
            PuttingInOven robotStateData

    let waitUntilDone: WaitUntilDone =
        fun (state: RobotState) ->
            let robotStateData: RobotStateData = extractRobotStateData state
            WaitingUntilDone robotStateData

    let takeOutOfOven: TakeOutOfOven =
        fun (state: RobotState) ->
            let robotStateData: RobotStateData = extractRobotStateData state
            TakingOutOfOven robotStateData

    let scanTicket: ScanTicket =
        fun (state: RobotState) ->
            let robotStateData: RobotStateData = extractRobotStateData state
            ScanningTicket robotStateData
```

At each step in the process, the `RobotStateData` is extracted from the `RobotState`, and then wrapped in the next state to be passed along to the next `RobotState` in the pipeline.

Now we can implement the pipeline so that it looks like the following:

```
module TestSuite =
    open E_RobotState.RobotState

    let newRobotState: RobotState = NewRobotState ""

    newRobotState
    |> ownTicket
    |> assemblePizza
    |> putInOven
    |> waitUntilDone
    |> takeOutOfOven
    |> scanTicket
```

First we initialise the `newRobotState`, passing in `""` as the `RobotStateData`, then we pass it into a pipeline that takes it through all the states, calling the functions we just implemented.

## Exercise E1: Errors in Assemble

To account for errors encountered while assembling a pizza, let's start by creating a type to handle the different types of errors:

```
module RobotErrors =
    type AssembleError =
        | NotEnoughDough
        | NotEnoughSauce
```

We define the `AssembleError` type, which can either be in the `NotEnoughDough` state or the `NotEnoughSauce` state.

Before we update out `AssemblePizzaState` implementation, let's add some error information to the `RobotStateData` to simulate whether we will have any errors during the pizza assembly process:

```
type RobotStateData = { AssembleError: AssembleError option }
```

Now, we need to update our `AssemblePizza` type. The logic we want here is for the function to return `RobotState` if there are no errors during the assembly process, and `AssembleError` otherwise.

Here's what our new type will look like:

```
type AssemblePizza = RobotState -> Result<RobotState, AssembleError>
```

It will return a `Result` type which will either be `RobotState` or `AssembleError`.

Here is the implementation:

```
let assemblePizza: AssemblePizza =
    fun (state: RobotState) ->
        let robotStateData: RobotStateData = extractRobotStateData state
        let assemblingPizzaState: RobotState = AssemblingPizza robotStateData

        match robotStateData.AssembleError with
        | Some NotEnoughDough -> Error NotEnoughDough
        | Some NotEnoughSauce -> Error NotEnoughSauce
        | None -> Ok assemblingPizzaState
```

First, we extract the `RobotStateData` from the state, and then define the new `RobotState` we want to return.

Then, we check the `RobotStateData` to see if there's any errors passed in the state.

If there is `NotEnoughDough` or `NotEnoughSauce`, it will return an error with its corresponding `AssembleError` state. Otherwise it returns `Ok` with its corresponding `RobotState`.

## Exercise E2: Errors in PutInOven

To account for errors encountered while putting pizzas in the oven, let's model those errors too:

```
type OvenError =
    | NoOvens
    | TechnicalFault
```

`OvenError` can either be in the `NoOvens` or `TechnicalFault` states.

Now we can update the `RobotStateData` to include that error:

```
type RobotStateData =
    { AssembleError: AssembleError option
        OvenError: OvenError option }
```

And the `PutInOven` method to handle the different `OvenError`s:

```
type PutInOven = RobotState -> Result<RobotState, OvenError>

let putInOven: PutInOven =
    fun (state: RobotState) ->
        let robotStateData: RobotStateData = extractRobotStateData state
        let puttingInOvenState: RobotState = PuttingInOven robotStateData

        match robotStateData.OvenError with
        | Some NoOvens -> Error NoOvens
        | Some TechnicalFault -> Error TechnicalFault
        | None -> Ok puttingInOvenState

```

First we update the `PutInOven` type to return `Result<RobotState, OvenError>`, then we update the implementation to check the `RobotStateData` to see if there are any `OvenError`s. If there are, return an `Error` with the corresponding state, otherwise return `Ok` with the `RobotState` it corresponds to.

## Exercise E3: Unifying Errors

To unify our current types into a single error type, we define `KitchenError` like this:

```
type KitchenError =
    | Assemble of AssembleError
    | Oven of OvenError
```

The `Assemble` state includes all errors related to `AssembleError` and `Oven` includes all errors related to `OvenError`.

For the implementation, first we update the `RobotStateData` to include our new `KitchenError`:

```
type RobotStateData =
    { AssembleError: AssembleError option
        OvenError: OvenError option
        KitchenError: KitchenError option }
```

Then we create new function types for our updated functions:

```
type AssemblePizza_v2 = RobotState -> Result<RobotState, KitchenError>
type PutInOven_v2 = RobotState -> Result<RobotState, KitchenError>
```

> Notice how they both return the same type now, `Result<RobotState, KitchenError>`.

And finally, we create the function defitions themselves. Here is the new `assemblePizza_v2`:

```
let assemblePizza_v2: AssemblePizza_v2 =
    fun (state: RobotState) ->
        let robotStateData: RobotStateData = extractRobotStateData state
        let assemblingPizzaState: RobotState = AssemblingPizza robotStateData

        match robotStateData.KitchenError with
        | Some(Assemble assembleError) ->
            match assembleError with
            | NotEnoughDough -> Error(Assemble NotEnoughDough)
            | NotEnoughSauce -> Error(Assemble NotEnoughSauce)
        | None -> Ok assemblingPizzaState
```

Now, it matches `KitchenStateError` with `Assemble`, and then does a further match within `Assemble` to extract the specific error that occurred during the assembly process. It then returns that as an `Error`, passing in `NotEnoughDough` or `NotEnoughSauce`.

Here is the new `putInOven_v2`:

```
let putInOven_v2: PutInOven_v2 =
    fun (state: RobotState) ->
        let robotStateData: RobotStateData = extractRobotStateData state
        let puttingInOvenState: RobotState = PuttingInOven robotStateData

        match robotStateData.KitchenError with
        | Some(Oven ovenError) ->
            match ovenError with
            | NoOvens -> Error(Oven NoOvens)
            | TechnicalFault -> Error(Oven TechnicalFault)
        | None -> Ok puttingInOvenState
```

This also matches `KitchenStateError`, but in this function it also matches with `Oven` and extracts the relevant error, either `NoOven` or `TechnicalFault`, and returns that as an `Error`.

## Exercise E4: Using Result.bind and Result.map

Now that we've updated our `assemblePizza` and `putInOven` functions to have two outputs, our pipeline is going to fail, as `putInOven` is expecting one input, and `waitUntilDone`, `takeOutOfOven`, and `scanTicket` are all expecting one input.

If we try to compile this:

```
newRobotState
|> ownTicket
|> assemblePizza_v2
|> putInOven_v2
|> waitUntilDone
|> takeOutOfOven
|> scanTicket
```

It will fail. To fix this, we can make use of `Result.bind` and `Result.map` to convert the functions in our pipelines to those that can work with our new functions.

To start off with, `assemblePizza_v2` now has two outputs, and `putInOven_v2` accepts one input and has two outputs. This is where we can use `Result.bind`. If we pass `putInOven_v2` as an argument to `Result.bind`, it will call `putInOven_v2` only if there are no errors coming from `assemblePizza_v2`. Otherwise it will return an `Error`. Thus converting `putInOven_v2` to a function that can accept the two inputs provided by `assemblePizza_v2`.

On the other hand, since `putInOven_v2` now has two outputs, and the following functions are now one-track functions that accept one input `RobotState` and return one output, `RobotState`, we can use `Result.map` to convert them two two-track input and two-track output functions, since the function following the one being called is only called when the previous function returns a success case.

Putting all this together, first we update the `robotStateData` object to include our new errors:

```
let robotStateData: RobotStateData =
    { AssembleError = None
        OvenError = None
        KitchenError = None }
```

And now, this is what our new pipeline will now look like:

```
newRobotState
|> ownTicket
|> assemblePizza_v2
|> Result.bind putInOven_v2
|> Result.map waitUntilDone
|> Result.map takeOutOfOven
|> Result.map scanTicket
```

The functions will still take in `RobotState` and pass them to the next function, but they can now also handle different kind of errors that may occur either while assembling the pizza or putting it into the oven.

[<- Back to Main](../README.md)
