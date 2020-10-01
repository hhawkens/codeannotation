namespace CodeAnnotation

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
};

[<Struct>]
type internal Token = {
    Start: int
    Len: int
    Block: CodeBuildingBlock
}

type internal SourceCodeRawText = string
type internal Keywords = Set<string>
