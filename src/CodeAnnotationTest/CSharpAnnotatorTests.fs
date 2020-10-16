namespace CodeAnnotation.Tests

open CodeAnnotation
open Xunit
open Xunit.Abstractions
open FsUnit.Xunit

type CSharpAnnotatorTests(_output:ITestOutputHelper) =

    [<Theory>]
    [<InlineData(0, 0)>]
    [<InlineData(1, 1)>]
    [<InlineData(100, 0x5EED)>]
    [<InlineData(200, 0xBEEF)>]
    [<InlineData(30000, 0xCAFE)>]
    member _.``Fuzzy Testing with Random Strings`` (length, seed) =
        let randString = StringFuzz.generateRandomString length seed
        let annotated = CSharp randString |> Annotator.annotate
        annotated.Length |> should greaterThanOrEqualTo randString.Length

    [<Fact>]
    member _.``C# Code Annotated`` () =
        Annotator.annotate (CSharp CSharpAnnotatorTestData.csharpCode)
        |> should equal CSharpAnnotatorTestData.annotatedCsharpCode
        // hint: check out project HtmlTestPrint for a visualization of the test source code used here

    [<Fact>]
    member _.``Empty String Is Returned Unchanged`` () =
        Annotator.annotate (CSharp "")
        |> should equal ""
