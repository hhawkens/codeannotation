module internal CodeAnnotation.Tokens

let private buildingBlocksPriority = // lower number means higher priority
    Map.ofArray [|(Comment, 0uy); (String, 1uy); (Keyword, 2uy)|]

let private overlaps t1 t2 =
    let struct(tFst, tSnd) =
        if t1.Start <= t2.Start then struct(t1, t2) else struct(t2, t1)
    tFst.Start + tFst.Len > tSnd.Start

let private tryPrioritizeByBuildingBlock (t1: Token) (t2: Token) =
    let findPriority x =
        match buildingBlocksPriority |> Map.tryFind x with | Some s -> s | None -> 255uy
    let struct(prio1, prio2) = struct(findPriority t1.Block, findPriority t2.Block)
    if prio1 < prio2 then Some t1
    else if prio2 < prio1 then Some t2
    else None

let private tryPrioritizeByStart t1 t2 =
    if t1.Start < t2.Start then Some t1
    else if t2.Start < t1.Start then Some t2
    else None

let private tryPrioritizeByLength t1 t2 =
    if t1.Len > t2.Len then Some t1
    else if t2.Len > t1.Len then Some t2
    else None

let private tryPrioritize t1 t2 = chooseoptional {
    return! tryPrioritizeByStart t1 t2
    return! tryPrioritizeByBuildingBlock t1 t2
    return! tryPrioritizeByLength t1 t2
}

let private prioritize t1 t2 =
    match tryPrioritize t1 t2 with | Some t -> t | None -> t1

let private tryAddTokenToList token = function
    | [] -> [token]
    | hd::tl -> if token |> overlaps hd then (prioritize hd token)::tl else token::hd::tl

let internal sortAndFilterDescending tokens =
    tokens
    |> Seq.sortBy (fun t -> t.Start)
    |> Seq.fold (fun state t -> tryAddTokenToList t state) []
