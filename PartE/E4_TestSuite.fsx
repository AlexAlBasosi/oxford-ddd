#load "E3_RobotState.fsx"

module TestSuite =
    open E3_RobotState.RobotState

    let robotStateData: RobotStateData =
        { AssembleError = None
          OvenError = None
          KitchenError = None }

    let newRobotState: RobotState = NewRobotState robotStateData

    newRobotState
    |> ownTicket
    |> assemblePizza_v2
    |> Result.bind putInOven_v2
    |> Result.map waitUntilDone
    |> Result.map takeOutOfOven
    |> Result.map scanTicket
