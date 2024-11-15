module SimpleTypes =

    type IngredientName = string
    type IngredientAmount = int

    type GramString = string
    type MilliliterString = string
    type TeaspoonString = string
    type TablespoonString = string

    type UnitMeasureData =
        { GramString: GramString
          MilliliterString: MilliliterString
          TeaspoonString: TeaspoonString
          TablespoonString: TablespoonString }

    type UnitMeasure =
        | Gram of UnitMeasureData
        | Milliliter of UnitMeasureData
        | Teaspoon of UnitMeasureData
        | Tablespoon of UnitMeasureData

    type StepComment = string

    type CelsiusString = string
    type FahrenheitString = string

    type UnitTemperatureData =
        { CelsiusString: CelsiusString
          FahrenheitString: FahrenheitString }

    type UnitTemperature =
        | Celsius of UnitTemperatureData
        | Fahrenheit of UnitTemperatureData

    type Temperature = string
    type TimeDuration = string

    type MinuteString = string
    type HourString = string

    type UnitTimeData =
        { MinuteString: MinuteString
          HourString: HourString }

    type UnitTime =
        | Minute of UnitTimeData
        | Hour of UnitTimeData
