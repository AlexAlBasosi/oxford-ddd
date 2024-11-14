#load "B1_Menu.fsx"

module ShoppingBasket =
    open B1_Menu

    type CustomerName = CustomerName of string
    type PhoneNumber = PhoneNumber of string

    type CustomerDetails =
        { Name: CustomerName
          PhoneNumber: PhoneNumber }

    type BasketQuantity = BasketQuantity of int

    type BasketItem =
        { Item: Menu.MenuItem
          Quantity: BasketQuantity }

    type KitchenId = KitchenId of int

    type ShoppingBasket =
        { CustomerDetails: CustomerDetails
          SelectedMenuItems: BasketItem list
          KitchenId: KitchenId }
