#load "D1_SimpleTypes.fsx"

module Ingredient =
    open D1_SimpleTypes.SimpleTypes

    type Ingredient =
        | Thing of IngredientAmount * IngredientName
        | Stuff of IngredientAmount * UnitMeasure * IngredientName
