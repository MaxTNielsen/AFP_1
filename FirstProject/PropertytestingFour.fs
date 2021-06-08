module PropertytestingFour
open Treerendering
open FsCheck

let predicatePosAndLabel f1 f2 = f1 <> f2

let predicateAlwaysFalse _ _ = false

let rec compareTrees x ((tree1 : Tree<'a*float>),(tree2 : Tree<'a*float>)) : bool =
    let (root1, subtree1) = match tree1 with | Node(node, subtree) -> (node, subtree)
    let (root2, subtree2) = match tree2 with | Node(node, subtree) -> (node, subtree)
    let (_, f1) = root1
    let (_, f2) = root2

    if x f1 f2 then false else
        let zipSubtrees = List.zip subtree1 subtree2
        List.forall (compareTrees predicatePosAndLabel) zipSubtrees


let identicalSubtreeInvariant (tree : Tree<string>) : bool = 

    // - Get random tree from FsCheck, 
    // - Put into tree, 
    // - Design the tree
    // - Extract the tree's from fscheck
    // - See if they are identically rendered.

    let sleaf s = Node(s, [])

    let simpleTree = Node("root", 
        [sleaf("first leaf");
        Node("fst branch", [
            sleaf("fst 1. leaf");
            Node("snd branch",[sleaf("some leaf");tree])]);
        Node("fst branch", [
            sleaf("snd 1. leaf");
            sleaf("snd 2. leaf")]);
        sleaf("second leaf");tree])

    let getSecondElem (tree : Tree<string*float>) = let matchTerm = (match tree with | Node(_, subtree) -> subtree) 
                                                    match matchTerm with 
                                                    | _ :: snd :: _ -> snd 
                                                    | _ -> failwith "Expecting 2 elements"
    let designedTree = fst (design_tree simpleTree)
    let (_, subtree) = match designedTree with | Node(label, subtree) -> (label, subtree)
    let firstTestTree = List.last subtree
    let secondTestTree = designedTree |> getSecondElem |> getSecondElem |> getSecondElem

    compareTrees predicateAlwaysFalse (firstTestTree,secondTestTree)

let testIdeSubInvariant =
    FsCheck.Check.Quick identicalSubtreeInvariant