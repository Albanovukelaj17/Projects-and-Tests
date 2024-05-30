using CommandLine;
using HSRM.CS.DistributedSystems.Hamster;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

Parser.Default.ParseArguments<ServerOptions>(args)
    .WithParsed(options =>
    {
        builder.WebHost.UseKestrel(kestrel =>
        {
            kestrel.Listen(IPAddress.Parse(options.IPAddress), options.Port, endpointOptions =>
            {
                endpointOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
            });
        });
    })
    .WithNotParsed(_ => Environment.Exit(2));

builder.Services.AddGrpc();

var app = builder.Build();
app.MapGrpcService<HamsterServer>();
app.Run();