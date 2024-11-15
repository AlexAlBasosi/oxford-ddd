#load "D1_SimpleTypes.fsx"

module Ingredient =
    open D1_SimpleTypes.SimpleTypes

    type ThingQuantity = int
    type ThingName = string

    type StuffQuantity = int
    type StuffName = string

    type Ingredient =
        | Thing of ThingQuantity * ThingName
        | Stuff of StuffQuantity * UnitMeasure * StuffName
