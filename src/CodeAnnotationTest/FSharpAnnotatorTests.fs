namespace CodeAnnotation.Tests

open CodeAnnotation
open Xunit
open Xunit.Abstractions
open FsUnit.Xunit

type FSharpAnnotatorTests(_output:ITestOutputHelper) =

    [<Fact>]
    member _.``F# Code Annotated`` () =
        Annotator.annotate (FSharp FSharpAnnotatorTestData.fsharpCode)
        |> should equal FSharpAnnotatorTestData.annotatedFsharpCode
        // hint: check out project HtmlTestPrint for a visualization of the test source code used here

    [<Fact>]
    member _.``Triple Quoted Strings Annotated`` () =
        let source = @""""""" - This is weird - """""""
        let expectedResult = @"{~String:"""""" - This is weird - """"""~}"
        Annotator.annotate (FSharp source) |> should equal expectedResult

    [<Fact>]
    member _.``Namespace With Spaces Annotated`` () =
        let source = "namespace First. Second .  Third\n"
        let expectedResult = "{~Keyword:namespace~} {~Namespace:First. Second .  Third~}\n"
        Annotator.annotate (FSharp source) |> should equal expectedResult

    [<Fact>]
    member _.``Type With Keyword In Name Annotated`` () =
        let source = "type privateType = int"
        let expectedResult = "{~Keyword:type~} {~Type:privateType~} = {~Type:int~}"
        Annotator.annotate (FSharp source) |> should equal expectedResult

    [<Theory>]
    [<InlineData("module Hello .    There\n", "{~Keyword:module~} {~Type:Hello .    There~}\n")>]
    [<InlineData("module private A . B\n", "{~Keyword:module~} {~Keyword:private~} {~Type:A . B~}\n")>]
    [<InlineData("module internal Ey.Bee.Cee\n", "{~Keyword:module~} {~Keyword:internal~} {~Type:Ey.Bee.Cee~}\n")>]
    [<InlineData("module public Ey.Bee. Cee\n", "{~Keyword:module~} {~Keyword:public~} {~Type:Ey.Bee. Cee~}\n")>]
    [<InlineData("module privateMod.Mod\n", "{~Keyword:module~} {~Type:privateMod.Mod~}\n")>]
    member _.``Module With Spaces And Visibility Annotated`` (source, expectedResult) =
        Annotator.annotate (FSharp source) |> should equal expectedResult

    [<Theory>]
    [<InlineData("let isopen arg\n = arg = PI", "{~Keyword:let~} isopen arg\n = arg = PI")>]
    [<InlineData("let istype arg\n = arg = PI", "{~Keyword:let~} istype arg\n = arg = PI")>]
    [<InlineData("let ismodule arg\n = arg = PI", "{~Keyword:let~} ismodule arg\n = arg = PI")>]
    [<InlineData("let isnamespace arg\n = arg = PI", "{~Keyword:let~} isnamespace arg\n = arg = PI")>]
    member _.``Only Real Keywords Annotated`` (source, expectedResult) =
        Annotator.annotate (FSharp source) |> should equal expectedResult
