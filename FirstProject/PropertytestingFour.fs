module PropertytestingFour
open Treerendering
open FsCheck

let predicatePosAndLabel f1 f2 =
    f1 <> f2

let predicateAlwaysFalse f1 f2 = false

let rec compareTrees x ((tree1 : Tree<'a*float>),(tree2 : Tree<'a*float>)) : bool =
    let (root1, subtree1) = match tree1 with | Node(node, subtree) -> (node, subtree) | _ -> failwith "smth"
    let (root2, subtree2) = match tree2 with | Node(node, subtree) -> (node, subtree) | _ -> failwith "smth"
    let (_, f1) = root1
    let (_, f2) = root2

    if x f1 f2 then false else
        let zipSubtrees = List.zip subtree1 subtree2
        List.forall (compareTrees predicatePosAndLabel) zipSubtrees

let identicalSubtreeInvariant (tree : Tree<string>) : bool = 

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

    let tree = simpleTree
    let getSecondElem (tree : Tree<string*float>) = match (match tree with | Node(label, subtree) -> subtree) with | head :: snd :: tail -> snd  
    let designedTree = fst (design_tree simpleTree)
    let (label, subtree) = match designedTree with | Node(label, subtree) -> (label, subtree)
    let firstTestTree = List.last subtree
    let secondTestTree = designedTree |> getSecondElem |> getSecondElem |> getSecondElem

    compareTrees predicateAlwaysFalse (firstTestTree,secondTestTree)

let testIdeSubInvariant =
    FsCheck.Check.Quick identicalSubtreeInvariant