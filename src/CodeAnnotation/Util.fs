[<AutoOpen>]
module internal CodeAnnotation.Util

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

    let internal tryMakeRegex txt =
        try Ok (Regex txt) with | _ -> Error (BadRegex txt)

    let internal validatePatterns (patterns: BuildingBlockPattern seq) =
        let validPatterns, errors =
            patterns |> Seq.fold (fun (validPatternsState, errorsState) pattern ->
                match tryMakeRegex pattern.Regex with
                | Ok regex -> {ValidRegex = regex; Block = pattern.Block}::validPatternsState, errorsState
                | Error err -> validPatternsState, err::errorsState
                ) ([],[])
        if errors = [] then ValidPatterns validPatterns else AnnotationErrors errors

    let internal debug a = printf "%A" a; a
