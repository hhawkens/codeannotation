namespace CodeAnnotation

open System.Text.RegularExpressions

/// Source code with all programming languages supported for annotation
type public SourceCode =
    | CSharp of string
    | FSharp of string

/// All pieces of code that can be annotated
[<Struct>]
type public CodeBuildingBlock =
    | Keyword
    | Namespace
    | Preprocessor
    | Type
    | TypeCase
    | Interface
    | Literal
    | String
    | Function
    | Constant
    | Comment
    | Macro
    | Event
    | Property
    | Attribute
    | Other of id: string

[<Struct>]
type public BuildingBlockPattern = {
    Regex: string
    Block: CodeBuildingBlock
}

type public SourceCodeRawText = string

type public Keywords = Set<string>

type public Annotate = SourceCodeRawText -> string

[<Struct>]
type public AnnotationError =
    | BadRegex of string

[<Struct>]
type internal Token = {
    Start: int
    Len: int
    Block: CodeBuildingBlock
}

[<Struct>]
type internal ValidBuildingBlockPattern = {
    ValidRegex: Regex
    Block: CodeBuildingBlock
}

type internal PatternValidationResult =
    | ValidPatterns of ValidBuildingBlockPattern list
    | AnnotationErrors of AnnotationError list
