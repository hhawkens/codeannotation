module internal CodeAnnotation.GenericAnnotator

open System.Text
open System.Text.RegularExpressions

let private keywordLabel = CodeBuildingBlock.Keyword.ToString()

let private buildAllWordsRegex patternsToExclude =
    let appendPattern txt pattern = sprintf "%s|%s" txt pattern.Regex
    let anyWordSearchPattern = sprintf @"(?<%s>[#@!$&%%]*\w+)" keywordLabel
    patternsToExclude // usually comment and string patterns
    |> Seq.fold appendPattern anyWordSearchPattern
    |> Regex

let private tokenFromMatch block (match': Match) =
    let groups = match'.Groups |> Seq.skip 1 |> Seq.filter (fun g -> g.Success) |> List.ofSeq
    match groups with
    | group::_ -> Some {Start = group.Index; Len = group.Length; Block = block}
    | _ -> None

let private tokensFromPatterns sourceCode (patterns: BuildingBlockPattern seq) =
    patterns
    |> Seq.collect (fun p -> matchAndExtract (tokenFromMatch p.Block) sourceCode (p.Regex |> Regex))

let private tryGetKeywordTokenFromMatch (keywords: Keywords) (match': Match) =
    let keywordGroups = match'.Groups |> Seq.filter (fun g -> g.Name = keywordLabel && g.Success) |> List.ofSeq
    match keywordGroups with
    | group::[] when keywords.Contains group.Value -> Some {Start = group.Index; Len = group.Length; Block = Keyword}
    | _ -> None

let private applyTokensToSource (sourceCode: SourceCodeRawText) (tokens: Token seq) =
    let annotatedCode = StringBuilder(sourceCode, sourceCode.Length * 2) // string builder capacity is a rough estimate
    for token in tokens do
        let substring = sourceCode.Substring(token.Start, token.Len).Trim()
        let replacement = formatAnnotation token.Block substring
        annotatedCode.Replace(substring, replacement, token.Start, token.Len) |> ignore
    annotatedCode.ToString()

let internal createAnnotator (patterns: BuildingBlockPattern seq) keywords =
    let patternsToExclude = patterns |> Seq.filter (fun p -> p.Block = Comment || p.Block = String)
    let allWordsRegex = buildAllWordsRegex patternsToExclude
    let annotate (sourceCode: SourceCodeRawText) =
        let tokens = tokensFromPatterns sourceCode patterns
        let keywordTokens = matchAndExtract (tryGetKeywordTokenFromMatch keywords) sourceCode allWordsRegex
        Seq.append keywordTokens tokens
        |> Tokens.sortAndFilterDescending
        |> applyTokensToSource sourceCode
    annotate
