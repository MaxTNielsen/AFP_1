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
open Benchmarking

module treerendering =



    // 10
    //let toGeneralTree (p:Program) : StringTree =
    //    Node("Not implemented",[]) *)

    

    [<EntryPoint>]
    let main argv =
        printfn "Running stuff"
        

       

        //let simpleTree = Node("root", 
        //                        [   sleaf("L1");
        //                            Node("B1", [
        //                                sleaf("01");
        //                                sleaf("02")]);
        //                            Node("B2", [
        //                                sleaf("03");
        //                                sleaf("04")]);
        //                            sleaf("L2")])
        //let tree = simpleTree

        //// Printing the designed tree
        //let designedTree = design_tree tree
        //let fstedTree = fst designedTree
        //let extents = snd designedTree
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
        // printfn "%s" (toPSslow tree)

        // printfn "%s" (toPSfast fstedTree)

        // Benchmarking, uncomment for benchmarking setups
        // benchmark_slow
        // benchmark_fast

        let sample = (Gen.sample 1 50 treeGenerator).Head
        printfn "%A" sample


        0 // return an integer exit code
