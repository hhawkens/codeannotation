namespace CodeAnnotation

[<AutoOpen>]
module internal OptionalChoiceBuilder =

    [<Struct>]
    type internal T =
        member _.ReturnFrom(x) = x

        member _.Combine(a, b) = match a with | Some _ -> a | None -> b

        member _.Delay(f) = f()

    let internal chooseoptional = T()
