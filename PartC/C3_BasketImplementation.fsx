#load "C3_ShoppingBasket.fsx"
#load "../PartC/C1_Order.fsx"
#load "C3_Menu.fsx"

module BasketImplementation =
    open C3_ShoppingBasket.ShoppingBasket
    open C1_Order.Order
    open C3_Menu.Menu

    // Transition types

    type InitBasket = BasketId -> CustomerDetails -> KitchenId -> TotalPrice -> ShoppingBasket

    type AddToBasket = BasketItem -> ShoppingBasket -> ShoppingBasket

    type RemoveFromBasket = BasketItem -> ShoppingBasket -> ShoppingBasket

    type Pay = ShoppingBasket -> Payment -> Order

    type ExtractBasketData = ShoppingBasket -> ActiveBasketData

    type CalculateTotalPrice = BasketItem list -> TotalPrice

    // Implementation

    let initBasket: InitBasket =
        fun (basketId: BasketId) (customerDetails: CustomerDetails) (kitchenId: KitchenId) (totalPrice: TotalPrice) ->
            let activeData: ActiveBasketData =
                { BasketId = basketId
                  CustomerDetails = customerDetails
                  SelectedMenuItems = []
                  KitchenId = kitchenId
                  TotalPrice = totalPrice }

            EmptyBasketState activeData

    let extractBasketData: ExtractBasketData =
        fun (shoppingBasket: ShoppingBasket) ->
            match shoppingBasket with
            | EmptyBasketState data -> data
            | ActiveBasketState data -> data

    let calculateTotalPrice: CalculateTotalPrice =
        fun (itemList: BasketItem list) ->
            let mutable totalPrice: float = 0

            for basketItem: BasketItem in itemList do
                let itemPrice: float = basketItem.Item.ItemPrice
                totalPrice <- totalPrice + itemPrice

            TotalPrice totalPrice

    let addToBasket: AddToBasket =
        fun (basketItem: BasketItem) (shoppingBasket: ShoppingBasket) ->

            let activeBasketData: ActiveBasketData = extractBasketData shoppingBasket

            let newBasketItems: BasketItem list =
                basketItem :: activeBasketData.SelectedMenuItems

            let totalPrice: TotalPrice = calculateTotalPrice newBasketItems

            let newBasketData: ActiveBasketData =
                { BasketId = activeBasketData.BasketId
                  CustomerDetails = activeBasketData.CustomerDetails
                  SelectedMenuItems = newBasketItems
                  KitchenId = activeBasketData.KitchenId
                  TotalPrice = totalPrice }

            ActiveBasketState newBasketData

    let debug (shoppingBasket: ShoppingBasket) =
        printfn "Debugging: %A" shoppingBasket
        shoppingBasket
        : ShoppingBasket
