#load "D1_SimpleTypes.fsx"
#load "D2_Ingredient.fsx"
#load "D3_Recipe.fsx"
#load "D4_Vocabulary.fsx"

module DSL =
    open D1_SimpleTypes.SimpleTypes
    open D2_Ingredient.Ingredient
    open D3_Recipe.Recipe
    open D4_Vocabulary.Vocabulary

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
