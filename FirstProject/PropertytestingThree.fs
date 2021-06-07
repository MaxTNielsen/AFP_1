module PropertytestingThree

open Treerendering
open PropertytestingOne
open PropertytestingTwo

let rec reflect (Node(v, subtrees): Tree<'a>) : Tree<'a> = 
                                            Node(v, List.map reflect (List.rev subtrees)) 

let rec reflectpos (Node((label, position), subtrees): Tree<'a*float>) : Tree<'a*float> = 
                                            Node((label, -position), List.map reflectpos subtrees )


let rec extractTree (designed:Tree<'a*float>) : Tree<'a> =
    let tree = match designed with | Node((label, _), subtrees) -> Node(label, List.map extractTree subtrees)
    tree

let reflectionInvariant (t:Tree<'a>) : bool = 
    let (originalTree, _) = design_tree t
    let reflectedDesignedTree = reflect (reflectpos originalTree)

    // Property 1
    let holdsForFitInvariant = fitInvariantFromDesigned originalTree && fitInvariantFromDesigned reflectedDesignedTree

    // Property 2
    let holdsForSymmetric = symmetricInvariant t && symmetricInvariant (extractTree reflectedDesignedTree)

    holdsForFitInvariant && holdsForSymmetric

let testReflectionInvariant =
    FsCheck.Check.Quick reflectionInvariant