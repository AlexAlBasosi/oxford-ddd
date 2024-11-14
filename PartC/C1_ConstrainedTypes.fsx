module ConstrainedTypes =

    module BasketQuantity =
        type BasketQuantity = private BasketQuantity of int

        type MakeBasketQuantity = BasketQuantity -> BasketQuantity option

        let MakeBasketQuantity (qty: int) =
            if qty >= 1 && qty <= 10 then
                Some(BasketQuantity qty)
            else
                None

        let getQuantity quantity =
            match quantity with
            | BasketQuantity quantity -> quantity

    module CustomerName =
        type CustomerName = private CustomerName of string

        type MakeCustomerName = CustomerName -> CustomerName option

        let MakeCustomerName (name: string) =
            if name.Length <= 50 then Some(CustomerName name) else None

        let getName name =
            match name with
            | CustomerName name -> name

    module PhoneNumber =
        open System.Text.RegularExpressions
        type PhoneNumber = private PhoneNumber of string

        type MakePhoneNumber = PhoneNumber -> PhoneNumber option

        let MakePhoneNumber (number: string) =
            let isValid =
                Regex.IsMatch(
                    number,
                    "^(((\+44\s?\d{4}|\(?0\d{4}\)?)\s?\d{3}\s?\d{3})|((\+44\s?\d{3}|\(?0\d{3}\)?)\s?\d{3}\s?\d{4})|((\+44\s?\d{2}|\(?0\d{2}\)?)\s?\d{4}\s?\d{4}))(\s?\#(\d{4}|\d{3}))?$"
                )

            if isValid then Some(PhoneNumber number) else None

        let getNumber number =
            match number with
            | PhoneNumber number -> number
