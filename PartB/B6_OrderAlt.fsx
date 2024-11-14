#load "B6_ShoppingBasket.fsx"

module OrderAlt =
    open B6_ShoppingBasket.ShoppingBasket

    type OrderId = OrderId of int
    type DateTime = System.DateTime
    type PaidAmount = PaidAmount of float
    type PaidTime = PaidDate of DateTime
    type PreparingTime = PreparingTime of DateTime
    type ReadyTime = ReadyTime of DateTime
    type CompletedTime = CompletedTime of DateTime

    type Payment =
        { PaidAmount: PaidAmount
          PaidTime: PaidTime }

    type PaidOrder =
        { OrderId: OrderId
          CustomerDetails: CustomerDetails
          SelectedMenuItems: BasketItem list
          KitchenId: KitchenId
          Payment: Payment }

    type PreparingOrder =
        { OrderId: OrderId
          CustomerDetails: CustomerDetails
          SelectedMenuItems: BasketItem list
          KitchenId: KitchenId
          PreparingTime: PreparingTime }

    type ReadyOrder =
        { OrderId: OrderId
          CustomerDetails: CustomerDetails
          SelectedMenuItems: BasketItem list
          KitchenId: KitchenId
          ReadyOrder: ReadyTime }

    type CompletedOrder =
        { OrderId: OrderId
          CustomerDetails: CustomerDetails
          SelectedMenuItems: BasketItem list
          KitchenId: KitchenId
          CompletedTime: CompletedTime }

    type Order =
        | Paid of PaidOrder
        | Preparing of PreparingOrder
        | Ready of ReadyOrder
        | Completed of CompletedOrder
