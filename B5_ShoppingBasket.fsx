#load "B3_Menu.fsx"
#load "B4_ConstrainedTypes.fsx"

// Helper definition for unknown type
type undefined = exn

module ShoppingBasket =
    open B3_Menu
    open B4_ConstrainedTypes.ConstrainedTypes.CustomerName
    open B4_ConstrainedTypes.ConstrainedTypes.PhoneNumber
    open B4_ConstrainedTypes.ConstrainedTypes.BasketQuantity

    type CustomerId = CustomerId of string
    type BasketItemId = BasketItemId of int
    type BasketId = BasketId of int
    type KitchenId = KitchenId of int

    type Payment = undefined

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

    type Order =
        { CustomerDetails: CustomerDetails
          SelectedMenuItems: BasketItem list
          KitchenId: KitchenId
          Payment: Payment }

    type ShoppingBasket =
        | EmptyBasketState
        | ActiveBasketState of ActiveBasketData
        | PaidBasketState of Order
