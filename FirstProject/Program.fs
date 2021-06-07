// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open AST
open FsCheck
open Treerendering
// Define a function to construct a message to print

(* type Tree<'a> = | Node of 'a * ('a Tree list);;
type IntTree = Tree<int>;;
type StringTree = Tree<string>;;
type StringIntTree = Tree<string*float>;;
type Program = int;;
type Extent = (float*float) list *)

module treerendering =

    


    // 2 Property-based testing



    (*     // 3
    let toPSslow (t:StringIntTree) : string =
        "Not implemented";;

    // 4
    let toPSfast (t:StringIntTree) : string =
        "Not implemented";;

    // 5
    let treeTofile (s:string) (t:StringTree) =
        failwith "not implemented";;

    // 6
    let posTreeTofile (s:string) (t:StringTree) =
        failwith "not implemented";;

    // 10
    let toGeneralTree (p:Program) : StringTree =
        Node("Not implemented",[]) *)

    

    [<EntryPoint>]
    let main argv =
        printfn "Running stuff"
        let sleaf s = Node(s, [])
        let leaf = sleaf "leaf"
        let lowBranch = Node("lowBranch", [leaf;leaf;leaf])
        let branch = Node("branch", [leaf;lowBranch;leaf])
        let upperbranch = Node("upper", [branch;leaf;lowBranch])
        // let complexTree = Node("complex root", [branch; upperbranch])

        let simpleTree = Node("root", 
                                [   sleaf("first leaf");
                                    Node("fst branch", [
                                        sleaf("fst 1. leaf");
                                        sleaf("fst 2. leaf")]);
                                    Node("snd branch", [
                                        sleaf("snd 1. leaf");
                                        sleaf("snd 2. leaf")]);
                                    sleaf("second leaf")])

        let designedTree = design_tree simpleTree
        let tree = fst designedTree
        let extents = snd designedTree
        
        printfn "designtree with input:\n %A \n\n results in:\n %A \n\n And the extents were: \n%A" simpleTree tree extents
        0 // return an integer exit code
