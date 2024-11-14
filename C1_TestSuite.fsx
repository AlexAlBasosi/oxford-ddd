#load "C1_ShoppingBasket.fsx"
#load "C1_ConstrainedTypes.fsx"
#load "B3_Menu.fsx"
#load "C1_BasketImplementation.fsx"

module TestSuite =
    open C1_ShoppingBasket.ShoppingBasket
    open C1_ConstrainedTypes.ConstrainedTypes.CustomerName
    open C1_ConstrainedTypes.ConstrainedTypes.PhoneNumber
    open C1_ConstrainedTypes.ConstrainedTypes.BasketQuantity
    open B3_Menu.Menu
    open C1_BasketImplementation.BasketImplementation


    // Initialise empty basket

    let basketId: BasketId = BasketId 1

    let customerId: CustomerId = CustomerId 1
    let customerName: CustomerName option = MakeCustomerName "John Smith"
    let phoneNumber: PhoneNumber option = MakePhoneNumber "074658392"

    let customerDetails: CustomerDetails =
        { CustomerId = customerId
          CustomerName = customerName
          PhoneNumber = phoneNumber }

    let kitchenId: KitchenId = KitchenId 1

    let emptyBasket: ShoppingBasket = initBasket basketId customerDetails kitchenId


    // Initialise menu item 1

    let pizzaName: PizzaName = PizzaName "Pepperoni Feast"
    let pizzaSize: PizzaSize = Large
    let PizzaRecipe: PizzaRecipe = Predefined

    let pepperoniPizza: Pizza =
        { Name = pizzaName
          Size = pizzaSize
          Recipe = PizzaRecipe }

    let coke: Drink = Coke

    let itemId1: ItemId = ItemId 1

    let itemChoice1: ItemChoice = Pizza pepperoniPizza

    let menuItem1: MenuItem =
        { ItemId = itemId1
          ItemChoice = itemChoice1 }

    let basketItemId1: BasketItemId = BasketItemId 1
    let quantityItem1: BasketQuantity option = MakeBasketQuantity 1

    let basketItem1: BasketItem =
        { BasketItemId = basketItemId1
          Item = menuItem1
          Quantity = quantityItem1 }


    // Initialise menu item 2

    let itemId2: ItemId = ItemId 2

    let itemChoice2: ItemChoice = Drink coke

    let menuItem2: MenuItem =
        { ItemId = itemId2
          ItemChoice = itemChoice2 }

    let basketItemId2: BasketItemId = BasketItemId 2
    let quantityItem2: BasketQuantity option = MakeBasketQuantity 2

    let basketItem2: BasketItem =
        { BasketItemId = basketItemId2
          Item = menuItem2
          Quantity = quantityItem2 }

    emptyBasket
    |> extractBasketData
    |> addToBasket basketItem1
    |> extractBasketData
    |> addToBasket basketItem2
