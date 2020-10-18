namespace CodeAnnotation.Tests

open CodeAnnotation
open Xunit
open Xunit.Abstractions

type CustomAnnotatorTests(_output:ITestOutputHelper) =

    [<Fact>]
    member _.``Bad Regex Yields Error`` () =
        let patterns = [|{Regex = @"\w"; Block = Type}; {Regex = "(()"; Block = Literal}|]
        let keyWords = seq {"one"; "two"} |> Set
        let customAnnotatorResult = Annotator.tryCreateCustomAnnotator patterns keyWords
        match customAnnotatorResult with
        | Ok _ -> Assert.True(false, "Expected result to be error")
        | Error _ -> Assert.True(true)

    [<Fact>]
    member _.``No Patterns And No Keywords Succeeds`` () =
        let patterns = [||]
        let keyWords = [||] |> Set
        let customAnnotatorResult = Annotator.tryCreateCustomAnnotator patterns keyWords
        match customAnnotatorResult with
        | Ok _ -> Assert.True(true)
        | Error _ -> Assert.True(false, "Expected result to be Ok")

    [<Fact>]
    member _.``Correct Input Yields Ok`` () =
        let patterns = [|{Regex = @"\w"; Block = Type}; {Regex = "\d"; Block = Literal}|]
        let keyWords = seq {"one"; "two"} |> Set
        let customAnnotatorResult = Annotator.tryCreateCustomAnnotator patterns keyWords
        match customAnnotatorResult with
        | Ok _ -> Assert.True(true)
        | Error _ -> Assert.True(false, "Expected result to be Ok")
