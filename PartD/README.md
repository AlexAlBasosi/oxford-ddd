[<- Back to Main](../README.md)

# Part D: Building a DSL for Recipes

Let’s take a look into modelling the recipes used by the kitchen, by building a Domain Specific Language (DSL) so that we can model any recipe.

## Exercise D1: The Simple Types

First, let’s model some of the basic type used by ingredients and recipes within the DSL:

```
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
```

Notice that we modelled the units themselves as union types. The remaining types are simple aliases such as strings and integers that will be used throughout the DSL.

## Exercise D2: Ingredients

Now, let’s look at modelling the `Ingredient` itself:

```
module Ingredient =
    open D1_SimpleTypes.SimpleTypes

    type Ingredient =
        | Thing of IngredientAmount * IngredientName
        | Stuff of IngredientAmount * UnitMeasure * IngredientName
```

We define `Ingredient` as a union type of tuples to store information related to them within `Ingredient`, using the types we defined in the previous section.

## Exercise D3: Recipes

Let’s look at modelling the `Recipe` and `RecipeStep` types:

```
module Recipe =
    open D1_SimpleTypes.SimpleTypes
    open D2_Ingredient.Ingredient

    type AddIngredientsData =
        { Ingredients: Ingredient list
          Comment: StepComment option }

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
```

First, we model the record types to be used by the different types of steps in the recipe. Notice how we’re reusing the `StepComment` type we defined earlier. If at some point different constraints are required for different types of comments, that can be refactored.

Then we define the `RecipeStep` as a union type. Depending on the type of recipe step in the process, it will be in a different state and will require different information.

Finally, we define the `Recipe` type as a list of `RecipeStep`s.

## Exercise D4: Vocabulary Functions

Let’s look at creating some vocabulary functions to help us build out our DSL. The functions modelled and implemented below all return objects of type `RecipeStep`:

```
module Vocabulary =
    open D1_SimpleTypes.SimpleTypes
    open D2_Ingredient.Ingredient
    open D3_Recipe.Recipe

    type Combine = StepComment -> Ingredient list -> RecipeStep
    type ThenAdd = StepComment -> Ingredient list -> RecipeStep
    type ThenDo = StepComment -> RecipeStep
    type BakeAt = Temperature -> UnitTemperature -> RecipeStep
    type BeatFor = TimeDuration -> UnitTime -> RecipeStep
    type CookFor = TimeDuration -> UnitTime -> RecipeStep

    let combine: Combine =
        fun (comment: StepComment) (ingredientList: Ingredient list) ->
            let addIngredientsData: AddIngredientsData =
                { Ingredients = ingredientList
                  Comment = comment }

            AddIngredients addIngredientsData

    let thenAdd: ThenAdd =
        fun (comment: StepComment) (ingredientList: Ingredient list) ->
            let addIngredientsData: AddIngredientsData =
                { Ingredients = ingredientList
                  Comment = comment }

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
```

First, we define the function types to ensure that any implementation is conforming to the expected inputs and outputs. The functions that we create correspond to a recipe step and build out the data object required for each step.

Then it returns that object as a `RecipeStep` wrapped in the state that corresponds to that recipe step.

If there are any prefixes required for a comment in that step, it is concatenated with that comment and plugged into that data object or, in some cases, defined within the object and plugged in as is. Such as within the `beatFor` and `cookFor` functions.

## Exercise D5: Using the DSL

Before we can use the DSL we just modelled to create a recipe, we need to make a few changes to the vocabulary functions:

```
type Take = IngredientAmount -> UnitMeasure -> IngredientName -> Ingredient
type Grab = IngredientAmount -> IngredientName -> Ingredient

let take: Take =
    fun (amount: IngredientAmount) (unit: UnitMeasure) (name: IngredientName) -> Stuff(amount, unit, name)

let grab: Grab =
    fun (amount: IngredientAmount) (name: IngredientName) -> Thing(amount, name)
```

First, we define a few additional functions, `take` and `grab`. Take handles creating an `Ingredient` of type `Stuff`, and `Grab` handles creating an `Ingredient` of type `Thing`.

One more change we need to make is to the `thenAdd` function, which is to add the functionality that allows comments to be optional:

```
type ThenAdd = StepComment option -> Ingredient list -> RecipeStep

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
```

If we don’t provide a comment when calling `thenAdd`, it will not provide it when creating the `addIngredientsData` object. Otherwise, the comment will be passed to the object and returned as a `RecipeStep`.

Finally, we can create the `Recipe` object using our new DSL:

```
let chocolateCake: Recipe =
    [ combine
            "in a large bowl"
            [ take 225 Grams "flour"
            take 350 Grams "sugar"
            take 85 Grams "cocoa"
            take 2 Tsps "baking soda"
            take 1 Tsps "baking powder"
            take 1 Tsps "salt" ]
        thenDo "make a well in the centre"
        thenAdd None [ grab 2 "eggs"; take 125 Mils "oil"; take 250 Mils "milk" ]
        beatFor 2 Mins
        bakeAt 175 C
        thenDo "add icing" ]
```

[<- Back to Main](../README.md)
