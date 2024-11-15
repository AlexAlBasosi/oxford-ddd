module SimpleTypes =

    type IngredientName = string
    type IngredientAmount = int

    type GramString = string
    type MilliliterString = string
    type TeaspoonString = string
    type TablespoonString = string

    type UnitMeasureStrings =
        { GramString: GramString
          MilliliterString: MilliliterString
          TeaspoonString: TeaspoonString
          TablespoonString: TablespoonString }

    type UnitMeasure =
        | Gram of UnitMeasureStrings
        | Milliliter of UnitMeasureStrings
        | Teaspoon of UnitMeasureStrings
        | Tablespoon of UnitMeasureStrings

    type StepComment = string

    type CelsiusString = string
    type FahrenheitString = string

    type UnitTemperatureStrings =
        { CelsiusString: CelsiusString
          FahrenheitString: FahrenheitString }

    type UnitTemperature =
        | Celsius of UnitTemperatureStrings
        | Fahrenheit of UnitTemperatureStrings

    type Temperature = string
    type TimeDuration = string

    type MinuteString = string
    type HourString = string

    type UnitTimeStrings =
        { MinuteString: MinuteString
          HourString: HourString }

    type UnitTime =
        | Minute of UnitTimeStrings
        | Hour of UnitTimeStrings
