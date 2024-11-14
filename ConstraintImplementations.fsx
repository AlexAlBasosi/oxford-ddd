type CustomerName = CustomerName of string

module CustomerName =
    let create (name: string) =
        if name.Length <= 50 then Some(CustomerName name) else None

type PhoneNumber = PhoneNumber of string

module PhoneNumber =
    open System.Text.RegularExpressions

    let create (number: string) =
        let isValid =
            Regex.IsMatch(
                number,
                "^(((\+44\s?\d{4}|\(?0\d{4}\)?)\s?\d{3}\s?\d{3})|((\+44\s?\d{3}|\(?0\d{3}\)?)\s?\d{3}\s?\d{4})|((\+44\s?\d{2}|\(?0\d{2}\)?)\s?\d{4}\s?\d{4}))(\s?\#(\d{4}|\d{3}))?$"
            )

        if isValid then Some(PhoneNumber number) else None

type ItemQuantity = ItemQuantity of int

module ItemQuantity =
    let create (qty: int) =
        if qty >= 1 && qty <= 100 then
            Some(ItemQuantity qty)
        else
            None
