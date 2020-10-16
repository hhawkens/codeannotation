namespace CodeAnnotation.Tests

open CodeAnnotation
open Xunit
open Xunit.Abstractions
open FsUnit.Xunit

type CSharpAnnotatorTests(_output:ITestOutputHelper) =

    [<Fact>]
    member _.``C# Code Annotated`` () =
        Annotator.annotate (CSharp CSharpAnnotatorTestData.csharpCode)
        |> should equal CSharpAnnotatorTestData.annotatedCsharpCode
        // hint: check out project HtmlTestPrint for a visualization of the test source code used here

    [<Fact>]
    member _.``Empty String Is Returned Unchanged`` () =
        Annotator.annotate (CSharp "")
        |> should equal ""
