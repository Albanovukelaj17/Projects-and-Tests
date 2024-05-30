using CommandLine;
using HSRM.CS.DistributedSystems.Hamster;

Environment.ExitCode = Parser.Default.ParseArguments(args,
    typeof(ListVerb),
    typeof(AddVerb),
    typeof(FeedVerb),
    typeof(StateVerb),
    typeof(BillVerb))
    .MapResult((VerbBase verb) => verb.Execute(), _ => 2);