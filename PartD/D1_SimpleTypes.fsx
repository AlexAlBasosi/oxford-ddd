module SimpleTypes =

    type IngredientName = string
    type IngredientAmount = int

    type UnitMeasure =
        | Grams
        | Mils
        | Tsps
        | Tbsps

    type StepComment = string

    type UnitTemperature =
        | C
        | F

    type Temperature = int
    type TimeDuration = int

    type UnitTime =
        | Mins
        | Hours
