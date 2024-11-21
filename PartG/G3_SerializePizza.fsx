#load "../PartC/C3_Menu.fsx"
#load "G1_PizzaDTO.fsx"

module SerializePizza =
    open C3_Menu.Menu
    open G1_PizzaDTO.PizzaDto

    type PizzaToppingToDto = Topping -> string
    type PizzaRecipeToDto = PizzaRecipe -> PizzaRecipeDto
    type PizzaSizeToDto = PizzaSize -> string
    type PizzaToDto = Pizza -> PizzaDto

    let pizzaToppingToDto: PizzaToppingToDto =
        fun (topping: Topping) ->
            match topping with
            | Pepperoni -> "pepperoni"
            | Ham -> "ham"
            | Mushrooms -> "mushrooms"
            | Onions -> "onions"
            | Pineapple -> "pineapple"

    let pizzaRecipeToDto: PizzaRecipeToDto =
        fun (pizzaRecipe: PizzaRecipe) ->
            match pizzaRecipe with
            | Predefined ->
                { tag = "predefined"
                  toppings = { tagTopping1 = ""; tagTopping2 = "" } }
            | CreateYourOwn(topping1, topping2) ->
                { tag = "createyourown"
                  toppings =
                    { tagTopping1 = pizzaToppingToDto topping1
                      tagTopping2 =
                        match topping2 with
                        | Some topping -> pizzaToppingToDto topping
                        | None -> "" } }

    let pizzaSizeToDto: PizzaSizeToDto =
        fun (pizzaSize: PizzaSize) ->
            match pizzaSize with
            | Large -> "large"
            | Medium -> "medium"
            | Small -> "small"

    let pizzaToDto: PizzaToDto =
        fun (pizza: Pizza) ->
            { name =
                let (PizzaName name) = pizza.Name
                name
              size =
                match pizza.Size with
                | Large -> { tag = pizzaSizeToDto Large }
                | Medium -> { tag = pizzaSizeToDto Medium }
                | Small -> { tag = pizzaSizeToDto Small }
              recipe = pizzaRecipeToDto pizza.Recipe }
