#load "D1_SimpleTypes.fsx"
#load "D2_Ingredient.fsx"

module Recipe =
    open D1_SimpleTypes.SimpleTypes
    open D2_Ingredient.Ingredient

    type AddIngredientsData =
        { Ingredients: Ingredient list
          Comment: StepComment }

    type OvenData =
        { Temperature: Temperature
          Unit: UnitTemperature }

    type TimedData =
        { Duration: TimeDuration
          Unit: UnitTime
          Comment: StepComment }

    type RecipeStep =
        | AddIngredients of AddIngredientsData
        | FollowInstruction of StepComment
        | UseOven of OvenData
        | Timed of TimedData

    type Recipe = RecipeStep list
