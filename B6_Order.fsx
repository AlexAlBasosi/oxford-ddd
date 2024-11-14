#load "B6_ShoppingBasket.fsx"

module Order =
    open B6_ShoppingBasket.ShoppingBasket

    type OrderId = OrderId of int
    type DateTime = System.DateTime
    type PaidAmount = PaidAmount of float
    type PaidTime = PaidDate of DateTime
    type PreparingTime = PreparingTime of DateTime
    type ReadyTime = ReadyTime of DateTime
    type CompletedTime = CompletedDate of DateTime

    type Payment =
        { PaidAmount: PaidAmount
          PaidTime: PaidTime }

    type PaidOrderInfo = { OrderId: OrderId; Payment: Payment }

    type PreparingOrderInfo =
        { OrderId: OrderId
          PreparingTime: PreparingTime }

    type ReadyOrderInfo =
        { OrderId: OrderId
          ReadyTime: ReadyTime }

    type CompletedOrderInfo =
        { OrderId: OrderId
          Payment: Payment
          PreparingTime: PreparingTime
          ReadyTime: ReadyTime
          CompletedTime: CompletedTime }

    type OrderStatus =
        | Paid of PaidOrderInfo
        | Preparing of PreparingOrderInfo
        | Ready of ReadyOrderInfo
        | Completed of CompletedOrderInfo

    type Order =
        { OrderId: OrderId
          CustomerDetails: CustomerDetails
          SelectedMenuItems: BasketItem list
          KitchenId: KitchenId
          Status: OrderStatus }
