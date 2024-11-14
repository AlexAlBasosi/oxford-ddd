#load "B3_Menu.fsx"

module ShoppingBasket =
    open B3_Menu

    type CustomerId = CustomerId of string
    type CustomerName = CustomerName of string
    type PhoneNumber = PhoneNumber of string

    type CustomerDetails =
        { CustomerId: CustomerId
          CustomerName: CustomerName
          PhoneNumber: PhoneNumber }

    type BasketQuantity = BasketQuantity of int
    type BasketItemId = BasketItemId of int

    type BasketItem =
        { BasketItemId: BasketItemId
          Item: Menu.MenuItem
          Quantity: BasketQuantity }

    type KitchenId = KitchenId of int
    type BasketId = BasketId of int

    type ShoppingBasket =
        { BasketId: BasketId
          CustomerDetails: CustomerDetails
          SelectedMenuItems: BasketItem list
          KitchenId: KitchenId }
