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

let sampleSize = 300
let sampleLength = 300

let generateSamples =
    let sample = Gen.sample sampleSize sampleLength treeGenerator
    List.map (fun a -> (fst (design_tree a))) sample

let benchmark_PostScriptRendering render_func s designed_samples =

    let oberservations = List.map (benchmarkStuff render_func) designed_samples
    let sum = List.sum oberservations
    let meanTime = sum / float(sampleSize)
    printfn "Running testof : %s - The mean was: %f - Observations: %A" s meanTime oberservations

let benchmark_slow sample = benchmark_PostScriptRendering toPSslow "toPSslow" sample

let benchmark_fast sample = benchmark_PostScriptRendering toPSfast "toPSfast" sample

