module Benchmarking

open PostScriptRendering
open Treerendering
open FsCheck

let sleaf s = Node(s, [])
let leaf = sleaf "leaf"
let lowBranch = Node("lowBranch", [leaf;leaf;leaf])
let branch = Node("branch", [leaf;lowBranch;leaf])
let upperbranch = Node("upper", [branch;leaf;lowBranch])
let complexTree = Node("complex root", [branch; upperbranch]) 

let treeGenerator = Arb.generate<Tree<string>>

let benchmarkStuff (eval:Tree<'a>->string) x : float =
    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let _ = eval x
    stopWatch.Stop()
    stopWatch.Elapsed.TotalMilliseconds

let benchmark_PostScriptRendering render_func s =
    let sampleSize = 50
    let sample = Gen.sample sampleSize sampleSize treeGenerator
    let designed_samples = List.map (fun a -> (fst (design_tree a))) sample

    let oberservations = List.map (benchmarkStuff render_func) designed_samples
    let sum = List.sum oberservations
    let meanTime = sum / float(sampleSize)
    printfn "Running testof : %s - The mean was: %f - Observations: %A" s meanTime oberservations

let benchmark_slow = benchmark_PostScriptRendering toPSslow "toPSslow"

let benchmark_fast = benchmark_PostScriptRendering toPSfast "toPSfast"

