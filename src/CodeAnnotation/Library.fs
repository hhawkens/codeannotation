module CodeAnnotation.Annotator

/// Annotates certain pieces of source code with their role.
/// Example for F#:
/// "let x = 0" ---> "{~Keyword:let~} x = {~Literal:0~}"
let public annotate (sourceCode: SourceCode) =
    match sourceCode with
    | CSharp source -> CSharpAnnotator.annotate source
    | FSharp source -> FSharpAnnotator.annotate source

/// Creates an annotator function for any programming language.
/// You need to provide the keywords and regex search patterns for
/// the language and get a function that annotates any source code
/// text written in said language.
let public createCustomAnnotator: BuildingBlockPattern seq -> Keywords -> SourceCodeRawText -> string =
    GenericAnnotator.createAnnotator

/// Provides a regex search pattern for given code building block
/// in an annotated piece of code
let public generateSearchPatternFor (block: CodeBuildingBlock) =
    formatAnnotation block "([\s\S]+?)"
