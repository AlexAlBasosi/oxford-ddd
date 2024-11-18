module RobotErrors =
    type AssembleError =
        | NotEnoughDough
        | NotEnoughSauce

    type OvenError =
        | NoOvens
        | TechnicalFault
