#load "D1_SimpleTypes.fsx"
#load "D2_Ingredient.fsx"
#load "D3_Recipe.fsx"

module Vocabulary =
    open D1_SimpleTypes.SimpleTypes
    open D2_Ingredient.Ingredient
    open D3_Recipe.Recipe

    type Take = IngredientAmount -> UnitMeasure -> IngredientName -> Ingredient
    type Grab = IngredientAmount -> IngredientName -> Ingredient
    type Combine = StepComment -> Ingredient list -> RecipeStep
    type ThenAdd = StepComment option -> Ingredient list -> RecipeStep
    type ThenDo = StepComment -> RecipeStep
    type BakeAt = Temperature -> UnitTemperature -> RecipeStep
    type BeatFor = TimeDuration -> UnitTime -> RecipeStep
    type CookFor = TimeDuration -> UnitTime -> RecipeStep

    let take: Take =
        fun (amount: IngredientAmount) (unit: UnitMeasure) (name: IngredientName) -> Stuff(amount, unit, name)

    let grab: Grab =
        fun (amount: IngredientAmount) (name: IngredientName) -> Thing(amount, name)

    let combine: Combine =
        fun (comment: StepComment) (ingredientList: Ingredient list) ->
            let addIngredientsData: AddIngredientsData =
                { Ingredients = ingredientList
                  Comment = Some comment }

            AddIngredients addIngredientsData

    let thenAdd: ThenAdd =
        fun (comment: StepComment option) (ingredientList: Ingredient list) ->
            let addIngredientsData: AddIngredientsData =
                match comment with
                | Some comment ->
                    { Ingredients = ingredientList
                      Comment = Some comment }
                | None ->
                    { Ingredients = ingredientList
                      Comment = None }

            AddIngredients addIngredientsData

    let thenDo: ThenDo =
        fun (comment: StepComment) ->
            let stepComment: StepComment = "Then " + comment

            FollowInstruction stepComment

    let bakeAt: BakeAt =
        fun (temperature: Temperature) (unitTemperature: UnitTemperature) ->
            let ovenData: OvenData =
                { Temperature = temperature
                  Unit = unitTemperature }

            UseOven ovenData

    let beatFor: BeatFor =
        fun (timeDuration: TimeDuration) (unitTime: UnitTime) ->
            let stepComment: StepComment = "Beat for "

            let timedData: TimedData =
                { Duration = timeDuration
                  Unit = unitTime
                  Comment = stepComment }

            Timed timedData

    let cookFor: CookFor =
        fun (timeDuration: TimeDuration) (unitTime: UnitTime) ->
            let stepComment: StepComment = "Cook for "

            let timedData: TimedData =
                { Duration = timeDuration
                  Unit = unitTime
                  Comment = stepComment }

            Timed timedData
