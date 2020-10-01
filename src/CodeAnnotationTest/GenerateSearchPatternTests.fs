namespace CodeAnnotation.Tests

open System.Text.RegularExpressions
open CodeAnnotation
open Xunit
open Xunit.Abstractions
open FsUnit.Xunit

type GenerateSearchPatternTests(_output:ITestOutputHelper) =

    [<Literal>]
    let testCode = """
{~Keyword:namespace~} {~Type:Test~}

{~Keyword:open~} {~Type:System.IO~}

{~Keyword:let~} number = {~Literal:0~}
"""

    [<Fact>]
    member _.``Search Pattern Finds All Annotations`` () =
        // This test assumes the function "annotate" works as intended

        let patternForKeywords = Annotator.generateSearchPatternFor Keyword
        let patternForType = Annotator.generateSearchPatternFor Type
        let patternForLiteral = Annotator.generateSearchPatternFor Literal

        let keywordMatches = Regex.Matches(testCode, patternForKeywords)
        let typeMatches = Regex.Matches(testCode, patternForType)
        let literalMatches = Regex.Matches(testCode, patternForLiteral)

        keywordMatches.Count |> should equal 3
        keywordMatches.[0].Value |> should equal "{~Keyword:namespace~}"
        keywordMatches.[1].Value |> should equal "{~Keyword:open~}"
        keywordMatches.[2].Value |> should equal "{~Keyword:let~}"

        typeMatches.Count |> should equal 2
        typeMatches.[0].Value |> should equal "{~Type:Test~}"
        typeMatches.[1].Value |> should equal "{~Type:System.IO~}"

        literalMatches.Count |> should equal 1
        literalMatches.[0].Value |> should equal "{~Literal:0~}"

    [<Fact>]
    member _.``"Other" Type Blocks Have Correct Pattern`` () =
        let otherSearchPattern = Annotator.generateSearchPatternFor (Other "Module")
        otherSearchPattern |> should startWith @"{~Other-Module:"
