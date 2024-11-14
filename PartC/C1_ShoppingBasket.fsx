#load "../PartB/B3_Menu.fsx"
#load "C1_ConstrainedTypes.fsx"

module ShoppingBasket =
    open B3_Menu
    open C1_ConstrainedTypes.ConstrainedTypes.CustomerName
    open C1_ConstrainedTypes.ConstrainedTypes.PhoneNumber
    open C1_ConstrainedTypes.ConstrainedTypes.BasketQuantity

    type CustomerId = CustomerId of int
    type BasketItemId = BasketItemId of int
    type BasketId = BasketId of int
    type KitchenId = KitchenId of int

    type CustomerDetails =
        { CustomerId: CustomerId
          CustomerName: CustomerName option
          PhoneNumber: PhoneNumber option }

    type BasketItem =
        { BasketItemId: BasketItemId
          Item: Menu.MenuItem
          Quantity: BasketQuantity option }

    type ActiveBasketData =
        { BasketId: BasketId
          CustomerDetails: CustomerDetails
          SelectedMenuItems: BasketItem list
          KitchenId: KitchenId }

    type ShoppingBasket =
        | EmptyBasketState of ActiveBasketData
        | ActiveBasketState of ActiveBasketData
