module Menu =

    type Topping =
        | Pepperoni
        | Ham
        | Mushrooms
        | Onions
        | Pineapple

    type PizzaName = PizzaName of string

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

    type ItemChoice =
        | Pizza of Pizza
        | Drink of Drink

    type ItemId = ItemId of int

    type MenuItem =
        { ItemId: ItemId
          ItemChoice: ItemChoice }

    type Menu = MenuItem list
