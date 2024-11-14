module Menu =

    type Topping =
        | Pepperoni
        | Ham
        | Mushrooms
        | Onions
        | Pineapple

    type PizzaName = string

    type PizzaRecipe =
        | Predefined
        | CreateYourOwn of Topping * Topping option

    type PizzaSize =
        | Large
        | Medium
        | Small

    type Pizza =
        { Name: PizzaName
          Size: PizzaSize
          Recipe: PizzaRecipe }

    type Drink =
        | Coke
        | DietCoke
        | Fanta

    type MenuItem =
        | Pizza of Pizza
        | Drink of Drink

    type Menu = MenuItem list
