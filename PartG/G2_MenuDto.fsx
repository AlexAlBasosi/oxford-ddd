#load "G1_PizzaDto.fsx"

module MenuDto =
    open G1_PizzaDto.PizzaDto

    type DrinkDto = { tag: string }

    type ItemChoiceDto = { pizza: PizzaDto; drink: DrinkDto }

    type MenuItemDto =
        { itemId: int
          itemChoice: ItemChoiceDto
          itemPrice: float }

    type MenuDto = MenuItemDto array
