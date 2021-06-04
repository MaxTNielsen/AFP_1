module tree_rendering =

    type Tree<'a> = | Node of 'a * ('a Tree list);;
    type IntTree = Tree<int>;;
    type StringTree = Tree<string>;;
    type StringIntTree = Tree<string*float>;;
    type Program = int;;

    let movetree (Node(label,subtrees) : 'a Tree) (x : float) : 'a Tree = Node(label+x,subtrees);;

    // 2 Property-based testing

    // 3
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
        Node("Not implemented",[])

    let x = 2+2
    printfn "%i" x
