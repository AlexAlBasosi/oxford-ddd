#load "E2_RobotErrors.fsx"

module RobotState =
    open E2_RobotErrors.RobotErrors

    type RobotStateData =
        { AssembleError: AssembleError option
          OvenError: OvenError option }

    type RobotState =
        | NewRobotState of RobotStateData
        | OwningTicket of RobotStateData
        | AssemblingPizza of RobotStateData
        | PuttingInOven of RobotStateData
        | WaitingUntilDone of RobotStateData
        | TakingOutOfOven of RobotStateData
        | ScanningTicket of RobotStateData

    type OwnTicket = RobotState -> RobotState
    type AssemblePizza = RobotState -> Result<RobotState, AssembleError>
    type PutInOven = RobotState -> Result<RobotState, OvenError>
    type WaitUntilDone = RobotState -> RobotState
    type TakeOutOfOven = RobotState -> RobotState
    type ScanTicket = RobotState -> RobotState

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

    let ownTicket: OwnTicket =
        fun (state: RobotState) ->
            let robotStateData: RobotStateData = extractRobotStateData state
            OwningTicket robotStateData

    let assemblePizza: AssemblePizza =
        fun (state: RobotState) ->
            let robotStateData: RobotStateData = extractRobotStateData state
            let assemblingPizzaState: RobotState = AssemblingPizza robotStateData

            match robotStateData.AssembleError with
            | Some NotEnoughDough -> Error NotEnoughDough
            | Some NotEnoughSauce -> Error NotEnoughSauce
            | None -> Ok assemblingPizzaState

    let putInOven: PutInOven =
        fun (state: RobotState) ->
            let robotStateData: RobotStateData = extractRobotStateData state
            let puttingInOvenState: RobotState = PuttingInOven robotStateData

            match robotStateData.OvenError with
            | Some NoOvens -> Error NoOvens
            | Some TechnicalFault -> Error TechnicalFault
            | None -> Ok puttingInOvenState

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
