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
open GCLRenderer

module treerendering =


    

    [<EntryPoint>]
    let main argv =
        printfn "Running stuff"
        

       

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

        //// Printing the designed tree
        let designedTree = design_tree tree
        let fstedTree = fst designedTree
        let extents = snd designedTree
        printfn "Part 1: Designing a tree\n using input:\n %A \n\n results in:\n %A \n\n And the extents were: \n%A\n\n" tree fstedTree extents

        //// Testing first property (uncomment lines below to enable the testing)
        printfn "\n\n Part 2.1 Atleast 1 unit apart\n"
        testFitInvariant

        //// Testing second property (uncomment line below to enable the testing)
        printfn "\n\n Part 2.2 Symmetric (Parrent allways centered above its children)\n"
        testSymmetricInvariant

        //// Testing third property (uncomment line below to enable the testing)
        printfn "\n\n Part 2.3 Reflective (When reflecting, property 1 and 2 still holds)\n"
        testReflectionInvariant

        //// Testing fourth property (uncomment line below to enable the testing)
        printfn "\n\n Part 2.4 Identical rendering (identical subtrees render identically)\n"
        testIdeSubInvariant

        // Postscript rendering:
        printfn "\n\n PostScript \n"
        printfn "%s" (toPSfast fstedTree)
        
        printfn "\n\n PostScript slow vs fast \n"
        let samples = generateSamples
        benchmark_slow samples
        benchmark_fast samples

        printfn "\n\n Abstract syntax tree is saved into FirstProject\\bin\\Debug\\net5.0\\GCprogram.ps \n" 
        TestGCLRendering

        0 // return an integer exit code
