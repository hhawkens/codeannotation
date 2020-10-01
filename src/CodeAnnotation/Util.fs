namespace CodeAnnotation

[<AutoOpen>]
module internal Util =

    open System.Text.RegularExpressions

    let private iterateMatch (regexMatch: Match) = seq {
        let mutable curr = regexMatch
        while curr.Success do
            yield curr
            curr <- curr.NextMatch()
    }

    let internal formatAnnotation (block: CodeBuildingBlock) txt =
        let blockText =
            match block with
                | Other id -> sprintf "%s-%s" "Other" id
                | b -> b.ToString()
        sprintf "{~%s:%s~}" blockText txt

    let internal matchAndExtract tryExtractFunc text (regex: Regex) =
        text
        |> regex.Match
        |> iterateMatch
        |> Seq.choose tryExtractFunc

    let internal debug a =
        printf "%A" a
        a

[<Struct>]
type internal OptionalChoiceBuilder =
    member _.ReturnFrom(x) = x

    member _.Combine(a, b) = match a with | Some _ -> a | None -> b

    member _.Delay(f) = f()

[<AutoOpen>]
module internal OptionalChoiceBuilder =

    let internal chooseoptional = OptionalChoiceBuilder()
