#load "C3_ShoppingBasket.fsx"
#load "C1_ConstrainedTypes.fsx"
#load "../PartC/C3_Menu.fsx"
#load "C3_BasketImplementation.fsx"
#load "C3_DiscountLibrary.fsx"

module TestSuite =
    open C3_ShoppingBasket.ShoppingBasket
    open C1_ConstrainedTypes.ConstrainedTypes.CustomerName
    open C1_ConstrainedTypes.ConstrainedTypes.PhoneNumber
    open C1_ConstrainedTypes.ConstrainedTypes.BasketQuantity
    open C3_Menu.Menu
    open C3_BasketImplementation.BasketImplementation
    open C3_DiscountLibrary.DiscountLibrary

    // Initialise empty basket

    let basketId: BasketId = BasketId 1

    let customerId: CustomerId = CustomerId 1
    let customerName: CustomerName option = MakeCustomerName "John Smith"
    let phoneNumber: PhoneNumber option = MakePhoneNumber "074658392"
    let totalPrice: TotalPrice = TotalPrice 0

    let customerDetails: CustomerDetails =
        { CustomerId = customerId
          CustomerName = customerName
          PhoneNumber = phoneNumber }

    let kitchenId: KitchenId = KitchenId 1

    let emptyBasket: ShoppingBasket =
        initBasket basketId customerDetails kitchenId totalPrice


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
    let itemPrice1: ItemPrice = 4.99

    let menuItem1: MenuItem =
        { ItemId = itemId1
          ItemChoice = itemChoice1
          ItemPrice = itemPrice1 }

    let basketItemId1: BasketItemId = BasketItemId 1
    let quantityItem1: BasketQuantity option = MakeBasketQuantity 1

    let basketItem1: BasketItem =
        { BasketItemId = basketItemId1
          Item = menuItem1
          Quantity = quantityItem1 }


    // Initialise menu item 2

    let itemId2: ItemId = ItemId 2

    let itemChoice2: ItemChoice = Drink coke
    let itemPrice2: ItemPrice = 2.99

    let menuItem2: MenuItem =
        { ItemId = itemId2
          ItemChoice = itemChoice2
          ItemPrice = itemPrice2 }

    let basketItemId2: BasketItemId = BasketItemId 2
    let quantityItem2: BasketQuantity option = MakeBasketQuantity 2

    let basketItem2: BasketItem =
        { BasketItemId = basketItemId2
          Item = menuItem2
          Quantity = quantityItem2 }

    emptyBasket
    |> addToBasket basketItem1
    |> setDiscountPercent2 10
    |> addToBasket basketItem2

    emptyBasket
    |> addToBasket basketItem1
    |> setDiscountPercentWrapper setDiscountPercent 10
    |> addToBasket basketItem2
