module ConstrainedTypes =

    module BasketQuantity =
        type BasketQuantity = private BasketQuantity of int

        type MakeBasketQuantity = BasketQuantity -> BasketQuantity option

        let getQuantity quantity =
            match quantity with
            | BasketQuantity quantity -> quantity

    module CustomerName =
        type CustomerName = private CustomerName of string

        type MakeCustomerName = CustomerName -> CustomerName option

        let getName name =
            match name with
            | CustomerName name -> name

    module PhoneNumber =
        open System.Text.RegularExpressions
        type PhoneNumber = private PhoneNumber of string

        type MakePhoneNumber = PhoneNumber -> PhoneNumber option

        let getNumber number =
            match number with
            | PhoneNumber number -> number
