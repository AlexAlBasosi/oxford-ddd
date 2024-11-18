#load "E_RobotState.fsx"

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
