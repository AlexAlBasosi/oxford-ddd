#load "C1_ShoppingBasket.fsx"
#load "../PartB/B7_Order.fsx"

module BasketImplementation =
    open C1_ShoppingBasket.ShoppingBasket
    open B7_Order.Order

    // Transition types

    type InitBasket = BasketId -> CustomerDetails -> KitchenId -> ShoppingBasket

    type AddToBasket = BasketItem -> ShoppingBasket -> ShoppingBasket

    type RemoveFromBasket = BasketItem -> ShoppingBasket -> ShoppingBasket

    type Pay = ShoppingBasket -> Payment -> Order

    // Implementation

    let initBasket: InitBasket =
        fun (basketId: BasketId) (customerDetails: CustomerDetails) (kitchenId: KitchenId) ->
            let activeData: ActiveBasketData =
                { BasketId = basketId
                  CustomerDetails = customerDetails
                  SelectedMenuItems = []
                  KitchenId = kitchenId }

            EmptyBasketState activeData

    let extractBasketData =
        fun (shoppingBasket: ShoppingBasket) ->
            match shoppingBasket with
            | EmptyBasketState data -> data
            | ActiveBasketState data -> data

    let addToBasket: AddToBasket =
        fun (basketItem: BasketItem) (shoppingBasket: ShoppingBasket) ->

            let activeBasketData: ActiveBasketData = extractBasketData shoppingBasket

            let newBasketItems: BasketItem list =
                basketItem :: activeBasketData.SelectedMenuItems

            let newBasketData: ActiveBasketData =
                { BasketId = activeBasketData.BasketId
                  CustomerDetails = activeBasketData.CustomerDetails
                  SelectedMenuItems = newBasketItems
                  KitchenId = activeBasketData.KitchenId }

            ActiveBasketState newBasketData

    let debug (shoppingBasket: ShoppingBasket) =
        printfn "Debugging: %A" shoppingBasket
        shoppingBasket
        : ShoppingBasket
