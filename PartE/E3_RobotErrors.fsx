module RobotErrors =
    type AssembleError =
        | NotEnoughDough
        | NotEnoughSauce

    type OvenError =
        | NoOvens
        | TechnicalFault

    type KitchenError =
        | Assemble of AssembleError
        | Oven of OvenError
