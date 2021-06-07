module PropertytestingThree

open Treerendering

let rec reflect (Node(v, subtrees): Tree<'a>) : Tree<'a> = Node(v, List.map reflect (List.rev subtrees)) 

let rec reflectpos (Node((label, position), subtrees): Tree<'a*float>) : Tree<'a*float> = Node((label, -position), List.map reflectpos subtrees )