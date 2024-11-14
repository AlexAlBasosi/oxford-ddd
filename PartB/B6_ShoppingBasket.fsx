#load "B3_Menu.fsx"
#load "B4_ConstrainedTypes.fsx"

module ShoppingBasket =
    open B3_Menu
    open B4_ConstrainedTypes.ConstrainedTypes.CustomerName
    open B4_ConstrainedTypes.ConstrainedTypes.PhoneNumber
    open B4_ConstrainedTypes.ConstrainedTypes.BasketQuantity

    type CustomerId = CustomerId of int
    type BasketItemId = BasketItemId of int
    type BasketId = BasketId of int
    type KitchenId = KitchenId of int

    type CustomerDetails =
        { CustomerId: CustomerId
          CustomerName: CustomerName
          PhoneNumber: PhoneNumber }

    type BasketItem =
        { BasketItemId: BasketItemId
          Item: Menu.MenuItem
          Quantity: BasketQuantity }

    type ActiveBasketData =
        { BasketId: BasketId
          CustomerDetails: CustomerDetails
          SelectedMenuItems: BasketItem list
          KitchenId: KitchenId }

    type ShoppingBasket =
        | EmptyBasketState of ActiveBasketData
        | ActiveBasketState of ActiveBasketData
