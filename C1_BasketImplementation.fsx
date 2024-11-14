#load "C1_ShoppingBasket.fsx"
#load "B7_Order.fsx"

module BasketImplementation =
    open C1_ShoppingBasket.ShoppingBasket
    open B7_Order.Order

    // Transition types

    type InitBasket = BasketId -> CustomerDetails -> KitchenId -> ShoppingBasket

    type AddToBasket = BasketItem -> ActiveBasketData -> ShoppingBasket

    type ExtractBasketData = ShoppingBasket -> ActiveBasketData

    type RemoveFromBasket = BasketItem -> ActiveBasketData -> ShoppingBasket

    type Pay = ActiveBasketData -> Payment -> Order

    // Implementation

    let initBasket: InitBasket =
        fun (basketId: BasketId) (customerDetails: CustomerDetails) (kitchenId: KitchenId) ->
            let activeData: ActiveBasketData =
                { BasketId = basketId
                  CustomerDetails = customerDetails
                  SelectedMenuItems = []
                  KitchenId = kitchenId }

            EmptyBasketState activeData

    let addToBasket: AddToBasket =
        fun (basketItem: BasketItem) (activeBasketData: ActiveBasketData) ->
            let newBasketItems: BasketItem list =
                basketItem :: activeBasketData.SelectedMenuItems

            let newBasketData =
                { BasketId = activeBasketData.BasketId
                  CustomerDetails = activeBasketData.CustomerDetails
                  SelectedMenuItems = newBasketItems
                  KitchenId = activeBasketData.KitchenId }

            ActiveBasketState newBasketData

    let extractBasketData: ExtractBasketData =
        fun (shoppingBasket: ShoppingBasket) ->
            match shoppingBasket with
            | EmptyBasketState activeBasketData -> activeBasketData
            | ActiveBasketState activeBasketData -> activeBasketData
