module PizzaDto =
    type ToppingDto =
        { tagTopping1: string
          tagTopping2: string }

    type PizzaRecipeDto = { tag: string; toppings: ToppingDto }

    type PizzaSizeDto = { tag: string }

    type PizzaDto =
        { name: string
          size: PizzaSizeDto
          recipe: PizzaRecipeDto }
