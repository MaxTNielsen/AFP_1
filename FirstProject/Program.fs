// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open AST
open FsCheck
open Treerendering
open PropertytestingOne
open PropertytestingTwo
open PropertytestingThree
open PropertytestingFour
open PostScriptRendering

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

        //let simpleTree = Node("root", 
        //                        [   sleaf("first leaf");
        //                            Node("fst branch", [
        //                                sleaf("fst 1. leaf");
        //                                sleaf("fst 2. leaf")]);
        //                            Node("snd branch", [
        //                                sleaf("snd 1. leaf");
        //                                sleaf("snd 2. leaf")]);
        //                            sleaf("second leaf")])

        let simpleTree = Node("root", 
                                [   sleaf("L1");
                                    Node("B1", [
                                        sleaf("01");
                                        sleaf("02")]);
                                    Node("B2", [
                                        sleaf("03");
                                        sleaf("04")]);
                                    sleaf("L2")])
        let tree = simpleTree

        


        // Printing the designed tree
        let designedTree = design_tree tree
        let fstedTree = fst designedTree
        let extents = snd designedTree
        //printfn "designtree with input:\n %A \n\n results in:\n %A \n\n And the extents were: \n%A\n\n" tree tree extents

        // Testing first property (uncomment lines below to enable the testing)
        //let treepaneder = CalculateTreeAppender tree
        //printfn "\n\n TreeAppender: %A\n\n" treepaneder
        // testFitInvariant

        // Testing second property (uncomment line below to enable the testing)
        // testSymmetricInvariant

        // Testing third property (uncomment line below to enable the testing)
        // testReflectionInvariant

        // Testing fourth property (uncomment line below to enable the testing)
        // testIdeSubInvariant

        // printfn "%s" (toPSslow (Node("ss",[])))
        printfn "%s" (toPSslow tree)

        0 // return an integer exit code
